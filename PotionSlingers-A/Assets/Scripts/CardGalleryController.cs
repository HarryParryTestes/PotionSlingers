using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGalleryController : MonoBehaviour
{
    public List<CardDisplay> cardDisplays;
    public List<Card> cards;
    public List<Card> searchableCards;
    public int cardIndex = 0;
    // Start is called before the first frame update

    public void searchCards(string category)
    {
        searchableCards.Clear();
        foreach(Card card in cards)
        {
            if (card.cardQuality.Contains(category) || card.cardType.Contains(category))
            {
                //Debug.Log("Match found, adding card...");
                searchableCards.Add(card);
            }
        }
        cardIndex = 0;
        changeCardDisplays();
    }

    public void initDisplays()
    {
        searchableCards.Clear();
        cardIndex = 0;
        //searchableCards = cards;
        foreach (Card card in cards)
        {
            searchableCards.Add(card);
        }
        changeCardDisplays();
    }

    public void rightSix()
    {
        // Lol I'm stupid
        //cardIndex = cardIndex + 6;
        Debug.Log("Card index: " + cardIndex);
        changeCardDisplays();
    }

    public void leftSix()
    {
        // Also lol this was dumb
        //Debug.Log("Card index before it's -12: " + cardIndex);
        cardIndex = cardIndex - 12;
        /*
        if (cardIndex < 12)
        {
            cardIndex = searchableCards.Count - cardIndex - 1;
        }
        */
        
        if(cardIndex < 0)
        {
            cardIndex = searchableCards.Count - 1;
        }
        Debug.Log("Card index: " + cardIndex);
        changeCardDisplays();
    }

    public void changeCardDisplays()
    {
        foreach(CardDisplay cd in cardDisplays)
        {
            Card temp = searchableCards[cardIndex];
            cd.updateCard(temp);
            cardIndex++;
            if(cardIndex > searchableCards.Count - 1)
            {
                cardIndex = 0;
            }
        }
    }


}
