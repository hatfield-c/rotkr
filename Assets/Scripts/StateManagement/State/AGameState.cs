using System;

public abstract class AGameState : IGameState
{
    public Action ExecuteComplete;
    public Action CancelComplete;

    public abstract void Execute();
    public abstract void Cancel();

    public static void EndGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
          Application.Quit();
        #endif
    }
}