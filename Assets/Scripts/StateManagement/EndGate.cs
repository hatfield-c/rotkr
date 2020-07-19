using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGate : MonoBehaviour
{
    public Action PlayerEnteredTheEnd;
    void OnTriggerEnter(Collider other)
    {
        ShipManager ship;
        ship = other.GetComponentInParent<ShipManager>();
        if(ship != null)
        {
            PlayerEnteredTheEnd?.Invoke();
        }
    }
    void OnDisable()
    {
        PlayerEnteredTheEnd = null;
    }
}
