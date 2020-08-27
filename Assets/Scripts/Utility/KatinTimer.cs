using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatinTimer
{
    protected float length;
    protected float elapsedTime;
    protected bool active = false;

    public delegate void Callback();
    protected Callback callback;

    public KatinTimer(){ }

    public void Init(float length, Callback callback){
        this.length = length;
        this.callback = callback;
        this.elapsedTime = 0;
        this.active = true;
    }

    public void update(){
        if(!this.active){
            return;
        }

        this.elapsedTime += Time.deltaTime;

        if(this.elapsedTime >= this.length){
            this.alarm();
        }
    }

    public void alarm(){
        this.active = false;
        this.elapsedTime = 0;
        this.length = 0;
        this.callback();
        this.callback = this.empty;
    }

    public void disable() {
        this.active = false;
        this.elapsedTime = 0;
        this.length = 0;
        this.callback = this.empty;
    }

    public void empty(){ }
}
