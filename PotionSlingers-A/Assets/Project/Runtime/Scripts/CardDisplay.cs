using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class CardDisplay : MonoBehaviour
{
    public Card card;
    public UniqueCard uniqueCard;
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

    public GameObject flames;
    public bool spicy;

    // Start is called before the first frame update
    void Start()
    {
        /*
        if(spicyPrefab != null)
        {
            GameObject obj = Instantiate(spicyPrefab, new Vector3(0, -25, 0), Quaternion.identity);
            Debug.Log("OBJ!");
        }
        */

        artworkImage = this.GetComponent<Image>();
        if (uniqueCard != null)
        {
            artworkImage.sprite = uniqueCard.cardSprite;
            return;
        }
        //artworkImage = this.GetComponent<Image>();
        if (card != null)
        {
            artworkImage.sprite = card.cardSprite;
            return;
        }
        
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

    public void makeSpicy()
    {
        card.spicy = true;
        if (flames != null)
            flames.SetActive(true);
    }

    public void updateCard(Card card)
    {
        artworkImage = this.GetComponent<Image>();
        this.card = card;
        artworkImage.sprite = card.cardSprite;

        if (card.spicy)
        {
            if(flames != null)
                flames.SetActive(true);
        }

        else
        {
            if (flames != null)
                flames.SetActive(false);
        }
        // trying something
        // might take this out for now.
        
        if(card.cardName == "placeholder" && this.gameObject.name != "DeckPile")
        {
            if(GetComponent<DragCard>() != null && !GetComponent<DragCard>().market && !GetComponent<DragCard>().loaded)
            {
                GetComponent<DragCard>().beforeDisappear();
            }
                
            this.gameObject.SetActive(false);
            
        }
        else
            this.gameObject.SetActive(true);
        
    }

    public void updateCard(CardDisplay cd)
    {
        artworkImage = this.GetComponent<Image>();
        this.card = cd.card;
        artworkImage.sprite = card.cardSprite;
        if (cd.spicy)
        {
            spicy = true;
            flames.SetActive(true);
        }

        else
        {
            spicy = false;
            flames.SetActive(false);
        }
    }

    public void updateCard(UniqueCard card)
    {
        artworkImage = this.GetComponent<Image>();
        this.uniqueCard = card;
        artworkImage.sprite = uniqueCard.cardSprite;
    }

    public void updateUniqueCard(UniqueCard card)
    {
        artworkImage = this.GetComponent<Image>();
        this.uniqueCard = card;
        artworkImage.sprite = card.cardSprite;
    }

    // called when a card in deck/holster is clicked
    public void clicked()
    {
        Debug.Log(card.cardName + " has been clicked.");
    }
}
