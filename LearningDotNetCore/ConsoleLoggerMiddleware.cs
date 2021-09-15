using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LearningDotNetCore
{
    public class ConsoleLoggerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Console.WriteLine("Before Request");
            await next(context);
            Console.WriteLine("After Request");
        }
    }
}
