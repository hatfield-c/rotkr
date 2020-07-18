using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelLoader : MonoBehaviour
{
    int queuedSceneIndex;

    public Animator transition;
    public float transitionDelay = .05f;
    public void Transition()
    {
        transition.SetTrigger("Start");
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
    public void LoadQueuedLevel()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.InsertCallback(transitionDelay, () => { SceneManager.LoadScene(queuedSceneIndex); });
    }
}
