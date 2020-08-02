using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HunkManager
{
    
    public Transform hunkGroup;
    public HunkJointData jointData;
    public HunkRigidbodyData rigidBodyData;
    public float hunkDespawnTime = 7f;

    protected List<Hunk> hunkList;

    public void Init(List<HunkData> hunkDatum){
        this.hunkGroup.parent = null;
        
        if(hunkDatum != null){
            this.hunkList = this.buildFromData(hunkDatum);
        } else {
            this.hunkList = this.build();
        }
    }

    protected List<Hunk> buildFromData(List<HunkData> hunkDatum){

        foreach(HunkData data in hunkDatum){
            GameObject hunkObject = this.hunkGroup.GetChild(data.HunkID).gameObject;
            Hunk hunk = hunkObject.GetComponent<Hunk>();
            hunk.Init(data);
        }

        return this.build();
    }

    protected List<Hunk> build(){
        List<Hunk> hunks = new List<Hunk>();
        
        foreach(Transform hunkTransform in this.hunkGroup){
            GameObject hunkObject = hunkTransform.gameObject;
            Hunk hunk = hunkObject.GetComponent<Hunk>();
            hunk.despawnTime = this.hunkDespawnTime;

            Rigidbody hunkBody = hunkObject.GetComponent<Rigidbody>();
            if(hunkBody == null){
                hunkBody = hunkObject.AddComponent<Rigidbody>();
                hunk.overrideRigidbody = true;
            }

            if(hunk.overrideRigidbody == true){
                hunkBody.mass = this.rigidBodyData.mass;
                hunkBody.useGravity = this.rigidBodyData.useGravity;
                hunkBody.drag = this.rigidBodyData.drag;
                hunkBody.angularDrag = this.rigidBodyData.angularDrag;
            }

            FixedJoint hunkJoint = hunkObject.AddComponent<FixedJoint>();
            hunk.joint = hunkJoint;

            if(hunk.predecessor != null){
                hunkJoint.connectedBody = hunk.predecessor.GetComponent<Rigidbody>();
                hunk.predecessor.GetComponent<Hunk>().childJoint = hunkJoint;
            } else {
                hunkJoint.connectedBody = this.jointData.origin.GetComponent<Rigidbody>();
            }

            hunkJoint.breakForce = this.jointData.breakForce;
            hunkJoint.breakTorque = this.jointData.breakTorque;
            hunkJoint.enableCollision = this.jointData.jointCollision;
            hunkJoint.enablePreprocessing = this.jointData.enablePreprocessing;
            hunkJoint.connectedMassScale = this.jointData.connectedMassScale;
            hunkJoint.massScale = this.jointData.massScale;
        }

        return hunks;
    }

}
