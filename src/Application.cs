
namespace Cli;

[Command("cli")]
[VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
[Subcommand(
    typeof(StartCommand)
)]
public class Application : CommandBase
{
    public Application(ILogger<Application> logger) : base(logger) { }
    protected override Task<int> ExecuteAsync(CommandLineApplication application)
    {
        application.ShowHelp();
        return Task.FromResult(0);
    }

    private string? GetVersion()
    {
        return GetVersion(typeof(Application));
    }

    private string? GetVersion(Type type)
    {
        return type?.Assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
    }
}