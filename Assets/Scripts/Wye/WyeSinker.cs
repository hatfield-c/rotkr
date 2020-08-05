﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WyeSinker : MonoBehaviour
{
    public Action WyeCompletelySunk;
    bool sunk;
    public float sinkAmount;
    public float sinkTolerance = 1f;
    public float sinkTime;

    protected float startPosition;
    protected float endPosition;
    protected float progress;
    protected float sinkRate;

    protected Vector3 position;

    void Start() {
        this.startPosition = this.transform.position.y;
        this.endPosition = this.startPosition - sinkAmount;
        this.progress = 0;
        this.sinkRate = 1 / this.sinkTime;
    }

    void Update(){
        if (!sunk){
            this.position.x = this.transform.position.x;
            this.position.y = Mathf.Lerp(this.startPosition, this.endPosition, this.progress);
            this.position.z = this.transform.position.z;

            this.transform.position = this.position;

            this.progress += this.sinkRate * Time.deltaTime;

            // Check to see if we're sunk
            if(this.position.y - this.endPosition <= sinkTolerance){
                WyeCompletelySunk?.Invoke();
                sunk = true;
            }
        }
        else
            return;
    }
}