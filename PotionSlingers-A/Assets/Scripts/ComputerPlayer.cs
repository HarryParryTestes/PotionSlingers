using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPlayer : CardPlayer
{

    public List<Card> AICards = new List<Card>(4);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AITurn()
    {
        Debug.Log("Please make me smart!");

        if (holster == null)
        {
            Debug.Log("Grabbing the holster");
            holster = gameObject.GetComponent<CardPlayer>().holster;
        }

        
        foreach(CardDisplay cd in holster.cardList)
        {
            AICards.Add(cd.card);
        }

        foreach(Card card in AICards)
        {
            Debug.Log(card.cardName);
        }
    }
}
