using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RatDeckGrabber
{ 
    #region parameters
    [SerializeField] float breakForce = 10f;
    [SerializeField] float reattachVelocity = 10f;
    [SerializeField] float deckDisplacement = 1f;
    #endregion

    #region blackboard variables
    protected Transform assignedShip;
    protected Transform assignedDeck;
    protected Transform ratTransform;
    protected Rigidbody shipBody;
    protected Rigidbody ratBody;

    protected GroundData groundData;
    #endregion

    #region private function
    public void Init(ShipReferences shipReferences, RatReferences ratReferences){
        this.assignedShip = shipReferences.ShipObject.transform;
        this.assignedDeck = shipReferences.DeckObject.transform;
        this.ratTransform = ratReferences.RatObject.transform;
        this.shipBody = shipReferences.ShipBody;
        this.ratBody = ratReferences.Ratbody;
    }

    public void UpdateState(GroundData groundData)
    {
        this.groundData = groundData;

        if(this.CanReattach(groundData)){
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
                force
            )
        ){
           this.DetachFromShip();
        }
    }

    bool CanReattach(GroundData groundData){
        return this.groundData.GetDeck() != null &&
        this.groundData.GetDeck().transform == this.assignedDeck &&
        this.ratTransform.parent == null;
    }

    bool DoesBreakFromShip(GameObject land, float force){
        return land != null && force > this.breakForce;
    }

    void AttachToShip(Transform ship){
        this.ratTransform.parent = this.assignedDeck.transform;
        this.ratTransform.position = this.GetDeckDisplacement(this.ratTransform.position);

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

    Vector3 GetDeckDisplacement(Vector3 originalPos){
        if(this.assignedShip == null){
            return Vector3.zero;
        }

        Vector3 localPos = this.assignedShip.InverseTransformPoint(originalPos);
        float heightDiff = this.deckDisplacement - localPos.y;
        Vector3 targetPos = new Vector3(localPos.x, this.deckDisplacement, localPos.z);
        Vector3 newPos = this.assignedShip.TransformPoint(targetPos);

        return newPos;
    }
    #endregion

    float VelocityDifference(){
        return Mathf.Abs(this.ratBody.velocity.magnitude - this.shipBody.velocity.magnitude);
    }

}
