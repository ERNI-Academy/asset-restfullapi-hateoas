using ERNI.Api.Hateoas.Dto;
using ERNI.Api.Hateoas.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace ERNI.Api.Hateoas.Formatter
{
    public class JsonHateoasFormatter : OutputFormatter
    {
        public JsonHateoasFormatter()
        {
            SupportedMediaTypes.Add("application/json+hateoas");
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var contextAccessor = GetService<IActionContextAccessor>(context);
            var urlHelperFactory = GetService<IUrlHelperFactory>(context);
            var dataShaper = GetService<IDataShaper>(context);
            var actionDescriptorProvider = GetService<IActionDescriptorCollectionProvider>(context);
            var urlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);
            var response = context.HttpContext.Response;

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

            if (context.Object is SerializableError error)
            {
                var errorOutput = JsonConvert.SerializeObject(error, serializerSettings);
                response.ContentType = "application/json";
                return response.WriteAsync(errorOutput);
            }

            var currentResponseType = context.ObjectType.GenericTypeArguments.FirstOrDefault() != null ?
                 context.ObjectType.GenericTypeArguments.FirstOrDefault() :
                 context.ObjectType;
            var assembly = Assembly.GetExecutingAssembly();
            var linkGenerators = assembly.GetTypes().Where(x => !x.IsInterface &&
                x.GetInterface(typeof(ILinkGenerator<>).Name) != null);

            var resultClass = Type.GetType(linkGenerators.FirstOrDefault(i => i.GetInterfaces().Any(j => j.GenericTypeArguments.Any(t => t == currentResponseType))).GetTypeInfo().FullName);
            var interfacef = resultClass.GetInterface(typeof(ILinkGenerator<>).Name);

            var properties = string.Empty;
            if (contextAccessor.ActionContext.HttpContext.Request.Query.ContainsKey("Fields"))
            {
                properties = contextAccessor.ActionContext.HttpContext.Request.Query["Fields"];
            }

            var linkGenerator = GetService<ILinkGenerator>(context, interfacef);
            var collection = context.Object as IEnumerable<object>;
            object resultResponse = null;
            if (collection != null)
            {
                var shapedData = dataShaper.ShapeData(context.Object, properties).ToList();
                var items = new List<object>(collection);
                for (int i = 0; i < collection.Count(); i++)
                {
                    shapedData[i].TryAdd("Links", linkGenerator.GetLinks(items[i]));
                }

                resultResponse = new LinkCollectionWrapper<ResponseDto>(shapedData);
                ((LinkCollectionWrapper<ResponseDto>)resultResponse).Links.AddRange(linkGenerator.GetLinks(context.Object).ToList());
            }
            else
            {
                var shapedData = dataShaper.ShapeData(context.Object, properties).FirstOrDefault();
                shapedData.TryAdd("Links", linkGenerator.GetLinks(context.Object));

                resultResponse = shapedData;
            }

            var output = JsonConvert.SerializeObject(resultResponse, serializerSettings);
            response.ContentType = "application/json+hateoas";
            return response.WriteAsync(output);
        }

        private T GetService<T>(OutputFormatterWriteContext context)
        {
            return (T)context.HttpContext.RequestServices.GetService(typeof(T));
        }

        private T GetService<T>(OutputFormatterWriteContext context, Type typeOfService)
        {
            return (T)context.HttpContext.RequestServices.GetService(typeOfService);
        }
    }
}
