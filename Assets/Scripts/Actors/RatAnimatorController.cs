using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float feetOffset = 0f;
    public float maxDistance = 10f;
    #endregion


    #region blackboard variables
    RaycastHit hit;
    Vector3 position;
    Vector3 direction;
    bool isHit;
    #endregion



    #region logic
    void Start()
    {
        //ChangeAnimationMode(RatAnimationMode.Default);
    }

    private void FixedUpdate()
    {
        bool wasGrounded = Grounded;
        Grounded = isGrounded();
        Swimming = isSwimming();

        if (Grounded)
        {
            Debug.Log("1");
            //Don't interrupt a task if you were already grounded
            if (wasGrounded != Grounded)
            {
                Debug.Log("2");
                ChangeAnimationMode(RatAnimationMode.Default);
            }
            else
            {
                Debug.Log("3");
            }
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
    void Update()
    {
        switch (AnimationMode)
        {
            case RatAnimationMode.Falling:
                break;
            case RatAnimationMode.Swimming:
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
        AnimationMode = mode;
        animator.SetInteger("animationMode", toInteger);
        if (mode == RatAnimationMode.Swimming)
        {
            // Start the drowned timer
        }
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
        return false;
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
