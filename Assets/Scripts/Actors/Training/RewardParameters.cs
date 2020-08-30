using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RewardParameters
{
    [Header("Rewards")]
    public float HitHunk;
    public float HitRat;
    public float Proximity;
    public float Aimed;

    [Header("Punishments")]
    public float Frame;
    public float MinDistance;
    public float TerrainCollide;

    [Header("Parameters")]

    public static float REWARD_HitHunk;
    public static float REWARD_HitRat;
    public static float REWARD_Proximity;
    public static float REWARD_Aimed;
    
    public static float PUNISH_Frame;
    public static float PUNISH_Rotation;
    public static float PUNISH_MinDistance;
    public static float PUNISH_TerrainCollide;

    public static float TIME_Length;
    public static bool INITIALIZED = false;

    public void Init(float timeLength){
        if(INITIALIZED){
            return;
        }

        TIME_Length = timeLength;

        REWARD_HitHunk = this.HitHunk / 100;
        REWARD_HitRat = this.HitRat / 100;
        REWARD_Proximity = this.TimeScaled(this.Proximity);
        REWARD_Aimed = this.TimeScaled(this.Aimed);

        PUNISH_Frame = this.TimeScaled(-this.Frame);
        PUNISH_MinDistance = -this.MinDistance / 100;
        PUNISH_TerrainCollide = -this.TerrainCollide / 100;

        INITIALIZED = true;
    }

    protected float TimeScaled(float value) {
        return ((value / 100) / TIME_Length) * Time.fixedDeltaTime;
    }

}
