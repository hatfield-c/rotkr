using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActorHealthManager
{
    public Action DeathAction;
    public float DeathDelay = 20f;
    [SerializeField] float DeathPercent = 100f;

    protected int maxHunks;
    protected int hunkCount;
    protected bool dead = false;

    public void Init(int hunkCount){
        this.maxHunks = hunkCount;
        this.hunkCount = hunkCount;
    }

    public void OnHunkBreak(){
        if(this.dead){
            return;
        }

        this.hunkCount--;

        if(this.GetHunkPercent() <= (this.DeathPercent / 100)){
            this.dead = true;
            this.DeathAction?.Invoke();
        }
    }

    public void ResetHealth(){
        this.dead = false;
        this.hunkCount = this.maxHunks;
    }

    public float GetHunkPercent(){
        return (float)this.hunkCount / (float)this.maxHunks;
    }

    public bool IsDead(){
        return this.dead;
    }

}
