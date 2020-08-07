using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    #region variables
    public int LayerCount = 3;
    public int LayerSectionCount = 6;
    public int LayerBranchRange = 3;
    #endregion

    #region references
    [SerializeField] GameObject playerPrefab = null;
    IGameState currentState;
    Sequence currentSequence;
    GameProgressionData progression;

    InputRunner inputRunner;

    // wye references
    LevelLoader levelLoader;
    ShipManager ship;
    PlayerShipMovement playerShipMovement;
    #endregion

    #region handlers
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // update references
        levelLoader = GameObject.FindObjectOfType<LevelLoader>();
        playerShipMovement = GameObject.FindObjectOfType<PlayerShipMovement>();
        if (playerShipMovement != null)
        {
            ship = playerShipMovement.GetComponent<ShipManager>();
        }
        else
        {
            ship = null;
        }

        // now that the scene is loaded, execute our current state
        currentSequence.Kill();
        Sequence sequence = DOTween.Sequence();
        sequence.InsertCallback(.15f, () => { currentState.Execute(); });
        sequence.SetAutoKill(false);
        sequence.Pause();
        currentSequence = sequence;
        currentSequence.Play();

    }
    #endregion

    #region logic
    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // General setup
        inputRunner = new InputRunner();
        DOTween.SetTweensCapacity(200, 125);

        // Setup our initial state
        LoadMainMenu(true);
    }

    void Start() {}

    void Update() {}
    #endregion

    #region private functions
    void ChangeState(IGameState state)
    {
        Debug.Log($"<color=orange>Changed from {currentState} to {state}.</color>");
        currentState = state;
    }
    void LoadWye(WyeData data)
    {
        WyeState wye;
        wye = new WyeState(data, progression.ShipData, playerPrefab, inputRunner.controls);
        
        wye.ExecuteComplete = () => 
        {
            if (wye.GetSuccess())
            {
                if (progression.CompleteSection(wye.Data))
                {
                    LoadVictory();
                }
                else
                    LoadLayerMap(progression.LayerMapDatum[progression.CurrentLayerIndex]);
            }
            else
                LoadMainMenu();
        };
        ChangeState(wye);
        levelLoader.QueueLevel(data.WyeType);
        levelLoader.Transition();
    }

    void LoadLayerMap(LayerMapData data = null)
    {
        LayerMapState layerMap;
        if (data != null)
            layerMap = new LayerMapState(data);
        else
            layerMap = new LayerMapState(new LayerMapData(LayerSectionCount, LayerBranchRange));
        
        layerMap.ExecuteComplete = () =>
        {
            LoadWye(layerMap.ChosenWye());
        };
        ChangeState(layerMap);
        levelLoader.QueueLevel(3);
        levelLoader.Transition();
    }

    void LoadMainMenu(bool isFirstLoad = false)
    {
        MainMenuState mainMenuState = new MainMenuState();
        mainMenuState.ExecuteComplete = () =>
        {
            switch (mainMenuState.ChosenGameEntryPoint())
            {
                case MainMenuState.GameEntryPoint.NewGame:
                    // Create new game
                    progression = new GameProgressionData(LayerCount, LayerSectionCount, LayerBranchRange);

                    // Grab the first wye created from our game progression path and load that wye as the first level in New Game
                    LayerMapData layer = progression.LayerMapDatum[progression.CurrentLayerIndex];
                    LayerSectionData section = layer.LayerSectionDatum[layer.CurrentSectionIndex];
                    WyeData wye = section.WyeDatum[0];
                    LoadWye(wye);
                    break;
                case MainMenuState.GameEntryPoint.Continue:
                    break;
            }
        };
        mainMenuState.CancelComplete = () =>
        {
            Application.Quit();
        };
        ChangeState(mainMenuState);
        if (!isFirstLoad)
        {
            levelLoader.QueueLevel(0);
            levelLoader.Transition();
        }
    }
    void LoadVictory()
    {
        VictoryState victory = new VictoryState(inputRunner.controls);
        victory.ExecuteComplete = () =>
        {
            LoadMainMenu();
        };
        ChangeState(victory);
        levelLoader.QueueLevel(4);
        levelLoader.Transition();
    }
    #endregion
}