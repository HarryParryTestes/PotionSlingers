using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

public class LobbyListItem : MonoBehaviour
{
    public CSteamID lobbySteamId;
    public string lobbyName;
    public int numberOfPlayers;
    public int maxNumberOfPlayers = 4;

    [SerializeField] private TMPro.TextMeshProUGUI LobbyNameText;
    [SerializeField] private TMPro.TextMeshProUGUI NumberOfPlayersText;
    public GameObject charMenu;

    // Start is called before the first frame update
    void Start()
    {
        charMenu = GameObject.Find("JoinLobbyCharacterMenu");
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
        numberOfPlayers += 1;
        SteamLobby.instance.JoinLobby(lobbySteamId);
    }

    public void setCharMenuActive()
    {
        charMenu.SetActive(true);
    }
}