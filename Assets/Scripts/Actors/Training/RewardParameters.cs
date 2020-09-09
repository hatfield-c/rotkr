using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RewardParameters
{
    [Header("Rewards")]
    public float Proximity;
    public float EndProximity;
    //public float Velocity;
    //public float Movement;

    [Header("Punishments")]
    //public float PlayerCollide;
    public float TerrainCollide;
    //public float TooClose;
    public float Inaction;
    public float Dizzy;

    [Header("Parameters")]

    public static float REWARD_Proximity;
    public static float REWARD_EndProximity;
    //public static float REWARD_Velocity;
    //public static float REWARD_Movement;

    //public static float PUNISH_PlayerCollide;
    public static float PUNISH_TerrainCollide;
    //public static float PUNISH_TooClose;
    public static float PUNISH_Inaction;
    public static float PUNISH_DIZZY;

    public static float TIME_Length;
    public static bool INITIALIZED = false;

    public void Init(float timeLength){
        if(INITIALIZED){
            return;
        }

        TIME_Length = timeLength;

        REWARD_Proximity = TimeScaled(this.Proximity);
        REWARD_EndProximity = this.EndProximity;

        PUNISH_TerrainCollide = -TimeScaled(this.TerrainCollide);
        PUNISH_Inaction = -TimeScaled(this.Inaction);
        PUNISH_DIZZY = -TimeScaled(this.Dizzy);

        INITIALIZED = true;
    }

    protected float TimeScaled(float value) {
        return (value / TIME_Length) * Time.fixedDeltaTime;
    }

}
