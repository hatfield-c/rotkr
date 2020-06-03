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
        GameObject root = new GameObject("Exit Gates");

        int numGate = this.GatePositions.Count;
        int closedNum = Random.Range(numGate - this.MaxGates, numGate - this.MinGates);

        int[] closedGates = this.chooseClosedGates(closedNum, numGate);
        for(int i = 0; i < numGate; i++){
            if(this.indexInArray(closedGates, i)){
                GameObject closedPrefab = this.GatePositions[i].prefab.transform.Find("exit_gate_closed").gameObject;
                GameObject closedGate = Instantiate(closedPrefab, closedPrefab.transform.position, closedPrefab.transform.rotation, root.transform);
            } else {
                GameObject openPrefab = this.GatePositions[i].prefab.transform.Find("exit_gate").gameObject;
                GameObject openGate = Instantiate(openPrefab, openPrefab.transform.position, openPrefab.transform.rotation, root.transform);
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
