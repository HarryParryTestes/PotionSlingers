using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Deck : MonoBehaviour
{
    // the deck is uninitialized to begin with
    public List<Card> deckList;
    public CardDisplay cardDisplay;
    private Sprite sprite;
    public Card placeholder;

    public Deck()
    {
        deckList = new List<Card>();
    }

    
    public void buildDeck()
    {
        putCardOnTop(cardDisplay.card);
    }

    // puts a card on the bottom of the deck
    public void putCardOnBottom(Card card)
    {
        deckList.Add(card);
        if(deckList.Count == 1)
        {
            updateCardSprite();
        }
    }

    // puts a card on top of deck
    // updates the deck sprite to match the top card in the deck
    public void putCardOnTop(Card card)
    {
        deckList.Insert(0, card);
        updateCardSprite();
    }
    public Card popCard()
    {
        if (deckList.Count >= 1)
        {
            Card temp = deckList[0];
            deckList.RemoveAt(0);
            if (temp.cardName == "EarlyBirdSpecial" && GameManager.manager.earlyBirdSpecial)
            {
                // +1 damage if put in holster this turn
                temp.effectAmount = 4;
            }
            //Debug.Log("Card popped: ");
            updateCardSprite();
            return temp;
        }
        Debug.Log("ERROR: Card not returned!");
        return cardDisplay.card;
    }

    /*
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        Debug.Log(name + "Game Object Click in Progress");
    }
    */

    // makes the sprite of the cardDisplay match the top card in the list
    public void updateCardSprite()
    {
        if(deckList.Count >= 1)
        {
            cardDisplay.updateCard(deckList[0]);
        }
        else
        {
            cardDisplay.updateCard(placeholder);
        }
        
    }


    // Start is called before the first frame update
    void Start()
    {
        buildDeck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
