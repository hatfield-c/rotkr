using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGate : MonoBehaviour
{
    public Action PlayerEnteredTheEnd;
    bool triggered;
    void OnTriggerEnter(Collider other)
    {
        ShipManager ship;
        ship = other.GetComponentInParent<ShipManager>();
        if(ship != null)
        {
            if (!triggered)
            {
                PlayerEnteredTheEnd?.Invoke();
                triggered = true;
            }
        }
    }
    void OnDisable()
    {
        PlayerEnteredTheEnd = null;
    }
}
