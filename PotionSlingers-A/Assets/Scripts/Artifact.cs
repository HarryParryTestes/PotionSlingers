using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : Card
{
    private Card[] artifactArr; // Space for loading a Card 

    public Artifact() {

    }

    public Artifact(int Id, string cardName, string cardType, string CardEffect, 
    int effectAmount, string cardQuality, int sell, int buy, string desc) {
        // super(Id, cardName, cardType, cardEffect, effectAmount, cardQuality, sell, buy, desc);
        artifactArr = new Card[1];
    }

    /* 
    * Runs the ability of the Artifact while discarding the loaded card's 
    * ability.
    * Takes parameter Player p. This is the player who will be affected
    */
    // public override void runAbility(Player p) {
    //     if(this.cardEffect.Equals("Attack")) {
    //         this.Attack(this.effectAmount, p);
    //     } else if(this.cardEffect.Equals("Heal")) {
    //         this.Heal(this.effectAmount, p);
    //     }

    //     sendCardToTrash(trashDeck);
    // }

    /*
    * sendCardToTrash removes the card from artifactArr and sends it to the
    * trash
    */
    // public void sendCardToTrash(Deck trash) {
    //     moveCardToDeck(this.artifactArr[0], trash);
    //     this.artifactArr[0] = null;
    // }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
