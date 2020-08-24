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
    public float MaxDistance;
    public float MinDistance;
    public float TerrainCollide;
    public float PlayerCollide;

    public static float REWARD_HitHunk;
    public static float REWARD_BreakHunk;
    public static float REWARD_HitRat;
    public static float REWARD_BreakRat;
    
    public static float PUNISH_Frame;
    public static float PUNISH_MaxDistance;
    public static float PUNISH_MinDistance;
    public static float PUNISH_TerrainCollide;
    public static float PUNISH_PlayerCollide;

    public void Init(){
        REWARD_HitHunk = this.HitHunk / 100;
        REWARD_BreakHunk = this.BreakHunk / 100;
        REWARD_HitRat = this.HitRat / 100;
        REWARD_BreakRat = this.BreakRat / 100;

        PUNISH_Frame = -this.Frame / 100;
        PUNISH_MaxDistance = -this.MaxDistance / 100;
        PUNISH_MinDistance = -this.MinDistance / 100;
        PUNISH_TerrainCollide = -this.TerrainCollide / 100;
        PUNISH_PlayerCollide = -this.PlayerCollide / 100;
    }
}
