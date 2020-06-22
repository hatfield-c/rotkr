using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HunkManager
{
    
    public Transform hunkBlueprint;
    public HunkJointData jointData;
    public HunkRigidbodyData rigidBodyData;

    protected List<Hunk> hunkList;

    public void Init(List<HunkData> hunkData){
        this.hunkBlueprint.parent = null;
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

            if(hunkObject.GetComponent<Rigidbody>() == null){
                Rigidbody hunkBody = hunkObject.AddComponent<Rigidbody>();
                hunkBody.mass = this.rigidBodyData.mass;
                hunkBody.useGravity = this.rigidBodyData.useGravity;
                hunkBody.drag = this.rigidBodyData.drag;
                hunkBody.angularDrag = this.rigidBodyData.angularDrag;
            }

            FixedJoint hunkJoint = hunkObject.AddComponent<FixedJoint>();
            hunkJoint.connectedBody = this.jointData.origin.GetComponent<Rigidbody>();
            hunkJoint.breakForce = this.jointData.breakForce;
            hunkJoint.breakTorque = this.jointData.breakTorque;
            hunkJoint.enableCollision = this.jointData.jointCollision;
            hunkJoint.enablePreprocessing = this.jointData.enablePreprocessing;
            
        }

        return hunks;
    }

}
