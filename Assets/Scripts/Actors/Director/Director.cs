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

    protected bool spawned = false;

    public void Init(SpawnPool Spawner, GameObject WaterPlane){
        this.Spawner = Spawner;
        this.WaterPlane = WaterPlane;
    }

    void FixedUpdate(){
        if(!this.spawned){
            this.spawned = true;

            ActorShip blueprint = this.Blueprints[0];
            SpawnParameters parameters = new SpawnParameters(this.WaterPlane, Difficulty.Easy);
            
            ActorShip instance = this.Spawner.Spawn(blueprint, parameters);
            this.ActiveShips.Add(instance);
        }
    }

    void Start(){
        if(DEBUG){
            this.Init(this.Spawner, this.WaterPlane);
        }
    }

}
