using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Point of contact for <see cref="ShipManager"/> to control the rats depending on the context. Handles management of the group of <see cref="RatStateManager"/>s.
/// </summary>
public class RatGroupManager : MonoBehaviour
{
    [SerializeField] GameObject ratPrefab = null;
    [SerializeField] Transform ratSpawnPoint = null;
    [SerializeField] Transform steeragePoint = null;
    [SerializeField] Transform harpoonPoint = null;
    [SerializeField] List<Transform> cannonPoints = null;
    [SerializeField] GameObject ratStatusNodePrefab = null;
    RectTransform ratHealthGroup = null;

    List<RatStateManager> rats = null;
    public int NewCurrentRatCount = 1;
    public int NewRatHealth = 100;
    int maxRatCount;
    Action allDeadCallback = null;
    void Start()
    {
    }

    void Update()
    {
    }

    #region public functions
    public void Init(ShipData shipData, ShipReferences shipReferences, GameObject waterPlane, RectTransform ratHealthGroup, Action allDeadCallback = null)
    {
        this.ratHealthGroup = ratHealthGroup;
        this.allDeadCallback = allDeadCallback;
        maxRatCount = shipData.MaxRatCount;
        NewCurrentRatCount = (NewCurrentRatCount > maxRatCount) ? maxRatCount : NewCurrentRatCount;
        rats = new List<RatStateManager>();
        // Spawn the rats from the shipData. If the ship didn't have any rats, make new ones.
        if (shipData.RatDatum.Count > 0)
        {
            foreach (RatData data in shipData.RatDatum)
            {
                RatStateManager rat = SpawnRat(data, shipReferences, waterPlane);
                rats.Add(rat);
                CreateRatStatusNode(data, rat);
            }
        }
        else
        {
            for(int i = 0; i < NewCurrentRatCount; i++)
            {
                RatData data = new RatData(NewRatHealth, NewRatHealth, ("Joe " + i));
                shipData.RatDatum.Add(data);
                RatStateManager rat = SpawnRat(data, shipReferences, waterPlane);
                rats.Add(rat);
                CreateRatStatusNode(data, rat);
            }
        }

        foreach(RatStateManager rat in rats)
        {
            rat.Death += OnDeath;
        }
        
    }
    #endregion

    #region private functions
    RatStateManager SpawnRat(RatData data, ShipReferences shipReferences,  GameObject waterPlane)
    {
        GameObject GO = Instantiate(ratPrefab, ratSpawnPoint.position, Quaternion.identity);
        RatStateManager rat = GO.GetComponent<RatStateManager>();
        if (rat == null)
        {
            Debug.LogError("There is no RatStateManager on the prefab! Resolve this");
            return GO.AddComponent<RatStateManager>();
        }
        else
        {
            rat.Init(data, shipReferences, waterPlane);
            return rat;
        }

    }
    void TeleportRat(RatStateManager rat, Vector3 targetPosition)
    {
        rat.transform.position = targetPosition;
    }

    void CreateRatStatusNode(RatData data, RatStateManager rat)
    {
        if (ratHealthGroup == null)
            return;
        GameObject GO = Instantiate(ratStatusNodePrefab, ratHealthGroup);
        GO.GetComponent<RatStatusNode>().Init(data, rat);
    }

    void OnDeath()
    {
        if (!AreAnyRatsAlive(rats))
            allDeadCallback?.Invoke();
    }
    bool AreAnyRatsAlive(List<RatStateManager> rats)
    {
        foreach(RatStateManager rat in rats)
        {
            if (rat.IsAlive)
                return true;
        }
        return false;
    }
    #endregion

}
