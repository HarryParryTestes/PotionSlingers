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

    [SerializeField] private TMPro.TextMeshProUGUI PlayerNameText;
    [SerializeField] private TMPro.TextMeshProUGUI PlayerReadyStatus;

    

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
    public void UpdatePlayerItemReadyStatus()
    {
        isPlayerReady = !isPlayerReady;
        player.CmdChangePlayerReadyStatus();
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