using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
using Steamworks;
using System.Linq;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    public int numPlayers = 1;

    [Header("UI Elements")]
    [SerializeField] private Text LobbyNameText;
    [SerializeField] private GameObject ContentPanel;
    [SerializeField] private GameObject PlayerListItemPrefab;
    [SerializeField] private Button ReadyUpButton;
    [SerializeField] private Button NotReadyUpButton;
    [SerializeField] private Button StartGameButton;
    public string characterName;

    public bool havePlayerListItemsBeenCreated = false;
    public List<PlayerListItem> playerListItems = new List<PlayerListItem>();
    public List<PlayerListItem> tempPlayerListItems = new List<PlayerListItem>();
    public List<GamePlayer> players = new List<GamePlayer>();
    public GameObject localGamePlayerObject;
    public GamePlayer localGamePlayerScript;
    public int playerConnId;

    public bool singleplayer = false;


    public ulong currentLobbyId;
    // Start is called before the first frame update
    private MyNetworkManager game;
    private MyNetworkManager Game
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

    void Start()
    {
        Debug.Log("Happened in Start in LobbyManager");
        DontDestroyOnLoad(this.gameObject);
    }

    void Awake()
    {
        MakeInstance();
        // ReadyUpButton.gameObject.SetActive(true);
        // ReadyUpButton.GetComponentInChildren<Text>().text = "Ready Up";
        // StartGameButton.gameObject.SetActive(false);
        Debug.Log("Happened in Awake in LobbyManager");
        DontDestroyOnLoad(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {

    }
    void MakeInstance()
    {
        if (instance == null)
            instance = this;
    }

    public void FindLocalPlayer()
    {
        localGamePlayerObject = GameObject.Find("LocalGamePlayer");
        localGamePlayerScript = localGamePlayerObject.GetComponent<GamePlayer>();
    }

    public void changeCharName(string name)
    {
        characterName = name;
        //if (localGamePlayerScript != null)
        //localGamePlayerScript.charName = name;
    }
    
    public void UpdateLobbyName()
    {
        Debug.Log("UpdateLobbyName");
        currentLobbyId = Game.GetComponent<SteamLobby>().current_lobbyID;
        string lobbyName = SteamMatchmaking.GetLobbyData((CSteamID)currentLobbyId, "name");
        Debug.Log("UpdateLobbyName: new lobby name will be: " + lobbyName);
        LobbyNameText.text = lobbyName;
    }
    public void UpdateUI()
    {
        Debug.Log("Executing UpdateUI");
        Debug.Log("players in lobby: " + playerListItems.Count);
        Debug.Log("players on network: " + Game.GamePlayers.Count);
        if (!havePlayerListItemsBeenCreated)
            CreatePlayerListItems();
        if (playerListItems.Count < Game.GamePlayers.Count)
            CreateNewPlayerListItems();
        if (playerListItems.Count > Game.GamePlayers.Count)
            RemovePlayerListItems();
        if (playerListItems.Count == Game.GamePlayers.Count)
            UpdatePlayerListItems();
    }

    public void instantiateItem()
    {
        int index = playerListItems.Count;

        // potentially rework this to only add these items to playerListItems List
        GameObject newPlayerListItem = Instantiate(PlayerListItemPrefab, Game.gameObject.transform) as GameObject;
        DontDestroyOnLoad(newPlayerListItem);
        PlayerListItem newPlayerListItemScript = newPlayerListItem.GetComponent<PlayerListItem>();
        newPlayerListItem.transform.SetParent(gameObject.transform);
        newPlayerListItem.transform.localPosition = new Vector3(-1150 + ((index + 1) * 450), -350, 0);
        // newPlayerListItem.transform.localScale = Vector3.one;
        newPlayerListItem.transform.localScale = new Vector3(.8f, .8f, .8f);
        newPlayerListItemScript.NotReadyButton.SetActive(false);
        // maybe add this back in? just tweak how it looks
        // newPlayerListItemScript.cpuToggleObject.SetActive(true);
        newPlayerListItemScript.SetSinglePlayerListItemValues(index);
        playerListItems.Add(newPlayerListItemScript);

        // instead of instantiating the objects with the LobbyManager, you want to instantiate them with the NetworkManager instead
    }

    public void createCPU()
    {
        Debug.Log("Creating CPU");
        GameObject go2 = new GameObject("CPU Item");
        go2.AddComponent<PlayerListItem>();
        go2.GetComponent<PlayerListItem>().playerName = "Computer";
        // playerListItems.Add(go2.GetComponent<PlayerListItem>());

        instantiateItem();
    }

    public void clearPlayerListItems()
    {

        DestroyPlayerListItems();
        // playerListItems.Clear();

        /*
        while (this.transform.Find("Player(Clone)"))
        {
            Destroy()
        }
        */

    }

    public void CreateSinglePlayerListItems()
    {
        singleplayer = true;
        Game.singlePlayerNames.Clear();

        // initialize player UI, then add computer players as necessary
        StartGameButton.gameObject.SetActive(true);

        GameObject go = new GameObject("Player Item");
        go.AddComponent<PlayerListItem>();
        // go.GetComponent<PlayerListItem>().playerName = SteamFriends.GetPersonaName();
        // go.GetComponent<PlayerListItem>().PlayerNameText.text = go.GetComponent<PlayerListItem>().playerName;
        // go.GetComponent<PlayerListItem>().PlayerNameText.text = "Denzill";

        // be careful, maybe add this back in?
        // playerListItems.Add(go.GetComponent<PlayerListItem>());

        instantiateItem();

        // make the rest of the four players
        createCPU();
        createCPU();
        createCPU();

        Game.numPlayers = 4;
    }

    public void CreateSinglePlayerListTwoItems()
    {
        singleplayer = true;
        Game.singlePlayerNames.Clear();

        // initialize player UI, then add computer players as necessary
        StartGameButton.gameObject.SetActive(true);

        GameObject go = new GameObject("Player Item");
        go.AddComponent<PlayerListItem>();
        // go.GetComponent<PlayerListItem>().playerName = SteamFriends.GetPersonaName();
        // go.GetComponent<PlayerListItem>().PlayerNameText.text = go.GetComponent<PlayerListItem>().playerName;
        // go.GetComponent<PlayerListItem>().PlayerNameText.text = "Denzill";

        // playerListItems.Add(go.GetComponent<PlayerListItem>());

        instantiateItem();

        // make the rest of the four players
        createCPU();
        createCPU();
        //createCPU();

        Game.numPlayers = 3;
    }

    public void CreateSinglePlayerListOneItems()
    {
        singleplayer = true;
        Game.singlePlayerNames.Clear();

        // initialize player UI, then add computer players as necessary
        StartGameButton.gameObject.SetActive(true);

        GameObject go = new GameObject("Player Item");
        go.AddComponent<PlayerListItem>();
        // go.GetComponent<PlayerListItem>().playerName = SteamFriends.GetPersonaName();
        // go.GetComponent<PlayerListItem>().PlayerNameText.text = go.GetComponent<PlayerListItem>().playerName;
        // go.GetComponent<PlayerListItem>().PlayerNameText.text = "Denzill";

        // playerListItems.Add(go.GetComponent<PlayerListItem>());

        instantiateItem();

        createCPU();
        //createCPU();
        //createCPU();

        Game.numPlayers = 2;
    }

    private void CreatePlayerListItems()
    {
        singleplayer = false;

        Debug.Log("Executing CreatePlayerListItems. This many players to create: " + Game.GamePlayers.Count.ToString());

        for (int i = 0; i < Game.GamePlayers.Count; i++)
        {
            Debug.Log("CreatePlayerListItems: Creating playerlistitem for player: " + Game.GamePlayers[i].playerName);
            GameObject newPlayerListItem = Instantiate(PlayerListItemPrefab) as GameObject;
            PlayerListItem newPlayerListItemScript = newPlayerListItem.GetComponent<PlayerListItem>();

            newPlayerListItemScript.playerName = Game.GamePlayers[i].playerName;
            //newPlayerListItemScript.ConnectionId = Game.GamePlayers[i].ConnectionId;
            newPlayerListItemScript.ConnectionId = i + 1;
            newPlayerListItemScript.isPlayerReady = Game.GamePlayers[i].isPlayerReady;
            newPlayerListItemScript.playerSteamId = Game.GamePlayers[i].playerSteamId;
            newPlayerListItemScript.charName = Game.GamePlayers[i].charName;
            newPlayerListItemScript.playerNumber = i + 1;
            newPlayerListItemScript.SetPlayerListItemValues();


            newPlayerListItem.transform.SetParent(gameObject.transform);
            Debug.Log("Number of GamePlayers in NetworkManager: " + Game.GamePlayers.Count);
            newPlayerListItem.transform.localPosition = new Vector3(-950 + ((i + 1) * 450), -350, 0);
            //newPlayerListItem.transform.localScale = Vector3.one;
            newPlayerListItem.transform.localScale = new Vector3(.8f, .8f, .8f);

            playerListItems.Add(newPlayerListItemScript);
        }
        /*
        foreach (GamePlayer player in Game.GamePlayers)
        {
            Debug.Log("CreatePlayerListItems: Creating playerlistitem for player: " + player.playerName);
            GameObject newPlayerListItem = Instantiate(PlayerListItemPrefab) as GameObject;
            PlayerListItem newPlayerListItemScript = newPlayerListItem.GetComponent<PlayerListItem>();

            newPlayerListItemScript.playerName = player.playerName;
            newPlayerListItemScript.ConnectionId = player.ConnectionId;
            newPlayerListItemScript.isPlayerReady = player.isPlayerReady;
            newPlayerListItemScript.playerSteamId = player.playerSteamId;
            newPlayerListItemScript.charName = player.charName;
            newPlayerListItemScript.SetPlayerListItemValues();


            newPlayerListItem.transform.SetParent(gameObject.transform);
            Debug.Log("Number of GamePlayers in NetworkManager: " + Game.GamePlayers.Count);
            newPlayerListItem.transform.localPosition = new Vector3(-950 + (player.playerNumber * 300), -350, 0);
            newPlayerListItem.transform.localScale = Vector3.one;

            playerListItems.Add(newPlayerListItemScript);
        }
        */

        havePlayerListItemsBeenCreated = true;
    }
    public void CreateNewPlayerListItems()
    {
        Debug.Log("Executing CreateNewPlayerListItems");

        for (int i = 0; i < Game.GamePlayers.Count; i++)
        {
            // changing this if statement from checking the connection id to checking for unique steam ids
            if (!playerListItems.Any(b => b.playerSteamId == Game.GamePlayers[i].playerSteamId))
            {
                Debug.Log("Unique Steam ID found, creating UI for player");
                Debug.Log("CreateNewPlayerListItems: Player not found in playerListItems: " + Game.GamePlayers[i].playerName);
                GameObject newPlayerListItem = Instantiate(PlayerListItemPrefab) as GameObject;
                PlayerListItem newPlayerListItemScript = newPlayerListItem.GetComponent<PlayerListItem>();

                newPlayerListItemScript.playerName = Game.GamePlayers[i].playerName;
                newPlayerListItemScript.ConnectionId = Game.GamePlayers[i].ConnectionId;
                newPlayerListItemScript.isPlayerReady = Game.GamePlayers[i].isPlayerReady;
                newPlayerListItemScript.playerSteamId = Game.GamePlayers[i].playerSteamId;
                newPlayerListItemScript.charName = Game.GamePlayers[i].charName;
                newPlayerListItemScript.SetPlayerListItemValues();


                newPlayerListItem.transform.SetParent(ContentPanel.transform);
                Debug.Log("Number of GamePlayers in NetworkManager: " + Game.GamePlayers.Count);
                newPlayerListItem.transform.localPosition = new Vector3(-950 + ((i + 1) * 450), -350, 0);
                //newPlayerListItem.transform.localScale = Vector3.one;
                newPlayerListItem.transform.localScale = new Vector3(.8f, .8f, .8f);
                

                playerListItems.Add(newPlayerListItemScript);
            }
        }

        /*
        foreach (GamePlayer player in Game.GamePlayers)
        {
            if (!playerListItems.Any(b => b.ConnectionId == player.ConnectionId))
            {
                Debug.Log("CreateNewPlayerListItems: Player not found in playerListItems: " + player.playerName);
                GameObject newPlayerListItem = Instantiate(PlayerListItemPrefab) as GameObject;
                PlayerListItem newPlayerListItemScript = newPlayerListItem.GetComponent<PlayerListItem>();

                newPlayerListItemScript.playerName = player.playerName;
                newPlayerListItemScript.ConnectionId = player.ConnectionId;
                newPlayerListItemScript.isPlayerReady = player.isPlayerReady;
                newPlayerListItemScript.playerSteamId = player.playerSteamId;
                newPlayerListItemScript.charName = player.charName;
                newPlayerListItemScript.SetPlayerListItemValues();


                newPlayerListItem.transform.SetParent(ContentPanel.transform);
                Debug.Log("Number of GamePlayers in NetworkManager: " + Game.GamePlayers.Count);
                newPlayerListItem.transform.localPosition = new Vector3(-950 + (player.playerNumber * 300), -350, 0);
                newPlayerListItem.transform.localScale = Vector3.one;

                playerListItems.Add(newPlayerListItemScript);
            }
        }
        */

    }
    private void RemovePlayerListItems()
    {
        // make this not fuck with singleplayer
        if (singleplayer)
        {
            return;
        }

        List<PlayerListItem> playerListItemsToRemove = new List<PlayerListItem>();
        foreach (PlayerListItem playerListItem in playerListItems)
        {
            if (!Game.GamePlayers.Any(b => b.ConnectionId == playerListItem.ConnectionId))
            {
                Debug.Log("RemovePlayerListItems: player list item fro connection id: " + playerListItem.ConnectionId.ToString() + " does not exist in the game players list");
                playerListItemsToRemove.Add(playerListItem);
            }
        }
        if (playerListItemsToRemove.Count > 0)
        {
            foreach (PlayerListItem playerListItemToRemove in playerListItemsToRemove)
            {
                GameObject playerListItemToRemoveObject = playerListItemToRemove.gameObject;
                playerListItems.Remove(playerListItemToRemove);
                Destroy(playerListItemToRemoveObject);
                playerListItemToRemoveObject = null;
            }
        }
    }
    private void UpdatePlayerListItems()
    {
        Debug.Log("Executing UpdatePlayerListItems");
        foreach (GamePlayer player in Game.GamePlayers)
        {
            foreach (PlayerListItem playerListItemScript in playerListItems)
            {
                // also changing this to check steam ids for the same reason as the other problem
                if (playerListItemScript.playerSteamId == player.playerSteamId)
                {
                    playerListItemScript.playerName = player.playerName;
                    playerListItemScript.isPlayerReady = player.isPlayerReady;
                    playerListItemScript.SetPlayerListItemValues();
                    if (player == localGamePlayerScript)
                    {
                        Debug.Log(player.playerName + " is the local player...");
                        playerListItemScript.playerName = player.playerName;
                        playerListItemScript.isPlayerReady = player.isPlayerReady;
                    }
                }
            }
        }
        CheckIfAllPlayersAreReady();
    }
    public void PlayerReadyUp()
    {
        Debug.Log("Executing PlayerReadyUp");
        localGamePlayerScript.ChangeReadyStatus();
    }
    void ChangeReadyUpButtonText()
    {
        if (localGamePlayerScript.isPlayerReady)
            ReadyUpButton.GetComponentInChildren<Text>().text = "NOT READY";
        else
            ReadyUpButton.GetComponentInChildren<Text>().text = "READY";
    }
    public void CheckIfAllPlayersAreReady()
    {
        Debug.Log("Executing CheckIfAllPlayersAreReady");
        bool areAllPlayersReady = false;
        // changing this to ensure that the proper fields are changed for UI in lobby manager

        for (int i = 0; i < Game.GamePlayers.Count; i++)
        {
            // add code in here to change lobby UI's charDisplay
            LobbyManager.instance.playerListItems[i].charDisplay.onCharacterClick(Game.GamePlayers[i].charName);
            if (Game.GamePlayers[i].isPlayerReady)
            {
                LobbyManager.instance.playerListItems[i].NotReadyButton.SetActive(false);
                LobbyManager.instance.playerListItems[i].ReadyButton.SetActive(true);
                areAllPlayersReady = true;
                Game.multiplayer = true;
            } else
            {
                LobbyManager.instance.playerListItems[i].NotReadyButton.SetActive(true);
                LobbyManager.instance.playerListItems[i].ReadyButton.SetActive(false);
                areAllPlayersReady = false;
            }
        }

        /*
        foreach (GamePlayer player in Game.GamePlayers)
        {
            if (player.isPlayerReady)
            {
                areAllPlayersReady = true;
            }
            else
            {
                Debug.Log("CheckIfAllPlayersAreReady: Not all players are ready. Waiting for: " + player.playerName);
                areAllPlayersReady = false;
                break;
            }
        }
        */

        if (areAllPlayersReady)
        {
            Debug.Log("CheckIfAllPlayersAreReady: All players are ready!");
            if (localGamePlayerScript.IsGameLeader)
            {
                Debug.Log("CheckIfAllPlayersAreReady: Local player is the game leader. They can start the game now.");
                StartGameButton.gameObject.SetActive(true);
            }
        }
        else
        {
            if (StartGameButton.gameObject.activeInHierarchy)
                StartGameButton.gameObject.SetActive(false);
        }
    }
    public void DestroyPlayerListItems()
    {
        foreach (PlayerListItem playerListItem in playerListItems)
        {
            GameObject playerListItemObject = playerListItem.gameObject;
            Destroy(playerListItemObject);
            playerListItemObject = null;
        }
        playerListItems.Clear();
    }
    public void StartGame()
    {
        localGamePlayerScript.CanLobbyStartGame();
    }
    public void PlayerQuitLobby()
    {
        Game.multiplayer = false;
        localGamePlayerScript.QuitLobby();
    }

}