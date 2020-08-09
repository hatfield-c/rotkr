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
    protected Sequence currentSequence = null;

    public void Init(ShipReferences shipReferences, RatReferences ratReferences, RatDeckGrabber deckGrabber){
        this.assignedDeck = shipReferences.DeckObject;
        this.aoeObject = ratReferences.AoeObject;
        this.deckGrabber = deckGrabber;
    }

    public void AttachActivate(){
        this.aoeObject.SetActive(false);
        this.deckGrabber.PlaceOnShip();
    }

    public void DetachActivate(){
        Sequence sequence = DOTween.Sequence();
        sequence.SetAutoKill(false);
        sequence.Pause();
        this.currentSequence = sequence.InsertCallback(this.pickupDelay, this.AllowPickup);
        this.currentSequence.Play();
    }

    public void AllowPickup(){
        this.aoeObject.SetActive(true);
    }

    public void TriggerActivate(Collider collider){
        if(collider.gameObject != this.assignedDeck){
            return;
        }

        this.AttachActivate();
        
    }

    void OnDestroy() {
        this.currentSequence.Kill();
    }
}
