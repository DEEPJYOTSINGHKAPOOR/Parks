using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LearningDotNetCore
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.Use(async (context,next)=>
            //{
            //    Console.WriteLine("Before Request");
            //    await next();
            //    Console.WriteLine("After Request");
            //});


            app.Map("/favicon.ico", (app) => { });

//            app.UseMiddleware<ConsoleLoggerMiddleware>();

            app.Map("/map", MapHandler);


            app.MapWhen( context=>
                context.Request.Query.ContainsKey("q"),
                    HandleRequestWithQuery
            );


            app.Run(async context =>
            {
                Console.WriteLine("Hello Run");
                await context.Response.WriteAsync("Hell World");
            }); 
        }

        private void HandleRequestWithQuery(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                Console.WriteLine("Contains Query");
               // await next();
            } );
        }

        private void MapHandler(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                Console.WriteLine("Hello for Map Method");
                await context.Response.WriteAsync("Hello from Map Method...");
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ConsoleLoggerMiddleware>();
        }

    }
}
