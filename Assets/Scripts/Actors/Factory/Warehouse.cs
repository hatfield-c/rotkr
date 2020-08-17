using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warehouse : MonoBehaviour
{

    protected Dictionary<string, List<ActorShip>> Shelves = new Dictionary<string, List<ActorShip>>();

    protected List<ActorShip> listBuffer = new List<ActorShip>();
    protected ActorShip shipBuffer;

    public void Init(List<ActorShip> inventory){
        foreach(ActorShip item in inventory){
            this.Shelves.Add(item.identity, new List<ActorShip>());
        }
    }
    
    public void StockItem(ActorShip ship){
        ship.transform.parent = this.transform;
        ship.transform.position = this.transform.position;
        ship.gameObject.SetActive(false);
        this.Shelves[ship.identity].Add(ship);
    }

    public ActorShip FetchItem(string identity){
        if(!this.Shelves.ContainsKey(identity)){
            return null;
        }

        this.listBuffer = this.Shelves[identity];

        if(this.listBuffer.Count < 1){
            return null;
        }

        this.shipBuffer = this.listBuffer[0];
        this.listBuffer.RemoveAt(0);
        
        this.shipBuffer.transform.parent = null;
        this.shipBuffer.gameObject.SetActive(true);

        return this.shipBuffer;
    }

    public bool HasItem(string identity){
        if(!this.Shelves.ContainsKey(identity)){
            return false;
        }

        if(this.Shelves[identity].Count < 1){
            return false;
        }

        return true;
    }

}
