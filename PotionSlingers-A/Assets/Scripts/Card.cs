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


    public Card() 
    {

    }

    public Card(int Id, string CardName, string CardType, string CardEffect, int EffectAmount, 
                string CardQuality) 
    {
        id = Id;
        cardName = CardName;
        cardType = CardType;
        cardEffect = CardEffect;
        effectAmount = EffectAmount;
        cardQuality = CardQuality;
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
}
