using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyFactory : MonoBehaviour
{
    
    public enum GameDifficulty {
        Easy,
        Medium,
        Hard
    }

    [Header("Possible Ships")]
    [SerializeField] List<ActorShip> Prefabs = new List<ActorShip>();
    
    [Header("References")]
    [SerializeField] Warehouse Warehouse = null;
    [SerializeField] Transform LayersContainer = null;
    [SerializeField] GameObject WaterPlane = null;

    [Header("Spawn Parameters")]
    [SerializeField] bool DEBUG = false;
    [SerializeField] float DesiredSpawnDepth = 8f;
    [SerializeField] int MaxCost = 1;
    [SerializeField] float SpawnDelay = 5;

    protected List<ActorShip> ActiveShips = new List<ActorShip>();
    protected EnemyFactory.GameDifficulty Difficulty;
    protected int MaxChallengeRating;
    protected int CurrentCost = 0;
    protected int CurrentChallengeRating = 0;

    public void Init(EnemyFactory.GameDifficulty Difficulty, GameObject WaterPlane, int MaxChallengeRating){
        this.Difficulty = Difficulty;
        this.WaterPlane = WaterPlane;
        this.MaxChallengeRating = MaxChallengeRating;

        this.Warehouse.Init(this.Prefabs);
        this.FillWarehouse();

        Sequence sequence = DOTween.Sequence();
        sequence.Pause();
        sequence.InsertCallback(this.SpawnDelay, () => { this.CheckInventory(); });
        sequence.Play();
    }

    public void CheckInventory(){
        if(this.ShouldSpawn()){
            this.Spawn();
        }
    }

    protected void Spawn(){
        ActorShip blueprint = this.ChooseBlueprint();
        Transform layer = this.GetSpawnLayer();
        Vector3 point = this.GetSpawnPoint(layer);

        ActorShip instance = this.DeployEnemy(blueprint.identity, point);

        if(instance == null){
            return;
        }

        this.ActiveShips.Add(instance);
        this.CurrentCost += instance.ComputationCost;
    }

    protected Vector3 GetSpawnPoint(Transform layer){
        if(layer.childCount < 1){
            return layer.position;
        }

        //todo: check that this point isn't occupied by a currently spawning ship
        int pointIndex = Random.Range(0, layer.childCount);

        return layer.GetChild(pointIndex).position;
    }

    protected Transform GetSpawnLayer(){
        if(this.LayersContainer.childCount < 1){
            return this.LayersContainer;
        }

        Transform closest = null;
        float smallestDistance = 999999999999;
        float distance;
        foreach(Transform layer in this.LayersContainer){
            if(layer.position.y + this.DesiredSpawnDepth > this.WaterPlane.transform.position.y){
                continue;
            }

            distance = this.WaterPlane.transform.position.y - layer.position.y;
            if(distance < smallestDistance){
                closest = layer;
                smallestDistance = distance;
            }
        }

        return closest;
    }

    protected ActorShip ChooseBlueprint(){
        if(this.Prefabs.Count < 1){
            return null;
        }

        return this.Prefabs[0];
    }

    protected ActorShip DeployEnemy(string identity, Vector3 position){
        if(!this.Warehouse.HasItem(identity)){
            return null;
        }

        ActorShip instance = this.Warehouse.FetchItem(identity);
        instance.transform.position = position;
        instance.transform.rotation = Quaternion.Euler(
            Random.Range(-14f, -5f),
            Random.Range(0f, 360f),
            0
        );

        instance.Reset();

        return instance;
    }

    protected ActorShip BuildShip(ActorShip blueprint){
        GameObject shipObject = Instantiate(blueprint.gameObject);

        ActorShip instance = shipObject.GetComponent<ActorShip>();
        instance.ShipManager.Init(
            this.WaterPlane,
            this.Difficulty
        );

        instance.RemoveShipAction += this.StoreShip;

        return instance; 
    }

    protected void FillWarehouse(){
        ActorShip shipBuffer;
        foreach(ActorShip prefab in this.Prefabs){
            for(int i = 0; i < this.GetMaxInstances(prefab); i++){
                shipBuffer = this.BuildShip(prefab);
                this.Warehouse.StockItem(shipBuffer);
            }
        }
    }

    protected void StoreShip(ActorShip ship){
        this.Warehouse.StockItem(ship);
    }

    protected int GetMaxInstances(ActorShip ship){
        return 1;
    }

    protected bool ShouldSpawn(){
        if(this.CurrentCost >= this.MaxCost){
            return false;
        }

        return true;
    }

    void Start(){
        if(DEBUG){
            this.Init(GameDifficulty.Easy, this.WaterPlane, 100);
        }
    }

}
