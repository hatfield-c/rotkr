using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{
    Scene queuedScene;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void LoadQueuedLevel()
    {
        SceneManager.LoadScene(queuedScene.buildIndex);
    }
    public void QueueLevel(int index)
    {
        queuedScene = SceneManager.GetSceneByBuildIndex(index);
    }
    public void QueueLevel(string scene)
    {
        queuedScene = SceneManager.GetSceneByName(scene);
    }
}
