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
    public bool isPlayerReady;
    public ulong playerSteamId;

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
        UpdatePlayerItemReadyStatus();
    }
    public void UpdatePlayerItemReadyStatus()
    {
        isPlayerReady = !isPlayerReady;
        if (isPlayerReady)
        {
            PlayerReadyStatus.text = "Ready";
            PlayerReadyStatus.color = Color.green;
        }
        else
        {
            PlayerReadyStatus.text = "Not Ready";
            PlayerReadyStatus.color = Color.red;
        }
    }
    
}