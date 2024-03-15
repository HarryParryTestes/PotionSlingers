using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
[System.Serializable]
public class Card : ScriptableObject
{
    public int id;
    public string cardName; // Ex: PS_AS_BasicBattleBaster
    public string cardType; // Potion, Artifact, Vessel, Ring
    public string cardEffect; // Attack, Heal, etc.
    public int effectAmount; // +2 Damage, +3 Heal, etc.
    public string cardQuality; // Cold, Dry, Wet, Hot, None
    //public Image cardImage; // Ex: PS_AS_BasicBattleBaster.png
    public Sprite cardSprite;
    public int buyPrice;
    public int sellPrice;
    public string desc;
    public bool spicy;

    public enum TYPE
    {
        POTION = 1,
        ARTIFACT = 2,
        VESSEL = 3,
        RING = 4
    }

    public enum EFFECT
    {
        ATTACK,
        HEAL,
        BONUS
    }

    public enum QUALITY
    {
        COLD,
        DRY,
        WET,
        HOT,
        NONE
    }

    /*
    public Card(int Id, string CardName, string CardType, string CardEffect, int EffectAmount, 
                string CardQuality, int SellPrice, int BuyPrice, string Description) 
    {
        id = Id;
        cardName = CardName;
        cardType = CardType;
        cardEffect = CardEffect;
        effectAmount = EffectAmount;
        cardQuality = CardQuality;
        sellPrice = SellPrice;
        buyPrice = BuyPrice;
        desc = Description;
        //cardImage = gameObject.GetComponent<Image>();
    }
    */

    /*
    // TODO: refactor this to include the buy and sell price, you'll have to refactor the code in CardDatabase.cs too
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
    */

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
