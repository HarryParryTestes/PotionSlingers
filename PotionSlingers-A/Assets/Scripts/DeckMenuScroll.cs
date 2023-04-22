using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckMenuScroll : MonoBehaviour
{
    public List<Card> deckList;
    public Card card;
    public Slider slider;

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

    public void addCardToTrash(CardDisplay cd)
    {
        Debug.Log(cd.card.cardName);
        for(int i = 0; i < GameManager.manager.players[GameManager.manager.myPlayerIndex].deck.deckList.Count; i++)
        {
            if(GameManager.manager.players[GameManager.manager.myPlayerIndex].deck.deckList[i].cardName == cd.card.cardName)
            {
                Debug.Log("Card found");
                GameManager.manager.players[GameManager.manager.myPlayerIndex].deck.deckList.RemoveAt(i);
                GameManager.manager.snakeBonus = true;
                GameManager.manager.chooseOpponentMenu.SetActive(true);
                GameManager.manager.displayOpponents();
                return;
            }
        }
        // update with placeholder
    }

    public void initDecklist()
    {
        deckList.Clear();

        foreach(Card cd in GameManager.manager.players[GameManager.manager.myPlayerIndex].deck.deckList)
        {
            deckList.Add(cd);
        }
    }

    public void displayCards()
    {
        initDecklist();
        cardIndex = 0;
        // trash = !trash;
        //slider.maxValue = deckList.Count - 3;
        slider.maxValue = 2;

        // menuUI.SetActive(true);
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
            }
            else if (deckList.Count == 2)
            {
                Card temp = deckList[0];
                cd1.updateCard(temp);
                Card temp2 = deckList[1];
                cd2.updateCard(temp2);
                cd3.updateCard(cd3.placeholder);
            }
            else if (deckList.Count == 0)
            {
                cd1.updateCard(cd1.placeholder);
                cd2.updateCard(cd2.placeholder);
                cd3.updateCard(cd3.placeholder);
            }
            return;
        }

        slider.maxValue = deckList.Count - 3;
        slider.gameObject.SetActive(true);
        foreach (CardDisplay cd in cdList)
        {
            cd.gameObject.SetActive(true);
        }
        displayCards();




    }
}