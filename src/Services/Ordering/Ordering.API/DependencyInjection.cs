using Carter;

namespace Ordering.API
{
    public static class DependencyInjection
    {
        //Before build applicarion
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddCarter();

            return services;
        }

        //After build applicarion
        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.MapCarter();

            return app;
        }
    }
}
