using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
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

    public GameObject errorText;

    public MyNetworkManager game;
    public MyNetworkManager Game
    {
        get
        {
            if (game != null)
            {
                return game;
            }
            return game = MyNetworkManager.singleton as MyNetworkManager;
        }
    }

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

    public IEnumerator showErrorMessage()
    {
        errorText.SetActive(true);
        yield return new WaitForSeconds(2);
        errorText.SetActive(false);

    }

    public void addCardToTrash(CardDisplay cd)
    {
        Debug.Log(cd.card.cardName);

        // Check if it's a starter card
        if(cd.card.cardQuality == "Starter")
        {
            Debug.Log("Error! Add UI in the DeckScrollMenu to show this!");
            StartCoroutine(showErrorMessage());
            return;
        }

        for(int i = 0; i < GameManager.manager.players[GameManager.manager.myPlayerIndex].deck.deckList.Count; i++)
        {
            if(GameManager.manager.players[GameManager.manager.myPlayerIndex].deck.deckList[i].cardName == cd.card.cardName)
            {
                Debug.Log("Card found");
                if (GameManager.manager.Game.multiplayer)
                {
                    GameManager.manager.snakeBonus = true;
                    // trigger Command that removes card from that player's deck
                    foreach (GamePlayer gp in Game.GamePlayers)
                    {
                        // if the steam usernames match
                        if (gp.playerName == GameManager.manager.currentPlayerName)
                        {
                            Debug.Log("Starting Mirror CmdTrashCardInDeck");
                            // do the Mirror Command
                            gp.CmdTrashCardInDeck(i);
                        }
                    }
                } else
                {
                    GameManager.manager.td.addCard(GameManager.manager.players[GameManager.manager.myPlayerIndex].deck.deckList[i]);
                    GameManager.manager.players[GameManager.manager.myPlayerIndex].deck.deckList.RemoveAt(i);
                    GameManager.manager.players[GameManager.manager.myPlayerIndex].deck.updateCardSprite();
                }
                
                GameManager.manager.snakeBonus = true;
                gameObject.SetActive(false);
                GameManager.manager.chooseOpponentMenu.SetActive(true);
                GameManager.manager.displayOpponents();
                return;
            }
        }
    }

    public void initDecklist()
    {
        deckList.Clear();

        foreach(Card cd in GameManager.manager.players[GameManager.manager.myPlayerIndex].deck.deckList)
        {
            deckList.Add(cd);
        }
    }

    public void initDecklist(Deck deck)
    {
        Debug.Log("DECK DECK DECK");

        deckList.Clear();

        foreach (Card cd in deck.deckList)
        {
            deckList.Add(cd);
        }
    }

    public void displayCards(Deck deck = null)
    {
        Card temp;
        Card temp2;
        Card temp3;

        if (deck != null)
        {
            initDecklist(deck);
        } else
            initDecklist();

        Debug.Log("Decklist count:" + deckList.Count);

        cardIndex = 0;
        // trash = !trash;
        //slider.maxValue = deckList.Count - 3;
        slider.maxValue = 2;

        // menuUI.SetActive(true);
        if (deckList.Count < 4)
        {
            slider.gameObject.SetActive(false);
            foreach (CardDisplay cd in cdList)
            {
                cd.gameObject.SetActive(true);
            }
            if (deckList.Count == 1)
            {
                temp = deckList[0];
                cd1.updateCard(temp);
                cd2.updateCard(cd2.placeholder);
                cd3.updateCard(cd3.placeholder);
            }
            else if (deckList.Count == 2)
            {
                temp = deckList[0];
                cd1.updateCard(temp);
                temp2 = deckList[1];
                cd2.updateCard(temp2);
                cd3.updateCard(cd3.placeholder);
            }
            else if (deckList.Count == 3)
            {
                temp = deckList[0];
                cd1.updateCard(temp);
                temp2 = deckList[1];
                cd2.updateCard(temp2);
                temp3 = deckList[2];
                cd3.updateCard(temp3);
            }
            else if (deckList.Count == 0)
            {
                cd1.updateCard(cd1.placeholder);
                cd2.updateCard(cd2.placeholder);
                cd3.updateCard(cd3.placeholder);
            }
            return;
        } else
        {
            temp = deckList[0];
            cd1.updateCard(temp);
            temp2 = deckList[1];
            cd2.updateCard(temp2);
            temp3 = deckList[2];
            cd3.updateCard(temp3);
        }

        

        slider.maxValue = deckList.Count - 3;
        slider.gameObject.SetActive(true);
        foreach (CardDisplay cd in cdList)
        {
            cd.gameObject.SetActive(true);
        }
        // displayCards();




    }
}