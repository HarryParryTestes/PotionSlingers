using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    public TMPro.TextMeshProUGUI currentPlayer;
    public TMPro.TextMeshProUGUI numPlayers;
    public TMPro.TextMeshProUGUI character;
    public TMPro.TextMeshProUGUI bonuses;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Get debug info from static instance of GameManager
    // this may change depending on what Charles implements

    public void getDebugInfo()
    {
        Debug.Log(GameManager.manager.numPlayers);
        currentPlayer.text = "Current Player: " + GameManager.manager.currentPlayerName;
        numPlayers.text = GameManager.manager.numPlayers + " players";
        // character.text = "Current Character: " + GameManager.manager.players[GameManager.manager.myPlayerIndex].character.character.cardName;
        character.text = "Current Character: " + GameManager.manager.players[GameManager.manager.myPlayerIndex].charName;

        // check for bonuses on current player
        // TODO: add check for bonuses
        string bonus = "";
        // GameManager.manager.players[GameManager.manager.myPlayerIndex].charName
        // bonuses.text = "Bonuses: " + bonus;


    }
}
