using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warehouse : MonoBehaviour
{

    protected Dictionary<string, List<IStorable>> Shelves = new Dictionary<string, List<IStorable>>();

    protected List<IStorable> listBuffer = new List<IStorable>();
    protected IStorable itemBuffer;

    public void Init(List<IStorable> inventory){
        foreach(IStorable item in inventory){
            this.Shelves.Add(item.GetArchetype(), new List<IStorable>());
        }
    }
    
    public void StockItem(IStorable item){
        item.GetGameObject().transform.parent = this.transform;
        item.GetGameObject().transform.position = this.transform.position;
        item.Disable();
        this.Shelves[item.GetArchetype()].Add(item);
    }

    public ActorShip FetchItem(string archetype){
        if(!this.Shelves.ContainsKey(archetype)){
            return null;
        }

        this.itemBuffer = this.Shelves[archetype];

        if(this.itemBuffer.Count < 1){
            return null;
        }

        this.itemBuffer = this.listBuffer[0];
        this.listBuffer.RemoveAt(0);
        
        this.itemBuffer.GetGameObject().transform.parent = null;
        this.itemBuffer.Enable();

        return this.itemBuffer;
    }

    public bool HasItem(string archetype){
        if(!this.Shelves.ContainsKey(archetype)){
            return false;
        }

        if(this.Shelves[archetype].Count < 1){
            return false;
        }

        return true;
    }

}
