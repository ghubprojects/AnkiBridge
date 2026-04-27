namespace LexiBridge.Web;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddWebServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        return builder;
    }
}
