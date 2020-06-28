using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameState
{
    void Execute();
    void Cancel();

}
public abstract class AGameState : IGameState
{
    public Action ExecuteComplete;
    public Action CancelComplete;

    public abstract void Execute();
    public abstract void Cancel();
}
