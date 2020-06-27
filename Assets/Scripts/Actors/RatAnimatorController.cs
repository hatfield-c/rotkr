using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RatAnimatorController : MonoBehaviour
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
    [SerializeField] CapsuleCollider ratCollider = null;
    [SerializeField] RatHealthSystem healthSystem = null;
    public RatAnimationMode AnimationMode;
    #endregion

    #region state variables
    public LayerMask LayerMask;
    public float SteerValue = 0.5f;
    public bool Grounded = false;
    public bool Swimming = false;
    public bool IsAlive = false;
    public float feetOffset = 0f;
    public float maxDistance = 10f;
    #endregion


    #region blackboard variables
    RaycastHit hit;
    Vector3 position;
    Vector3 direction;
    bool isHit;
    #endregion

    #region handlers
    void OnEnable()
    {
        healthSystem.Death += OnDeath;
        healthSystem.Life += OnLife;
    }
    void OnDisable()
    {
        healthSystem.Death -= OnDeath;
        healthSystem.Life -= OnLife;
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
    #endregion

    #region logic

    void Start()
    {
        IsAlive = healthSystem.IsAlive();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = Grounded;
        Grounded = isGrounded();
        Swimming = isSwimming();
        if (IsAlive)
        {
            if (Grounded)
            {
                //Don't interrupt a task if you were already grounded
                if (wasGrounded != Grounded)
                    ChangeAnimationMode(RatAnimationMode.Default);
            }
            else
            {
                if (Swimming)
                {
                    ChangeAnimationMode(RatAnimationMode.Swimming);
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
        if(mode == AnimationMode) { return; }
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
    bool isGrounded()
    {        
        position = transform.TransformPoint(ratCollider.center);
        direction = -transform.up;

        isHit = Physics.SphereCast(position, ratCollider.radius, direction, out hit,
            maxDistance);
        return isHit;
    }
    bool isSwimming()
    {
        return true;
    }
    void OnDrawGizmos()
    {
        if (isHit)
        {

            Gizmos.color = Color.red;
            Gizmos.DrawRay(position, direction * hit.distance);
            Gizmos.DrawWireSphere(position + direction * hit.distance, ratCollider.radius);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(position, direction * maxDistance);
        }
    }
    #endregion
}
