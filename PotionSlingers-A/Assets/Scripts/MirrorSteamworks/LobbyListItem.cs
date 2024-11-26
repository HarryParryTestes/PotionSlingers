using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using Mirror;

public class LobbyListItem : MonoBehaviour
{
    public CSteamID lobbySteamId;
    public string lobbyName;
    public int numberOfPlayers;
    public int maxNumberOfPlayers = 4;

    public GameObject ob;
    public GameObject ob2;

    [SerializeField] private TMPro.TextMeshProUGUI LobbyNameText;
    [SerializeField] private TMPro.TextMeshProUGUI NumberOfPlayersText;

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

    // Start is called before the first frame update
    void Start()
    {
        ob = MainMenu.menu.lobbyMenu;
        ob2 = GameObject.Find("Scroll View");
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetLobbyItemValues()
    {
        LobbyNameText.text = lobbyName;
        NumberOfPlayersText.text = "Players: " + numberOfPlayers.ToString() + "/" + maxNumberOfPlayers.ToString();
    }
    public void JoinLobby()
    {
        Debug.Log("JoinLobby: Player selected to join lobby with steam id of: " + lobbySteamId.ToString());
        Debug.Log("Going to the lobby");
        ob2.SetActive(false);
        ob.SetActive(true);
        numberOfPlayers += 1;
        Game.StartClient();
        SteamLobby.instance.JoinLobby(lobbySteamId);
    }
}