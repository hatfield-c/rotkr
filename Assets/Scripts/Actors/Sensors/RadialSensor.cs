using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialSensor : ASensor {
    [SerializeField] float maxAngle = 0;

    protected List<GameObject> debugObj = new List<GameObject>();

    public override List<bool> ReadSensors() {
        if (this.sensorCount < 1) {
            return this.results;
        }

        this.results.Clear();

        float angleStep = 0f;
        float startAngle = (180f - maxAngle) / 2f;

        if (this.sensorCount > 1) {
            angleStep = this.maxAngle / (this.sensorCount - 1);
        }

        for (int i = 0; i < this.sensorCount; i++) {
            this.startBuffer = this.transform.position;

            float theta = (i * angleStep) + startAngle;
            this.dirBuffer.x = Mathf.Cos(theta * Mathf.Deg2Rad);
            this.dirBuffer.y = 0f;
            this.dirBuffer.z = Mathf.Sin(theta * Mathf.Deg2Rad);

            this.dirBuffer = this.transform.TransformDirection(this.dirBuffer);

            bool hit = this.ReadSensor(
                this.startBuffer,
                this.dirBuffer
            );

            this.results.Add(hit);

            if (this.debug) {
                GameObject objDebug = GameObject.CreatePrimitive(PrimitiveType.Cube);
                objDebug.transform.position = this.startBuffer;
                objDebug.transform.LookAt(this.dirBuffer * this.distance + this.startBuffer);
                objDebug.transform.localScale = Vector3.forward * this.distance + (new Vector3(1, 1, 0));
                objDebug.transform.Translate(Vector3.forward * (this.distance / 2));

                BoxCollider collider = objDebug.GetComponent<BoxCollider>();
                Destroy(collider);

                if (this.debugObj.Count == this.sensorCount) {
                    Destroy(this.debugObj[i]);
                    this.debugObj[i] = objDebug;
                } else {
                    this.debugObj.Add(objDebug);
                }

                if (hit) {
                    Debug.Log($"Sensor ID {this.GetId()} ray {i} hit.");
                }
            }
        }

        return this.results;

    }

    void FixedUpdate() {
        this.ReadSensors();
    }
}
