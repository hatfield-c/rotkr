using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackedSensor : ASensor {
    [SerializeField] float offset = 0;

    protected List<GameObject> debugObj = new List<GameObject>();

    public override List<bool> ReadSensors() {
        if(this.sensorCount < 1) {
            return this.results;
        }

        this.results.Clear();

        float totalOffset = this.offset * 2f;

        float spacing = 0f;
        float localOffset = 0f;

        if (this.sensorCount > 1f) {
            spacing = totalOffset / (this.sensorCount - 1f);
            localOffset = this.offset;
        } else {
            totalOffset = 0;
        }

        for(int i = 0; i < this.sensorCount; i++) {
            this.startBuffer = this.transform.TransformPoint(
                Vector3.up * ( localOffset - (i * spacing) )
            );

            this.dirBuffer = this.transform.TransformDirection(Vector3.forward);

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

    private void FixedUpdate() {
        this.ReadSensors();
    }

}
