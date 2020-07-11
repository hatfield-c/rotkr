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
    [SerializeField] MainMenuUI MainMenuUI = null;
    MainMenuState mainMenuState;
    IGameState currentState;

    // Singleton references
    public LevelLoader levelLoader;
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
    }
    #endregion

    #region logic
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }        
    }


    void Start()
    {
        mainMenuState = new MainMenuState(MainMenuUI);
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
        currentState.Execute();
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
            TypeOfWye randomWyeType = TypeOfWye.Spillway;
            if (Random.Range(0f, 1f) > 0.5f)
                randomWyeType = TypeOfWye.CollectionChamber;
            wye = new WyeState(new WyeData { WyeType = randomWyeType });
        }
        else
        {
            wye = new WyeState(new WyeData { WyeType = chosenWyeType });
        }
        
        wye.ExecuteComplete = () => 
        {
            wyesCompleted += 1;
            LoadWye();
        };
        ChangeState(wye);
        currentState.Execute();
    }
    #endregion
}