using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePermutation : MonoBehaviour, IPermutable
{
    public List<PermutationLayer> layer;
    
    public float spawnRate = 1.0f;

    void Start(){ }

    void Update(){}

    public GameObject generate(){
        if(this.spawnRate > 1){
            this.spawnRate = 1;
        }

        if(this.spawnRate < 0){
            this.spawnRate = 0;
        }

        if(this.layer.Count > 0){
            return this.permuteLayer(this.layer);
        } else {
            return new GameObject("Pipes");
        }
    }

    protected GameObject permuteLayer(List<PermutationLayer> layer){
        int layerChoice = Random.Range(0, layer.Count);
        GameObject variantPrefab = layer[layerChoice].prefab;

        GameObject permutation = new GameObject(variantPrefab.name);

        foreach(Transform pipeTransform in variantPrefab.transform){
            float diceRoll = Random.Range(0.0f, 1.0f);

            if(diceRoll <= this.spawnRate){
                GameObject pillar = Instantiate(pipeTransform.gameObject, pipeTransform.position, pipeTransform.rotation, permutation.transform);
            }
        }

        return permutation;
    }
}
