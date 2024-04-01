namespace Cli.WebServer;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRouting();
        services.AddControllers();
        services.AddLogging(logger => logger.AddSimpleConsole(options =>
        {
            options.SingleLine = true;
            options.TimestampFormat = "hh:mm:ss ";
            options.ColorBehavior = LoggerColorBehavior.Enabled;
        }));
        services.AddSingleton<WebResource>();
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseEndpoints(routes =>
       {
           // routes.MapRoute(name: "api", template: "one", defaults: new { controller = "Home", action = "PageOne" });
           // routes.MapRoute(name: "goto_two", template: "two/{id?}", defaults: new { controller = "Home", action = "PageTwo" });
           // routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
           routes.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
       });
        WebResource resource = app.ApplicationServices.GetRequiredService<WebResource>();
        app.Map(Constant.LivenessUrl, App => App.Run(async ctx => await ctx.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(true.ToString()))));
        app.Map(Constant.FavIconUrl, App => App.Run(async ctx => await ctx.Response.Body.WriteAsync(resource.ReadBytes("Cli.Client.favicon.ico"))));
        app.Map(Constant.WebUrl, App => App.Run(async ctx =>
       {
           string? value = ctx.Request.Path.Value;
           string url = value == null ||
                        string.IsNullOrWhiteSpace(value) ||
                        value.Equals("/") ? "/index.html" : value;
           url = url.Replace("/", ".");
           string path = $"Cli.Client{url}";
           var content = resource.ReadBytes(path);
           await ctx.Response.Body.WriteAsync(content, 0, content.Length);
       }));
    }
}