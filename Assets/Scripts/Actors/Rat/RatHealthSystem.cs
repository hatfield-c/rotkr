using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class RatHealthSystem : MonoBehaviour, IDamageable, IRepairable
{
    #region events
    public Action Death;
    public Action Life;
    public Action<float, float, float> ChangedHealth;
    #endregion

    #region variables
    [SerializeField] float health = 100f;
    public float Health => health;
    public float MaxHealth = 100f;
    // How much damage the rat takes every drowning cycle
    public float DrowningDamage = 10f;
    bool drowning;
    #endregion

    public void Init(RatData data)
    {
        SetMaxHealth(data.MaxHealth);
        SetHealth(data.CurrentHealth);
        if (IsAlive())
            Life?.Invoke();
        else
            Death?.Invoke();
    }
    #region logic
    void Start() { }
    void FixedUpdate()
    {
        if(drowning)
        {
            TakeDamage(DrowningDamage * Time.fixedDeltaTime);
        }
    }
    #endregion

    #region public functions
    public void TakeDamage(float value)
    {
        if (health <= 0f)
        {
            Debug.Log($"health is already: {health}, returning");
            return;
        }
        SetHealth(health - value);
    }
    public void Repair(float value)
    {
        health -= value;
    }
    public float GetHealth()
    {
        return health;
    }
    public bool IsAlive()
    {
        if (health > 0f)
            return true;
        else
            return false;
    }
    public void ResetHealth()
    {
        SetHealth(MaxHealth);
    }
    public void Drown()
    {
        drowning = true;
    }

    public void StopDrowning()
    {
        drowning = false;
    }
    #endregion

    #region private functions
    void SetHealth(float value)
    {
        float oldHealth = health;
        health = (value < 0f) ? 0f : value;
        if (health == 0f)
        {
            Die();
        }
        if(oldHealth <= 0f && health > 0f)
        {
            Life?.Invoke();
        }

        ChangedHealth?.Invoke(oldHealth, health, MaxHealth);
    }
    void SetMaxHealth(float value)
    {
        MaxHealth = value;
    }
    void Die()
    {
        Death?.Invoke();
    }
    #endregion
}
