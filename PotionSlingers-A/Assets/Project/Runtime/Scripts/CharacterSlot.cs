using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSlot : MonoBehaviour, IDropHandler
{
    // CardDisplay cd;
    CharacterDisplay chard;

    void Awake()
    {
        if (this.gameObject.GetComponent<CharacterDisplay>() != null)
            // this isn't working fix this

            chard = this.gameObject.GetComponent<CharacterDisplay>();
            Debug.Log(chard.gameObject.name);
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

        // attacking player
        if (chard != null)
        {
            Debug.Log("Throw triggered?");
            chard.gameObject.GetComponent<CardThrow>().throwCard();
        }
    }
}
