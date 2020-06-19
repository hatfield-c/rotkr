using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAnimatorController : MonoBehaviour
{
    [SerializeField] Animator animator;
    public float steerValue = 0.5f;
    public enum RatAnimationMode
    {
        Falling = 0,
        Swimming = 1,
        Repairing = 2,
        Steering = 3,
        Default = 4
    };

    public RatAnimationMode AnimationMode;
    void Start()
    {
        ChangeAnimationMode(RatAnimationMode.Steering);
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
        float toFloat = (float)AnimationMode;
        animator.SetFloat("animationMode", toFloat);
    }

    void UpdateSteerValue(float value)
    {
        animator.SetFloat("steerValue", value);
    }
}
