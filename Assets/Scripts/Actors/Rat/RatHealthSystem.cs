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
    #endregion

    #region variables
    [SerializeField] float health = 100f;
    public float Health => health;
    public float MaxHealth = 100f;
    // How many seconds the rat needs to drown for to TakeDamage
    public float DrowningPeriod = 1f;
    // How much damage the rat takes every drowning cycle
    public float DrowningDamage = 10f;
    Sequence currentSequence = null;
    #endregion

    #region logic
    void Start()
    {
        //TODO: Modify when this thing is initialized later
        Life?.Invoke();
    }
    void Update()
    {

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
        if (currentSequence != null)
            return;
        Sequence sequence = DOTween.Sequence();
        sequence.InsertCallback(DrowningPeriod, () =>
        {
            TakeDamage(DrowningDamage);
            Drown();
            currentSequence = null;
        });
        sequence.SetAutoKill(false);
        sequence.Pause();
        currentSequence.Kill();
        currentSequence = sequence;
        currentSequence.Play();
    }

    public void StopDrowning()
    {
        currentSequence.Kill();
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
    }
    void Die()
    {
        Death?.Invoke();
    }
    #endregion
}
