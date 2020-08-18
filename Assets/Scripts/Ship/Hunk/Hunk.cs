using System;
using UnityEngine;
using DG.Tweening;

public class Hunk : MonoBehaviour
{
    HunkData data;
    HunkJointData jointParameters;

    public bool overrideRigidbody = true;
    [SerializeField] Hunk Predecessor = null;
    [SerializeField] new Rigidbody rigidbody = null;

    public Action HunkBroken;
    public FixedJoint Joint;
    public FixedJoint ChildJoint = null;

    Sequence currentSequence = null;
    float despawnTime;

    protected Vector3 origPosition;
    protected Quaternion origRotation;

    void Start() {

    }

    void Update() {}

    public void Init(
        HunkData data, 
        HunkJointData jointData, 
        HunkRigidbodyData rigidbodyData,
        Action HunkBroken,
        float despawnTime
    ){
        this.data = data;
        this.HunkBroken = HunkBroken;
        this.jointParameters = jointData;

        this.despawnTime = despawnTime;

        // Setup this object.
        if (this.rigidbody == null) {
            this.rigidbody = GetComponent<Rigidbody>();
            if (this.rigidbody == null) {
                this.overrideRigidbody = true;
                this.rigidbody = gameObject.AddComponent<Rigidbody>();
            }
                
        }

        if (this.overrideRigidbody == true) {
            this.rigidbody.mass = rigidbodyData.mass;
            this.rigidbody.useGravity = rigidbodyData.useGravity;
            this.rigidbody.drag = rigidbodyData.drag;
            this.rigidbody.angularDrag = rigidbodyData.angularDrag;
        }
        else
            this.rigidbody.useGravity = false;

        this.origPosition = this.jointParameters.origin.transform.InverseTransformPoint(this.transform.position);
        this.origRotation = this.transform.localRotation;

        this.CreateJoint();

        // Toggle this object on or off.
        if (data.Deleted){
            this.Despawn();
        }
    }

    // Returns the hunk that was repaired
    public Hunk Repair(){
        if(!this.IsDeleted()){
            return null;
        }

        if(this.HasPredecessor() && this.Predecessor.IsDeleted()){
            return this.Predecessor.Repair();
        }

        this.data.Deleted = false;
        return this;
    }

    public bool IsDeleted(){
        return this.data.Deleted;
    }

    public bool HasPredecessor(){
        return this.Predecessor != null;
    }

    public void EnableHunk(){
        this.gameObject.SetActive(true);

        this.rigidbody.velocity = Vector3.zero;
        this.transform.position = this.jointParameters.origin.transform.TransformPoint(this.origPosition);
        this.transform.localRotation = this.origRotation;
        this.CreateJoint();

        Sequence sequence = DOTween.Sequence();
        sequence.SetAutoKill(false);
        sequence.Pause();
        currentSequence = sequence.InsertCallback(0.01f, () => {
            this.gameObject.SetActive(false);
            this.gameObject.SetActive(true);    
        });
        currentSequence.Play();

        //this.gameObject.SetActive(false);
        //this.gameObject.SetActive(true);
    }

    public void DisableHunk(){
        this.Despawn();
    }

    void CreateJoint(){
        Destroy(this.Joint);
        this.Joint = null;

        this.data.Deleted = false;
        FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();

        if (this.Predecessor != null) {
            fixedJoint.connectedBody = this.Predecessor.rigidbody;
            this.Predecessor.ChildJoint = fixedJoint;
        }
        else
            fixedJoint.connectedBody = this.jointParameters.origin.GetComponent<Rigidbody>();

        fixedJoint.breakForce = this.jointParameters.breakForce;
        fixedJoint.breakTorque = this.jointParameters.breakTorque;
        fixedJoint.enableCollision = this.jointParameters.jointCollision;
        fixedJoint.enablePreprocessing = this.jointParameters.enablePreprocessing;
        fixedJoint.connectedMassScale = this.jointParameters.connectedMassScale;
        fixedJoint.massScale = this.jointParameters.massScale;

        this.Joint = fixedJoint;
    }

    void OnJointBreak(float breakForce){
        this.Joint = null;

        if(this.rigidbody != null)
            this.rigidbody.useGravity = true;

        this.data.Deleted = true;
        this.DetachChildren();
        this.HunkBroken?.Invoke();

        Sequence sequence = DOTween.Sequence();
        sequence.SetAutoKill(false);
        sequence.Pause();
        currentSequence = sequence.InsertCallback(this.despawnTime, this.Despawn);
        currentSequence.Play();
    }

    void OnDestroy() {
        currentSequence.Kill();
    }

    void DetachChildren() {
        if (this.ChildJoint == null) {
            return;
        }

        this.ChildJoint.breakForce = 0;
    }
    void Despawn() {
        DetachChildren();

        if(this.Joint != null){
            Destroy(this.Joint);
            this.Joint = null;
        }

        this.gameObject.SetActive(false);
    }
}
