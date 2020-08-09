using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class OverboardSwimmer
{
    [SerializeField] float pickupDelay = 3;

    protected GameObject assignedDeck = null;
    protected GameObject aoeObject = null;
    protected RatDeckGrabber deckGrabber = null;
    protected Sequence aoeSequence = null;

    public void Init(ShipReferences shipReferences, RatReferences ratReferences, RatDeckGrabber deckGrabber){
        this.assignedDeck = shipReferences.DeckObject;
        this.aoeObject = ratReferences.AoeObject;
        this.deckGrabber = deckGrabber;

        Sequence sequence = DOTween.Sequence();
        sequence.SetAutoKill(false);
        sequence.Pause();
        sequence.OnComplete(this.ResetTween);
        sequence.InsertCallback(this.pickupDelay, this.AllowPickup);
        this.aoeSequence = sequence;
    }

    public void AttachActivate(){
        this.aoeObject.SetActive(false);
        this.deckGrabber.PlaceOnShip();
    }

    public void OverboardActivate(){
        this.aoeSequence.Play();
    }

    public void AllowPickup(){
        if(this.deckGrabber.IsAttached()){
            return;
        }

        this.aoeObject.SetActive(true);
    }

    public void TriggerActivate(Collider collider){
        if(collider.gameObject != this.assignedDeck){
            return;
        }

        this.AttachActivate();
        
    }

    void ResetTween(){
        this.aoeSequence.Restart();
        this.aoeSequence.Pause();
    }

    void OnDestroy() {
        this.aoeSequence.Kill();
    }
}
