using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Deck : MonoBehaviour, IPointerDownHandler
{
    // the deck is uninitialized to begin with
    public List<Card> deckList;
    public CardDisplay cardDisplay;
    private Sprite sprite;

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
        Card temp = deckList[0];
        if(deckList.Count > 1)
        {
            deckList.RemoveAt(0);
            Debug.Log("Card popped: ");
            updateCardSprite();
        }
        return temp;
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        Debug.Log(name + "Game Object Click in Progress");
    }

    // makes the sprite of the cardDisplay match the top card in the list
    public void updateCardSprite()
    {
        cardDisplay.card = deckList[0];
        cardDisplay.artworkImage.sprite = cardDisplay.card.cardSprite;
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
