using UnityEngine;
using UnityEngine.SceneManagement;

public class LayerMapState : AGameState
{
    /// <summary>
    /// Recreate from data 
    /// </summary>
    /// <param name="data"></param>
    public LayerMapState(LayerMapData data)
    {
        this.data = data;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    ~LayerMapState()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region references
    LayerMapData data;
    LayerMapStateReferences refs;
    WyeData chosenWye;
    #endregion

    #region handlers
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        refs = Object.FindObjectOfType<LayerMapStateReferences>();
    }
    #endregion

    #region logic
    #endregion

    #region public functions
    public override void Execute()
    {
        Debug.Log("<color=orange>Execute called for LayerMapState</color>");
        // Setup
        refs.wyeNodeGroupManager.Init(data);
        refs.BTN_Go.onClick.AddListener(() =>
        {
            chosenWye = refs.wyeNodeGroupManager.GetSelectedWyeData();

            refs.BTN_Go.onClick.RemoveAllListeners();
            ExecuteComplete?.Invoke();
        });
    }
    public override void Cancel()
    {
        CancelComplete?.Invoke();
    }

    public WyeData ChosenWye()
    {
        return chosenWye;
    }
    #endregion
}