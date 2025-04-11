using AccidentMonitor.Infrastructure;
using AccidentMonitor.Infrastructure.Data;
using AccidentMonitor.Infrastructure.MQTT;
using AccidentMonitor.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddEnvironmentVariables();
        builder.AddServiceDefaults();
        builder.AddKeyVaultIfConfigured();
        builder.AddApplicationServices();
        builder.AddInfrastructureServices();
        builder.AddWebServices();

        var app = builder.Build();

        await app.InitializeMqttAsync();

        if (app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            await app.InitializeDatabaseAsync();
        }
        else
        {
            app.UseHsts();
        }

        //app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseSwaggerUi(settings =>
        {
            settings.Path = "/api";
            settings.DocumentPath = "/api/specification.json";
        });

        app.MapRazorPages();

        app.MapFallbackToFile("index.html");

        app.UseExceptionHandler(options => { });

        app.Map("/", () => Results.Redirect("/api"));

        app.MapDefaultEndpoints();
        app.MapEndpoints();

        await app.RunAsync();
    }
}
