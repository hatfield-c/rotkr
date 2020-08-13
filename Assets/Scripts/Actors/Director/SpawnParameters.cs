using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParameters
{
    public GameObject WaterPlane;
    public Director.Difficulty Difficulty;

    public SpawnParameters(GameObject WaterPlane, Director.Difficulty Difficulty){
        this.WaterPlane = WaterPlane;
        this.Difficulty = Difficulty;
    }
}
