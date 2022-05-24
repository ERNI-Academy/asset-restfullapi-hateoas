using ERNI.Api.Hateoas.Sample.Services;

namespace ERNI.Api.Hateoas.Sample.Extension
{
    public static class ServicesExtension
    {
        public static void RegisterServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStudentsService, StudentsService>();
        }
    }
}
