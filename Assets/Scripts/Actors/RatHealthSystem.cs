using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatHealthSystem : MonoBehaviour, IDamageable, IRepairable
{
    #region variables
    [SerializeField] float health = 100f;
    public float Health => health;
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
        health += value;
    }
    public void Repair(float value)
    {
        health -= value;
    }
    public float GetHealth()
    {
        return health;
    }
    #endregion

    #region private functions
    void SetHealth(float value)
    {
        health = value;
    }


    #endregion
}
