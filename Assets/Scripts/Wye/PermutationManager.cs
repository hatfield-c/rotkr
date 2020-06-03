using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermutationManager : MonoBehaviour
{
    public List<PermutationLayer> layers;

    void Start(){
        foreach(PermutationLayer layer in this.layers){
            IPermutable Permuter = layer.prefab.GetComponent<IPermutable>();
            GameObject permutation = Permuter.generate();

            permutation.transform.parent = this.transform;
        }
    }

    void Update(){
        
    }
}