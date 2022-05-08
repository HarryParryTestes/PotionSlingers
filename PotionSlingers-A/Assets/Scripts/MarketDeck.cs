using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MarketDeck : MonoBehaviour
{
    // the deck is uninitialized to begin with
    public List<Card> deckList;
    public int cardInt;
    public CardDisplay cardDisplay1;
    public CardDisplay cardDisplay2;
    public CardDisplay cardDisplay3;
    private Sprite sprite;

    public Card popCard()
    {
        if (deckList.Count > 1)
        {
            Card temp = deckList[0];
            deckList.RemoveAt(0);
            //Debug.Log("Card popped: ");
            return temp;
        }
        Debug.Log("ERROR: Card not returned!");
        return cardDisplay1.card;
    }

    /*
    public void updateCardSprite(CardDisplay cardDisplay)
    {
        Card card = popCard(); 
        cardDisplay.card = card;
        cardDisplay.artworkImage.sprite = cardDisplay.card.cardSprite;
    }
    */

    public List<Card> shuffle()
    {
        return deckList.OrderBy(x => Random.value).ToList();
    }

    public void initCardDisplays()
    {
        Card card1 = popCard();
        Card card2 = popCard();
        Card card3 = popCard();
        cardDisplay1.updateCard(card1);
        cardDisplay2.updateCard(card2);
        cardDisplay3.updateCard(card3);
    }

    // Start is called before the first frame update
    public void init()
    {
        // bye bye shuffle :(

        //deckList = shuffle();
        //for(int i = 0; i < deckList.Count; i++)
        //{
        //    Debug.Log("Card " + i + ": " + deckList[i]);
        //}
        initCardDisplays();
    }

    public void setCardInt(int card)
    {
        cardInt = card;
    }
}
