using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorCannon : Cannon {

    [Header("AI Parameters")]
    [SerializeField] protected float range = 100f;
    [SerializeField] protected float sweep = 5f;
    [SerializeField] protected float shootAngle = 15f;
    [SerializeField] protected float angleOffset = 5f;

    protected GameObject playerObject;
    protected Quaternion orientation;

    public void Init(GameObject playerObject) {
        this.playerObject = playerObject;
        this.orientation = this.transform.localRotation;
    }

    public override void reload() {
        base.reload();

        this.transform.localRotation = orientation;
    }

    public override void fire() {
        this.transform.LookAt(this.playerObject.transform);

        float offset = Random.Range(-this.angleOffset, this.angleOffset);

        this.transform.Rotate(-this.shootAngle, offset, 0f);

        base.fire();
    }

    void FixedUpdate()
    {
        float distance = Vector3.Distance(this.transform.position, this.playerObject.transform.position);

        float angle = Vector3.SignedAngle(
            Vector3.forward,
            this.transform.InverseTransformPoint(this.playerObject.transform.position).normalized,
            Vector3.up
        );

        if(distance <= this.range && Mathf.Abs(angle) <= this.sweep && this.IsLoaded()) {

            this.lightFuse();
        }
    }
}
