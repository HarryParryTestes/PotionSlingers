using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashDeck : MonoBehaviour
{
    public List<Card> deckList;
    public Card card;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void addCard(CardDisplay cd)
    {
        deckList.Add(cd.card);
        cd.updateCard(card);
    }
}


