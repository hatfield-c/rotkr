using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RatDeckGrabber
{ 
    #region parameters
    [SerializeField] float breakForce = 10f;
    [SerializeField] float reattachVelocity = 10f;
    [SerializeField] float deckDisplacement = 1f;;
    #endregion

    #region blackboard variables
    protected Transform assignedShip;
    protected Transform ratTransform;
    protected Rigidbody shipBody;
    protected Rigidbody ratBody;

    protected GroundData groundData;
    #endregion

    #region private function
    public void Init(Transform assignedShip, Transform ratTransform, Rigidbody shipBody, Rigidbody ratBody){
        this.assignedShip = assignedShip;
        this.ratTransform = ratTransform;
        this.shipBody = this.assignedShip.gameObject.GetComponent<Rigidbody>();
        this.ratBody = ratBody;
        this.AttachToShip(this.assignedShip);
    }

    public void UpdateState(GroundData groundData)
    {
        this.groundData = groundData;

        if(this.groundData.GetDeck() != null && this.ratTransform.parent == null){
            if(
                this.VelocityDifference() > 0 &&
                this.VelocityDifference() <= this.reattachVelocity
            ){
                this.AttachToShip(this.groundData.GetDeck().transform);
            }
        }
    }

    public void OnCollisionEnter(Collision collision){
        float force = collision.impulse.magnitude / Time.fixedDeltaTime;

        if(
            this.DoesBreakFromShip(
                this.groundData.GetDeck(), 
                //collision.collider.tag, 
                force
            )
        ){
           this.DetachFromShip();
        }
    }

    bool DoesBreakFromShip(GameObject land, float force){
        return land != null && force > this.breakForce;
    }

    void AttachToShip(Transform ship){
        this.ratTransform.parent = ship;
        this.ratBody.isKinematic = true;
        this.ratBody.useGravity = false;

        this.ratBody.constraints = RigidbodyConstraints.None;
    }

    void DetachFromShip(){
            this.ratTransform.parent = null;
            this.ratBody.isKinematic = false;
            this.ratBody.useGravity = true;

            this.ratBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
    #endregion

    float VelocityDifference(){
        return Mathf.Abs(this.ratBody.velocity.magnitude - this.shipBody.velocity.magnitude);
    }

}
