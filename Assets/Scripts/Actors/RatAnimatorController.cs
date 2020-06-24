using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAnimatorController : MonoBehaviour
{
    #region references
    [SerializeField] Animator animator = null;
    [SerializeField] CapsuleCollider ratCollider = null;
    #endregion

    #region state variables
    public float SteerValue = 0.5f;
    public bool Grounded = false;
    public bool Swimming = false;
    public float feetOffset = 0f;
    #endregion

    public enum RatAnimationMode
    {
        Default = 0,
        Steering = 1,
        Repairing = 2,
        Swimming = 3,
        Falling = 4,
        Drowned = 5
    };

    public RatAnimationMode AnimationMode;
    void Start()
    {
        ChangeAnimationMode(RatAnimationMode.Default);
    }

    private void FixedUpdate()
    {
        bool wasGrounded = Grounded;
        Grounded = isGrounded();
        Swimming = isSwimming();

        if (Grounded)
        {
            //Don't interrupt a task if you were already grounded
            if(wasGrounded != Grounded)
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

        //TODO: Detect if we're falling

        //TODO: Detect if we're in water
    }

    public void ChangeAnimationMode(RatAnimationMode mode)
    {
        if(mode == AnimationMode) { return; }
        int toInteger = (int)AnimationMode;
        animator.SetInteger("animationMode", toInteger);
    }

    void UpdateSteerValue(float value)
    {
        animator.SetFloat("steerValue", value);
    }
    bool isGrounded()
    {
        float checkDistance = 3f;
        //Physics.BoxCast(ratCollider.center, 
        return true;
    }
    bool isSwimming()
    {
        return false;
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
    private void OnDrawGizmos()
    {
        float maxDistance = 10f;
        RaycastHit hit;

        Vector3 position = ratCollider.center;
        Vector3 direction = -transform.up;

        bool isHit = Physics.SphereCast(position, ratCollider.radius, direction, out hit,
            maxDistance);

        if (isHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(position, direction * hit.distance);
            Gizmos.DrawWireSphere(position + direction * hit.distance, transform.lossyScale.x/2);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(position, direction * maxDistance);
        }
    }
}
