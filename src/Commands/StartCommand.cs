namespace Cli.Commands;

[Command(Name = "start", Description = "Start the web UI")]
public class StartCommand : CommandBase
{
    public StartCommand(ILogger<StartCommand> logger) : base(logger) { }
    [Option("-r|--protocol", "Spacify the protocol", CommandOptionType.SingleValue)]
    [AllowedValues("http", "https", IgnoreCase = true)]
    public string Protocol { get; } = "http";

    // [Argument(1, "Port", "Port number to execute the Data Migration Web")]
    [Option("-p|--port", "UI port number", CommandOptionType.SingleValue)]
    public int Port { get; } = 8080;

    protected override Task<int> ExecuteAsync(CommandLineApplication application)
    {
        Debug($"Press Ctrl+C to stop the server");
        var apiApplication = application.GetRequiredService<ApiApp>();
        Task.Run(() => apiApplication.Run());
        var webApplication = application.GetRequiredService<WebApp>();
        webApplication.Run(Protocol, Port);
        return Task.FromResult(0);
    }
}