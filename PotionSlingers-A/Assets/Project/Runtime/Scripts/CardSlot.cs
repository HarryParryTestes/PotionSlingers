using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IDropHandler
{
    CardDisplay cd;

    void Awake()
    {
        if (this.gameObject.GetComponent<CardDisplay>() != null)
            cd = this.gameObject.GetComponent<CardDisplay>();
    }

   public void OnDrop(PointerEventData eventData)
    {
        /*
        add in implementation to distinguish between GameObjects
        that have a CardDisplay vs a CharacterDisplay
        */

        GameObject heldCard = eventData.pointerDrag;

        DragCard dc = heldCard.GetComponent<DragCard>();
        // grabbing the card held by the cursor
        CardDisplay grabbedCard = heldCard.GetComponent<CardDisplay>();

        // test this and double check
        if(dc != null)
        {
            if (dc.market)
            {
                Debug.Log("Buy triggered?");
                if (heldCard.GetComponent<TopMarketBuy>() != null)
                {
                    // buy the card
                    Debug.Log("Buy");
                }
                handleBuy(dc.marketCardInt);
                return;
                // heldObject.GetComponent<CardThrow>().throwCard();
            }
        }
        

        if (heldCard.GetComponent<CharacterSlot>() != null)
        {
            Debug.Log("Trying to load deck card!!!");
            // GameManager.manager.setSCInt(grabbedCard);
            GameManager.manager.setLoadedInt(cd);
            GameManager.manager.preLoadPotion(grabbedCard);
            return;
        }

        if (cd != null)
        {
            // call the events needed to get GameManager to trigger throw and load events
            // GameManager.manager.setSCInt(grabbedCard.card.cardName);
            GameManager.manager.setSCInt(grabbedCard);
            // GameManager.manager.setLoadedInt(cd.card.cardName);
            GameManager.manager.setLoadedInt(cd);
            GameManager.manager.preLoadPotion();
        }
    }

    public void handleBuy(int cardInt)
    {
        if (cardInt == 1 || cardInt == 2 || cardInt == 3)
        {
            GameManager.manager.md1.cardInt = cardInt;
            GameManager.manager.topMarketBuy();
        }

        if (cardInt == 4 || cardInt == 5 || cardInt == 6)
        {
            GameManager.manager.md2.cardInt = cardInt - 3;
            GameManager.manager.bottomMarketBuy();
        }
    }
}
