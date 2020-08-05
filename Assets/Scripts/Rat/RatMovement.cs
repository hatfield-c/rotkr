using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatMovement : MonoBehaviour
{
    #region parameters
    [SerializeField] float maxDistance = 1.3f;
    [SerializeField] float breakForce = 10f;
    [SerializeField] float reattachVelocity = 10f;
    #endregion

    #region references
    [SerializeField] Rigidbody ratBody = null;
    [SerializeField] CapsuleCollider ratCollider = null;
    [SerializeField] Transform assignedShip = null;
    [SerializeField] string deckTag = "ship_deck";

    protected GameObject deckUnderfoot;
    protected Rigidbody shipBody;
    #endregion

    #region blackboard variables
    public bool grounded = false;
    RaycastHit hit;
    Vector3 position;
    Vector3 direction;
    bool isHit = false;
    #endregion

    #region private function
    void Start(){
        this.shipBody = this.assignedShip.gameObject.GetComponent<Rigidbody>();
        this.AttachToShip(this.assignedShip);
    }

    void FixedUpdate()
    {
        this.UpdateGroundState();

        if(this.deckUnderfoot != null && this.transform.parent == null){
            if(
                Mathf.Abs(this.ratBody.velocity.magnitude - this.shipBody.velocity.magnitude) > 0 &&
                Mathf.Abs(this.ratBody.velocity.magnitude - this.shipBody.velocity.magnitude) <= this.reattachVelocity
            ){
                this.AttachToShip(this.deckUnderfoot.transform);
            }
        }
    }

    protected void UpdateGroundState(){
        this.position = transform.TransformPoint(ratCollider.center);
        this.direction = -transform.up;

        this.grounded = Physics.SphereCast(
            this.position, 
            ratCollider.radius, 
            this.direction, 
            out hit,
            this.maxDistance
        );

        this.isHit = this.grounded;

        if(hit.collider != null && hit.collider.tag == this.deckTag){
            this.deckUnderfoot = hit.collider.gameObject;
        } else {
            this.deckUnderfoot = null;
        }
    }

    void OnCollisionEnter(Collision collision){
        float force = collision.impulse.magnitude / Time.fixedDeltaTime;

        if(
            this.DoesBreakFromShip(
                this.deckUnderfoot, 
                collision.collider.tag, 
                force
            )
        ){
           this.DetachFromShip();
        }
    }

    bool DoesBreakFromShip(GameObject land, string objectTag, float force){
        return land != null && objectTag != this.deckTag && force > this.breakForce;
    }

    void AttachToShip(Transform ship){
        this.transform.parent = ship;
        this.ratBody.isKinematic = true;
        this.ratBody.useGravity = false;

        this.ratBody.constraints = RigidbodyConstraints.None;
    }

    void DetachFromShip(){
            this.transform.parent = null;
            this.ratBody.isKinematic = false;
            this.ratBody.useGravity = true;

            this.ratBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
    #endregion

    #region
    public bool IsGrounded(){
        return this.grounded;
    }
    #endregion

    void OnDrawGizmos()
    {
        if (isHit)
        {

            Gizmos.color = Color.red;
            Gizmos.DrawRay(position, direction * hit.distance);
            Gizmos.DrawWireSphere(position + direction * hit.distance, ratCollider.radius);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(position, direction * maxDistance);
        }
    }
}
