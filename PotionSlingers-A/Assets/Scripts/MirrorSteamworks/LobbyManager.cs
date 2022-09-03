using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
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
    private List<PlayerListItem> playerListItems = new List<PlayerListItem>();
    public List<GamePlayer> players = new List<GamePlayer>();
    public GameObject localGamePlayerObject;
    public GamePlayer localGamePlayerScript;
    public int playerConnId;


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
    void Awake()
    {
        MakeInstance();
        // ReadyUpButton.gameObject.SetActive(true);
        // ReadyUpButton.GetComponentInChildren<Text>().text = "Ready Up";
        // StartGameButton.gameObject.SetActive(false);
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
    private void CreatePlayerListItems()
    {
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
                    //playerListItemScript.playerName = player.playerName;
                    //playerListItemScript.isPlayerReady = player.isPlayerReady;
                    //playerListItemScript.SetPlayerListItemValues();
                    if (player == localGamePlayerScript)
                    {
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
        localGamePlayerScript.QuitLobby();
    }

}