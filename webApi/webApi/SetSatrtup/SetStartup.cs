using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using webApiCommon.Helper;

namespace webApi.SetSatrtup
{
    public static class SetStartup
    {
        /// <summary>
        /// swagger设置
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddSwaggerSetUp(this IServiceCollection services)
        {

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
                c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改
                var xmlModelPath = Path.Combine(AppContext.BaseDirectory, "webApiModel.xml");//这个就是Model层的xml文件名
                c.IncludeXmlComments(xmlModelPath);

                c.OperationFilter<SecurityRequirementsOperationFilter>();
                //token绑定到configureservices
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)，
                    Type = SecuritySchemeType.ApiKey
                });
            });

        }
        /// <summary>
        /// 设置授权
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddAuthorizationSetup(this IServiceCollection services) {
            if (services == null) throw new ArgumentNullException(nameof(services));
            //读取配置文件
            var symmetricKeyAsBase64 = AppSettings.app(new string[] { "AppSettings", "JwtSetting", "SecretKey" });
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var Issuer = AppSettings.app(new string[] { "AppSettings", "JwtSetting", "Issuer" });
            var Audience = AppSettings.app(new string[] { "AppSettings", "JwtSetting", "Audience" });

            // 令牌验证参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = Issuer,//发行人
                ValidateAudience = true,
                ValidAudience = Audience,//订阅人
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30),
                RequireExpirationTime = true,
            };
            // 2.1【认证】、core自带官方JWT认证
            // 开启Bearer认证
            services.AddAuthentication("Bearer")
                // 添加JwtBearer服务
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = tokenValidationParameters;
                    o.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            // 如果过期，则把<是否过期>添加到，返回头信息中
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}
