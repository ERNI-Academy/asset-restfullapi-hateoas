using ERNI.Api.Hateoas.Dto;
using ERNI.Api.Hateoas.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace ERNI.Api.Hateoas.Formatter;

public class GenericFormatter
{
    internal IActionContextAccessor contextAccessor;
    internal IUrlHelperFactory urlHelperFactory;
    internal IDataShaper dataShaper;
    internal IActionDescriptorCollectionProvider actionDescriptorProvider;
    internal IUrlHelper urlHelper;

    public GenericFormatter(OutputFormatterWriteContext context)
    {
        InitializeServices(context);
    }

    internal static IEnumerable<Type> GetLinkGenerators()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        return assemblies.SelectMany(t => t.GetTypes()).Where(x => !x.IsInterface &&
                    x.GetInterface(typeof(ILinkGenerator<>).Name) != null);
    }


    internal string GetProperties()
    {
        var properties = string.Empty;
        if (contextAccessor.ActionContext.HttpContext.Request.Query.ContainsKey("Fields"))
        {
            properties = contextAccessor.ActionContext.HttpContext.Request.Query["Fields"];
        }

        return properties;
    }

    internal T GetService<T>(OutputFormatterWriteContext context)
    {
        return (T)context.HttpContext.RequestServices.GetService(typeof(T));
    }

    internal T GetService<T>(OutputFormatterWriteContext context, Type typeOfService)
    {
        return (T)context.HttpContext.RequestServices.GetService(typeOfService);
    }

    internal void InitializeServices(OutputFormatterWriteContext context)
    {
        contextAccessor = GetService<IActionContextAccessor>(context);
        urlHelperFactory = GetService<IUrlHelperFactory>(context);
        dataShaper = GetService<IDataShaper>(context);
        actionDescriptorProvider = GetService<IActionDescriptorCollectionProvider>(context);
        urlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);
    }

    internal ILinkGenerator GetLinkGenerator(OutputFormatterWriteContext context)
    {
        var currentResponseType = context.ObjectType.GenericTypeArguments.FirstOrDefault() != null ?
             context.ObjectType.GenericTypeArguments.FirstOrDefault() :
             context.ObjectType;


        var linkGenerators = GetLinkGenerators();
        var resultClass = Type.GetType(linkGenerators.FirstOrDefault(i => i.GetInterfaces().Any(j => j.GenericTypeArguments.Any(t => t == currentResponseType))).GetTypeInfo().AssemblyQualifiedName);
        var interfacef = resultClass.GetInterface(typeof(ILinkGenerator<>).Name);
        var linkGenerator = GetService<ILinkGenerator>(context, interfacef);
        return linkGenerator;
    }

    internal object GetResultResponse(OutputFormatterWriteContext context)
    {
        var linkGenerator = GetLinkGenerator(context);
        string properties = GetProperties();

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

        return resultResponse;
    }

}
