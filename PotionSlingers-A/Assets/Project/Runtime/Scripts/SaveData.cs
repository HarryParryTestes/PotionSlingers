using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class SaveData
{
    // Gamestate info will be saved here
    // public GameManager saveManager = new GameManager();
    // public TMPro.TextMeshProUGUI saveGameText;
    // public string playerName;
    public string playerCharName;
    // public int playerHealth;
    // public int playerCubes;
    // public List<string> playerDeck = new List<string>();
    // public List<string> playerHolster = new List<string>();
    // public string oppName;
    // public string oppCharName;
    // public int oppHealth;
    // public int oppCubes;
    public int stage;
    public bool savedGame;
    // public List<string> oppDeck = new List<string>();
    // public List<string> oppHolster = new List<string>();

    public SaveData()
    {

    }

    public SaveData(string playerCharName, int stage)
    {
        this.playerCharName = playerCharName;
        this.stage = stage;
    }

    public SaveData (GameManager manager)
    {
        stage = manager.stage;

        // playerName = manager.players[0].name;
        playerCharName = manager.players[0].charName;
        // oppName = manager.players[1].name;
        // oppCharName = manager.players[1].charName;
        // playerHealth = manager.players[0].hp;
        // playerCubes = manager.players[0].hpCubes;

        /*
        foreach(Card card in manager.players[0].deck.deckList)
        {
            playerDeck.Add(card.cardName);
        }

        foreach (CardDisplay cd in manager.players[0].holster.cardList)
        {
            playerHolster.Add(cd.card.cardName);
        }

        // playerDeck = manager.players[0].deck.deckList;
        // playerHolster = manager.players[0].holster.cardList;
        
        oppHealth = manager.players[1].hp;
        oppCubes = manager.players[1].hpCubes;
        foreach (Card card in manager.players[1].deck.deckList)
        {
            oppDeck.Add(card.cardName);
        }

        foreach (CardDisplay cd in manager.players[1].holster.cardList)
        {
            oppHolster.Add(cd.card.cardName);
        }

        */
        // oppDeck = manager.players[2].deck.deckList;
        // oppHolster = manager.players[2].holster.cardList;
    }

    
}
