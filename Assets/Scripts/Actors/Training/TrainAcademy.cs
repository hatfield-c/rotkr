using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainAcademy : MonoBehaviour
{
    [Header("References")]
    public WyeSinker Sinker;
    public Transform SpawnPoints;
    public GameObject WaterLevel;
    public Warehouse Warehouse;
    public TargetShip TargetPrefab;
    public ShipAgentTrain AgentPrefab;

    public RewardParameters RewardParameters;

    [Header("Parameters")]
    public float spawnScale = 0.05f;

    protected ShipAgentTrain Agent;
    protected TargetShip Target;
    protected float framePunish;
    protected float minDistPunish;

    public void ResetAcademy(){
        Debug.Log("Academy reset.");
        this.SpawnPoints.parent = this.transform;
        this.SpawnPoints.localScale = Vector3.one;
        this.SpawnPoints.parent = null;
        this.Sinker.Reset();
 
        List<Vector3> spawnPoints = this.AvailablePoints();
     
        this.Agent.transform.position = this.ChooseSpawnPoint(spawnPoints);
        this.Agent.ResetAgent();   

        this.Target.transform.position = this.ChooseSpawnPoint(spawnPoints);
        this.Target.Reset();
    }
  
    void Start(){
        this.RewardParameters.Init();
        this.framePunish = (RewardParameters.PUNISH_Frame / this.Sinker.GetSinkTime()) * Time.fixedDeltaTime;

        Debug.Log("Academy start.");
        this.Agent = Instantiate(this.AgentPrefab);
        this.Target = Instantiate(this.TargetPrefab);

        this.Agent.shipManager.Init(
            this.Warehouse,
            this.Target.gameObject,
            this.WaterLevel,
            EnemyFactory.GameDifficulty.Easy,
            this.EmptyStore
        );
        this.Agent.minDistPunish = (RewardParameters.PUNISH_MinDistance / this.Sinker.GetSinkTime()) * Time.fixedDeltaTime;
        this.Agent.maxDistPunish = (RewardParameters.PUNISH_MaxDistance / this.Sinker.GetSinkTime()) * Time.fixedDeltaTime;
        
        this.Target.Init(
            this.Agent,
            this.WaterLevel,
            this.SpawnPoints,
            new List<Collider>()
        );

        this.Sinker.WyeCompletelySunk += this.EndEpisode;
        this.ResetAcademy();
    }

    void FixedUpdate(){
        float scaleLerp = Mathf.Lerp(1f, this.spawnScale, this.Sinker.GetProgress());
        this.SpawnPoints.localScale = scaleLerp * Vector3.one;

        this.Agent.AddReward(this.framePunish);
    }

    protected void EndEpisode(){
        Debug.Log("Episode End");
        this.Agent.EndEpisode();
        this.ResetAcademy();
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
