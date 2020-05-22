using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
    public float range = 80.0f;
    public Crosshairs crosshairs;
    public GameObject harpoonGun;
    public Camera cam;

    public float grappleForce = 4.5f;
    public float grappleDamper = 7f;
    public float massScale = 4.5f;

    protected RectTransform reticuleUI;
    protected bool freezeMouse = true;

    protected bool latched = false;
    protected Vector3 latchPoint;
    protected GameObject harpoonLine;
    protected SpringJoint joint;

    // Start is called before the first frame update
    void Start()
    {
        this.reticuleUI = this.crosshairs.crosshairs.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.freezeMouse){
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.lockState = CursorLockMode.None;
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            this.freezeMouse = !this.freezeMouse;
        }

        int layerMask = 1 << 11;
        Ray aimRay = this.cam.ScreenPointToRay(this.reticuleUI.position);
        //Debug.DrawRay(aimRay.origin, aimRay.direction * this.range, Color.red);

        RaycastHit hitData;
        bool canHit = Physics.Raycast(aimRay, out hitData, this.range, layerMask);

        if(canHit){
            this.crosshairs.crosshairs.SetActive(false);
            this.crosshairs.crosshairsTarget.SetActive(true);

            if(Input.GetMouseButtonDown(0)){
                this.latchPoint = hitData.point;
                this.latched = true;

                this.joint = this.gameObject.AddComponent<SpringJoint>();
                this.joint.autoConfigureConnectedAnchor = false;
                this.joint.connectedAnchor = this.latchPoint;
                this.joint.anchor = this.transform.InverseTransformPoint(this.harpoonGun.transform.position);
                //Debug.Log(this.harpoonGun.transform.position);

                float grappleDistance = Vector3.Distance(this.latchPoint, this.harpoonGun.transform.position);

                this.joint.maxDistance = grappleDistance * 0.8f;
                this.joint.minDistance = grappleDistance * 0.25f;
                
                this.joint.spring = this.grappleForce;
                this.joint.damper = this.grappleDamper;
                this.joint.massScale = this.massScale;
            }
        } else {
            this.crosshairs.crosshairs.SetActive(true);
            this.crosshairs.crosshairsTarget.SetActive(false);
        }

        if(Input.GetMouseButtonUp(0)){
            this.latched = false;
            Destroy(this.harpoonLine);
            Destroy(this.joint);
        }

        if(this.latched){
            this.DrawHarpoon();
        }
    }

    protected void DrawHarpoon(){
        Vector3 startPoint = this.transform.position;
        Vector3 centerPoint = new Vector3((startPoint.x + this.latchPoint.x) / 2, (startPoint.y + this.latchPoint.y) / 2, (startPoint.z + this.latchPoint.z) / 2);
        Vector3 harpoonDir = this.latchPoint - startPoint;

        Destroy(this.harpoonLine);

        this.harpoonLine = new GameObject();
        GameObject lineMesh = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        lineMesh.transform.parent = this.harpoonLine.transform;
        lineMesh.GetComponent<Collider>().enabled = false;

        this.harpoonLine.transform.position = centerPoint;
        this.harpoonLine.transform.LookAt(this.latchPoint);

        float scaleAmount = harpoonDir.magnitude * 0.5f;
        lineMesh.transform.localScale = new Vector3(1, scaleAmount, 1);
        lineMesh.transform.Rotate(new Vector3(90, 0, 0));
    }
}

[System.Serializable]
public struct Crosshairs{
    public GameObject crosshairs;
    public GameObject crosshairsTarget;
}