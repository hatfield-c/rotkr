using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorShip : MonoBehaviour
{
    public ActorShipManager ShipManager;
    [MinAttribute(1)]
    public int ComputationCost = 1;
    [MinAttribute(1)]
    public int ChallengeRating = 1;
}
