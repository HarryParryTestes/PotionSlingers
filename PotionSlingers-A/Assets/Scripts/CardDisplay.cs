using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public Image artworkImage;

    // Start is called before the first frame update
    void Start()
    {
        artworkImage = this.GetComponent<Image>();
        artworkImage.sprite = card.cardSprite;
    }

    public void setCardImage()
    {
        artworkImage = this.GetComponent<Image>();
        artworkImage.sprite = card.cardSprite;
    }

    // called when a card in deck/holster is clicked
    public void clicked()
    {
        Debug.Log(card.cardName + " has been clicked.");
    }
}
