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
    public string playerName;
    public string playerCharName;
    public int playerHealth;
    public int playerCubes;
    public List<string> playerDeck = new List<string>();
    public List<string> playerHolster = new List<string>();
    public string oppName;
    public string oppCharName;
    public int oppHealth;
    public int oppCubes;
    public List<string> oppDeck = new List<string>();
    public List<string> oppHolster = new List<string>();

    public SaveData (GameManager manager)
    {
        playerName = manager.players[0].name;
        playerCharName = manager.players[0].charName;
        playerHealth = manager.players[0].hp;
        playerCubes = manager.players[0].hpCubes;

        foreach(Card card in manager.players[0].deck.deckList)
        {
            playerDeck.Add(card.cardName);
        }
        // playerDeck = manager.players[0].deck.deckList;
        // playerHolster = manager.players[0].holster.cardList;
        oppName = manager.players[2].name;
        oppCharName = manager.players[2].charName;
        oppHealth = manager.players[2].hp;
        oppCubes = manager.players[2].hpCubes;
        foreach (Card card in manager.players[2].deck.deckList)
        {
            oppDeck.Add(card.cardName);
        }
        // oppDeck = manager.players[2].deck.deckList;
        // oppHolster = manager.players[2].holster.cardList;
    }

    
}
