using System.Collections;
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
    [SerializeField] RatDeckGrabber DeckGrabber = null;
    [SerializeField] RatGroundChecker GroundChecker = null;
    [SerializeField] RatReferences RatReferences = null;
    [SerializeField] Animator animator = null;
    [SerializeField] RatHealthSystem healthSystem = null;
    [SerializeField] BuoyancyManager buoyancyManager = null;
    public RatAnimationMode AnimationMode;
    #endregion

    #region DEBUG
    public ShipReferences DEBUG_ShipReferences;
    #endregion

    #region state variables
    public LayerMask LayerMask;
    public GroundData GroundData;
    public float SteerValue = 0.5f;
    public bool Grounded = false;
    public bool Swimming = false;
    public bool IsAlive = false;
    public float feetOffset = 0f;
    #endregion

    #region handlers
    void OnEnable()
    {   
        if(this.DEBUG_ShipReferences.ShipObject != null){
            this.Init(this.DEBUG_ShipReferences);
        }

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

    public void Init(ShipReferences shipReferences){
        this.GroundChecker.Init(this.RatReferences);
        this.DeckGrabber.Init(shipReferences, this.RatReferences);
    }

    void Start()
    {
        IsAlive = healthSystem.IsAlive();
    }

    void FixedUpdate()
    {
        this.GroundData = this.GroundChecker.GetGroundData();
        this.DeckGrabber.UpdateState(this.GroundData);

        bool wasGrounded = this.Grounded;
        this.Grounded = this.GroundData.IsGrounded();
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
    
    void OnCollisionEnter(Collision collision){
        this.DeckGrabber.CollisionCheck(collision);
    }
    //bool isSwimming()
    //{
    //    return true;
    //}
    #endregion
}
