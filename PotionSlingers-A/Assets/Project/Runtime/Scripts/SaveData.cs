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
    public int playerHealth = 10;
    public int maxHealth = 10;
    public int pipCount = 6;
    public int playerCubes = 3;
    public int currencyCubes = 0;
    public int shields = 0;
    public bool canBeFlipped;
    public bool flipped;
    public bool transition;
    public bool carnivalWin = false;
    public List<string> playerDeck = new List<string>();
    public List<string> playerHolster = new List<string>();
    public List<string> playerFashion = new List<string>();
    public List<string> potionDeck = new List<string>();
    public List<string> itemDeck = new List<string>();
    public List<string> trashDeck = new List<string>();
    public List<string[]> playerLoadedCards = new List<string[]>();
    // public Dictionary<string, string[]> playerHolsterDictionary = new Dictionary<string, string[]>();
    public List<int> cardDurabilities = new List<int>();
    public List<string> cardStatuses = new List<string>();
    public List<string> marketStatuses = new List<string>();
    public List<string> marketCards = new List<string>();
    public List<string[]> marketCrucibleCards = new List<string[]>();
    public List<string> deckStatuses = new List<string>();
    // public string oppName;
    // public string oppCharName;
    public int opp1Health;
    public int opp1Cubes;
    public int opp1Shields = 0;
    public bool opp1Dead = false;
    public int opp2Health;
    public int opp2Cubes;
    public int opp2Shields = 0;
    public bool opp2Dead = false;
    public int opp3Health;
    public int opp3Cubes;
    public int opp3Shields = 0;
    public bool opp3Dead = false;
    public int stage;
    public int slingerEncounter;
    public bool savedGame;
    public bool newStage;
    public bool selectedStage;
    public string currentEnemyName = "Dippit";
    public string slingerName;
    public List<string> visitedEnemies = new List<string>();
    // public List<string> oppDeck = new List<string>();
    // public List<string> oppHolster = new List<string>();

    public SaveData()
    {

    }

    public SaveData(string playerCharName, int stage, string name)
    {
        this.playerCharName = playerCharName;
        this.stage = stage;
        this.currentEnemyName = name;
        playerCubes = 3;
        playerHealth = 10;
    }

    public SaveData(string playerCharName, int stage)
    {
        this.playerCharName = playerCharName;
        this.stage = stage;
        playerCubes = 3;
        playerHealth = 10;
    }

    public SaveData (GameManager manager)
    {
        stage = manager.stage;

        playerCharName = manager.players[0].charName;
    }

    
}
