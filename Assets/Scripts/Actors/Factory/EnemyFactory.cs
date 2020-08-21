using System.Linq;
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
    [SerializeField] Warehouse EnemyWarehouse = null;
    [SerializeField] Warehouse LootWarehouse = null;
    [SerializeField] Transform LayersContainer = null;
    [SerializeField] GameObject WaterPlane = null;

    [Header("Spawn Parameters")]
    [SerializeField] bool DEBUG = false;
    [SerializeField] float DesiredSpawnDepth = 2f;
    [SerializeField] int MaxCost = 1;
    [SerializeField] float InitialSpawnDelay = 5;

    protected List<ActorShip> ActiveShips = new List<ActorShip>();
    protected EnemyFactory.GameDifficulty Difficulty;
    protected int MaxChallengeRating;
    protected int CurrentCost = 0;
    protected int CurrentChallengeRating = 0;

    public void Init(EnemyFactory.GameDifficulty Difficulty, GameObject WaterPlane, int MaxChallengeRating){
        this.Difficulty = Difficulty;
        this.WaterPlane = WaterPlane;
        this.MaxChallengeRating = MaxChallengeRating;

        this.EnemyWarehouse.Init(this.Prefabs.Cast<IStorable>().ToList());
        this.LootWarehouse.Init();
        this.FillEnemyWarehouse();
        this.FillLootWarehouse();

        Sequence sequence = DOTween.Sequence();
        sequence.Pause();
        sequence.InsertCallback(this.InitialSpawnDelay, () => { this.CheckInventory(); });
        sequence.Play();
    }

    public void CheckInventory(){
        if(this.CanSpawn() && this.ShouldSpawn()){
            this.Spawn();
            this.CheckInventory();
        }
    }

    protected void Spawn(){
        ActorShip blueprint = this.ChooseBlueprint();
        Transform layer = this.GetSpawnLayer();

        if(layer == null){
            Debug.LogError($"<color=red>No valid layer found! Set EnemyFactory.LayerContainer, or increase EnemyFactory.SpawnTime delay.</color>");
        }

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
        if(!this.EnemyWarehouse.HasItem(identity)){
            return null;
        }

        ActorShip instance = (ActorShip)this.EnemyWarehouse.FetchItem(identity);
        instance.transform.position = position;
        instance.transform.rotation = Quaternion.Euler(
            Random.Range(-14f, -5f),
            Random.Range(0f, 360f),
            0
        );
        instance.Enable();

        return instance;
    }

    protected ActorShip BuildShip(ActorShip blueprint){
        ActorShip instance = Instantiate<ActorShip>(blueprint);
        instance.ShipManager.Init(
            this.LootWarehouse,
            this.WaterPlane,
            this.Difficulty,
            this.StoreShip
        );

        return instance; 
    }

    protected void FillEnemyWarehouse(){
        ActorShip shipBuffer;
        foreach(ActorShip prefab in this.Prefabs){
            for(int i = 0; i < this.GetMaxInstances(prefab); i++){
                shipBuffer = this.BuildShip(prefab);
                shipBuffer.Disable();
                this.EnemyWarehouse.StockItem((IStorable)shipBuffer);
            }
        }
    }

    protected void FillLootWarehouse(){
        // todo: develop system for creating needed loot instances, to ensure
        // there is enough loot to go around, but not so much that we waste
        // memory.
        // For now, just create 8 instance of all loot kinds

        GameObject objectBuffer;
        List<IStorable> possibleLoot = this.GetPossibleLoot();
        foreach(IStorable storable in possibleLoot){            
            this.LootWarehouse.AddShelf(storable);

            for(int i = 0; i < 8; i++){
                objectBuffer = Instantiate(storable.GetMyGameObject());
                ILoot loot = objectBuffer.GetComponent<ILoot>();

                loot.Init(
                    this.WaterPlane, 
                    (IStorable istorable) => {
                        istorable.Disable();
                        this.LootWarehouse.StockItem(istorable);
                    }
                );

                ((IStorable)loot).Disable();

                this.LootWarehouse.StockItem((IStorable)loot);
            }
        }
    }

    protected List<IStorable> GetPossibleLoot(){
        List<IStorable> lootList = new List<IStorable>();

        foreach(ActorShip shipPrefab in this.Prefabs){
            lootList.AddRange(shipPrefab.ShipManager.GetPossibleLoot());
        }

        return lootList;
    }

    protected void StoreShip(IStorable istorable){
        ActorShip ship = (ActorShip)istorable;

        this.CurrentCost -= ship.ComputationCost;
        this.ActiveShips.Remove(ship);
        ship.Disable();
        this.EnemyWarehouse.StockItem(ship);

        this.CheckInventory();
    }

    protected int GetMaxInstances(ActorShip ship){
        //todo: Determine max instances of each kind of ship

        return 2;
    }

    protected bool ShouldSpawn(){
        if(this.CurrentCost >= this.MaxCost){
            return false;
        }

        return true;
    }

    protected bool CanSpawn(){
        if(this.EnemyWarehouse.GetTotalCount() < 1){
            return false;
        }

        foreach(ActorShip prefab in this.Prefabs){
            if(
                prefab.ComputationCost + this.CurrentCost <= this.MaxCost &&
                this.EnemyWarehouse.GetItemCount(prefab.GetArchetype()) > 0
            ){
                return true;
            }
        }

        return false;
    }

    void Start(){
        if(DEBUG){
            this.Init(EnemyFactory.GameDifficulty.Easy, this.WaterPlane, 100);
        }
    }

}
