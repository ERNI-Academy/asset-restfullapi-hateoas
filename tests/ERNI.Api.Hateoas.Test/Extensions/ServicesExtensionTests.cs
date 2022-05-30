using ERNI.Api.Hateoas.Formatter;
using ERNI.Api.Hateoas.Services;
using ERNI.Api.Hateoas.Test.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;

namespace ERNI.Api.Hateoas.Extension.Tests;

public class ServicesExtensionTests
{
    [Fact()]
    public void AddHateoasTest()
    {
        // arrange
        var builder = WebApplication.CreateBuilder();

        // act
        builder.Services.AddControllers().AddHateoas(this.GetType().Assembly);

        // assert
        using (new AssertionScope())
        {
            builder.Services.Where(item => item.ImplementationType != null).Any(item => item.ImplementationType.FullName == typeof(StudentLinkGenerator).FullName).Should().BeTrue();
            builder.Services.Where(item => item.ImplementationType != null).Any(item => item.ImplementationType.FullName == typeof(ActionContextAccessor).FullName).Should().BeTrue();
            builder.Services.Where(item => item.ImplementationType != null).Any(item => item.ImplementationType.FullName == typeof(DataShaper).FullName).Should().BeTrue();
            builder.Services.Where(item => item.ImplementationType != null).Any(item => item.ImplementationType.FullName == typeof(SortHelper<>).FullName).Should().BeTrue();
            builder.Services.Any(item => item.ServiceType.FullName == typeof(IUrlHelper).FullName);
            builder.Services.Any(item => item.ServiceType.FullName == typeof(IConfigureOptions<MvcOptions>).FullName);
        }
    }
}