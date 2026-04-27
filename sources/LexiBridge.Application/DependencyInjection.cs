using LexiBridge.Application.Behaviors;
using LexiBridge.Application.ExceptionHandlers;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace LexiBridge.Application;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        var assembly = Assembly.GetExecutingAssembly();

        // Register MediatR services and behaviors
        services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(ValidationExceptionHandler<,,>));
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return builder;
    }
}
