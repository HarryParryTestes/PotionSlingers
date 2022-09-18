using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGalleryController : MonoBehaviour
{
    public List<CardDisplay> cardDisplays;
    public List<Card> cards;
    public int cardIndex = 0;
    // Start is called before the first frame update

    public void initDisplays()
    {
        cardIndex = 0;
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
        cardIndex = cardIndex - 12;
        if(cardIndex < 0)
        {
            cardIndex = 12;
        }
        Debug.Log("Card index: " + cardIndex);
        changeCardDisplays();
    }

    public void changeCardDisplays()
    {
        foreach(CardDisplay cd in cardDisplays)
        {
            Card temp = cards[cardIndex];
            cd.updateCard(temp);
            cardIndex++;
            if(cardIndex > 18)
            {
                cardIndex = 0;
            }
        }
    }


}
