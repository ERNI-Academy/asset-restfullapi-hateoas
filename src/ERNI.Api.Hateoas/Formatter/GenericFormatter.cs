using ERNI.Api.Hateoas.Dto;
using ERNI.Api.Hateoas.Services;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Reflection;

namespace ERNI.Api.Hateoas.Formatter;

public class GenericFormatter
{
    internal IDataShaper dataShaper;
    internal OutputFormatterWriteContext context;

    public GenericFormatter(OutputFormatterWriteContext outputFormatterWriteContext)
    {
        context = outputFormatterWriteContext; 
        dataShaper = GetService<IDataShaper>();
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
        if (context.HttpContext.Request.Query.ContainsKey("Fields"))
        {
            properties = context.HttpContext.Request.Query["Fields"];
        }

        return properties;
    }

    internal T GetService<T>()
    {
        return (T)context.HttpContext.RequestServices.GetService(typeof(T));
    }

    internal T GetService<T>(Type typeOfService)
    {
        return (T)context.HttpContext.RequestServices.GetService(typeOfService);
    }

    internal ILinkGenerator GetLinkGenerator()
    {
        var currentResponseType = context.ObjectType.GenericTypeArguments.FirstOrDefault() != null ?
             context.ObjectType.GenericTypeArguments.FirstOrDefault() :
             context.ObjectType;


        var linkGenerators = GetLinkGenerators();
        var resultClass = Type.GetType(linkGenerators.FirstOrDefault(i => i.GetInterfaces().Any(j => j.GenericTypeArguments.Any(t => t == currentResponseType))).GetTypeInfo().AssemblyQualifiedName);
        var interfacef = resultClass.GetInterface(typeof(ILinkGenerator<>).Name);
        var linkGenerator = GetService<ILinkGenerator>(interfacef);
        return linkGenerator;
    }

    internal object GetResultResponse()
    {
        var linkGenerator = GetLinkGenerator();
        string properties = GetProperties();

        var collection = context.Object as IEnumerable<object>;
        object resultResponse;
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
