using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Data;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Data.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor,DispatchDomainEventsInterceptor>();

            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                options.AddInterceptors(
                    sp.GetServices<ISaveChangesInterceptor>()
                );
                options.UseSqlServer(configuration.GetConnectionString("Database"));
            });

            services.AddScoped<IApplicationDbContext, AppDbContext>();

            return services;
        }
    }
}
