using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HunkManager
{
    public GameObject jointOrigin;
    public Transform hunkBlueprint;
    public float breakForce = 0.0f;
    public float breakTorque = 0.0f;
    public bool jointCollision = false;
    public bool enablePreprocessing = true;

    protected List<Hunk> hunkList;

    public void Init(List<HunkData> hunkData){
        if(hunkData != null){
            this.hunkList = this.loadHunkData(hunkData);
        } else {
            this.hunkList = this.createHunks();
        }
    }

    protected List<Hunk> loadHunkData(List<HunkData> hunkData){
        return null;
    }

    protected List<Hunk> createHunks(){
        List<Hunk> hunks = new List<Hunk>();
        
        foreach(Transform hunkTransform in this.hunkBlueprint){
            GameObject hunkObject = hunkTransform.gameObject;

            FixedJoint hunkJoint = hunkObject.AddComponent<FixedJoint>();
            //hunkJoint.connectedBody = jointOrigin.rigidbody;
            hunkJoint.breakForce = this.breakForce;
            hunkJoint.breakTorque = this.breakTorque;
            hunkJoint.enableCollision = this.jointCollision;
            hunkJoint.enablePreprocessing = this.enablePreprocessing;

        }

        return hunks;
    }

}
