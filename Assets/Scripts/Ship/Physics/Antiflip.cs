using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antiflip : MonoBehaviour
{
    #region references
    public Rigidbody shipBody;
    #endregion

    #region parameters
    [SerializeField] float maxAngle = 30;
    [SerializeField] float threshold = 3;
    #endregion

    #region blackboard
    protected Vector3 angleVelocityBuffer;
    #endregion

    #region private functions
    void FixedUpdate()
    {
        this.DampenAngularVelocity();
        this.ClampRotation();
    }

    void DampenAngularVelocity(){
        float angularX = this.shipBody.angularVelocity.x * Time.fixedDeltaTime;
        float angularZ = this.shipBody.angularVelocity.z * Time.fixedDeltaTime;

        float xAngle = this.shipBody.rotation.eulerAngles.x;
        float zAngle = this.shipBody.rotation.eulerAngles.z;

        this.angleVelocityBuffer.x = this.GetNewRotationVelocity(xAngle, angularX);
        this.angleVelocityBuffer.y = this.shipBody.angularVelocity.y;
        this.angleVelocityBuffer.z = this.GetNewRotationVelocity(zAngle, angularZ);

        this.shipBody.angularVelocity = this.angleVelocityBuffer;
    }

    float GetNewRotationVelocity(float angle, float angularVelocity){
        float angularNew = angularVelocity;
        float angleDiff;

        if(angle >= 0 && angle < 180){
            angleDiff = this.maxAngle - angle;

        } else {
            angleDiff = angle - this.maxAngle;
        }

        float radianDiff = angleDiff * Mathf.Deg2Rad; 

        if(angularVelocity >= radianDiff){
            if(radianDiff < this.threshold){
                angularNew = 0f;
            } else {
                angularNew = (0.95f * radianDiff);
            }
        }

        return angularNew / Time.fixedDeltaTime;
    }

    void ClampRotation(){
        this.shipBody.rotation = Quaternion.Euler(
            this.GetClampedAngle(this.shipBody.rotation.eulerAngles.x),
            this.shipBody.rotation.eulerAngles.y,
            this.GetClampedAngle(this.shipBody.rotation.eulerAngles.z)
        );
    }

    float GetClampedAngle(float angle){
        if(angle >= this.maxAngle && angle < 180){
            return this.maxAngle;
        }

        if(angle <= 360 - this.maxAngle && angle > 180){
            return 360 - this.maxAngle;
        }

        return angle;
    }
    #endregion
}
