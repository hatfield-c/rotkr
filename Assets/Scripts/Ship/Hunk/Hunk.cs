using System.Collections;
using UnityEngine;

public class Hunk : MonoBehaviour
{
    HunkData data;

    public bool overrideRigidbody = true;
    public GameObject predecessor;
    
    public FixedJoint joint;
    public FixedJoint childJoint = null;

    protected KatinTimer despawnTimer;
    public float despawnTime;

    void Start() {
        this.despawnTimer = new KatinTimer();
    }

    void Update() {
        this.despawnTimer.update();        
    }

    void OnJointBreak(float breakForce){
        Rigidbody hunkBody = this.gameObject.GetComponent<Rigidbody>();
        if(hunkBody != null){
            hunkBody.useGravity = true;
        }

        if(this.childJoint != null){
            this.childJoint.breakForce = 0;
        }

        this.despawnTimer.Init(this.despawnTime, this.Despawn);
    }

    public void Init(HunkData data)
    {
        this.data = data;

        if (data.Deleted)
            this.Despawn();
    }

    public void Despawn(){
        this.gameObject.SetActive(false);
    }
}
