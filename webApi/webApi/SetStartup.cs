using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace webApi
{
    public static class SetStartup
    {
        public static void AddSwaggerSetUp(this IServiceCollection services) {

            if (services == null) throw new ArgumentNullException(nameof(services));
            var ApiName = "Webapi.Core";
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = $"{ApiName}接口文档-netcore 3.0",
                    Description = $"{ApiName} Http api v1",
                });
                c.OrderActionsBy(o => o.RelativePath);
            });

        }
    }
}
