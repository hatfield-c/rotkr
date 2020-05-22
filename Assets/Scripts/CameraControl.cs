using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    public Transform camTransform;

    private Camera cam;

    private float distance = 10.0f;
 
    //private float currentX = 0.0f;
    //private float currentY = 0.0f;

    public float sensitivityX = 0.5f;
    public float sensitivityY = 0.5f;

    //private float Y_ANGLE_MIN = 0.0f;
    //private float Y_ANGLE_MAX = 50.0f;

    //private float X_ANGLE_MIN = -100.0f;
    //private float X_ANGLE_MAX = 100.0f;

    void Start() {
        this.camTransform = this.transform;
        this.cam = Camera.main;
    }

    void LateUpdate(){  
        Vector3 dir = new Vector3(0, 0, -this.distance);
        
        float xMove = Input.GetAxis("Mouse X") * this.sensitivityX;
        float yMove = Input.GetAxis("Mouse Y") * this.sensitivityY;

        this.transform.RotateAround(this.target.position, Vector3.up, xMove);
        this.transform.RotateAround(this.target.position, -Vector3.right, yMove);

        if(this.transform.eulerAngles.z != 0){

            this.transform.eulerAngles = new Vector3(
                this.transform.eulerAngles.x,
                this.transform.eulerAngles.y,
                0
            );
        }
    }

}
