using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class FashionUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject hoverBox;
    public Card card;

    // Start is called before the first frame update
    void Start()
    {
        // GetComponent<Image>().sprite = card.cardSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateCard(Card card)
    {
        Debug.Log("Updating " + card + " sprite.");
        gameObject.SetActive(true);
        this.card = card;
        if(card.cardType == "Ring")
            GetComponent<Image>().sprite = card.cardSprite;
        else
            GetComponent<Image>().sprite = card.cardlessSprite;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        // don't display the hoverbox if you're dragging a card around
        if (pointerEventData.pointerDrag != null)
        {
            return;
        }

        if (hoverBox != null)
        {
            hoverBox.SetActive(true);
            hoverBox.GetComponent<HoverBox>().UpdateText(card);
        }

    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        // don't display the hoverbox if you're dragging a card around
        if (pointerEventData.pointerDrag != null)
        {
            return;
        }

        if (hoverBox != null)
        {
            hoverBox.SetActive(false);
            // hoverBox.GetComponent<HoverBox>().UpdateText(card);
        }

    }
}
