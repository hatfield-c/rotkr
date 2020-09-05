using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackedSensor : ASensor {
    [SerializeField] float offset = 0;

    public override Vector3 GetStartPos(int rayIndex) {
        float totalOffset = this.offset * 2f;

        float spacing = 0f;
        float localOffset = 0f;

        if (this.sensorCount > 1f) {
            spacing = totalOffset / (this.sensorCount - 1f);
            localOffset = this.offset;
        } else {
            totalOffset = 0;
        }

        this.startBuffer = this.transform.TransformPoint(
                Vector3.up * (localOffset - (rayIndex * spacing))
        );

        return this.startBuffer;
    }

    public override Vector3 GetDir(int rayIndex) {
        return this.transform.TransformDirection(Vector3.forward);
    }

}
