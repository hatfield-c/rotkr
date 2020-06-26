using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RatHealthSystem : MonoBehaviour, IDamageable, IRepairable
{
    #region variables
    [SerializeField] float health = 100f;
    public float Health => health;
    // How many seconds the rat needs to drown for to TakeDamage
    public float DrowningPeriod = 1f;
    // How much damage the rat takes every drowning cycle
    public float DrowningDamage = 10f;
    Sequence currentSequence = null;
    #endregion

    #region logic
    void Start()
    {
    }
    void Update()
    {

    }
    #endregion

    #region public functions
    public void TakeDamage(float value)
    {
        health -= value;
        //TODO: health death logic
    }
    public void Repair(float value)
    {
        health -= value;
    }
    public float GetHealth()
    {
        return health;
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
        health = value;
    }
    #endregion
}
