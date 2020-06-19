using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAnimatorController : MonoBehaviour
{
    [SerializeField] Animator animator;
    public float steerValue = 0.5f;
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
                UpdateSteerValue(steerValue);
                break;
            case RatAnimationMode.Default:
                break;
        }
    }

    public void ChangeAnimationMode(RatAnimationMode mode)
    {
        int toInteger = (int)AnimationMode;
        animator.SetInteger("animationMode", toInteger);
    }

    void UpdateSteerValue(float value)
    {
        animator.SetFloat("steerValue", value);
    }
}
