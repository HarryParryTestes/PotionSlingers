using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int id;
    public string cardName; // Ex: PS_AS_BasicBattleBaster
    public string cardType; // Potion, Artifact, Vessel, Ring
    public string cardEffect; // Attack, Heal, etc.
    public int effectAmount; // +2 Damage, +3 Heal, etc.
    public string cardQuality; // Cold, Dry, Wet, Hot, None
    public Image cardImage; // Ex: PS_AS_BasicBattleBaster.png
    public Sprite cardSprite;
    public int buyPrice;
    public int sellPrice;
    public string desc;


    public Card() 
    {

    }

    public Card(int Id, string CardName, string CardType, string CardEffect, int EffectAmount, 
                string CardQuality, int sell, int buy, string description) 
    {
        id = Id;
        cardName = CardName;
        cardType = CardType;
        cardEffect = CardEffect;
        effectAmount = EffectAmount;
        cardQuality = CardQuality;
        sellPrice = sell;
        buyPrice = buy;
        desc = description;
        //cardImage = gameObject.GetComponent<Image>();
    }

    public static Card MakeCardObject(int id, string cardName, string cardType, string cardEffect,
        int effectAmount, string cardQuality)
    {
        GameObject go = new GameObject("CardInstance" + id);
        Card ret = go.AddComponent<Card>();
        ret.id = id;
        ret.cardName = cardName;
        ret.cardType = cardType;
        ret.cardEffect = cardEffect;
        ret.effectAmount = effectAmount;
        ret.cardQuality = cardQuality;

        return ret;
    }

    public void runAbility() {
        Debug.Log("This should be overwritten");
    }

    //Add x points to the given player's health
    //Parameters:
        //x: Number of points to add
        //player: The player whose health is to be added
    // public void Heal(int x, Player player) {
    //     player.health += x;
    //     if(player.health > 10) {
    //         player.health = 10;
    //     }
    // }

    //Subtract x points to the given player's health
    //Parameters:
        //x: Number of points to subtract
        //player: The player from which health is to be taken
    // public void Attack(int x, Player player) {
    //     player.health -= x;
    //     if(player.health < 0) {
    //         player.health = 0;
    //     }
    // }

    // public void moveCardToDeck(Card card, Deck deck) {
    //     deck.add(card);
    // }

    public string getCardEffect() {
        return this.cardEffect;
    }

    public int getEffectAmount() {
        return this.effectAmount;
    }
}
