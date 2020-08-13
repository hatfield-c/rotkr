using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

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

    #region variables
    RatData data;
    public Action<float, float, float> ChangedHealth;
    public Action<RatAnimationMode> ChangedStatus;
    #endregion

    #region references
    [SerializeField] RatDeckGrabber deckGrabber = null;
    [SerializeField] RatGroundChecker groundChecker = null;
    [SerializeField] OverboardSwimmer overboardSwimmer = null;
    [SerializeField] RatReferences ratReferences = null;
    [SerializeField] Animator animator = null;
    [SerializeField] RatHealthSystem healthSystem = null;
    [SerializeField] BuoyancyManager buoyancyManager = null;
    public RatAnimationMode AnimationMode;
    #endregion

    #region DEBUG
    public ShipReferences DEBUG_ShipReferences;
    public bool DEBUG = false;
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
        healthSystem.Death += OnDeath;
        healthSystem.Life += OnLife;
        healthSystem.ChangedHealth += OnChangedHealth;
        buoyancyManager.UnderWater += OnUnderWater;
        buoyancyManager.AboveWater += OnAboveWater;
        deckGrabber.DetachedAction += overboardSwimmer.OverboardActivate;
        overboardSwimmer.Rescued += deckGrabber.PlaceOnShip;
    }
    void OnDisable()
    {
        healthSystem.Death -= OnDeath;
        healthSystem.Life -= OnLife;
        healthSystem.ChangedHealth -= OnChangedHealth;
        buoyancyManager.UnderWater -= OnUnderWater;
        buoyancyManager.AboveWater -= OnAboveWater;
        deckGrabber.DetachedAction -= overboardSwimmer.OverboardActivate;
        overboardSwimmer.Rescued -= deckGrabber.PlaceOnShip;
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
    void OnAboveWater()
    {
        healthSystem.StopDrowning();
        Swimming = false;
    }
    void OnChangedHealth(float oldHealth, float newHealth, float maxHealth)
    {
        data.CurrentHealth = newHealth;
        ChangedHealth?.Invoke(oldHealth, newHealth, maxHealth);
    }
    #endregion

    #region logic

    public void Init(RatData data, ShipReferences shipReferences, GameObject waterPlane)
    {
        this.data = data;
        groundChecker.Init(shipReferences, ratReferences);
        deckGrabber.Init(shipReferences, ratReferences);
        overboardSwimmer.Init(shipReferences, ratReferences, deckGrabber.IsAttached);
        buoyancyManager.Init(waterPlane);
        healthSystem.Init(data);
    }

    void Start()
    {
        IsAlive = healthSystem.IsAlive();

        if(DEBUG)
        {
            Init(new RatData(100, 100, "Jerry"), DEBUG_ShipReferences, FindObjectOfType<WaterCalculator>().gameObject);
        }
    }

    void FixedUpdate()
    {
        GroundData = groundChecker.GetGroundData();
        deckGrabber.UpdateState(GroundData);


        //bool wasGrounded = Grounded;
        Grounded = GroundData.IsGrounded();
        if (IsAlive)
        {
            if (Grounded)
            {
                // Don't interrupt a task if you were already grounded.
                //if (wasGrounded != Grounded)
                ChangeAnimationMode(RatAnimationMode.Default);
            }
            else
            {
                if (Swimming)
                    ChangeAnimationMode(RatAnimationMode.Swimming);
                else
                    ChangeAnimationMode(RatAnimationMode.Falling);
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
        ChangeStatus(AnimationMode, mode);
        int toInteger = (int)mode;
        animator.SetInteger("animationMode", toInteger);
        AnimationMode = mode;

    }
    public void GoRepairThisPlace(Vector3 pos)
    {
        //TODO: 
    }
    public void GoSteerTheShip(Vector3 pos)
    {
        //TODO:
        Debug.Log("Look-look at me. I am the captain now.");
    }
    #endregion

    #region private
    void ChangeStatus(RatAnimationMode oldMode, RatAnimationMode newMode)
    {
        //if((int)oldMode != (int)newMode)
        //    ChangedStatus?.Invoke(newMode);
    }
    void UpdateSteerValue(float value)
    {
        animator.SetFloat("steerValue", value);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        deckGrabber.CollisionCheck(collision);
    }

    void OnTriggerEnter(Collider collider)
    {
        overboardSwimmer.TriggerActivate(collider);
    }
    #endregion

    void OnDrawGizmos()
    {
        RaycastHit hit;

        Vector3 position = this.transform.TransformPoint(this.ratReferences.ShipCollider.center);
        Vector3 direction = -this.transform.up;

        bool grounded = Physics.SphereCast(
            this.transform.TransformPoint(this.ratReferences.ShipCollider.center), 
            this.ratReferences.ShipCollider.radius / 2,
            -this.transform.up, 
            out hit,
            this.groundChecker.maxDistance
        );

        if (grounded)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(position, direction * hit.distance);
            Gizmos.DrawWireSphere(position + direction * hit.distance, this.ratReferences.ShipCollider.radius);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(position, direction * this.groundChecker.maxDistance);
            Gizmos.DrawWireSphere(position + direction * this.groundChecker.maxDistance, this.ratReferences.ShipCollider.radius);
        }
    }
}
