﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RatStateManager : MonoBehaviour
{
    #region enums
    public enum RatAnimationMode
    {
        Default = 0,
        Steering = 1,
        Repairing = 2,
        Swimming = 3,
        Falling = 4,
        Drowned = 5
    };
    #endregion

    #region references
    [SerializeField] Animator animator = null;
    [SerializeField] RatHealthSystem healthSystem = null;
    [SerializeField] BuoyancyManager buoyancyManager = null;
    public RatAnimationMode AnimationMode;
    public RatMovement ratMovement;
    #endregion

    #region state variables
    public LayerMask LayerMask;
    public float SteerValue = 0.5f;
    public bool Grounded = false;
    public bool Swimming = false;
    public bool IsAlive = false;
    public float feetOffset = 0f;
    #endregion

    #region handlers
    void OnEnable()
    {
        healthSystem.Death += OnDeath;
        healthSystem.Life += OnLife;
        buoyancyManager.UnderWater += OnUnderWater;
    }
    void OnDisable()
    {
        healthSystem.Death -= OnDeath;
        healthSystem.Life -= OnLife;
        buoyancyManager.UnderWater -= OnUnderWater;
    }
    void OnDeath()
    {
        IsAlive = healthSystem.IsAlive();
        ChangeAnimationMode(RatAnimationMode.Drowned);
    }
    void OnLife()
    {
        IsAlive = healthSystem.IsAlive();
    }
    void OnUnderWater()
    {
        healthSystem.Drown();
        Swimming = true;
    }
    #endregion

    #region logic

    void Start()
    {
        IsAlive = healthSystem.IsAlive();
    }

    void FixedUpdate()
    {
        bool wasGrounded = Grounded;
        this.updateGrounded();
        //Swimming = isSwimming();
        if (IsAlive)
        {
            if(Swimming)
            {
                ChangeAnimationMode(RatAnimationMode.Swimming);
            }
            else
            {
                if (Grounded)
                {
                    //Don't interrupt a task if you were already grounded
                    if (wasGrounded != Grounded)
                        ChangeAnimationMode(RatAnimationMode.Default);
                }
                else
                {
                    ChangeAnimationMode(RatAnimationMode.Falling);
                }
            }
            
        }
        else
        {
            ChangeAnimationMode(RatAnimationMode.Drowned);
        }
        
    }
    void Update()
    {
        switch (AnimationMode)
        {
            case RatAnimationMode.Falling:
                break;
            case RatAnimationMode.Swimming:
                healthSystem.Drown();
                break;
            case RatAnimationMode.Repairing:
                break;
            case RatAnimationMode.Steering:
                UpdateSteerValue(SteerValue);
                break;
            case RatAnimationMode.Default:
                break;
        }
    }
    #endregion

    #region public
    public void ChangeAnimationMode(RatAnimationMode mode)
    {
        if(mode != RatAnimationMode.Swimming){
            Swimming = false;
        }
        int toInteger = (int)mode;
        
        animator.SetInteger("animationMode", toInteger);
        AnimationMode = mode;

    }
    public void GoRepairThisPlace(Vector3 pos)
    {
        //TODO: 
    }
    public void SteerTheShip(Vector3 pos)
    {
        //TODO:
        Debug.Log("Look-look at me. I am the captain now.");
    }
    #endregion

    #region private
    void UpdateSteerValue(float value)
    {
        animator.SetFloat("steerValue", value);
    }
    
    void updateGrounded()
    {        
        this.Grounded = this.ratMovement.IsGrounded();
    }
    //bool isSwimming()
    //{
    //    return true;
    //}
    #endregion
}
