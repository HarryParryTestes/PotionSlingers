using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSlot : MonoBehaviour, IDropHandler
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
            // heldObject.GetComponent<CardThrow>().throwCard();
        }

        // attacking player
        if (cd != null && !dc.market)
        {
            Debug.Log("Throw triggered?");
            cd.gameObject.GetComponent<CardThrow>().throwCard();
        }
    }
}
