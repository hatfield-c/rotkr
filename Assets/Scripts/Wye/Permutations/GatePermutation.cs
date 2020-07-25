using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatePermutation : MonoBehaviour, IPermutable
{
    public List<PermutationLayer> GatePositions;
    public int MinGates;
    public int MaxGates;

    void Start(){ }

    void Update(){}

    public GameObject generate(){
        
        return this.permuteGates(this.GatePositions);
    }

    protected GameObject permuteGates(List<PermutationLayer> layer){
        GameObject root = this.gameObject;

        int numGate = this.GatePositions.Count;
        int closedNum = Random.Range(numGate - this.MaxGates, numGate - this.MinGates + 1);

        int[] closedGates = this.chooseClosedGates(closedNum, numGate);
        for(int i = 0; i < numGate; i++){
            if(this.indexInArray(closedGates, i)){
                GameObject extraGate = this.GatePositions[i].prefab.transform.Find("exit_gate").gameObject;
                Destroy(extraGate);
            } else {
                GameObject extraGate = this.GatePositions[i].prefab.transform.Find("exit_gate_closed").gameObject;
                Destroy(extraGate);
            }
        }

        return root;
    }

    protected int[] chooseClosedGates(int closedNum, int numGate){
        int[] closedIndices = new int[closedNum];
        for(int i = 0; i < closedNum; i++){
            closedIndices[i] = -1;
        }

        for(int i = 0; i < closedNum; i++){
            int chosenIndex = Random.Range(0, numGate);

            while(this.indexInArray(closedIndices, chosenIndex)){
                chosenIndex = Random.Range(0, numGate);
            }

            closedIndices[i] = chosenIndex;
        }

        return closedIndices;
    }

    protected bool indexInArray(int[] arr, int index){
        foreach(int entry in arr){
            if(entry == index){
                return true;
            }
        }

        return false;
    }
}
