using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vessel : MonoBehaviour
{
    private Card[] vesselArr; //Array for loading the 2 cards in the Vessel
    private int attackTotal; //The combined attack from all cards in the Vessel
    private int healTotal; //The combined healing from all cards in the Vessel

    public Vessel() {

    }

    public Vessel(int Id, string cardName, string cardType, string CardEffect, 
    int effectAmount, string cardQuality, int sell, int buy, string desc) {
        super(Id, cardName, cardType, cardEffect, effectAmount, cardQuality, sell, buy, desc);
        vesselArr = new Card[2];
    }

    /*
    * Runs the ability of the Vessel while adding the cards' in the vesselArr's abilities
    * Only available when the Vessel is full
    */
    public override void runAbility(Player p) {
        if(this.vesselArr.Length != 2) {
            Console.Write("The Vessel needs to hold 2 cards in order to be thrown");
            return;
        }

        attackTotal = 0;
        healTotal = 0;

        addCardTotals(this);
        addCardTotals(this.vesselArr[0]);
        addCardTotals(this.vesselArr[1]);

        attackTotal(attackTotal, p);
        Heal(healtotal, p);

        //Remove the cards from the vessel and send them back to the player's hand
        foreach(Card c in vesselArr) {
            sendCardToBottomOfDeck(c, p.deck);
        }
        vesselArr[0] = null;
        vesselArr[0] = null;
    }

    /*
    * Helper function to runAbility that adds the cardEffect of the passed Card
    * to attackTotal or healTotal
    */
    private void addCardTotals(Card card) {
        if(card.getCardEffect().Equals("Attack")) {
            attackTotal += card.getEffectAmount();
        } else if(card.getCardEffect().Equals("Heal")) {
            healTotal += card.getEffectAmount();
        }
    }

    /*
    * sends the passed Card to the bottom of the passed player's deck
    */
    public void sendCardToBottomOfDeck(Card card, Deck playerDeck) {
        moveCardToDeck(card, playerDeck);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
