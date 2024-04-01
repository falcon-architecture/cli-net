namespace Cli;

public class Program
{
    protected Program() { }

    public static async Task<int> Main(params string[] args)
    {
        var serviceProvider = GetServiceProvider();
        var application = new CommandLineApplication<Application>();
        application
            .Conventions
            .UseDefaultConventions()
            .UseConstructorInjection(serviceProvider);
        return await application.ExecuteAsync(args);
    }
    public static ServiceProvider GetServiceProvider()
    {
        return new ServiceCollection()
            .AddLogging(logging =>
            {
                logging.AddSimpleConsole(options =>
                {
                    options.SingleLine = true;
                    options.TimestampFormat = "hh:mm:ss ";
                    options.ColorBehavior = LoggerColorBehavior.Enabled;
                });
                logging.SetMinimumLevel(LogLevel.Trace);
            })
            .AddSingleton<WebApp>()
            .AddSingleton<ApiApp>()
            .BuildServiceProvider();
    }
}