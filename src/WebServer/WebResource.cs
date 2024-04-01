namespace Cli.WebServer;

public class WebResource
{
    private readonly ILogger<WebResource> Logger;
    public WebResource(ILogger<WebResource> logger)
    {
        Logger = logger;
    }
    private static Dictionary<string, string> Resources = new Dictionary<string, string>();
    public string Read(string resourcePath)
    {
        if (!Resources.ContainsKey(resourcePath))
        {
            if (!ResourceNames.Contains(resourcePath))
            {
                Logger.LogWarning($"There is no resource available. Path: {resourcePath}");
                Logger.LogDebug($"Available Resources: {string.Join(Environment.NewLine, ResourceNames)}");
                return string.Empty;
            }
            Stream? templateStream = typeof(WebResource).Assembly.GetManifestResourceStream(resourcePath);
            string resource = templateStream == null ? string.Empty : new StreamReader(templateStream).ReadToEnd();
            Resources.Add(resourcePath, resource ?? string.Empty);
        }
        return Resources[resourcePath];
    }
    public byte[] ReadBytes(string resourcePath)
    {
        var content = Read(resourcePath);
        var data = Encoding.UTF8.GetBytes(content);
        return data;
    }
    private static string[] resourceNames = typeof(WebResource).Assembly.GetManifestResourceNames();
    public static string[] ResourceNames => resourceNames;
}