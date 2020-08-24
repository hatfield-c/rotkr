using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WyeStateReferences : MonoBehaviour
{
    public GameObject WaterPlane;
    public WyeSinker WyeSinker;
    public EnemyFactory EnemyFactory;
    public List<Transform> SpawnPoints;
    public List<EndGate> EndGates;
    public RepairMenu RepairMenu;
    public RectTransform RatHealthGroup;
    public TextMeshProUGUI ScrapDisplay;
}
