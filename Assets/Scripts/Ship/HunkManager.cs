using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HunkManager
{
    
    public Transform hunkGroup;
    public HunkJointData jointData;
    public HunkRigidbodyData rigidBodyData;

    protected List<Hunk> hunkList;

    public void Init(List<HunkData> hunkData){
        this.hunkGroup.parent = null;
        
        if(hunkData != null){
            this.hunkList = this.buildFromData(hunkData);
        } else {
            this.hunkList = this.build();
        }
    }

    protected List<Hunk> buildFromData(List<HunkData> hunkData){

        foreach(HunkData data in hunkData){
            if(data.deleted == true){
                GameObject hunkObject = this.hunkGroup.GetChild(data.hunkId).gameObject;

                //TODO: Disable this object instead, keep it for pooling and recreation/repairing
                Object.Destroy(hunkObject);
            }
        }

        return this.build();
    }

    protected List<Hunk> build(){
        List<Hunk> hunks = new List<Hunk>();
        
        foreach(Transform hunkTransform in this.hunkGroup){
            GameObject hunkObject = hunkTransform.gameObject;
            Hunk hunk = hunkObject.GetComponent<Hunk>();

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
