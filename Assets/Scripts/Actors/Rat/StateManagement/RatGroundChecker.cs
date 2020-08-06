using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RatGroundChecker
{
    [SerializeField] float maxDistance = 1.3f;
    [SerializeField] string deckTag = "ship_deck";
    [SerializeField] CapsuleCollider ratCollider = null;

    protected RaycastHit castData;
    protected Transform ratTransform;

    public void Init(Transform ratTransform){
        this.ratTransform = ratTransform;
    }

    public GroundData GetGroundData(){
        if(this.ratCollider == null){
            return new GroundData(false, null);
        }

        bool grounded = Physics.SphereCast(
            this.ratTransform.TransformPoint(ratCollider.center), 
            this.ratCollider.radius, 
            -this.ratTransform.up, 
            out this.castData,
            this.maxDistance
        );

        GroundData data;
        if(castData.collider != null && castData.collider.tag == this.deckTag){
            data = new GroundData(
                grounded,
                castData.collider.gameObject
            );
        } else {
            data = new GroundData(
                grounded,
                null
            );
        }

        return data;
    }
}
