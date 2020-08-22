using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Academy : MonoBehaviour
{
    [Header("References")]
    public WyeSinker Sinker;
    public Transform SpawnPoints;
    public GameObject WaterLevel;
    public Warehouse Warehouse;
    public TargetShip TargetPrefab;
    public ShipAgentTrain AgentPrefab;


    [Header("Parameters")]
    public float spawnScale = 0.05f;

    protected ShipAgentTrain Agent;
    protected TargetShip Target;

    public void Reset(){
        List<Vector3> spawnPoints = this.AvailablePoints();

        Destroy(this.Agent);
        this.Agent = Instantiate(this.AgentPrefab);
        this.Agent.transform.position = this.ChooseSpawnPoint(spawnPoints);
        this.Agent.shipManager.Init(
            this.Warehouse,
            this.WaterLevel,
            EnemyFactory.GameDifficulty.Easy,
            this.EmptyStore
        );

        Destroy(this.Target);
        this.Target = Instantiate(this.TargetPrefab);
        this.Target.transform.position = this.ChooseSpawnPoint(spawnPoints);
        this.Target.Init(
            this.Agent, 
            this.WaterLevel,
            this.SpawnPoints,
            new List<Collider>()
        );

        this.SpawnPoints.parent = this.transform;
        this.SpawnPoints.localScale = Vector3.one;

        this.SpawnPoints.parent = null;
        this.Sinker.Reset();
    }

    void Start(){
        this.Reset();
    }

    void FixedUpdate(){
        float scaleLerp = Mathf.Lerp(1f, this.spawnScale, this.Sinker.GetProgress());
        //this.SpawnPoints.localScale = scaleLerp * Vector3.one;
    }

    void OnEnable(){
        this.Sinker.WyeCompletelySunk += this.Reset;
    }

    void OnDisable(){
        this.Sinker.WyeCompletelySunk -= this.Reset;
    }

    protected List<Vector3> AvailablePoints(){
        List<Vector3> points = new List<Vector3>();

        foreach(Transform point in this.SpawnPoints){
            points.Add(point.position);
        }

        return points;
    }

    protected Vector3 ChooseSpawnPoint(List<Vector3> points){

        if(points == null){
            int pointIndex = Random.Range(0, this.SpawnPoints.childCount);
            return this.SpawnPoints.GetChild(pointIndex).position;
        }

        int listIndex = Random.Range(0, points.Count);
        Vector3 point = points[listIndex];
        points.RemoveAt(listIndex);

        return point;
    }

    protected void EmptyStore(IStorable iStorable){}

}
