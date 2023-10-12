using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IDropHandler
{
    CardDisplay cd;

    void Awake()
    {
        cd = this.gameObject.GetComponent<CardDisplay>();
    }

   public void OnDrop(PointerEventData eventData)
    {
        GameObject heldCard = eventData.pointerDrag;
        DragCard dc = heldCard.GetComponent<DragCard>();
        CardDisplay grabbedCard = heldCard.GetComponent<CardDisplay>();
        if (dc.grabbed)
        {
            Debug.Log(grabbedCard.card.cardName);
        }

        // call the events needed to get GameManager to trigger throw and load events
        GameManager.manager.setSCInt(grabbedCard.card.cardName);
        GameManager.manager.setLoadedInt(cd.card.cardName);
        GameManager.manager.preLoadPotion();
        
    }
}
