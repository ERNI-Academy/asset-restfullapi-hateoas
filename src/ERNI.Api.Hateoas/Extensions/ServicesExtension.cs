using ERNI.Api.Hateoas.Formatter;
using ERNI.Api.Hateoas.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ERNI.Api.Hateoas.Extension;

public static class ServicesExtension
{
    public static IMvcBuilder AddHateoas(this IMvcBuilder builder, params Assembly[] assemblies)
    {
        builder.Services.RegisterServices();
        builder.Services.RegisterLinkGeneratorAssemblies(assemblies);

        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        builder.Services.AddScoped<IUrlHelper>(factory =>
        {
            var actionContext = factory.GetService<IActionContextAccessor>().ActionContext;
            return new UrlHelper(actionContext);
        });

        builder.AddMvcOptions(o =>
        {
            o.OutputFormatters.Add(new JsonHateoasFormatter());
            o.OutputFormatters.Add(new XmlHateoasFormatter());
        });
        return builder;
    }

    private static void RegisterServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IDataShaper, DataShaper>();
        serviceCollection.AddScoped(typeof(ISortHelper<>), typeof(SortHelper<>));
    }

    private static void RegisterLinkGeneratorAssemblies(this IServiceCollection serviceCollection, params Assembly[] assemblies)
    {
        var assembly = assemblies != null ? assemblies : new[] { Assembly.GetExecutingAssembly() };
        var linkGenerators = assembly.SelectMany(i => i.GetTypes().Where(x => !x.IsInterface &&
            x.GetInterface(typeof(ILinkGenerator<>).Name) != null));

        foreach (var linkGenerator in linkGenerators)
        {
            var interfaceLinkGenerator = linkGenerator.GetTypeInfo().ImplementedInterfaces.FirstOrDefault(i => i.Name != nameof(ILinkGenerator));
            serviceCollection.AddScoped(interfaceLinkGenerator, linkGenerator);
        }
    }
}
