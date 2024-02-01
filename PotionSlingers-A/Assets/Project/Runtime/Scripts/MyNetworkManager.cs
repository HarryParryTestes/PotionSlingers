using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Steamworks;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MyNetworkManager : NetworkManager
{
    [SerializeField] private GamePlayer gamePlayerPrefab;
    [SerializeField] private GameObject PlayerListItemPrefab;
    [SerializeField] public int minPlayers = 2;
    [SerializeField] public int numPlayers = 2;
    public bool tutorial = false;
    public bool multiplayer = false;
    public bool quickplay = false;
    public bool storyMode = false;
    public bool savedGame = false;
    public bool completedTutorial = false;
    public bool completedGame = false;
    public LobbyManager lobbyManager;
    public LobbyManager storyModeLobby;
    public SteamLobby steamLobby;
    public GameObject sceneTransition;
    public GameObject loadingScreen;
    public GameObject loadingText;
    public Animator animator;
    public AnimationHolder canvas;
    public Texture2D texture;
    public Texture2D texture2;
    public string storyModeCharName = "Bolo";
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
    public List<string> singlePlayerNames = new List<string>();
    // Start is called before the first frame update
    public void Start()
    {
        // MAKE SURE YOU PUT THIS BACK AFTER YOU"RE DONE TESTING

        Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
        DontDestroyOnLoad(animator);
        DontDestroyOnLoad(sceneTransition);
        DontDestroyOnLoad(loadingScreen);
    }

    public void checkCompletedTutorial()
    {
        // DontDestroyOnLoad the animator controller
        if (completedTutorial)
        {
            Debug.Log("Completed Tutorial!");
            // TODO: do achievement check in here once you make it in Steamworks
            SteamUserStats.SetAchievement("BEAT_TUTORIAL");
        }
    }

    public void Update()
    {
        /*
        if (Input.GetMouseButton(0))
        {
            Cursor.SetCursor(texture2, Vector2.zero, CursorMode.Auto);
        } else
        {
            Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
        }
        */
    }

    public void pointCursor()
    {
        // switch cursor
        // Debug.Log("setting cursor?");
        Cursor.SetCursor(texture2, Vector2.zero, CursorMode.Auto);
        // Cursor.SetCursor(texture2, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void obsidianCursor()
    {
        // switch cursor
        // Debug.Log("setting cursor?");
        Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
        // Cursor.SetCursor(texture, Vector2.zero, CursorMode.ForceSoftware);
    }

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

    /*
    public void instantiateItem()
    {
        int index = LobbyManager.instance.playerListItems.Count - 1;

        // potentially rework this to only add these items to playerListItems List
        GameObject newPlayerListItem = Instantiate(PlayerListItemPrefab) as GameObject;
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

        // instead of instantiating the objects with the LobbyManager, you want to instantiate them with the NetworkManager instead
    }
    */

    public void openDiscordLink()
    {
        Application.OpenURL("https://discord.gg/xjU69XMZ8W");
    }

    public void openWebsiteLink()
    {
        Application.OpenURL("https://www.potionslingers.com/");
    }

    public void openRulesLink()
    {
        Application.OpenURL("https://www.potionslingers.com/s/Potionslingers-Full-Rules.pdf");
    }

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

    public void StartStoryModeGame()
    {
        multiplayer = false;
        tutorial = false;
        quickplay = false;
        storyMode = true;

        // copy this code from singleplayer implementations
        foreach (PlayerListItem item in storyModeLobby.playerListItems)
        {
            Debug.Log("Adding name");
            // GamePlayer GamePlayerInstance = Instantiate(gamePlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            // check char names
            // GamePlayerInstance.CmdChangeCharacter(item.charName);
            // GamePlayerInstance.charName = item.charName;

            // Debug.Log("Adding GamePlayer with character " + GamePlayerInstance.charName + "!");
            // GamePlayers.Add(GamePlayerInstance);

            PlayerListItem single = Instantiate(item);
            DontDestroyOnLoad(single.gameObject);
            if (single.charName == "blank")
            {
                singlePlayerNames.Add("Bolo");
            }
            else
            {
                singlePlayerNames.Add(single.charName);
            }
            // SinglePlayer singleScript = single.AddComponent<SinglePlayer>();
        }

        StartCoroutine(LoadLevel());
    }

    public void StartSingleplayerGame()
    {
        multiplayer = false;
        tutorial = false;

        // GET CHARNAME INFO FROM LOBBYMANAGER OBJECTS
        // The info should be correct and persist to the next scene

       // GameObject ob = GameObject.Find("Player(Clone)");
       // Debug.Log(ob);

        
        // try to make copy GamePlayers out of the playerListItems List in LobbyManager
        foreach(PlayerListItem item in LobbyManager.instance.playerListItems)
        {
            // GamePlayer GamePlayerInstance = Instantiate(gamePlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            // check char names
            // GamePlayerInstance.CmdChangeCharacter(item.charName);
            // GamePlayerInstance.charName = item.charName;

            // Debug.Log("Adding GamePlayer with character " + GamePlayerInstance.charName + "!");
            // GamePlayers.Add(GamePlayerInstance);

            PlayerListItem single = Instantiate(item);
            DontDestroyOnLoad(single.gameObject);
            if(single.charName == "blank")
            {
                singlePlayerNames.Add("Bolo");
            } else
            {
                singlePlayerNames.Add(single.charName);
            }
            // SinglePlayer singleScript = single.AddComponent<SinglePlayer>();
        }

        // make sure you debug this
        StartCoroutine(LoadLevel());

        // ServerChangeScene("GameScene");
    }

    public IEnumerator LoadLevel()
    {
        canvas = GameObject.Find("Canvas").GetComponent<AnimationHolder>();

        loadingScreen = canvas.loadingScreen;
        loadingText = canvas.loadingText;

        animator.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        if(loadingScreen != null && loadingText != null)
        {
            loadingScreen.SetActive(true);
            loadingText.SetActive(true);
        } 
        /*
        else
        {
            canvas.loadingScreen.SetActive(true);
            canvas.loadingText.SetActive(true);
        }
        */
        animator.SetTrigger("End");
        yield return new WaitForSeconds(2);
        ServerChangeScene("TownCenter");
    }

    public IEnumerator LoadStory()
    {
        canvas = GameObject.Find("Canvas").GetComponent<AnimationHolder>();

        loadingScreen = canvas.loadingScreen;
        loadingText = canvas.loadingText;

        animator.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        if (loadingScreen != null && loadingText != null)
        {
            loadingScreen.SetActive(true);
            loadingText.SetActive(true);
        }
        /*
        else
        {
            canvas.loadingScreen.SetActive(true);
            canvas.loadingText.SetActive(true);
        }
        */
        animator.SetTrigger("End");
        yield return new WaitForSeconds(2);
        ServerChangeScene("StoryMode");
    }

    public void StartGame()
    {
        multiplayer = true;


        StartCoroutine(LoadLevel());
        // ServerChangeScene("GameScene");
    }

    public void StartTutorial()
    {
        tutorial = true;
        StartCoroutine(LoadLevel());
        // ServerChangeScene("GameScene");
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