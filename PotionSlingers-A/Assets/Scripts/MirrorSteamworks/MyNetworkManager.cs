using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.SceneManagement;

public class MyNetworkManager : NetworkManager
{
    [SerializeField] private GamePlayer gamePlayerPrefab;
    [SerializeField] public int minPlayers = 2;
    public bool tutorial = false;
    public bool multiplayer = false;
    public LobbyManager lobbyManager;
    public SteamLobby steamLobby;
    /*
    public LobbyManager Manager
    {
        get
        {
            if (lobbyManager != null)
            {
                return lobbyManager;
            }
            return lobbyManager = LobbyManager.singleton as LobbyManager;
        }
    }
    */

    public List<GamePlayer> GamePlayers { get; } = new List<GamePlayer>();
    [SerializeField] private PlayerListItem playerListPrefab;
    public List<string> charNames = new List<string>();
    public List<bool> charBools = new List<bool>();
    // Start is called before the first frame update
    public void OnStartServer()
    {
        Debug.Log("Starting Server");
    }
    public void OnClientConnect(NetworkConnectionToClient conn)
    {
        Debug.Log("Client connected.");
        base.OnClientConnect(conn);
    }
    public void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log("Checking if player is in correct scene. Player's scene name is: " + SceneManager.GetActiveScene().name.ToString() + ". Correct scene name is: TitleScreen");
        if (SceneManager.GetActiveScene().name == "TitleScreen")
        {
            bool isGameLeader = GamePlayers.Count == 0; // isLeader is true if the player count is 0, aka when you are the first player to be added to a server/room

            Debug.Log("Instantiating player?");
            GamePlayer GamePlayerInstance = Instantiate(gamePlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            //lobbyManager.localGamePlayerScript = GamePlayerInstance;
            Debug.Log("Connection ID: " + conn.connectionId);
            LobbyManager.instance.playerConnId = conn.connectionId;



            GamePlayerInstance.IsGameLeader = isGameLeader;
            GamePlayerInstance.ConnectionId = conn.connectionId;
            //GamePlayerInstance.ConnectionId = GamePlayers.Count + 1;
            GamePlayerInstance.playerNumber = GamePlayers.Count + 1;

            GamePlayerInstance.playerSteamId = SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.instance.current_lobbyID, GamePlayers.Count);

            NetworkServer.AddPlayerForConnection(conn, GamePlayerInstance.gameObject);
            Debug.Log("Player added. Player name: " + GamePlayerInstance.playerName + ". Player connection id: " + GamePlayerInstance.ConnectionId.ToString());
        }
    }

    /*
    void Update()
    {
        if(lobbyManager == null)
        {
            Debug.Log("Lobby managed");
            lobbyManager = GameObject.Find("OldLobbyMenu").GetComponent<LobbyManager>();
        }
    }
    */

    public void getCharsAndBools()
    {
        // TODO: do this
        // take the items and item them in the items
        foreach (GamePlayer gp in GamePlayers)
        {
            charNames.Add(gp.charName);
            // charBools.Add(hotItem.computerPlayer);
        }
    }

    public void StartSingleplayerGame()
    {
        multiplayer = false;
        tutorial = false;
        ServerChangeScene("GameScene");
    }

    public void StartGame()
    {
        multiplayer = true;
        ServerChangeScene("GameScene");
    }

    public void StartTutorial()
    {
        tutorial = true;
        ServerChangeScene("GameScene");
    }

    private bool CanStartGame()
    {
        if (numPlayers < minPlayers)
            return false;
        foreach (GamePlayer player in GamePlayers)
        {
            if (!player.isPlayerReady)
                return false;
        }
        return true;
    }

    public void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (conn.identity != null)
        {
            GamePlayer player = conn.identity.GetComponent<GamePlayer>();
            GamePlayers.Remove(player);
        }
        base.OnServerDisconnect(conn);
    }

    public void OnStopServer()
    {
        GamePlayers.Clear();
    }
    public void HostShutDownServer()
    {
        GameObject NetworkManagerObject = GameObject.Find("NetworkManager");
        //Destroy(this.GetComponent<SteamManager>());
        Destroy(NetworkManagerObject);
        //NetworkManager.Shutdown();
        //SceneManager.LoadScene("Scene_Steamworks");

        Start();

    }
}