using System;

public abstract class AGameState : IGameState
{
    public Action ExecuteComplete;
    public Action CancelComplete;

    public abstract void Execute();
    public abstract void Cancel();
}