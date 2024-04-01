namespace Cli.Commands;

public abstract class CommandBase
{
    [Option("-v|--verbose", "Show detailed logs", CommandOptionType.SingleOrNoValue)]
    private bool VerboseFlag { get; } = false;
    private readonly ILogger _logger;

    protected virtual Task<int> BeforeExecuteAsync(CommandLineApplication application) => Task.FromResult(0);
    protected abstract Task<int> ExecuteAsync(CommandLineApplication application);
    protected virtual Task<int> AfterExecuteAsync(CommandLineApplication application) => Task.FromResult(0);
    protected virtual Task<int> FinallyAsync(CommandLineApplication application) => Task.FromResult(0);
    protected CommandBase(ILogger logger)
    {
        _logger = logger;
    }

    protected async Task<int> OnExecuteAsync(CommandLineApplication application)
    {
        Verbose("Started");
        var watch = Stopwatch.StartNew();
        int result = 0;
        try
        {
            await BeforeExecuteAsync(application);
            result = await ExecuteAsync(application);
            await AfterExecuteAsync(application);
        }
        catch (Exception ex)
        {
            result = 1;
            Error(ex);
        }
        finally
        {
            await FinallyAsync(application);
            watch.Stop();

        }
        Verbose("Completed");
        Debug($"Execution Time: {watch.Elapsed} ({watch.ElapsedMilliseconds}ms)");
        return result;
    }
    protected void Error(Exception ex)
    {
        _logger.LogError(ex.Message, ex.StackTrace, ex);
    }
    protected void Debug(string template, params object?[] parameters)
    {
        _logger.LogDebug(template, parameters);
    }
    protected void Verbose(string template, params object?[] parameters)
    {
        if (VerboseFlag)
        {
            _logger.LogInformation(template, parameters);
        }
    }
}