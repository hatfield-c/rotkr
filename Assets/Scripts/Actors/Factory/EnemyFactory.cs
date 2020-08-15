using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    
    public enum GameDifficulty {
        Easy,
        Medium,
        Hard
    }

    [Header("Possible Ships")]
    [SerializeField] List<ActorShip> Blueprints = new List<ActorShip>();
    
    [Header("References")]
    [SerializeField] List<Transform> Layers = new List<Transform>();
    [SerializeField] GameObject WaterPlane;

    [Header("Spawn Parameters")]
    [SerializeField] bool DEBUG = false;
    [SerializeField] int MaxCost = 1;

    protected List<ActorShip> ActiveShips = new List<ActorShip>();
    protected EnemyFactory.GameDifficulty Difficulty;
    protected int MaxChallengeRating;
    protected int CurrentCost = 0;
    protected int CurrentChallengeRating = 0;

    public void Init(EnemyFactory.GameDifficulty Difficulty, GameObject WaterPlane, int MaxChallengeRating){
        this.Difficulty = Difficulty;
        this.WaterPlane = WaterPlane;
        this.MaxChallengeRating = MaxChallengeRating;

        this.CheckInventory();
    }

    public void CheckInventory(){
        while(this.ShouldSpawn()){
            ActorShip chosenShip = this.Spawn();
            this.CurrentCost += chosenShip.ComputationCost;
        }
    }

    protected ActorShip Spawn(){
        ActorShip blueprint = this.ChooseBlueprint();
        Transform layer = this.GetSpawnLayer();
        Vector3 point = this.GetSpawnPoint(layer);

        ActorShip instance = this.CreateEnemy(blueprint, point);
        this.ActiveShips.Add(instance);

        return instance;
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
        Transform closest = null;
        float smallestDistance = 999999999999;
        float distance;
        foreach(Transform layer in this.Layers){
            distance = layer.position.y - this.WaterPlane.transform.position.y;
            if(distance < smallestDistance){
                closest = layer;
                smallestDistance = distance;
            }
        }

        return closest;
    }

    protected ActorShip ChooseBlueprint(){
        if(this.Blueprints.Count < 1){
            return null;
        }

        return this.Blueprints[0];
    }

    protected ActorShip CreateEnemy(ActorShip blueprint, Vector3 position){
        GameObject shipObject = Instantiate(
            blueprint.gameObject, 
            position, 
            Quaternion.Euler(
                Random.Range(-14f, -5f),
                Random.Range(0f, 360f),
                0
            ), 
            null
        );

        ActorShip instance = shipObject.GetComponent<ActorShip>();
        instance.ShipManager.Init(
            this.WaterPlane,
            this.Difficulty
        );

        return instance; 
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
