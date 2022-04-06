using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniqueCardDisplay : MonoBehaviour
{
    public UniqueCard card;
    public Image artworkImage;
    // Start is called before the first frame update
    void Start()
    {
        artworkImage = this.GetComponent<Image>();
        artworkImage.sprite = card.cardSprite;
    }
}
