using System;
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

    #region references
    protected Transform assignedShip;
    protected Transform assignedDeck;
    protected Transform ratTransform;
    protected Rigidbody shipBody;
    protected Rigidbody ratBody;
    protected CapsuleCollider damageCollider;
    protected CapsuleCollider shipCollider;
    protected string deckTag;
    protected string hunkTag;
    #endregion

    #region events
    public Action GetReattached;
    public Action GetDetached;
    #endregion

    #region blackboard variables
    protected GroundData groundData;
    #endregion

    #region public function
    public void Init(ShipReferences shipReferences, RatReferences ratReferences){
        this.assignedShip = shipReferences.ShipObject.transform;
        this.assignedDeck = shipReferences.DeckObject.transform;
        this.shipBody = shipReferences.ShipBody;
        this.deckTag = shipReferences.DeckTag;
        this.hunkTag = shipReferences.HunkTag;

        this.ratTransform = ratReferences.RatObject.transform;
        this.ratBody = ratReferences.Ratbody;
        this.damageCollider = ratReferences.DamageCollider;
        this.shipCollider = ratReferences.ShipCollider;
    }

    public void UpdateState(GroundData groundData){
        this.groundData = groundData;

        if(this.CanReattach(groundData)){
            if(
                this.VelocityDifference() > 0 &&
                this.VelocityDifference() <= this.reattachVelocity
            ){
                this.Reattach();
            }
        }
    }

    public void CollisionCheck(Collision collision){
        if(this.IsIgnoredCollision(collision.collider.tag)){
            return;
        }

        float force = collision.impulse.magnitude / Time.fixedDeltaTime;
        if(this.DoesBreakFromShip(force, collision)){
           this.DetachFromShip();
        }
    }

    public void PlaceOnShip(){
        Vector3 position = this.GetValidSpawnPoint();

        this.ratTransform.position = position;
        this.Reattach();
    }

    public Vector3 GetRelativeDeckPosition(Vector3 originalPos){
        if(this.assignedDeck == null){
            return Vector3.zero;
        }

        Vector3 localPos = this.assignedDeck.InverseTransformPoint(originalPos);
        Vector3 targetPos = new Vector3(localPos.x, this.deckDisplacement, localPos.z);
        Vector3 newPos = this.assignedDeck.TransformPoint(targetPos);

        return newPos;
    }
    #endregion

    #region private function
    bool CanReattach(GroundData groundData){
        return this.groundData.GetDeck() != null &&
        this.IsAssignedDeck(this.groundData.GetDeck().transform) &&
        !this.IsAttached();
    }

    bool DoesBreakFromShip(float force, Collision collision){
        return this.IsAttached() && force > this.breakForce;
    }

    bool IsAttached(){
        return this.ratTransform.parent != null;
    }

    bool IsAssignedDeck(Transform deck){
        return deck == this.assignedDeck;
    }

    bool IsIgnoredCollision(string tag){
        return tag == this.deckTag || tag == this.hunkTag;
    }

    void Reattach(){
        this.ratBody.constraints = RigidbodyConstraints.None;

        this.ratTransform.parent = this.assignedShip.transform;
        this.ratTransform.localRotation = Quaternion.Euler(0, this.ratTransform.localRotation.eulerAngles.y, 0);
        this.ratTransform.position = this.GetRelativeDeckPosition(this.ratTransform.position);

        this.ratBody.isKinematic = true;
        this.ratBody.useGravity = false;

        this.shipCollider.enabled = false;
        this.GetReattached?.Invoke();
    }

    void DetachFromShip(){
        this.ratTransform.parent = null;
        this.ratBody.isKinematic = false;
        this.ratBody.useGravity = true;

        this.ratBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        this.ratTransform.eulerAngles = this.GetDetachRotation();

        this.shipCollider.enabled = true;
        this.GetDetached?.Invoke();
    }

    Vector3 GetValidSpawnPoint(){
        //** todo: Determine where to place the rat on the ship

        return this.GetRelativeDeckPosition(this.assignedShip.position);
    }

    Vector3 GetDetachRotation(){
        return Vector3.up * this.ratTransform.eulerAngles.y;
    }
    #endregion

    float VelocityDifference(){
        return Mathf.Abs(this.ratBody.velocity.magnitude - this.shipBody.velocity.magnitude);
    }

}
