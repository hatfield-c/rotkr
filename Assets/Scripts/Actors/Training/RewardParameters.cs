using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RewardParameters
{
    [Header("Rewards")]
    public float HitHunk;
    public float BreakHunk;
    public float HitRat;
    public float BreakRat;

    [Header("Punishments")]
    public float Frame;
    public float Rotation;
    public float MaxDistance;
    public float MinDistance;
    public float TerrainCollide;
    public float PlayerCollide;

    [Header("Parameters")]
    public float MinValue = 0.00001f;

    public static float REWARD_HitHunk;
    public static float REWARD_BreakHunk;
    public static float REWARD_HitRat;
    public static float REWARD_BreakRat;
    
    public static float PUNISH_Frame;
    public static float PUNISH_Rotation;
    public static float PUNISH_MaxDistance;
    public static float PUNISH_MinDistance;
    public static float PUNISH_TerrainCollide;
    public static float PUNISH_PlayerCollide;

    public static float VALUE_Min;
    public static float TIME_Length;
    public static bool INITIALIZED = false;

    public void Init(float timeLength){
        if(INITIALIZED){
            return;
        }

        TIME_Length = timeLength;
        VALUE_Min = this.MinValue;

        REWARD_HitHunk = this.HitHunk / 100;
        REWARD_BreakHunk = this.BreakHunk / 100;
        REWARD_HitRat = this.HitRat / 100;
        REWARD_BreakRat = this.BreakRat / 100;

        PUNISH_Frame = ((-this.Frame / 100) / TIME_Length) * Time.fixedDeltaTime;
        PUNISH_Rotation = ((-this.Rotation / 100) / TIME_Length) * Time.fixedDeltaTime;
        PUNISH_MaxDistance = ((-this.MaxDistance / 100) / TIME_Length) * Time.fixedDeltaTime;
        PUNISH_MinDistance = ((-this.MinDistance / 100) / TIME_Length) * Time.fixedDeltaTime;
        PUNISH_TerrainCollide = -this.TerrainCollide / 100;
        PUNISH_PlayerCollide = -this.PlayerCollide / 100;

        INITIALIZED = true;
    }

    public static float GetTerrainPunishment(float t){
        return Mathf.Lerp(PUNISH_TerrainCollide, VALUE_Min, t / TIME_Length);
    }

    public static float GetPlayerPunishment(float t){
        return Mathf.Lerp(PUNISH_PlayerCollide, VALUE_Min, t / TIME_Length);
    }
}
