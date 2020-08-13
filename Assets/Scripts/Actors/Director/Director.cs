using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    
    public enum Difficulty {
        Easy,
        Medium,
        Hard
    }

    [SerializeField] bool DEBUG = false;
    [SerializeField] List<ActorShip> Blueprints = new List<ActorShip>();

    [SerializeField] SpawnPool Spawner;
    [SerializeField] GameObject WaterPlane;

    protected List<ActorShip> ActiveShips = new List<ActorShip>();

    protected int spawnCount = 0;

    public void Init(SpawnPool Spawner, GameObject WaterPlane){
        this.Spawner = Spawner;
        this.WaterPlane = WaterPlane;
    }

    void FixedUpdate(){
        if(this.spawnCount < 1){
            this.spawnCount++;

            ActorShip blueprint = this.Blueprints[0];
            SpawnParameters parameters = new SpawnParameters(this.WaterPlane, Difficulty.Easy);
            
            ActorShip instance = this.Spawner.Spawn(blueprint, parameters, new Vector3(this.spawnCount * 20, 0, 0));
            this.ActiveShips.Add(instance);
        }
    }

    void Start(){
        if(DEBUG){
            this.Init(this.Spawner, this.WaterPlane);
        }
    }

}
