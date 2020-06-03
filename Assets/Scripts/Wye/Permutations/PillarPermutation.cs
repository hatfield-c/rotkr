using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarPermutation : MonoBehaviour, IPermutable
{
    public List<PermutationLayer> topLayer;
    public List<PermutationLayer> bottomLayer;
    
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

        GameObject permutationRoot = new GameObject("Pillars");

        if(this.topLayer.Count > 0){
            GameObject topPermutation = this.permuteLayer(this.topLayer);
            topPermutation.transform.parent = permutationRoot.transform;
        }

        if(this.bottomLayer.Count > 0){
            GameObject bottomPermutation = this.permuteLayer(this.bottomLayer);
            bottomPermutation.transform.parent = permutationRoot.transform;
        }

        return permutationRoot;
    }

    protected GameObject permuteLayer(List<PermutationLayer> layer){
        int layerChoice = Random.Range(0, layer.Count);
        GameObject variantPrefab = layer[layerChoice].prefab;
        GameObject pillarPrefab = variantPrefab.transform.GetChild(0).gameObject;

        GameObject permutation = new GameObject(variantPrefab.name);

        foreach(Transform pillarTransform in variantPrefab.transform){
            float diceRoll = Random.Range(0.0f, 1.0f);

            if(diceRoll <= this.spawnRate){
                GameObject pillar = Instantiate(pillarPrefab, pillarTransform.position, pillarTransform.rotation, permutation.transform);
            }
        }

        return permutation;
    }

}
