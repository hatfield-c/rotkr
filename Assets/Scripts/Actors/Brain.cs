using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Brain
{
    [SerializeField] EnemyFactory.GameDifficulty Difficulty;
    public NNBehaviour PatrolBehavior = new NNBehaviour();
    public NNBehaviour CombatBehavior = new NNBehaviour();
}
