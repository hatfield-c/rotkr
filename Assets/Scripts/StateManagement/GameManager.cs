using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    #region state variables
    int wyesCompleted = 0;
    #endregion

    #region references
    [SerializeField] GameObject playerPrefab;
    [SerializeField] MainMenuUI mainMenuUI = null;
    MainMenuState mainMenuState;
    IGameState currentState;

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
        if(playerShipMovement != null)
        {
            ship = playerShipMovement.GetComponent<ShipManager>();
        }
        else
        {
            ship = null;
        }

        // now that the scene is loaded, execute our current state
        currentState.Execute();
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
        }

        // General setup
        inputRunner = new InputRunner();

        // Setup our initial state
        mainMenuState = new MainMenuState(mainMenuUI);
        mainMenuState.ExecuteComplete = () =>
        {
            switch (mainMenuState.ChosenGameEntryPoint())
            {
                case MainMenuState.GameEntryPoint.NewGame:
                    LoadWye();
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
    }


    void Start()
    {

    }

    void Update() { }
    #endregion

    #region private functions
    void ChangeState(IGameState state)
    {
        Debug.Log($"<color=red>Changed from {currentState} to {state}.</color>");
        currentState = state;
    }
    void LoadWye(TypeOfWye chosenWyeType = TypeOfWye.None)
    {
        WyeState wye;
        
        if (chosenWyeType == TypeOfWye.None)
        {
            chosenWyeType = TypeOfWye.Spillway;
            if (Random.Range(0f, 1f) > 0.5f)
                chosenWyeType = TypeOfWye.CollectionChamber;
            wye = new WyeState(new WyeData { WyeType = chosenWyeType }, playerPrefab, inputRunner.controls);
        }
        else
        {
            wye = new WyeState(new WyeData { WyeType = chosenWyeType }, playerPrefab, inputRunner.controls);
        }
        
        wye.ExecuteComplete = () => 
        {
            wyesCompleted += 1;
            LoadWye();
        };
        ChangeState(wye);
        levelLoader.QueueLevel(chosenWyeType);
        levelLoader.Transition();
    }
    #endregion
}