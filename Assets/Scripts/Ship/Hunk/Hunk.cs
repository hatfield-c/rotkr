using UnityEngine;
using DG.Tweening;

public class Hunk : MonoBehaviour
{
    HunkData data;

    public bool overrideRigidbody = true;
    [SerializeField] Hunk Predecessor = null;
    [SerializeField] new Rigidbody rigidbody = null;

    
    public FixedJoint Joint;
    public FixedJoint ChildJoint = null;

    Sequence currentSequence = null;
    float despawnTime;

    void Start() {}

    void Update() {}

    void OnJointBreak(float breakForce){
        if(this.rigidbody != null)
            this.rigidbody.useGravity = true;

        this.data.Deleted = true;
        this.DetachChildren();

        Sequence sequence = DOTween.Sequence();
        sequence.SetAutoKill(false);
        sequence.Pause();
        currentSequence = sequence.InsertCallback(this.despawnTime, this.Despawn);
        currentSequence.Play();
    }

    void OnDestroy() {
        currentSequence.Kill();
    }

    public void Init(HunkData data, HunkJointData jointData, HunkRigidbodyData rigidbodyData, float despawnTime){
        this.data = data;
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

        // Setup fixed joint.
        FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();
        Joint = fixedJoint;

        if (Predecessor != null) {
            fixedJoint.connectedBody = Predecessor.rigidbody;
            Predecessor.ChildJoint = fixedJoint;
        }
        else
            fixedJoint.connectedBody = jointData.origin.GetComponent<Rigidbody>();

        fixedJoint.breakForce = jointData.breakForce;
        fixedJoint.breakTorque = jointData.breakTorque;
        fixedJoint.enableCollision = jointData.jointCollision;
        fixedJoint.enablePreprocessing = jointData.enablePreprocessing;
        fixedJoint.connectedMassScale = jointData.connectedMassScale;
        fixedJoint.massScale = jointData.massScale;

        // Toggle this object on or off.
        if (data.Deleted)
            this.Despawn();

    }
    void DetachChildren() {
        if (this.ChildJoint != null) {
            this.ChildJoint.breakForce = 0;
        }
    }
    void Despawn() {
        DetachChildren();
        this.gameObject.SetActive(false);
    }
}
