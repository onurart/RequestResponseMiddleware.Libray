using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Nuget.RequestResponseMiddleware.Libray;
using Nuget.RequestResponseMiddleware.Libray.Models;
using System;

namespace Test.Api
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
            services.AddLogging(conf => { conf.AddConsole(); });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Test.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test.Api v1"));
            }

            app.UseRouting();

            app.UseAuthorization();
            app.AddTBRequestResponseMiddleware(opt =>
            {
                //opt.UseHandler(async context =>
                //{
                //    Console.WriteLine($"RequstBody: {context.RequestBody}");
                //    Console.WriteLine($"ResponseBody: {context.ResponseBody}");
                //    Console.WriteLine($"timimg: {context.FormattedCreationTime}");
                //    Console.WriteLine($"Url: {context.Url}");

                //});
                opt.UseLogger(app.ApplicationServices.GetRequiredService<ILoggerFactory>(), opt =>
                {
                    opt.LogLevel = LogLevel.Error;
                    opt.LoggerCategoryName = "CategoriName";
                    opt.LoggingFields.Add(LogFields.Request);
                    opt.LoggingFields.Add(LogFields.Response);
                    opt.LoggingFields.Add(LogFields.ResponseTiming);
                    opt.LoggingFields.Add(LogFields.Path);
                    opt.LoggingFields.Add(LogFields.QueryString);
                });

            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
