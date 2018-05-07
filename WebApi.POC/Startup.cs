using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Text;
using WebApi.POC.Repository;
using WebApi.POC.Repository.Local;
using WebApi.POC.Services;
using WebApi.POC.Swagger;
using WebApi.POC.Swagger.Mock;
using WebApi.POC.Utils;
using WebApi.Security;
using WebApi.Shared;
using WebApi.Shared.Constants;

namespace WebApi.POC
{
    /// <summary>
    /// Class to start p the app
    /// </summary>
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IHostingEnvironment Environment { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void ConfigureServices(IServiceCollection services)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {

            if (Environment.IsEnvironment("SwaggerMock"))
            {
                services.AddScoped<IKeyStorageContainer, SwaggerMockedStorageContainer>();
            }
            else
            {
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IDemandsRepository, DemandsRepository>(); 
                services.AddScoped<IKeyStorageContainer, DbKeyStorageContainer>();
            }

            if (Environment.EnvironmentName.Contains("Swagger"))
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info
                    {
                        Title = "Poc API V1",
                        Version = "v1"
                    });
                    c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = "header",
                        Type = "apiKey"
                    });
                    c.OperationFilter<AuthorizationFilter>();

                    var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "WebApi.POC.xml");
                    c.IncludeXmlComments(filePath);
                });
            }

            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<ICryptoService, CryptoService>();

            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<ICryptoService, CryptoService>();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddSingleton(new NHSessionFactory(connectionString));
            services.AddScoped(s => {
                var session = s.GetRequiredService<NHSessionFactory>().SessionFactory.OpenSession();
                session.DefaultReadOnly = true;
                return session;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30),
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = ServerConstants.SERVER_URL,
                    ValidAudience = "myClient",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SomeSecureRandomKey"))
                };
            });

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            services.AddMvc().AddJsonOptions(o => {
                o.SerializerSettings.ContractResolver = new NHibernateContractResolver();
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (env.EnvironmentName.Contains("Swagger"))
            {
                app.UseSwagger(o =>
                {
                    o.RouteTemplate = "docs/{documentName}/swagger.json";
                });
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/docs/v1/swagger.json", "Poc API V1");
                    c.RoutePrefix = "docs";
                });
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseAuthentication();
            app.UseMvc();
        }
    }

    class NHibernateContractResolver : DefaultContractResolver
    {
        protected override JsonContract CreateContract(Type objectType)
        {
            if (typeof(NHibernate.Proxy.INHibernateProxy).IsAssignableFrom(objectType))
                return base.CreateContract(objectType.BaseType);
            else
                return base.CreateContract(objectType);
        }
    }
}
