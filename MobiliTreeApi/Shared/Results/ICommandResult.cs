namespace MobiliTreeApi.Shared.Results;

/// <summary>
/// Command result interface for event-sourced operations.
/// </summary>
public interface ICommandResult
{
    bool UnitOfWorkShouldComplete { get; }

    static ICommandResult Done => new CommandDone();
    static ICommandResult WasAlreadyDone => new CommandWasAlreadyDone();
}

public class CommandDone : ICommandResult
{
    public bool UnitOfWorkShouldComplete => true;
}

public class CommandWasAlreadyDone : ICommandResult
{
    public bool UnitOfWorkShouldComplete => false;
}

public class CommandFailed(string errorMessage) : ICommandResult
{
    public bool UnitOfWorkShouldComplete => false;
    public string ErrorMessage { get; set; } = errorMessage;
}
