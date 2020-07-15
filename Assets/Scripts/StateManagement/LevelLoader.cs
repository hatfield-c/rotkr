using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{
    int queuedSceneIndex;

    public void LoadQueuedLevel()
    {
        SceneManager.LoadScene(queuedSceneIndex);
    }
    public void QueueLevel(int index)
    {
        queuedSceneIndex = index;
    }
    public void QueueLevel(TypeOfWye wyeType)
    {
        switch (wyeType)
        {
            case TypeOfWye.None:
                break;
            case TypeOfWye.CollectionChamber:
                queuedSceneIndex = 1;
                break;
            case TypeOfWye.Spillway:
                queuedSceneIndex = 2;
                break;
            default:
                break;
        }
    }
}
