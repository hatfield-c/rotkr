using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainagePermutation : MonoBehaviour, IPermutable
{
    public List<PermutationLayer> layer;

    void Start(){ }

    void Update(){}

    public GameObject generate(){
        
        if(this.layer.Count > 0){
            return this.permuteLayer(this.layer);
        } else{
            return new GameObject("Drainage");
        }
    }

    protected GameObject permuteLayer(List<PermutationLayer> layer){
        int layerChoice = Random.Range(0, layer.Count);

        GameObject permutation = Instantiate(
            layer[layerChoice].prefab, 
            layer[layerChoice].prefab.transform.position, 
            layer[layerChoice].prefab.transform.rotation
        );

        return permutation;
    }
}
