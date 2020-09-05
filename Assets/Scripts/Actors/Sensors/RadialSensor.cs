using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialSensor : ASensor {
    [SerializeField] float maxAngle = 0;

    public override Vector3 GetStartPos(int rayIndex) {
        return this.transform.position;
    }

    public override Vector3 GetDir(int rayIndex) {
        float angleStep = 0f;
        float startAngle = (180f - maxAngle) / 2f;

        if (this.sensorCount > 1) {
            angleStep = this.maxAngle / (this.sensorCount - 1);
        }

        float theta = (rayIndex * angleStep) + startAngle;
        this.dirBuffer.x = Mathf.Cos(theta * Mathf.Deg2Rad);
        this.dirBuffer.y = 0f;
        this.dirBuffer.z = Mathf.Sin(theta * Mathf.Deg2Rad);

        return this.transform.TransformDirection(this.dirBuffer);
    }
}
