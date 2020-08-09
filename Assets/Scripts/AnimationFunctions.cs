using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctions : MonoBehaviour
{
    [SerializeField] LevelLoader levelLoader = null;

    public void SignalLoadLevel()
    {
        levelLoader.LoadQueuedLevel();
    }
}
