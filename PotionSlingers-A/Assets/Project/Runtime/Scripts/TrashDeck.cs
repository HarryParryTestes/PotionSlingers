using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class TrashDeck : MonoBehaviour, IDropHandler
{
    public List<Card> deckList;
    public Card card;
    public Slider slider;
    public bool trash = false;

    public GameObject menuUI;

    public List<CardDisplay> cdList;

    public int cardIndex = 0;

    public CardDisplay cd1;
    public CardDisplay cd2;
    public CardDisplay cd3;

    // Start is called before the first frame update
    void Start()
    {
        //cardIndex = 0;
        slider.onValueChanged.AddListener((v) =>
        {
            Debug.Log("Value changed to: " + (int)v);
            cardIndex = (int)v;
            displayCards();
        });
    }

    public void OnDrop(PointerEventData eventData)
    {
        /*
        add in implementation to distinguish between GameObjects
        that have a CardDisplay vs a CharacterDisplay
        */
        // Debug.Log("Drop happened on character");

        GameObject heldCard = eventData.pointerDrag;
        DragCard dc = heldCard.GetComponent<DragCard>();
        // grabbing the card held by the cursor
        CardDisplay grabbedCard = heldCard.GetComponent<CardDisplay>();

        // if the card is a card that is in your holster
        if (!dc.market && grabbedCard != null)
        {
            Debug.Log("Trash triggered?");
            // buy the card
            Debug.Log("Trash");
            GameManager.manager.setSCInt(grabbedCard.card.cardName);
            GameManager.manager.trashCard();
        }
    }

    public void changeTrashBool()
    {
        trash = !trash;

        /*
        Market_Hover m1 = cd1.gameObject.GetComponent<Market_Hover>();
        m1.viewCardMenu.gameObject.SetActive(false);
        m1.cardMenu.gameObject.SetActive(false);
        m1.onlyViewCardMenu.gameObject.SetActive(false);
        m1.scarpettaMenu.gameObject.SetActive(false);

        Market_Hover m2 = cd2.gameObject.GetComponent<Market_Hover>();
        m2.viewCardMenu.gameObject.SetActive(false);
        m2.cardMenu.gameObject.SetActive(false);
        m2.onlyViewCardMenu.gameObject.SetActive(false);
        m2.scarpettaMenu.gameObject.SetActive(false);

        Market_Hover m3 = cd3.gameObject.GetComponent<Market_Hover>();
        m3.viewCardMenu.gameObject.SetActive(false);
        m3.cardMenu.gameObject.SetActive(false);
        m3.onlyViewCardMenu.gameObject.SetActive(false);
        m3.scarpettaMenu.gameObject.SetActive(false);
        */
    }

    public void addCard(Card card)
    {
        deckList.Add(card);
    }

    public void addCard(CardDisplay cd)
    {
        deckList.Add(cd.card);
        if (cd.card.cardType == "Artifact")
        {
            if (cd.aPotion != null && cd.aPotion.card.cardName != "placeholder"){
                deckList.Add(cd.aPotion.card);
                cd.aPotion.updateCard(card);
                cd.aPotion.gameObject.SetActive(false);
            }
        }

        if (cd.card.cardType == "Vessel")
        {
            if (cd.vPotion1 != null && cd.vPotion1.card.cardName != "placeholder")
            {
                deckList.Add(cd.vPotion1.card);
                cd.vPotion1.updateCard(card);
                cd.vPotion1.gameObject.SetActive(false);
                cd.vPotion2.gameObject.SetActive(false);

            }

            if (cd.vPotion2 != null && cd.vPotion2.card.cardName != "placeholder")
            {
                deckList.Add(cd.vPotion2.card);
                cd.vPotion2.updateCard(card);
                cd.vPotion1.gameObject.SetActive(false);
                cd.vPotion2.gameObject.SetActive(false);

            }
        }
        cd.updateCard(card);
    }

    public void displayCards()
    {
        foreach (CardDisplay cd in cdList)
        {
            if (cardIndex > deckList.Count - 1)
            {
                Debug.Log("Reached end of trash");
                cardIndex = 0;
                cd.updateCard(cd.placeholder);
            }
            else
            {
                Card temp = deckList[cardIndex];
                cd.updateCard(temp);
                cardIndex++;
            }
        }
        /*
        foreach(CardDisplay cd in cdList)
        {
            Card temp = deckList[cardIndex];
            cd.updateCard(temp);
            cardIndex++;
        } 
        cardIndex = cardIndex - 2;
        */
    }

    public void displayTrash()
    {
        cardIndex = 0;
        trash = !trash;
        //slider.maxValue = deckList.Count - 3;
        slider.maxValue = 2;

        if (trash)
        {
            menuUI.SetActive(true);
            if (deckList.Count < 3)
            {
                slider.gameObject.SetActive(false);
                foreach (CardDisplay cd in cdList)
                {
                    cd.gameObject.SetActive(true);
                }
                if (deckList.Count == 1)
                {
                    Card temp = deckList[0];
                    cd1.updateCard(temp);
                    cd2.updateCard(cd2.placeholder);
                    cd3.updateCard(cd3.placeholder);
                } else if (deckList.Count == 2)
                {
                    Card temp = deckList[0];
                    cd1.updateCard(temp);
                    Card temp2 = deckList[1];
                    cd2.updateCard(temp2);
                    cd3.updateCard(cd3.placeholder);
                } else if (deckList.Count == 0)
                {
                    cd1.updateCard(cd1.placeholder);
                    cd2.updateCard(cd2.placeholder);
                    cd3.updateCard(cd3.placeholder);
                }
                return;
            }

            slider.maxValue = deckList.Count - 3;
            slider.gameObject.SetActive(true);
            foreach(CardDisplay cd in cdList)
            {
                cd.gameObject.SetActive(true);
            }
            displayCards();       
        }
        else
        {
            menuUI.SetActive(false);
            //slider.gameObject.SetActive(false);
            /*
            foreach (CardDisplay cd in cdList)
            {
                cd.gameObject.SetActive(false);
            }
            */
        }
    }
}


