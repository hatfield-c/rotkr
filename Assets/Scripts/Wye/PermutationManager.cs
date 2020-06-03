using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermutationManager : MonoBehaviour
{
    public List<PermutationLayer> layers;

    void Start(){
        foreach(PermutationLayer layer in this.layers){
            IPermutable permuter = layer.prefab.GetComponent<IPermutable>();
            
            if(!(permuter is null)){
                GameObject permutation = permuter.generate();
                permutation.transform.parent = this.transform;
            }
        }
    }

    void Update(){
        
    }
}