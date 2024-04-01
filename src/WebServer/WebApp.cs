namespace Cli.WebServer;
public class WebApp
{
    private readonly ILogger<WebApp> _logger;
    public WebApp(ILogger<WebApp> logger)
    {
        _logger = logger;
    }
    public void Run(string protocol = "http", int port = 8080)
    {
        var host = new WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .ConfigureServices(services => services.AddSingleton(_logger))
        .UseStartup<Startup>()
        .UseUrls($"{protocol}://127.0.0.1:{port}")
        .Build();
        _logger.LogTrace($"WebHost Builder Ready");
        host.Start();
        _logger.LogTrace($"WebHost started");
        var addresses = host.ServerFeatures.Get<IServerAddressesFeature>()?.Addresses.ToList();
        var address = addresses?.FirstOrDefault(x => x.StartsWith("https://")) ?? addresses?.FirstOrDefault();
        _logger.LogDebug($"WebHost address: " + address);
        OpenBrowser(address + "/web");
        host.WaitForShutdown();
    }

    public void OpenBrowser(string url)
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else
            {
                Console.WriteLine("Please open the Url in browser: {0}", url);
            }
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message, ex);
        }
    }
}