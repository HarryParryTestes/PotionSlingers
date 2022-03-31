using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // the deck is uninitialized to begin with
    //public List<Card> deckList;
    public bool isBuilt;

    public Deck()
    {
        isBuilt = false;
        //deckList = new List<Card>();
    }

    /*
    public void buildDeck()
    {
        if(isBuilt == false)
        {
            foreach (Card card in CardDatabase.cardList)
            {
                putCardOnTop(card);
            }
            isBuilt = true;
            Debug.Log("Deck built:");
        }
    }

    // puts a card on the bottom of the deck
    public void putCardOnBottom(Card card)
    {
        deckList.Add(card);
    }

    // puts a card on top of deck
    // get deque working for this?
    public void putCardOnTop(Card card)
    {
        deckList.Insert(0, card);
    }
    public Card popCard()
    {
        Card temp = deckList[0];
        if(deckList.Count > 1 && isBuilt)
        {
            deckList.RemoveAt(0);
            Debug.Log("Card popped: ");
        }
        return temp;
    }
    */

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
