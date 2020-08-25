using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrainAcademy : MonoBehaviour
{
    [Header("References")]
    public WyeSinker Sinker;
    public Transform SpawnPoints;
    public GameObject WaterLevel;
    public Warehouse Warehouse;
    public PermutationManager PermutationManager;
    public TextMeshPro RewardText;
    public TargetShip TargetPrefab;
    public ShipAgentTrain AgentPrefab;

    public RewardParameters RewardParameters;

    [Header("Parameters")]
    public float spawnScale = 0.05f;

    protected ShipAgentTrain Agent;
    protected TargetShip Target;
    protected float framePunish;
    protected float minDistPunish;

    private List<Vector3> spawnBuffer = new List<Vector3>();
    private List<Collider> terrainColliders = new List<Collider>();
    private Collider colliderBuffer;

    public void ResetAcademy(){
        this.SpawnPoints.parent = this.transform;
        this.SpawnPoints.localScale = Vector3.one;
        this.SpawnPoints.parent = null;
        this.Sinker.Reset();
 
        this.spawnBuffer = this.AvailablePoints();
     
        this.Agent.transform.position = this.ChooseSpawnPoint(this.spawnBuffer);
        this.Agent.ResetAgent();   

        this.PermutationManager.Reset();

        this.Target.transform.position = this.ChooseSpawnPoint(this.spawnBuffer);
        this.ExtractTerrainColliders();
        this.Target.Reset(this.terrainColliders);
    }
  
    void Start(){
        this.RewardParameters.Init();
        this.framePunish = (RewardParameters.PUNISH_Frame / this.Sinker.GetSinkTime()) * Time.fixedDeltaTime;

        this.Agent = Instantiate(this.AgentPrefab);
        this.Target = Instantiate(this.TargetPrefab);

        this.Agent.shipManager.Init(
            this.Warehouse,
            this.Target.gameObject,
            this.WaterLevel,
            EnemyFactory.GameDifficulty.Easy,
            this.EmptyStore
        );
        this.Agent.resetFunction = this.EndEpisode;
        this.Agent.minDistPunish = (RewardParameters.PUNISH_MinDistance / this.Sinker.GetSinkTime()) * Time.fixedDeltaTime;
        this.Agent.maxDistPunish = (RewardParameters.PUNISH_MaxDistance / this.Sinker.GetSinkTime()) * Time.fixedDeltaTime;
        
        this.Target.Init(
            this.Agent,
            this.WaterLevel,
            this.SpawnPoints
        );

        this.Sinker.WyeCompletelySunk += this.EndEpisode;
        this.ResetAcademy();
    }

    void FixedUpdate(){
        float scaleLerp = Mathf.Lerp(1f, this.spawnScale, this.Sinker.GetProgress());
        this.SpawnPoints.localScale = scaleLerp * Vector3.one;

        this.Agent.AddReward(this.framePunish);
        this.RewardText.text = this.Agent.GetCumulativeReward().ToString();
    }

    protected void EndEpisode(){
        this.Agent.EndEpisode();
        this.ResetAcademy();
    }

    protected List<Vector3> AvailablePoints(){
        this.spawnBuffer.Clear();

        foreach(Transform point in this.SpawnPoints){
            this.spawnBuffer.Add(point.position);
        }

        return this.spawnBuffer;
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

    protected void ExtractTerrainColliders(){
        this.terrainColliders.Clear();
        this.ExtractTerrainColliders(this.transform);
    }

    protected void ExtractTerrainColliders(Transform parent){
        this.colliderBuffer = parent.gameObject.GetComponent<Collider>();

        if(this.colliderBuffer != null){
            this.terrainColliders.Add(this.colliderBuffer);
        }

        foreach(Transform child in parent){
            this.ExtractTerrainColliders(child);
        }
    }

    protected void EmptyStore(IStorable iStorable){}

}
