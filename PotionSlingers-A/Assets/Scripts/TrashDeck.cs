using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashDeck : MonoBehaviour
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

    public void changeTrashBool()
    {
        trash = !trash;
    }

    public void addCard(CardDisplay cd)
    {
        deckList.Add(cd.card);
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


