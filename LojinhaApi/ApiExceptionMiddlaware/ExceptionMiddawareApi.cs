﻿using Domain.Entities;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;

namespace LojinhaApi.ApiExceptionMiddlaware
{
    public static class ExceptionMiddawareApi 
    {
       
        public static void ConfigureExcepetionHandler( this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();


                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            Trace = contextFeature.Error.StackTrace
                        }.ToString() + "Ocorreu um erro fora do controllador");
                    }
                }));                         
        }
    }
}
