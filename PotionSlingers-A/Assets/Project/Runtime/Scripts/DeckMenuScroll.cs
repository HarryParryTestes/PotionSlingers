using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Steamworks;
using Mirror;
using DG.Tweening;

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

    public CardDatabase database;
    public Holster holster;

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


    /*
     for (int i = 0; i < saveData.playerDeck.Count; i++)
                {
                    foreach (Card card in database.cardList)
                    {
                        if (card.name == saveData.playerDeck[i])
                        {
                            try
                            {
                                if (saveData.deckStatuses[i] == null)
                                    players[0].deck.putCardOnBottom(card, "none");
                                else
                                    players[0].deck.putCardOnBottom(card, saveData.deckStatuses[i]);
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                players[0].deck.putCardOnBottom(card, "none");
                            }

                        }
                    }
                }

    for (int i = 0; i < 4; i++)
                {
                    foreach (Card card in database.cardList)
                    {
                        if (card.name == saveData.playerHolster[i])
                        {
                            players[0].holster.cardList[i].updateCard(card);
                            // add durability here
                            if (saveData.cardDurabilities.Any())
                                players[0].holster.cardList[i].durability = saveData.cardDurabilities[i];
                        }
                    }
                }

    foreach (string thang in saveData.playerLoadedCards[i])
                        {
                            if (thang != "placeholder" && card.cardName == thang)
                            {
                                Debug.Log("updating saved loaded card " + card.cardName + " in card number " + (i + 1));
                                loadIt(playerHolster.cardList[i], card);
                            }
                        }
     */

    public void initHolsterDisplay()
    {
        displayHolsterInStoryModeMenu();
        holster.transform.parent.parent.parent.gameObject.SetActive(true);
        Invoke("displayHolsterInStoryModeMenu", 0.01f);
    }

    public void displayHolsterInStoryModeMenu()
    {
        SaveData saveData = SaveSystem.LoadGameData();

        for (int i = 0; i < holster.cardList.Count; i++)
        {
            foreach (Card card in database.cardList)
            {
                if (card.name == saveData.playerHolster[i])
                {
                    holster.cardList[i].updateCard(card);
                    // add durability here
                    if (saveData.cardDurabilities.Any())
                        holster.cardList[i].durability = saveData.cardDurabilities[i];
                }
            }
        }

        for (int i = 0; i < holster.cardList.Count; i++)
        {
            foreach (Card card in database.cardList)
            {
                foreach (string thang in saveData.playerLoadedCards[i])
                {
                    if (thang != "placeholder" && card.cardName == thang)
                    {
                        Debug.Log("updating saved loaded card " + card.cardName + " in card number " + (i + 1));
                        loadIt(holster.cardList[i], card);
                    }
                }
            }
            
        }

        


    }

    public void displayDeckInStoryModeMenu()
    {
        this.gameObject.SetActive(true);

        Card temp;
        Card temp2;
        Card temp3;

        SaveData saveData = SaveSystem.LoadGameData();

        for (int i = 0; i < saveData.playerDeck.Count; i++)
        {
            foreach (Card card in database.cardList)
            {
                if (card.name == saveData.playerDeck[i])
                {                    
                    deckList.Add(card);
                }
            }
        }

        Debug.Log("Decklist count:" + deckList.Count);

        if (deckList.Count < 4)
        {
            slider.gameObject.SetActive(false);
            slider.maxValue = 0;
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
        }

        foreach (CardDisplay cd in cdList)
        {
            if (cardIndex > deckList.Count - 1)
            {
                Debug.Log("Reached end of trash");
                cardIndex = 0;
                cd.updateCard(cd.placeholder);
                break;
            }
            else
            {
                Card temp4 = deckList[cardIndex];
                cd.updateCard(temp4);
                cardIndex++;
            }
        }
        slider.maxValue = deckList.Count - 3;
        slider.gameObject.SetActive(true);
        cardIndex = 0;
        return;

        cardIndex = 0;
        // trash = !trash;
        //slider.maxValue = deckList.Count - 3;
        slider.maxValue = 2;

        // menuUI.SetActive(true);
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
                temp = deckList[cardIndex];
                cd.updateCard(temp);
                cardIndex++;
            }
        }


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
        }
        else
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

    public IEnumerator showErrorMessage()
    {
        errorText.SetActive(true);
        yield return new WaitForSeconds(2);
        errorText.SetActive(false);

    }

    public void addCardToTrash(CardDisplay cd)
    {
        Debug.Log(cd.card.cardName);

        if (GameManager.manager.miner)
        {
            for (int i = 0; i < GameManager.manager.players[GameManager.manager.myPlayerIndex].deck.deckList.Count; i++)
            {
                if (GameManager.manager.players[GameManager.manager.myPlayerIndex].deck.deckList[i].cardName == cd.card.cardName)
                {
                    Debug.Log("Card found");

                    Card card = GameManager.manager.players[GameManager.manager.myPlayerIndex].deck.deckList[i];
                    GameManager.manager.players[GameManager.manager.myPlayerIndex].deck.deckList.RemoveAt(i);
                    GameManager.manager.players[GameManager.manager.myPlayerIndex].deck.putCardOnTop(card);
                    GameManager.manager.players[GameManager.manager.myPlayerIndex].deck.updateCardSprite();


                    GameManager.manager.miner = false;
                    // GameManager.manager.nicklesAttackMenu.SetActive(true);
                    gameObject.SetActive(false);
                    GameManager.manager.sendMessage("Added a card on top of your Deck!");
                    return;
                }
            }
            return;
        }

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
                GameManager.manager.nicklesAttackMenu.SetActive(true);
                gameObject.SetActive(false);

                // REFACTOR THIS!!! Super old!
                // GameManager.manager.chooseOpponentMenu.SetActive(true);
                // GameManager.manager.displayOpponents();
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
        if (GameManager.manager.miner)
            if(menuUI != null)
                menuUI.GetComponent<TMPro.TextMeshProUGUI>().text = "Put a card on top of your Deck?";           
        else
            if (menuUI != null)
                menuUI.GetComponent<TMPro.TextMeshProUGUI>().text = "Trash a non-starter card?";

        Card temp;
        Card temp2;
        Card temp3;

        if (deck != null)
        {
            Debug.Log("Init with deck");
            initDecklist(deck);
        }
        else
        {
            Debug.Log("Init without deck");
            initDecklist();
        }

        Debug.Log("Decklist count:" + deckList.Count);

        if (deckList.Count < 4)
        {
            slider.gameObject.SetActive(false);
            slider.maxValue = 0;
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
        }

        foreach (CardDisplay cd in cdList)
        {
            if (cardIndex > deckList.Count - 1)
            {
                Debug.Log("Reached end of trash");
                cardIndex = 0;
                cd.updateCard(cd.placeholder);
                break;
            }
            else
            {
                Card temp4 = deckList[cardIndex];
                cd.updateCard(temp4);
                cardIndex++;
            }
        }
        slider.maxValue = deckList.Count - 3;
        slider.gameObject.SetActive(true);
        cardIndex = 0;
        return;

        cardIndex = 0;
        // trash = !trash;
        //slider.maxValue = deckList.Count - 3;
        slider.maxValue = 2;

        // menuUI.SetActive(true);
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
                temp = deckList[cardIndex];
                cd.updateCard(temp);
                cardIndex++;
            }
        }

        
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

    public void loadIt(CardDisplay cd, Card card)
    {
        Debug.Log("Load Potion");

        // if it's an artifact or vessel
        if (card.cardType == "Potion")
        {
            // Loading a Vessel:
            if (cd.card.cardType == "Vessel")
            {
                // Enable Vessel menu if it wasn't already enabled.
                Debug.Log("Vessel menu enabled.");
                cd.vesselSlot1.transform.parent.gameObject.SetActive(true);
                cd.vesselSlot1.SetActive(true);
                cd.vesselSlot2.SetActive(false);
                cd.vesselSlot3.SetActive(false);
                cd.vesselSlot4.SetActive(false);

                if (cd.card.cardEffect == "FourLoad")
                {
                    Debug.Log("Four slot logic");
                }

                // Check for existing loaded potion(s) if Vessel menu was already enabled.
                if (cd.vPotion1.card.cardName != "placeholder")
                {
                    // If Vessel slot 2 is filled.
                    if (cd.vPotion2.card.cardName != "placeholder")
                    {
                        // insert logic for loading the third and fourth potion here                                
                        if (cd.card.cardEffect == "FourLoad")
                        {
                            Debug.Log("Reached here in 3 4 slot");
                            if (cd.vPotion3.card.cardName != "placeholder")
                            {
                                // Fill Vessel slot 4 with loaded potion.
                                cd.vesselSlot1.SetActive(true);
                                cd.vesselSlot2.SetActive(true);
                                cd.vesselSlot3.SetActive(true);
                                cd.vesselSlot4.SetActive(true);
                                Card placeholder = card;
                                cd.vPotion4.card = card;
                                cd.vPotion4.updateCard(card);
                                // FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_Load");

                                // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                                // sendSuccessMessage(5);
                                // players[myPlayerIndex].checkVesselBonusAnimation(cd);
                                // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                                Debug.Log("Potion loaded in Vessel slot 4!");

                                // MATTEO: Add Loading potion SFX here.

                                // // Updates Holster card to be empty.
                                // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card = placeholder;
                                // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(placeholder);
                            }
                            else
                            {
                                // Fill Vessel slot 3 with loaded potion.
                                cd.vesselSlot1.SetActive(true);
                                cd.vesselSlot2.SetActive(true);
                                cd.vesselSlot3.SetActive(true);
                                Card placeholder = cd.vPotion3.card;
                                cd.vPotion3.card = card;
                                cd.vPotion3.updateCard(card);
                                // FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_Load");

                                // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                                // sendSuccessMessage(5);
                                // players[myPlayerIndex].checkVesselBonusAnimation(cd);
                                // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                                Debug.Log("Potion loaded in Vessel slot 3!");

                                // MATTEO: Add Loading potion SFX here.

                                // // Updates Holster card to be empty.
                                // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card = placeholder;
                                // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(placeholder);
                            }
                            return;
                        }

                        // Debug.Log("Vessel is fully loaded!");
                        // DONE: Insert error that displays on screen.
                        // sendErrorMessage(9);
                    }
                    else
                    {
                        // Fill Vessel slot 2 with loaded potion.
                        cd.vesselSlot1.SetActive(true);
                        cd.vesselSlot2.SetActive(true);
                        Card placeholder = cd.vPotion2.card;
                        cd.vPotion2.card = card;
                        cd.vPotion2.updateCard(card);
                        // FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_Load");

                        // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                        // sendSuccessMessage(5);
                        // players[myPlayerIndex].checkVesselBonusAnimation(cd);
                        // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        Debug.Log("Potion loaded in Vessel slot 2!");

                        // MATTEO: Add Loading potion SFX here.

                        // // Updates Holster card to be empty.
                        // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card = placeholder;
                        // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(placeholder);
                    }
                }
                // Vessel slot 1 is unloaded.
                else
                {
                    cd.vesselSlot1.SetActive(true);
                    Card placeholder = cd.vPotion1.card;
                    cd.vPotion1.card = card;
                    cd.vPotion1.updateCard(card);
                    // FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_Load");

                    // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                    // sendSuccessMessage(5);
                    // players[myPlayerIndex].checkVesselBonusAnimation(cd);
                    // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                    Debug.Log("Potion loaded in Vessel slot 1!");

                    // MATTEO: Add Loading potion SFX here.

                    // // Updates Holster card to be empty.
                    // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card = placeholder;
                    // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(placeholder);
                }
            }

            // Loading an Artifact:
            else if (cd.card.cardType == "Artifact")
            {
                // Enable Artifact menu if it wasn't already enabled.
                Debug.Log("Artifact menu enabled.");
                cd.artifactSlot.transform.parent.gameObject.SetActive(true);
                cd.artifactSlot.transform.gameObject.SetActive(true);

                // Check for existing loaded potion if Artifact menu was already enabled.
                if (cd.aPotion.card.cardName != "placeholder")
                {
                    Debug.Log("Artifact is fully loaded!");
                    // DONE: Insert error that displays on screen.
                    // sendErrorMessage(8);
                    // playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                }
                // Artifact slot is unloaded.
                else
                {
                    Card placeholder = cd.aPotion.card;
                    cd.aPotion.card = card;
                    cd.aPotion.updateCard(card);
                    // FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_Load");
                    // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                    // sendSuccessMessage(5);
                    // players[myPlayerIndex].checkArtifactBonusAnimation(cd);
                    Debug.Log("Potion loaded in Artifact slot!");

                    // MATTEO: Add Loading potion SFX here.

                    // // Updates Holster card to be empty.
                    // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card = placeholder;
                    // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(placeholder);
                }
            }
        }
        else
        {
            // add error message
            Debug.Log("That error message...");
            // sendErrorMessage(12);
        }
    }

        /*
        loadedCardInt = loadedInt;
        loadPotion(card);
        Destroy(cardDisplay, 0.2f);
        */
    }