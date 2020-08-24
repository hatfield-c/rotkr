using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Brain
{
    [SerializeField] EnemyFactory.GameDifficulty Difficulty = EnemyFactory.GameDifficulty.Easy;
    public NNBehaviour PatrolBehavior = new NNBehaviour();
    public NNBehaviour CombatBehavior = new NNBehaviour();

    public EnemyFactory.GameDifficulty GetDifficulty(){
        return this.Difficulty;
    }
}
