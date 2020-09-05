using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermutationManager : MonoBehaviour
{
    public List<PermutationLayer> layers;

    protected List<GameObject> objectList = new List<GameObject>();

    void Start(){
        this.Reset();
    }

    public void Reset(){
        if(!this.enabled){
            return;
        }

        foreach(GameObject gameObject in this.objectList){
            Destroy(gameObject);
        }

        this.objectList.Clear();

        foreach(PermutationLayer layer in this.layers){
            IPermutable permuter = layer.prefab.GetComponent<IPermutable>();
            
            if(!(permuter is null)){
                GameObject permutation = permuter.generate();
                permutation.transform.position = this.transform.position;
                permutation.transform.parent = this.transform;

                this.objectList.Add(permutation);
            }
        }
    }
}