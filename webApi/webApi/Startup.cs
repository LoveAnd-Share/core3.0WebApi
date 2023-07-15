using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;
using webApiCommon.Helper;
using webApi.SetSatrtup;

namespace webApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //ע���ȡappsettings��ȡ��
            services.AddSingleton(new AppSettings(Configuration));

            var text = AppSettings.app(new string[] { "AppSettings","ConnectionString"});
            //jwt��Ȩ��֤
            services.AddAuthorizationSetup();
            //ע��swagger
            services.AddSwaggerSetUp();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Webapi.Core v1");
                c.RoutePrefix = "";
            });
            //ע���м����˳��UseRouting������ǰ�ߣ�UseAuthentication��UseAuthorizationǰ��
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapControllerRoute(
                //    name: default,
                //    pattern: "{controller=Home}/{action=Index}/{id?}"
                //    );
            });
        }
    }
}
