using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Steamworks;



public class PlayerListItem : MonoBehaviour
{
    public string playerName;
    public int ConnectionId;
    public bool isPlayerReady = false;
    public CSteamID playerSteamId;
    public int playerNumber;

    public string charName = "blank";
    public GamePlayer player;
    public bool computerPlayer = false;

    public GameObject ReadyButton;
    public GameObject NotReadyButton;

    public CharacterDisplay charDisplay;

    [SerializeField] private TMPro.TextMeshProUGUI PlayerNameText;
    [SerializeField] private TMPro.TextMeshProUGUI PlayerReadyStatus;

    public PlayerListItem(string playerName)
    {
        this.playerName = playerName;
        computerPlayer = true;
    }

    public PlayerListItem()
    {
        playerName = SteamFriends.GetPersonaName();
        // something is fucked up about this i gotta figure it out 
        // PlayerNameText.text = MainMenu.menu.greetingName;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetPlayerListItemValues()
    {
        PlayerNameText.text = playerName;
        player.playerName = playerName;
        player.playerSteamId = playerSteamId;
        player.ConnectionId = ConnectionId;
        player.charName = charName;
        //UpdatePlayerItemReadyStatus();
    }

    public void SetSinglePlayerListItemValues()
    {
        playerName = "Computer";
        PlayerNameText.text = playerName;
        player.charName = "Bolo";
        computerPlayer = true;
    }

    // TODO: Make references to the GameObjects used for the Ready and Not Ready buttons and
    // use SetActive() depending on the isPlayerReady boolean
    public void UpdatePlayerItemReadyStatus()
    {
        //isPlayerReady = !isPlayerReady;
        player.ChangeReadyStatus();
        /*
        if (isPlayerReady)
        {
            //PlayerReadyStatus.text = "READY";
            //PlayerReadyStatus.color = Color.green;
        }
        else
        {
            //PlayerReadyStatus.text = "NOT READY";
            //PlayerReadyStatus.color = Color.red;
        }
        */
    }
    
}