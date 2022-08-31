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
    public LobbyManager lobbyManager;
    public List<GamePlayer> GamePlayers { get; } = new List<GamePlayer>();
    [SerializeField] private PlayerListItem playerListPrefab;
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
            bool isGameLeader = GamePlayers.Count == 1; // isLeader is true if the player count is 0, aka when you are the first player to be added to a server/room

            Debug.Log("Instantiating player?");
            GamePlayer GamePlayerInstance = Instantiate(gamePlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            lobbyManager.localGamePlayerScript = GamePlayerInstance;



            GamePlayerInstance.IsGameLeader = isGameLeader;
            GamePlayerInstance.ConnectionId = conn.connectionId;
            GamePlayerInstance.playerNumber = GamePlayers.Count + 1;

            GamePlayerInstance.playerSteamId = SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.instance.current_lobbyID, GamePlayers.Count);

            NetworkServer.AddPlayerForConnection(conn, GamePlayerInstance.gameObject);
            Debug.Log("Player added. Player name: " + GamePlayerInstance.playerName + ". Player connection id: " + GamePlayerInstance.ConnectionId.ToString());
        }
    }
    public void StartGame()
    {  
            ServerChangeScene("GameScene");
            GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            gameManager.numPlayers = 2;
            gameManager.init();
    }

    public void StartTutorial()
    {
        ServerChangeScene("GameScene");
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.numPlayers = GamePlayers.Count;
        gameManager.initPlayersTutorial();
        gameManager.initDecks();
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