using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Brain
{
    [SerializeField] Director.Difficulty Difficulty;
    public NNBehaviour PatrolBehavior = new NNBehaviour();
    public NNBehaviour CombatBehavior = new NNBehaviour();
}
