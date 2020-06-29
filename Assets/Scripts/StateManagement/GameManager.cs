using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region references
    MainMenuState mainMenuState;
    IGameState currentState;
    #endregion

    #region state variables
    int wyesCompleted = 0;
    #endregion
    void Start()
    {
        mainMenuState = new MainMenuState();
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
        ChangeState(mainMenuState);
        currentState.Execute();
    }

    void Update()
    {
        
    }
    void ChangeState(IGameState state)
    {
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
}