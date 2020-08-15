using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParameters
{
    public GameObject WaterPlane;
    public EnemyFactory.Difficulty Difficulty;

    public SpawnParameters(GameObject WaterPlane, EnemyFactory.Difficulty Difficulty){
        this.WaterPlane = WaterPlane;
        this.Difficulty = Difficulty;
    }
}
