using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;

[System.Serializable]
public class MarketDeck : NetworkBehaviour
{
    // the deck is uninitialized to begin with
    public List<Card> deckList;
    public List<Card> tempDeckList;
    public int cardInt;
    public CardDisplay cardDisplay1;
    public CardDisplay cardDisplay2;
    public CardDisplay cardDisplay3;
    public CardDisplay cardDisplay4;
    private Sprite sprite;
    private static System.Random rng = new System.Random();
   
    void Start()
    {

    }     

    public Card popCard()
    {
        if (deckList.Count > 1)
        {
            Card temp = deckList[0];
            deckList.RemoveAt(0);
            //Debug.Log("Card popped: ");
            // Early Bird Special logic
            if(temp.cardName == "EarlyBirdSpecial")
            {
                GameManager.manager.earlyBirdSpecial = true;
            }            

            return temp;
        }
        else
        {
            // implement refilling the deck here
            Debug.Log("ERROR: Card not returned!");

            Debug.Log("This triggered");
            foreach (Card card in tempDeckList)
            {
                deckList.Add(card);
            }
            deckList = shuffle(deckList);
            Card temp = deckList[0];
            deckList.RemoveAt(0);
            //Debug.Log("Card popped: ");
            // Early Bird Special logic
            if (temp.cardName == "EarlyBirdSpecial")
            {
                GameManager.manager.earlyBirdSpecial = true;
            }
            return temp;
            // return cardDisplay1.card;
        } 
    }

    public List<Card> shuffle(List<Card> list)
    {

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Card value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }

    /*
    public void updateCardSprite(CardDisplay cardDisplay)
    {
        Card card = popCard(); 
        cardDisplay.card = card;
        cardDisplay.artworkImage.sprite = cardDisplay.card.cardSprite;
    }
    */

    // shuffle method kind of sucked before
    // gonna try fisher-yates implementation
    // wow that works so well
    public List<Card> shuffle()
    {
        SaveData saveData = SaveSystem.LoadGameData();

        // story mode check
        // WHEN YOU ADD OTHER SLINGER NPCS CHANGE THIS!!!
        if (GameManager.manager.Game.storyMode && (saveData.currentEnemyName != "Saltimbocca"))
        {
            for(int i = 0; i < deckList.Count; i++)
            {
                if (GameManager.manager.database.GetComponent<CardDatabase>().trashBonusCards.Contains(deckList[i]))
                {
                    Debug.Log("Trash card! Delete it!");
                    deckList.RemoveAt(i);
                }
            }
        }

        int n = deckList.Count;
        while (n > 0)
        {
            n--;
            int k = rng.Next(n + 1);
            Card value = deckList[k];
            deckList[k] = deckList[n];
            deckList[n] = value;
        }

        return deckList;
    }

    public void initCardDisplays()
    {
        Debug.Log("This triggered");
        foreach (Card card in deckList)
        {
            tempDeckList.Add(card);
        }
        tempDeckList = shuffle(tempDeckList);

        Card card1 = popCard();
        Card card2 = popCard();
        Card card3 = popCard();
        cardDisplay1.updateCard(card1);
        cardDisplay2.updateCard(card2);
        cardDisplay3.updateCard(card3);
        cardDisplay4.updateCard(deckList[0]);
        cardDisplay4.fadeMarketCard();
    }

    // Start is called before the first frame update
    public void init()
    {
        // bye bye shuffle :(

        //deckList = shuffle();
        //for(int i = 0; i < deckList.Count; i++)
        //{
        //    Debug.Log("Card " + i + ": " + deckList[i]);
        //}
        initCardDisplays();
    }

    public void setCardInt(int card)
    {
        cardInt = card;
    }
}
