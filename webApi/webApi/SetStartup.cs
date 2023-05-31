using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

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
                //获取xml注释文件的目录
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "webApi.xml");
                c.IncludeXmlComments(xmlPath,true);//默认的第二个参数是false，这个是controller的注释，记得修改
                var xmlModelPath = Path.Combine(AppContext.BaseDirectory, "webApiModel.xml");//这个就是Model层的xml文件名
                c.IncludeXmlComments(xmlModelPath);
            });
            
        }
    }
}
