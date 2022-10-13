using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using Steamworks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePlayer : NetworkBehaviour
{
    [Header("GamePlayer Info")]
    [SyncVar(hook = nameof(HandlePlayerNameUpdate))] public string playerName;
    [SyncVar] public int ConnectionId;
    [SyncVar] public int playerNumber;
    public int charIndex = 0;
    [SyncVar(hook = nameof(HandleCharNameUpdate))] public string charName = "Bolo";
    [Header("Game Info")]
    [SyncVar] public bool IsGameLeader = false;
    [SyncVar(hook = nameof(HandlePlayerReadyStatusChange))] public bool isPlayerReady;
    [SyncVar] public CSteamID playerSteamId;
    [SyncVar] public int hp;
    [SyncVar] public int essenceCubes;

    public PlayerListItem item;

    public CharacterDisplay charDisplay;
    public TMPro.TextMeshProUGUI usernameText;
    //public TMPro.TextMeshProUGUI readyUp;

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
    public override void OnStartAuthority()
    {
        Debug.Log("I want to see if this triggers");
        gameObject.name = "LocalGamePlayer";
        LobbyManager.instance.FindLocalPlayer();
        ConnectionId = LobbyManager.instance.playerConnId;
        if(Game.GamePlayers.Count == 0)
        {
            IsGameLeader = true;
        }
        //CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        LobbyManager.instance.localGamePlayerScript.CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        playerNumber = 1;
        playerSteamId = SteamUser.GetSteamID();
        //usernameText.text = item.playerName;
        //item.playerName = playerName;
        charName = LobbyManager.instance.characterName;
        DontDestroyOnLoad(this.gameObject);
    }
    [Command]
    private void CmdSetPlayerName(string playerName)
    {
        Debug.Log("CmdSetPlayerName: Setting player name to: " + playerName);
        this.HandlePlayerNameUpdate(this.playerName, playerName);
    }

    public void changeCharName(string character)
    {
        charName = character;
        //onCharacterClick(character);
    }

    public void selectCharNameRight()
    {
        charIndex += 1;
        if (charIndex > 8)
        {
            charIndex = 0;
        }
        charName = MainMenu.menu.characters[charIndex].cardName;
        LobbyManager.instance.localGamePlayerScript.charIndex = charIndex;
        LobbyManager.instance.localGamePlayerScript.charName = MainMenu.menu.characters[charIndex].cardName;
        LobbyManager.instance.localGamePlayerScript.CmdChangeCharacter(MainMenu.menu.characters[charIndex].cardName);
        LobbyManager.instance.UpdateUI();
    }

    public void selectCharNameLeft()
    {
        charIndex -= 1;
        if (charIndex < 0)
        {
            charIndex = 8;
        }
        charName = MainMenu.menu.characters[charIndex].cardName;
        LobbyManager.instance.localGamePlayerScript.charIndex = charIndex;
        LobbyManager.instance.localGamePlayerScript.charName = MainMenu.menu.characters[charIndex].cardName;
        LobbyManager.instance.localGamePlayerScript.CmdChangeCharacter(MainMenu.menu.characters[charIndex].cardName);
        LobbyManager.instance.UpdateUI();
    }

    // BAD! Don't use!
    public void ReadyUp()
    {
        //isPlayerReady = !isPlayerReady;
        LobbyManager.instance.localGamePlayerScript.isPlayerReady = !LobbyManager.instance.localGamePlayerScript.isPlayerReady;
        if (LobbyManager.instance.localGamePlayerScript.isPlayerReady)
        {
            item.NotReadyButton.SetActive(false);
            item.ReadyButton.SetActive(true);
        } else
        {
            item.NotReadyButton.SetActive(true);
            item.ReadyButton.SetActive(false);
        }
        
        LobbyManager.instance.CheckIfAllPlayersAreReady();
    }

    public override void OnStartClient()
    {
        Debug.Log("Starting client");
        Game.GamePlayers.Add(this);
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        //LobbyManager.instance.UpdateLobbyName();
        LobbyManager.instance.UpdateUI();        
    }
    // Start is called before the first frame update
    void Start()
    {
        //usernameText.text = SteamLobby.instance.greetingName;
        usernameText.text = item.playerName;
        item.playerName = playerName;
        charName = LobbyManager.instance.characterName;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Command]
    public void CmdChangeCharacter(string character)
    {
        Debug.Log("Changing character to: " + character);
        this.HandleCharNameUpdate(this.charName, character);
    }

    public void onCharacterClick(string character)
    {
        Debug.Log("Send CharReq");
        foreach (Character character2 in MainMenu.menu.characters)
        {
            if (character2.cardName == character)
            {
                Debug.Log(character + " chosen");
                charDisplay.updateCharacter(character2);
            }
        }
    }

    [Command]
    public void CmdShuffleDecks()
    {
        // shuffle market decks
        Debug.Log("Executing CmdShuffleDecks on the server for player: " + playerName);
        GameManager.manager.md1.shuffle();
        GameManager.manager.md2.shuffle();
    }

    [ClientRpc]
    public void RpcSellCard(string name, int selectedCard)
    {
        Debug.Log("Selling card for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == name)
            {
                cp.addPips(cp.holster.cardList[selectedCard - 1].card.sellPrice);
                GameManager.manager.td.addCard(cp.holster.cardList[selectedCard - 1]);
                return;
            }
        }
    }

    [Command]
    public void CmdSellCard(string throwerName, int selectedCard)
    {
        Debug.Log("Executing CmdSellCard on the server for player: " + playerName);
        RpcSellCard(throwerName, selectedCard);
    }

    [ClientRpc]
    public void RpcCycleCard(string name, int selectedCard)
    {
        Debug.Log("Cycling card for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == name)
            {
                // Cycling a Potion (costs 0 pips to do)
                if (cp.holster.cardList[selectedCard - 1].card.cardType == "Potion")
                {
                    cp.deck.putCardOnBottom(cp.holster.cardList[selectedCard - 1].card);
                    cp.holster.cardList[selectedCard - 1].updateCard(GameManager.manager.players[0].holster.card1.placeholder);
                    GameManager.manager.sendSuccessMessage(7);
                    cp.holster.cardList[selectedCard - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                    // MATTEO: Add Cycle SFX here.

                }
                // If Player has no pips to cycle an Artifact, Vessel, or Ring.
                else if (cp.pips < 1)
                {
                    // "You don't have enough Pips to cycle this!"
                    GameManager.manager.sendErrorMessage(5);
                }
                // Player has enough pips to cycle an Artifact, Vessel, or Ring.
                else if (cp.holster.cardList[selectedCard - 1].card.cardType == "Artifact" ||
                        cp.holster.cardList[selectedCard - 1].card.cardType == "Vessel" ||
                        cp.holster.cardList[selectedCard - 1].card.cardType == "Ring")
                {
                    // bool connected = networkManager.sendCycleRequest(selectedCardInt, 1);
                    cp.subPips(1);
                    cp.deck.putCardOnBottom(cp.holster.cardList[selectedCard - 1].card);
                    cp.holster.cardList[selectedCard - 1].updateCard(GameManager.manager.players[0].holster.card1.placeholder);
                    GameManager.manager.sendSuccessMessage(7);
                    cp.holster.cardList[selectedCard - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                    // MATTEO: Add Cycle SFX here.
                }
                else
                {
                    Debug.Log("ERROR: Card needs to be of type Potion, Artifact, Vessel, Ring");
                }
                return;
            }
        }
    }

    [Command]
    public void CmdCycleCard(string throwerName, int selectedCard)
    {
        Debug.Log("Executing CmdCycleCard on the server for player: " + playerName);
        RpcCycleCard(throwerName, selectedCard);
    }

    [ClientRpc]
    public void RpcLoadCard(string name, int selectedCard, int loadedCard)
    {
        Debug.Log("Loading card for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == name)
            {
                GameManager.manager.selectedCardInt = selectedCard;
                GameManager.manager.loadedCardInt = loadedCard;
                GameManager.manager.loadPotion();
                return;
            }
        }
    }

    [Command]
    public void CmdLoadCard(string throwerName, int selectedCard, int loadedCard)
    {
        Debug.Log("Executing CmdSellCard on the server for player: " + playerName);
        RpcLoadCard(throwerName, selectedCard, loadedCard);
    }

    [ClientRpc]
    public void RpcBuyCard(string name, int marketCard)
    {
        Card card;
        Debug.Log("Buying card for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == name)
            {
                switch (marketCard)
                {
                    case 1:
                        /*
                        cp.subPips(GameManager.manager.md1.cardDisplay1.card.buyPrice);
                        cp.deck.putCardOnTop(GameManager.manager.md1.cardDisplay1.card);
                        card = GameManager.manager.md1.popCard();
                        GameManager.manager.md1.cardDisplay1.updateCard(card);
                        GameManager.manager.sendSuccessMessage(1);
                        */
                        GameManager.manager.topMarketBuy();
                        break;
                    case 2:
                        GameManager.manager.topMarketBuy();
                        break;
                    case 3:
                        GameManager.manager.topMarketBuy();
                        break;
                    case 4:
                        GameManager.manager.bottomMarketBuy();
                        break;
                    case 5:
                        GameManager.manager.bottomMarketBuy();
                        break;
                    case 6:
                        GameManager.manager.bottomMarketBuy();
                        break;
                    default:
                        break;

                }
                return;
            }
        }
    }

    [Command]
    public void CmdBuyCard(string throwerName, int marketCard)
    {
        switch (marketCard)
        {
            case 1:
                GameManager.manager.md1.cardInt = 1;
                RpcBuyCard(throwerName, marketCard);
                break;
            case 2:
                GameManager.manager.md1.cardInt = 2;
                RpcBuyCard(throwerName, marketCard);
                break;
            case 3:
                GameManager.manager.md1.cardInt = 3;
                RpcBuyCard(throwerName, marketCard);
                break;
            case 4:
                GameManager.manager.md2.cardInt = 1;
                RpcBuyCard(throwerName, marketCard);
                break;
            case 5:
                GameManager.manager.md2.cardInt = 2;
                RpcBuyCard(throwerName, marketCard);
                break;
            case 6:
                GameManager.manager.md2.cardInt = 3;
                RpcBuyCard(throwerName, marketCard);
                break;
            default:
                break;

        }
    }

    [Command(requiresAuthority = false)]
    public void CmdEndTurn()
    {
        Debug.Log("Executing CmdEndTurn on the server for player: " + playerName);
        // change GameManager's myPlayerIndex
        //GameManager.manager.myPlayerIndex++;
        GameManager.manager.endTurn();
    }

    [ClientRpc]
    public void RpcTrashCard(string name, int selectedCard)
    {
        Debug.Log("Trashing card for: " + playerName);
        GameManager.manager.selectedCardInt = selectedCard;
        GameManager.manager.trashCard();
    }

    [Command]
    public void CmdTrashCard(string throwerName, int selectedCard)
    {
        Debug.Log("Executing CmdTrashCard on the server for player: " + playerName);

        //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].placeholder);
        //td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
        //GameManager.manager.td.addCard(GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.selectedCardInt - 1]);
        RpcTrashCard(throwerName, selectedCard);
    }

    public void HandleCharNameUpdate(string oldValue, string newValue)
    {
        Debug.Log("Player name has been updated for: " + oldValue + " to new value: " + newValue);
        if (isServer)
            this.charName = newValue;
            //LobbyManager.instance.localGamePlayerScript.charName = newValue;
            Debug.Log("I am a server");
            onCharacterClick(newValue);
            //LobbyManager.instance.UpdateUI();
            
        if (isClient)
        {
            Debug.Log("I am a client");
            this.charName = newValue;
            onCharacterClick(newValue);
            LobbyManager.instance.UpdateUI();
            
        }

    }

    public void HandlePlayerNameUpdate(string oldValue, string newValue)
    {
        Debug.Log("Player name has been updated for: " + oldValue + " to new value: " + newValue);
        if (isServer)
            this.playerName = newValue;
            this.usernameText.text = newValue;
        if (isClient)
        {
            //this.playerName = newValue;
            //this.usernameText.text = newValue;
            LobbyManager.instance.UpdateUI();
        }

    }

    [Command(requiresAuthority = false)]
    public void CmdSubHealth(int damage)
    {
        Debug.Log("Executing CmdSubHealth on the server for player: " + playerName);
        // call the ClientRPC in isServer
        
        if (isServer)
            RpcSubHealth(damage);

        if (isClient)
            GameManager.manager.tempPlayer.updateHealthUI();
    }

    [ClientRpc]
    public void RpcSubHealth(int newValue)
    {
        Debug.Log("Subtracting health for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == playerName)
            {
                cp.subHealth(newValue);
            }
        }
    }

    public void ChangeReadyStatus()
    {
        Debug.Log("Executing ChangeReadyStatus for player: " + this.playerName);
        //CmdChangePlayerReadyStatus();
        LobbyManager.instance.localGamePlayerScript.CmdChangePlayerReadyStatus();
    }

    [Command]
    public void CmdChangePlayerReadyStatus()
    {
        Debug.Log("Executing CmdChangePlayerReadyStatus on the server for player: " + this.playerName);
        this.HandlePlayerReadyStatusChange(this.isPlayerReady, !this.isPlayerReady);
    }
    void HandlePlayerReadyStatusChange(bool oldValue, bool newValue)
    {
        if (isServer)
            this.isPlayerReady = newValue;
        if (isClient)
            LobbyManager.instance.UpdateUI();
    }
    public void CanLobbyStartGame()
    {
        if (hasAuthority)
            CmdCanLobbyStartGame();
    }
    [Command]
    void CmdCanLobbyStartGame()
    {
        Game.StartGame();
    }
    public void QuitLobby()
    {
        if (hasAuthority)
        {
            if (IsGameLeader)
            {
                Game.StopHost();
            }
            else
            {
                Game.StopClient();
            }
        }
    }
    private void OnDestroy()
    {
        if (hasAuthority)
        {
            LobbyManager.instance.DestroyPlayerListItems();
            SteamMatchmaking.LeaveLobby((CSteamID)LobbyManager.instance.currentLobbyId);
        }
        Debug.Log("LobbyPlayer destroyed. Returning to main menu.");
    }
    public override void OnStopClient()
    {
        Debug.Log(playerName + " is quiting the game.");
        Game.GamePlayers.Remove(this);
        Debug.Log("Removed player from the GamePlayer list: " + this.playerName);
        LobbyManager.instance.UpdateUI();
    }

    public void ChooseCharacter(string character)
    {
        //Debug.Log("Send CharReq");
        foreach (Character character2 in MainMenu.menu.characters)
        {
            if (character2.cardName == character)
            {
                Debug.Log(character + " chosen");
                charDisplay.updateCharacter(character2);
            }
        }
        // bool connected = networkManager.SendCharacterRequest(character);
    }
}