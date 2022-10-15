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

    [Command]
    public void CmdThrowCard(string throwerName, string opponentName, int selectedCard)
    {
        Debug.Log("Executing CmdThrowCard on the server for player: " + playerName);
        RpcThrowCard(throwerName, opponentName, selectedCard);
    }

    [ClientRpc]
    public void RpcThrowCard(string throwerName, string opponentName, int selectedCardInt)
    {
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if(cp.name == opponentName)
            {
                GameManager.manager.tempPlayer = cp;
            }
        }
        Debug.Log("Throwing card for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == throwerName)
            {
                int damage = 0;
                Debug.Log("GameManager Throw Potion");

                if (cp.holster.cardList[selectedCardInt - 1].card.cardType == "Potion")
                {
                    Debug.Log("POTION");
                    if (cp.isTwins)
                    {
                        if (!cp.character.character.flipped)
                        {
                            cp.addHealth(1);
                        }
                        else
                        {
                            cp.addHealth(2);
                        }
                    }
                    damage = cp.holster.cardList[selectedCardInt - 1].card.effectAmount;
                    Debug.Log("Original damage: " + damage);
                    damage = cp.checkArtifactBonus(damage, cp.holster.cardList[selectedCardInt - 1]);
                    Debug.Log("Damage after thrower bonuses: " + damage);
                    damage = GameManager.manager.tempPlayer.checkDefensiveBonus(damage);
                    Debug.Log("Damage after defensive bonuses: " + damage);

                    GameManager.manager.sendSuccessMessage(2); // Only display on thrower's client.
                    cp.potionsThrown++;
                    //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].placeholder);

                    GameManager.manager.td.addCard(cp.holster.cardList[selectedCardInt - 1]);

                    if (cp.blackRainBonus)
                    {
                        GameManager.manager.put4CardsInHolster();
                        cp.blackRainBonus = false;
                    }

                    cp.holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();

                    // may need to add this back in
                    foreach (CardPlayer cp2 in GameManager.manager.players)
                    {
                        if (cp2.name == opponentName)
                        {
                            Debug.Log("Damaging player: "+ opponentName);
                            cp2.subHealth(damage);
                        }
                    }
                    //GameManager.manager.tempPlayer.subHealth(damage);

                }
                else if (cp.holster.cardList[selectedCardInt - 1].card.cardType == "Artifact")
                {
                    if (cp.holster.cardList[selectedCardInt - 1].aPotion.card.cardName != "placeholder")
                    {
                        damage = cp.holster.cardList[selectedCardInt - 1].card.effectAmount;
                        Debug.Log("Original damage: " + damage);
                        damage = cp.checkArtifactBonus(damage, cp.holster.cardList[selectedCardInt - 1]);
                        Debug.Log("Damage after thrower bonuses: " + damage);
                        damage = GameManager.manager.tempPlayer.checkDefensiveBonus(damage);
                        Debug.Log("Damage after defensive bonuses: " + damage);

                        // Update response to account for trashing loaded artifact's potion and not the artifact
                        cp.artifactsUsed++;
                        GameManager.manager.td.addCard(cp.holster.cardList[selectedCardInt - 1].aPotion);
                        cp.holster.cardList[selectedCardInt - 1].artifactSlot.transform.parent.gameObject.SetActive(false);

                        // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                        // bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, true, false);
                        foreach (CardPlayer cp2 in GameManager.manager.players)
                        {
                            if (cp2.name == opponentName)
                            {
                                Debug.Log("Damaging player: " + opponentName);
                                cp2.subHealth(damage);
                            }
                        }
                        //GameManager.manager.tempPlayer.subHealth(damage);

                        // ISADORE LOGIC
                        if (cp.isIsadore && cp.artifactsUsed == 2)
                        {
                            cp.character.canBeFlipped = true;
                            // add success message for "You can now flip your card!" or something
                            GameManager.manager.sendSuccessMessage(13);

                        }
                        else
                        {
                            GameManager.manager.sendSuccessMessage(3);
                        }

                        cp.holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        // MATTEO: Add Artifact using SFX here.

                    }
                    else
                    {
                        // "Can't use an unloaded Artifact!"
                        GameManager.manager.sendErrorMessage(1);
                    }

                }
                else if (cp.holster.cardList[selectedCardInt - 1].card.cardType == "Vessel")
                {
                    if (cp.holster.cardList[selectedCardInt - 1].vPotion1.card.cardName != "placeholder" &&
                                cp.holster.cardList[selectedCardInt - 1].vPotion2.card.cardName != "placeholder")
                    {
                        if (cp.isTwins && cp.character.character.flipped)
                        {
                            cp.addHealth(4);
                        }
                        //int damage = players[throwerIndex].holster.card1.vPotion1.card.effectAmount + players[throwerIndex].holster.card1.vPotion2.card.effectAmount;
                        damage = cp.holster.cardList[selectedCardInt - 1].card.effectAmount;
                        Debug.Log("Original damage: " + damage);
                        damage = cp.checkArtifactBonus(damage, cp.holster.cardList[selectedCardInt - 1]);
                        Debug.Log("Damage after thrower bonuses: " + damage);
                        damage = GameManager.manager.tempPlayer.checkDefensiveBonus(damage);
                        Debug.Log("Damage after defensive bonuses: " + damage);

                        // TODO: fix bonus damage
                        //damage = players[throwerIndex].checkBonus(damage, selectedCardInt);
                        cp.deck.putCardOnBottom(cp.holster.cardList[selectedCardInt - 1].vPotion1.card);
                        cp.deck.putCardOnBottom(cp.holster.cardList[selectedCardInt - 1].vPotion2.card);
                        cp.holster.card1.vPotion1.updateCard(cp.holster.cardList[selectedCardInt - 1].placeholder);
                        cp.holster.card1.vPotion2.updateCard(cp.holster.cardList[selectedCardInt - 1].placeholder);
                        GameManager.manager.td.addCard(cp.holster.cardList[selectedCardInt - 1]);
                        cp.holster.cardList[selectedCardInt - 1].vesselSlot1.transform.parent.gameObject.SetActive(false);
                        cp.holster.cardList[selectedCardInt - 1].vesselSlot2.transform.parent.gameObject.SetActive(false);
                        // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                        // bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, false, true);
                        foreach (CardPlayer cp2 in GameManager.manager.players)
                        {
                            if (cp2.name == opponentName)
                            {
                                Debug.Log("Damaging player: " + opponentName);
                                cp2.subHealth(damage);
                            }
                        }
                        //GameManager.manager.tempPlayer.subHealth(damage);
                        GameManager.manager.sendSuccessMessage(4);
                        cp.holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();

                        // MATTEO: Add Vessel throw SFX here.

                    }
                    else
                    {
                        // "Can't throw an unloaded Vessel!"
                        //Debug.Log("Vessel Error");
                        GameManager.manager.sendErrorMessage(2);
                    }
                }
                return;
            }
        }
    }

    [ClientRpc]
    public void RpcSellCard(string name, int selectedCardInt)
    {
        GameManager.manager.mirrorCommand = true;
        Debug.Log("Selling card for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == name)
            {
                // LOGIC FOR BOLO SELLING ABILITY
                // Bolo not flipped and he's selling something that's not a potion
                if (cp.isBolo && !cp.character.character.flipped && cp.holster.cardList[selectedCardInt - 1].card.cardType != "Potion")
                {
                    cp.addPips(cp.holster.cardList[selectedCardInt - 1].card.sellPrice + 1);
                    // Bolo flipped selling anything
                }
                else if (cp.isBolo && cp.character.character.flipped)
                {
                    cp.addPips(cp.holster.cardList[selectedCardInt - 1].card.sellPrice + 1);
                }
                else
                {
                    // everyone else
                    cp.addPips(cp.holster.cardList[selectedCardInt - 1].card.sellPrice);
                }
                GameManager.manager.td.addCard(cp.holster.cardList[selectedCardInt - 1]);

                // bool connected = networkManager.sendSellRequest(selectedCardInt, players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.sellPrice);
                cp.holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                GameManager.manager.sendSuccessMessage(8);
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
        GameManager.manager.mirrorCommand = true;
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
    public void RpcLoadCard(string name, int selectedCardInt, int loadedCardInt)
    {
        Debug.Log("Loading card for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == name)
            {
                if (GameManager.manager.starterPotion && !GameManager.manager.usedStarterPotion)
                {
                    // Loading a Vessel:
                    if (cp.holster.cardList[loadedCardInt].card.cardType == "Vessel")
                    {
                        // TODO: Make another error message
                        Debug.Log("This error message???");
                        GameManager.manager.sendErrorMessage(12);
                    }
                    else if (cp.holster.cardList[loadedCardInt].card.cardType == "Artifact")
                    {
                        // Enable Artifact menu if it wasn't already enabled.
                        Debug.Log("Artifact menu enabled.");
                        cp.holster.cardList[loadedCardInt].artifactSlot.transform.parent.gameObject.SetActive(true);

                        // Check for existing loaded potion if Artifact menu was already enabled.
                        if (cp.holster.cardList[loadedCardInt].aPotion.card.cardName != "placeholder")
                        {
                            Debug.Log("Artifact is fully loaded!");
                            // DONE: Insert error that displays on screen.
                            GameManager.manager.sendErrorMessage(8);
                        }
                        // Artifact slot is unloaded.
                        else
                        {
                            Card placeholder = cp.holster.cardList[loadedCardInt].aPotion.card;
                            cp.holster.cardList[loadedCardInt].aPotion.card = GameManager.manager.starterPotionDisplay.card;
                            cp.holster.cardList[loadedCardInt].aPotion.updateCard(GameManager.manager.starterPotionDisplay.card);
                            GameManager.manager.usedStarterPotion = true;
                            // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                            GameManager.manager.sendSuccessMessage(5);
                            //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                            Debug.Log("Starter potion loaded in Artifact slot!");

                            // MATTEO: Add Loading potion SFX here.

                            // // Updates Holster card to be empty.
                            cp.holster.cardList[selectedCardInt - 1].card = placeholder;
                            cp.holster.cardList[selectedCardInt - 1].updateCard(placeholder);
                        }
                    }
                    GameManager.manager.starterPotion = false;
                    return;
                }
                else
                {
                    // if it's an artifact or vessel
                    if (cp.holster.cardList[selectedCardInt - 1].card.cardType == "Potion")
                    {
                        // Loading a Vessel:
                        if (cp.holster.cardList[loadedCardInt].card.cardType == "Vessel")
                        {
                            // Enable Vessel menu if it wasn't already enabled.
                            Debug.Log("Vessel menu enabled.");
                            cp.holster.cardList[loadedCardInt].vesselSlot1.transform.parent.gameObject.SetActive(true);

                            // Check for existing loaded potion(s) if Vessel menu was already enabled.
                            if (cp.holster.cardList[loadedCardInt].vPotion1.card.cardName != "placeholder")
                            {
                                // If Vessel slot 2 is filled.
                                if (cp.holster.cardList[loadedCardInt].vPotion2.card.cardName != "placeholder")
                                {
                                    Debug.Log("Vessel is fully loaded!");
                                    // DONE: Insert error that displays on screen.
                                    GameManager.manager.sendErrorMessage(9);
                                }
                                else
                                {
                                    // Fill Vessel slot 2 with loaded potion.
                                    Card placeholder = cp.holster.cardList[loadedCardInt].vPotion2.card;
                                    cp.holster.cardList[loadedCardInt].vPotion2.card = cp.holster.cardList[selectedCardInt - 1].card;
                                    cp.holster.cardList[loadedCardInt].vPotion2.updateCard(cp.holster.cardList[selectedCardInt - 1].card);

                                    // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                                    GameManager.manager.sendSuccessMessage(5);
                                    cp.holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                                    Debug.Log("Potion loaded in Vessel slot 2!");

                                    // MATTEO: Add Loading potion SFX here.

                                    // // Updates Holster card to be empty.
                                    cp.holster.cardList[selectedCardInt - 1].card = placeholder;
                                    cp.holster.cardList[selectedCardInt - 1].updateCard(placeholder);
                                }
                            }
                            // Vessel slot 1 is unloaded.
                            else
                            {
                                Card placeholder = cp.holster.cardList[loadedCardInt].vPotion1.card;
                                cp.holster.cardList[loadedCardInt].vPotion1.card = cp.holster.cardList[selectedCardInt - 1].card;
                                cp.holster.cardList[loadedCardInt].vPotion1.updateCard(cp.holster.cardList[selectedCardInt - 1].card);

                                // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                                GameManager.manager.sendSuccessMessage(5);
                                cp.holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                                Debug.Log("Potion loaded in Vessel slot 1!");

                                // MATTEO: Add Loading potion SFX here.

                                // // Updates Holster card to be empty.
                                cp.holster.cardList[selectedCardInt - 1].card = placeholder;
                                cp.holster.cardList[selectedCardInt - 1].updateCard(placeholder);
                            }
                        }

                        // Loading an Artifact:
                        else if (cp.holster.cardList[loadedCardInt].card.cardType == "Artifact")
                        {
                            // Enable Artifact menu if it wasn't already enabled.
                            Debug.Log("Artifact menu enabled.");
                            cp.holster.cardList[loadedCardInt].artifactSlot.transform.parent.gameObject.SetActive(true);

                            // Check for existing loaded potion if Artifact menu was already enabled.
                            if (cp.holster.cardList[loadedCardInt].aPotion.card.cardName != "placeholder")
                            {
                                Debug.Log("Artifact is fully loaded!");
                                // DONE: Insert error that displays on screen.
                                GameManager.manager.sendErrorMessage(8);
                            }
                            // Artifact slot is unloaded.
                            else
                            {
                                Card placeholder = cp.holster.cardList[loadedCardInt].aPotion.card;
                                cp.holster.cardList[loadedCardInt].aPotion.card = cp.holster.cardList[selectedCardInt - 1].card;
                                cp.holster.cardList[loadedCardInt].aPotion.updateCard(cp.holster.cardList[selectedCardInt - 1].card);
                                // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                                GameManager.manager.sendSuccessMessage(5);
                                cp.holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                                Debug.Log("Potion loaded in Artifact slot!");

                                // MATTEO: Add Loading potion SFX here.

                                // // Updates Holster card to be empty.
                                cp.holster.cardList[selectedCardInt - 1].card = placeholder;
                                cp.holster.cardList[selectedCardInt - 1].updateCard(placeholder);
                            }
                        }
                    }
                    else
                    {
                        // add error message
                        Debug.Log("That error message...");
                        GameManager.manager.sendErrorMessage(12);
                    }
                }
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
    public void RpcBuyTopCard(string name, int marketCard)
    {
        Card card;
        Debug.Log("Buying potion card for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == name)
            {
                // add in logic
                switch (marketCard)
                {
                    case 1:
                        if (cp.pips >= GameManager.manager.md1.cardDisplay1.card.buyPrice && !cp.isSaltimbocca)
                        {
                            // All rings cost 4 logic
                            if (GameManager.manager.md1.cardDisplay1.card.cardType == "Ring" && cp.doubleRingBonus)
                            {
                                GameManager.manager.md1.cardDisplay1.card.buyPrice = 4;
                            }

                            // if The Early Bird Special was drawn this turn
                            if (GameManager.manager.md1.cardDisplay1.card.cardName == "EarlyBirdSpecial" && GameManager.manager.earlyBirdSpecial)
                            {
                                // it buys for 3 pips
                                GameManager.manager.md1.cardDisplay1.card.buyPrice = 3;
                            }
                            cp.subPips(GameManager.manager.md1.cardDisplay1.card.buyPrice);
                            cp.deck.putCardOnTop(GameManager.manager.md1.cardDisplay1.card);
                            card = GameManager.manager.md1.popCard();
                            GameManager.manager.md1.cardDisplay1.updateCard(card);
                            GameManager.manager.md1.cardDisplay1.gameObject.GetComponent<Market_Hover>().resetCard();
                            GameManager.manager.sendSuccessMessage(1);
                            // bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay1.card.buyPrice, 1);
                            // SALTIMBOCCA LOGIC
                        }
                        else if (cp.isSaltimbocca && cp.pips >= (GameManager.manager.md1.cardDisplay1.card.buyPrice - 1))
                        {
                            // All rings cost 4 logic
                            if (GameManager.manager.md1.cardDisplay1.card.cardType == "Ring" && cp.doubleRingBonus)
                            {
                                GameManager.manager.md1.cardDisplay1.card.buyPrice = 4;
                            }

                            // if The Early Bird Special was drawn this turn
                            if (GameManager.manager.md1.cardDisplay1.card.cardName == "EarlyBirdSpecial" && GameManager.manager.earlyBirdSpecial)
                            {
                                // it buys for 3 pips
                                GameManager.manager.md1.cardDisplay1.card.buyPrice = 3;
                            }

                            if (GameManager.manager.md1.cardDisplay1.card.buyPrice == 1)
                            {
                                cp.subPips(GameManager.manager.md1.cardDisplay1.card.buyPrice);
                            }
                            else
                            {
                                cp.subPips(GameManager.manager.md1.cardDisplay1.card.buyPrice - 1);
                            }
                            cp.deck.putCardOnTop(GameManager.manager.md1.cardDisplay1.card);
                            card = GameManager.manager.md1.popCard();
                            GameManager.manager.md1.cardDisplay1.updateCard(card);
                            //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                            GameManager.manager.md1.cardDisplay1.gameObject.GetComponent<Market_Hover>().resetCard();
                            GameManager.manager.sendSuccessMessage(1);
                        }
                        else
                        {
                            GameManager.manager.sendErrorMessage(6);
                        }
                        break;
                    case 2:
                        if (cp.pips >= GameManager.manager.md1.cardDisplay2.card.buyPrice && !cp.isSaltimbocca)
                        {
                            // All rings cost 4 logic
                            if (GameManager.manager.md1.cardDisplay2.card.cardType == "Ring" && cp.doubleRingBonus)
                            {
                                GameManager.manager.md1.cardDisplay2.card.buyPrice = 4;
                            }

                            // if The Early Bird Special was drawn this turn
                            if (GameManager.manager.md1.cardDisplay2.card.cardName == "EarlyBirdSpecial" && GameManager.manager.earlyBirdSpecial)
                            {
                                // it buys for 3 pips
                                GameManager.manager.md1.cardDisplay2.card.buyPrice = 3;
                            }
                            cp.subPips(GameManager.manager.md1.cardDisplay2.card.buyPrice);
                            cp.deck.putCardOnTop(GameManager.manager.md1.cardDisplay2.card);
                            card = GameManager.manager.md1.popCard();
                            GameManager.manager.md1.cardDisplay2.updateCard(card);
                            GameManager.manager.md1.cardDisplay2.gameObject.GetComponent<Market_Hover>().resetCard();
                            GameManager.manager.sendSuccessMessage(1);
                        }
                        else if (cp.isSaltimbocca && cp.pips >= (GameManager.manager.md1.cardDisplay2.card.buyPrice - 1))
                        {
                            // All rings cost 4 logic
                            if (GameManager.manager.md1.cardDisplay2.card.cardType == "Ring" && cp.doubleRingBonus)
                            {
                                GameManager.manager.md1.cardDisplay2.card.buyPrice = 4;
                            }

                            // if The Early Bird Special was drawn this turn
                            if (GameManager.manager.md1.cardDisplay2.card.cardName == "EarlyBirdSpecial" && GameManager.manager.earlyBirdSpecial)
                            {
                                // it buys for 3 pips
                                GameManager.manager.md1.cardDisplay2.card.buyPrice = 3;
                            }

                            if (GameManager.manager.md1.cardDisplay2.card.buyPrice - 1 == 0)
                            {
                                cp.subPips(GameManager.manager.md1.cardDisplay2.card.buyPrice);
                            }
                            else
                            {
                                cp.subPips(GameManager.manager.md1.cardDisplay2.card.buyPrice - 1);
                            }
                            cp.deck.putCardOnTop(GameManager.manager.md1.cardDisplay2.card);
                            card = GameManager.manager.md1.popCard();
                            GameManager.manager.md1.cardDisplay2.updateCard(card);
                            GameManager.manager.md1.cardDisplay2.gameObject.GetComponent<Market_Hover>().resetCard();
                            GameManager.manager.sendSuccessMessage(1);
                        }
                        else
                        {
                            GameManager.manager.sendErrorMessage(6);
                        }
                        break;
                    case 3:
                        if (cp.pips >= GameManager.manager.md1.cardDisplay3.card.buyPrice && cp.isSaltimbocca)
                        {
                            // All rings cost 4 logic
                            if (GameManager.manager.md1.cardDisplay3.card.cardType == "Ring" && cp.doubleRingBonus)
                            {
                                GameManager.manager.md1.cardDisplay3.card.buyPrice = 4;
                            }

                            // if The Early Bird Special was drawn this turn
                            if (GameManager.manager.md1.cardDisplay3.card.cardName == "EarlyBirdSpecial" && GameManager.manager.earlyBirdSpecial)
                            {
                                // it buys for 3 pips
                                GameManager.manager.md1.cardDisplay3.card.buyPrice = 3;
                            }
                            cp.subPips(GameManager.manager.md1.cardDisplay3.card.buyPrice);
                            cp.deck.putCardOnTop(GameManager.manager.md1.cardDisplay3.card);
                            card = GameManager.manager.md1.popCard();
                            GameManager.manager.md1.cardDisplay3.updateCard(card);
                            GameManager.manager.md1.cardDisplay3.gameObject.GetComponent<Market_Hover>().resetCard();
                            GameManager.manager.sendSuccessMessage(1);
                            // bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay3.card.buyPrice, 1);
                        }
                        else if (cp.isSaltimbocca && cp.pips >= (GameManager.manager.md1.cardDisplay3.card.buyPrice - 1))
                        {
                            // All rings cost 4 logic
                            if (GameManager.manager.md1.cardDisplay3.card.cardType == "Ring" && cp.doubleRingBonus)
                            {
                                GameManager.manager.md1.cardDisplay3.card.buyPrice = 4;
                            }

                            // if The Early Bird Special was drawn this turn
                            if (GameManager.manager.md1.cardDisplay3.card.cardName == "EarlyBirdSpecial" && GameManager.manager.earlyBirdSpecial)
                            {
                                // it buys for 3 pips
                                GameManager.manager.md1.cardDisplay3.card.buyPrice = 3;
                            }

                            if (GameManager.manager.md1.cardDisplay3.card.buyPrice - 1 == 0)
                            {
                                cp.subPips(GameManager.manager.md1.cardDisplay3.card.buyPrice);
                            }
                            else
                            {
                                cp.subPips(GameManager.manager.md1.cardDisplay3.card.buyPrice - 1);
                            }
                            cp.deck.putCardOnTop(GameManager.manager.md1.cardDisplay3.card);
                            card = GameManager.manager.md1.popCard();
                            GameManager.manager.md1.cardDisplay3.updateCard(card);
                            GameManager.manager.md1.cardDisplay3.gameObject.GetComponent<Market_Hover>().resetCard();
                            GameManager.manager.sendSuccessMessage(1);
                        }
                        else
                        {
                            GameManager.manager.sendErrorMessage(6);
                        }
                        break;
                    default:
                        Debug.Log("MarketDeck Error!");
                        break;
                }
                return;
            }
        }
    }

    [ClientRpc]
    public void RpcBuyBottomCard(string name, int marketCard)
    {
        Card card;
        Debug.Log("Buying bottom card for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == name)
            {
                // add in logic
                switch (marketCard)
                {
                    case 1:
                        if (cp.pips >= GameManager.manager.md2.cardDisplay1.card.buyPrice && !cp.isSaltimbocca)
                        {
                            // All rings cost 4 logic
                            if (GameManager.manager.md2.cardDisplay1.card.cardType == "Ring" && cp.doubleRingBonus)
                            {
                                GameManager.manager.md2.cardDisplay1.card.buyPrice = 4;
                            }

                            // if The Early Bird Special was drawn this turn
                            if (GameManager.manager.md2.cardDisplay1.card.cardName == "EarlyBirdSpecial" && GameManager.manager.earlyBirdSpecial)
                            {
                                // it buys for 3 pips
                                GameManager.manager.md2.cardDisplay1.card.buyPrice = 3;
                            }
                            cp.subPips(GameManager.manager.md2.cardDisplay1.card.buyPrice);
                            cp.deck.putCardOnTop(GameManager.manager.md2.cardDisplay1.card);
                            card = GameManager.manager.md2.popCard();
                            GameManager.manager.md2.cardDisplay1.updateCard(card);
                            GameManager.manager.md2.cardDisplay1.gameObject.GetComponent<Market_Hover>().resetCard();
                            GameManager.manager.sendSuccessMessage(1);
                            // bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay1.card.buyPrice, 1);
                            // SALTIMBOCCA LOGIC
                        }
                        else if (cp.isSaltimbocca && cp.pips >= (GameManager.manager.md2.cardDisplay1.card.buyPrice - 1))
                        {
                            // All rings cost 4 logic
                            if (GameManager.manager.md2.cardDisplay1.card.cardType == "Ring" && cp.doubleRingBonus)
                            {
                                GameManager.manager.md2.cardDisplay1.card.buyPrice = 4;
                            }

                            // if The Early Bird Special was drawn this turn
                            if (GameManager.manager.md2.cardDisplay1.card.cardName == "EarlyBirdSpecial" && GameManager.manager.earlyBirdSpecial)
                            {
                                // it buys for 3 pips
                                GameManager.manager.md2.cardDisplay1.card.buyPrice = 3;
                            }

                            if (GameManager.manager.md2.cardDisplay1.card.buyPrice == 1)
                            {
                                cp.subPips(GameManager.manager.md2.cardDisplay1.card.buyPrice);
                            }
                            else
                            {
                                cp.subPips(GameManager.manager.md2.cardDisplay1.card.buyPrice - 1);
                            }
                            cp.deck.putCardOnTop(GameManager.manager.md2.cardDisplay1.card);
                            card = GameManager.manager.md2.popCard();
                            GameManager.manager.md2.cardDisplay1.updateCard(card);
                            GameManager.manager.md2.cardDisplay1.gameObject.GetComponent<Market_Hover>().resetCard();
                            GameManager.manager.sendSuccessMessage(1);
                        }
                        else
                        {
                            GameManager.manager.sendErrorMessage(6);
                        }
                        break;
                    case 2:
                        if (cp.pips >= GameManager.manager.md2.cardDisplay2.card.buyPrice && !cp.isSaltimbocca)
                        {
                            // All rings cost 4 logic
                            if (GameManager.manager.md2.cardDisplay2.card.cardType == "Ring" && cp.doubleRingBonus)
                            {
                                GameManager.manager.md2.cardDisplay2.card.buyPrice = 4;
                            }

                            // if The Early Bird Special was drawn this turn
                            if (GameManager.manager.md2.cardDisplay2.card.cardName == "EarlyBirdSpecial" && GameManager.manager.earlyBirdSpecial)
                            {
                                // it buys for 3 pips
                                GameManager.manager.md2.cardDisplay2.card.buyPrice = 3;
                            }
                            cp.subPips(GameManager.manager.md2.cardDisplay2.card.buyPrice);
                            cp.deck.putCardOnTop(GameManager.manager.md2.cardDisplay2.card);
                            card = GameManager.manager.md2.popCard();
                            GameManager.manager.md2.cardDisplay2.updateCard(card);
                            GameManager.manager.md2.cardDisplay2.gameObject.GetComponent<Market_Hover>().resetCard();
                            GameManager.manager.sendSuccessMessage(1);
                        }
                        else if (cp.isSaltimbocca && cp.pips >= (GameManager.manager.md2.cardDisplay2.card.buyPrice - 1))
                        {
                            // All rings cost 4 logic
                            if (GameManager.manager.md2.cardDisplay2.card.cardType == "Ring" && cp.doubleRingBonus)
                            {
                                GameManager.manager.md2.cardDisplay2.card.buyPrice = 4;
                            }

                            // if The Early Bird Special was drawn this turn
                            if (GameManager.manager.md2.cardDisplay2.card.cardName == "EarlyBirdSpecial" && GameManager.manager.earlyBirdSpecial)
                            {
                                // it buys for 3 pips
                                GameManager.manager.md2.cardDisplay2.card.buyPrice = 3;
                            }

                            if (GameManager.manager.md2.cardDisplay2.card.buyPrice - 1 == 0)
                            {
                                cp.subPips(GameManager.manager.md2.cardDisplay2.card.buyPrice);
                            }
                            else
                            {
                                cp.subPips(GameManager.manager.md2.cardDisplay2.card.buyPrice - 1);
                            }
                            cp.deck.putCardOnTop(GameManager.manager.md2.cardDisplay2.card);
                            card = GameManager.manager.md2.popCard();
                            GameManager.manager.md2.cardDisplay2.updateCard(card);
                            GameManager.manager.md2.cardDisplay2.gameObject.GetComponent<Market_Hover>().resetCard();
                            GameManager.manager.sendSuccessMessage(1);
                        }
                        else
                        {
                            GameManager.manager.sendErrorMessage(6);
                        }
                        break;
                    case 3:
                        if (cp.pips >= GameManager.manager.md2.cardDisplay3.card.buyPrice && cp.isSaltimbocca)
                        {
                            // All rings cost 4 logic
                            if (GameManager.manager.md2.cardDisplay3.card.cardType == "Ring" && cp.doubleRingBonus)
                            {
                                GameManager.manager.md2.cardDisplay3.card.buyPrice = 4;
                            }

                            // if The Early Bird Special was drawn this turn
                            if (GameManager.manager.md2.cardDisplay3.card.cardName == "EarlyBirdSpecial" && GameManager.manager.earlyBirdSpecial)
                            {
                                // it buys for 3 pips
                                GameManager.manager.md2.cardDisplay3.card.buyPrice = 3;
                            }
                            cp.subPips(GameManager.manager.md2.cardDisplay3.card.buyPrice);
                            cp.deck.putCardOnTop(GameManager.manager.md2.cardDisplay3.card);
                            card = GameManager.manager.md2.popCard();
                            GameManager.manager.md2.cardDisplay3.updateCard(card);
                            GameManager.manager.md2.cardDisplay3.gameObject.GetComponent<Market_Hover>().resetCard();
                            GameManager.manager.sendSuccessMessage(1);
                            // bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay3.card.buyPrice, 1);
                        }
                        else if (cp.isSaltimbocca && cp.pips >= (GameManager.manager.md2.cardDisplay3.card.buyPrice - 1))
                        {
                            // All rings cost 4 logic
                            if (GameManager.manager.md2.cardDisplay3.card.cardType == "Ring" && cp.doubleRingBonus)
                            {
                                GameManager.manager.md2.cardDisplay3.card.buyPrice = 4;
                            }

                            // if The Early Bird Special was drawn this turn
                            if (GameManager.manager.md2.cardDisplay3.card.cardName == "EarlyBirdSpecial" && GameManager.manager.earlyBirdSpecial)
                            {
                                // it buys for 3 pips
                                GameManager.manager.md2.cardDisplay3.card.buyPrice = 3;
                            }

                            if (GameManager.manager.md2.cardDisplay3.card.buyPrice - 1 == 0)
                            {
                                cp.subPips(GameManager.manager.md2.cardDisplay3.card.buyPrice);
                            }
                            else
                            {
                                cp.subPips(GameManager.manager.md2.cardDisplay3.card.buyPrice - 1);
                            }
                            cp.deck.putCardOnTop(GameManager.manager.md2.cardDisplay3.card);
                            card = GameManager.manager.md2.popCard();
                            GameManager.manager.md2.cardDisplay3.updateCard(card);
                            GameManager.manager.md2.cardDisplay3.gameObject.GetComponent<Market_Hover>().resetCard();
                            GameManager.manager.sendSuccessMessage(1);
                        }
                        else
                        {
                            GameManager.manager.sendErrorMessage(6);
                        }
                        break;
                    default:
                        Debug.Log("MarketDeck Error!");
                        break;
                }
                return;
            }
        }
    }

    [Command]
    public void CmdBuyTopCard(string throwerName, int marketCard)
    {
        switch (marketCard)
        {
            case 1:
                RpcBuyTopCard(throwerName, marketCard);
                break;
            case 2:
                RpcBuyTopCard(throwerName, marketCard);
                break;
            case 3:
                RpcBuyTopCard(throwerName, marketCard);
                break;
            default:
                break;

        }
    }

    [Command]
    public void CmdBuyBottomCard(string throwerName, int marketCard)
    {
        switch (marketCard)
        {
            case 1:
                RpcBuyBottomCard(throwerName, marketCard);
                break;
            case 2:
                RpcBuyBottomCard(throwerName, marketCard);
                break;
            case 3:
                RpcBuyBottomCard(throwerName, marketCard);
                break;
            default:
                break;

        }
    }

    [ClientRpc]
    public void RpcEndTurn(string name)
    {
        Debug.Log("Ending turn for: " + playerName);
        for (int i = 0; i < Game.GamePlayers.Count; i++)
        {
            if (Game.GamePlayers[i].playerName == name)
            {
                Debug.Log("Found them");
                // Logic to check for end of turn effect ring
                foreach (CardDisplay cd in GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList)
                {
                    if (cd.card.cardName == "Vengeful Ring of the Cursed Mutterings")
                    {
                        if (GameManager.manager.players[GameManager.manager.myPlayerIndex].doubleRingBonus)
                        {
                            GameManager.manager.dealDamageToAll(4);
                        }
                        else
                        {
                            GameManager.manager.dealDamageToAll(2);
                        }
                    }
                }
                GameManager.manager.players[GameManager.manager.myPlayerIndex].currentPlayerHighlight.SetActive(false);

                GameManager.manager.myPlayerIndex = i + 1;
                if (GameManager.manager.myPlayerIndex >= GameManager.manager.numPlayers)
                {
                    GameManager.manager.myPlayerIndex = 0;
                }
                GameManager.manager.sendSuccessMessage(18);
                GameManager.manager.currentPlayerName = Game.GamePlayers[GameManager.manager.myPlayerIndex].playerName;
                foreach (CardPlayer cp in GameManager.manager.players)
                {
                    if(cp.name == GameManager.manager.currentPlayerName)
                    {
                        GameManager.manager.onStartTurn(cp);
                    }
                }
                //GameManager.manager.onStartTurn(GameManager.manager.players[GameManager.manager.myPlayerIndex]);
                return;
            }
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdEndTurn(string name)
    {
        Debug.Log("Executing CmdEndTurn on the server for player: " + playerName);
        RpcEndTurn(name);
    }

    [ClientRpc]
    public void RpcTrashCard(string name, int selectedCard)
    {
        Debug.Log("Trashing card for: " + playerName);

        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == playerName)
            {
                // Savory Layer Cake logic
                if (cp.holster.cardList[selectedCard - 1].card.cardName == "SavoryLayerCake")
                {
                    // Heals for +3 HP if trashed
                    cp.addHealth(3);
                }
                if (GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[selectedCard - 1].card.cardQuality != "Starter")
                {
                    cp.cardsTrashed++;
                }
               GameManager.manager.td.addCard(cp.holster.cardList[selectedCard - 1]);

                if (cp.isSaltimbocca && cp.cardsTrashed == 4)
                {
                    GameManager.manager.sendSuccessMessage(15);
                    cp.character.canBeFlipped = true;
                }
                cp.holster.cardList[selectedCard - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                // GameManager.manager.sendSuccessMessage(9);
                return;
            }
        }
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