using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // the deck is uninitialized to begin with
    public Queue deckQueue = null;
    public bool isBuilt;

    public Deck()
    {
        isBuilt = false;
        deckQueue = new Queue();
    }

    public void setBuilt (bool isBuilt)
    {
        this.isBuilt = isBuilt;
    }

    // puts a card on the bottom of the deck
    public void putCardOnBottom(Card card)
    {
        deckQueue.Enqueue(card);
    }

    // puts a card on top of deck
    // get deque working for this?
    public void putCardOnTop(Card card)
    {
        // figure this out lol

    }
    public Card popCard()
    {
        Card temp = default(Card);
        if(deckQueue.Count > 1 && isBuilt)
        {
            temp = (Card)deckQueue.Dequeue();
            Debug.Log("Card popped: ");
        }
        return temp;
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
