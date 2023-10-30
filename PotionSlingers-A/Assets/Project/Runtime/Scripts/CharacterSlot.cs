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

        // attacking player
        if (cd != null)
        {
            Debug.Log("Throw triggered?");
            cd.gameObject.GetComponent<CardThrow>().throwCard();
        }
    }
}
