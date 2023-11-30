using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CharacterSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    // CardDisplay cd;
    public CardPlayer cp;
    public CharacterDisplay cd;

    void Awake()
    {
        if (this.gameObject.GetComponent<CharacterDisplay>() != null)
        {
            cp = this.gameObject.GetComponent<CardPlayer>();
            cd = cp.character;
            Debug.Log(cp.gameObject.name);
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

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (this.gameObject.name == "DeckPile")
        {
            transform.position += new Vector3(0, 100, 0);
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (this.gameObject.name == "DeckPile")
        {
            transform.position -= new Vector3(0, 100, 0);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        /*
        add in implementation to distinguish between GameObjects
        that have a CardDisplay vs a CharacterDisplay
        */
        Debug.Log("Drop happened on character");

        GameObject heldCard = eventData.pointerDrag;
        DragCard dc = heldCard.GetComponent<DragCard>();
        // grabbing the card held by the cursor
        CardDisplay grabbedCard = heldCard.GetComponent<CardDisplay>();

        if (dc.market)
        {
            Debug.Log("Buy triggered?");
            if (heldCard.GetComponent<TopMarketBuy>() != null)
            {
                // buy the card
                Debug.Log("Buy");
            }
            handleBuy(dc.marketCardInt);
            // heldObject.GetComponent<CardThrow>().throwCard();
        } else if(this.gameObject.name == "DeckPile")
        {
            Debug.Log("Dropped card triggered?");
            GameManager.manager.setSCInt(grabbedCard.card.cardName);
            GameManager.manager.cycleCard();
        }

        // attacking player
        if (cd != null && !dc.market)
        {
            Debug.Log("Throw triggered?");
            cd.gameObject.GetComponent<CardThrow>().throwCard();
        }
    }
}
