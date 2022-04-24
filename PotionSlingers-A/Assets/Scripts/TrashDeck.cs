using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashDeck : MonoBehaviour
{
    public List<Card> deckList;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void addCard(Card card)
    {
        deckList.Add(card);
    }
}


