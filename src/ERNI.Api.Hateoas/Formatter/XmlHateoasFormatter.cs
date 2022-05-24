using ERNI.Api.Hateoas.Dto;
using ERNI.Api.Hateoas.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace ERNI.Api.Hateoas.Formatter
{
    public class XmlHateoasFormatter : OutputFormatter
    {
        public XmlHateoasFormatter()
        {
            SupportedMediaTypes.Add("application/xml+hateoas");
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var contextAccessor = GetService<IActionContextAccessor>(context);
            var urlHelperFactory = GetService<IUrlHelperFactory>(context);
            var dataShaper = GetService<IDataShaper>(context);
            var actionDescriptorProvider = GetService<IActionDescriptorCollectionProvider>(context);
            var urlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);
            var response = context.HttpContext.Response;

            if (context.Object is SerializableError error)
            {
                var streamError = new MemoryStream();
                var errorSerializer = new XmlSerializer(typeof(SerializableError));
                errorSerializer.Serialize(streamError, error);
                response.ContentType = "application/xml";
                return response.WriteAsync(Encoding.Default.GetString(streamError.ToArray()));
            }

            var currentResponseType = context.ObjectType.GenericTypeArguments.FirstOrDefault() != null ?
                 context.ObjectType.GenericTypeArguments.FirstOrDefault() :
                 context.ObjectType;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var linkGenerators = assemblies.SelectMany(t=>t.GetTypes()).Where(x => !x.IsInterface &&
            x.GetInterface(typeof(ILinkGenerator<>).Name) != null);
            
            var resultClass = Type.GetType(linkGenerators.FirstOrDefault(i => i.GetInterfaces().Any(j => j.GenericTypeArguments.Any(t => t == currentResponseType))).GetTypeInfo().AssemblyQualifiedName);

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

            var stream = new MemoryStream();
            var serializer = new XmlSerializer(resultResponse.GetType());
            serializer.Serialize(stream, resultResponse);

            var output = Encoding.Default.GetString(stream.ToArray());
            response.ContentType = "application/xml+hateoas";
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
