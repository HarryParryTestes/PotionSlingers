using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public Image artworkImage;
    public Card placeholder;

    // Artifact loaded potion slot:
    public GameObject artifactSlot;
    public CardDisplay aPotion;
    
    // Vessel loaded potion slots:
    public GameObject vesselSlot1;
    public GameObject vesselSlot2;
    public CardDisplay vPotion1;
    public CardDisplay vPotion2;

    // Start is called before the first frame update
    void Start()
    {
        artworkImage = this.GetComponent<Image>();
        artworkImage.sprite = card.cardSprite;
    }

    // public void OnPointerDown(PointerEventData pointerEventData)
    // {
    //     //Output the name of the GameObject that is being clicked
    //     Debug.Log("Card is clicked!");
    //     attached = !attached;
    //     if(!attached) {
    //         this.transform.position = Input.mousePosition;
    //         Hover_Card.canHover = true;
    //     }
    // }

    public void updateCard(Card card)
    {
        artworkImage = this.GetComponent<Image>();
        this.card = card;
        artworkImage.sprite = card.cardSprite;
    }

    // called when a card in deck/holster is clicked
    public void clicked()
    {
        Debug.Log(card.cardName + " has been clicked.");
    }
}
