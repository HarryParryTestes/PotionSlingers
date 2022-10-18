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

    [TargetRpc]
    public void RpcEverybodyTrashOneCard(string throwerName)
    {
        Debug.Log("Pulling up Opponent Holster Menu for: " + playerName);
        foreach(CardPlayer cp in GameManager.manager.players)
        {
            if(cp.name == throwerName)
            {
                GameManager.manager.tempPlayer = cp;
                GameManager.manager.opponentHolsterMenu.SetActive(true);
                GameManager.manager.displayOpponentHolster();
            }
        }
    }

    [TargetRpc]
    public void RpcTrashOneCard(string throwerName)
    {
        Debug.Log("Pulling up Opponent Trash Menu for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if(cp.name == throwerName)
            {
                GameManager.manager.tempPlayer = cp;
                GameManager.manager.opponentHolsterMenu.SetActive(true);
                GameManager.manager.displayOpponentHolster();
            }
        }
    }

    [TargetRpc]
    public void RpcStarterPotionMenu(string throwerName)
    {
        Debug.Log("Pulling up Opponent Holster Menu for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == throwerName)
            {
                GameManager.manager.starterPotionMenu.SetActive(true);
            }
        }
    }

    [TargetRpc]
    public void RpcPluotPotionMenu(string throwerName)
    {
        Debug.Log("Pulling up Opponent Holster Menu for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == throwerName)
            {
                GameManager.manager.pluotPotionMenu.SetActive(true);
            }
        }
    }

    [ClientRpc]
    public void RpcSetPluotBonus(string throwerName, string bonus)
    {
        Debug.Log("Setting Pluot bonus for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == throwerName)
            {
                cp.pluotBonusType = bonus;
            }
        }
    }

    [Command]
    public void CmdSetPluotBonus(string throwerName, string bonus)
    {
        // shuffle market decks
        Debug.Log("Executing CmdSetPluotBonus on the server for player: " + playerName);
        RpcSetPluotBonus(throwerName, bonus);
    }

    [Command]
    public void CmdEverybodyTrashOneCard(string throwerName)
    {
        // shuffle market decks
        Debug.Log("Executing CmdEverybodyTrashOneCard on the server for everyone except player: " + playerName);
        foreach (GamePlayer gp in Game.GamePlayers)
        {
            if(gp.playerName != throwerName)
            {
                gp.RpcEverybodyTrashOneCard(gp.playerName);
            }
        }
        
    }

    [Command]
    public void CmdAddStarterPotion(string throwerName)
    {
        Debug.Log("Executing CmdThrowCard on the server for player: " + playerName);
        RpcAddStarterPotion(throwerName);
    }

    [ClientRpc]
    public void RpcAddStarterPotion(string throwerName)
    {
        Debug.Log("Adding starter potion for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == throwerName)
            {
                cp.deck.putCardOnTop(GameManager.manager.starterPotionCard);
            }
        }
    }

    [Command]
    public void CmdThrowCard(string throwerName, string opponentName, int selectedCard)
    {
        Debug.Log("Executing CmdThrowCard on the server for player: " + playerName);
        RpcThrowCard(throwerName, opponentName, selectedCard);
    }

    [ClientRpc]
    public void RpcTakeMarketCard(string throwerName, int marketCard)
    {
        Debug.Log("Throwing card for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == throwerName)
            {
                switch (marketCard)
                {
                    case 1:
                        cp.deck.putCardOnTop(GameManager.manager.md1.cardDisplay1.card);
                        Card card1 = GameManager.manager.md1.popCard();
                        GameManager.manager.md1.cardDisplay1.updateCard(card1);
                        GameManager.manager.sendSuccessMessage(17);
                        break;
                    case 2:
                        cp.deck.putCardOnTop(GameManager.manager.md1.cardDisplay2.card);
                        Card card2 = GameManager.manager.md1.popCard();
                        GameManager.manager.md1.cardDisplay2.updateCard(card2);
                        GameManager.manager.sendSuccessMessage(17);
                        break;
                    case 3:
                        cp.deck.putCardOnTop(GameManager.manager.md1.cardDisplay3.card);
                        Card card3 = GameManager.manager.md1.popCard();
                        GameManager.manager.md1.cardDisplay3.updateCard(card3);
                        GameManager.manager.sendSuccessMessage(17);
                        break;
                    case 4:
                        cp.deck.putCardOnTop(GameManager.manager.md2.cardDisplay1.card);
                        Card card4 = GameManager.manager.md2.popCard();
                        GameManager.manager.md2.cardDisplay1.updateCard(card4);
                        GameManager.manager.sendSuccessMessage(17);
                        break;
                    case 5:
                        cp.deck.putCardOnTop(GameManager.manager.md2.cardDisplay2.card);
                        Card card5 = GameManager.manager.md2.popCard();
                        GameManager.manager.md2.cardDisplay2.updateCard(card5);
                        GameManager.manager.sendSuccessMessage(17);
                        break;
                    case 6:
                        cp.deck.putCardOnTop(GameManager.manager.md2.cardDisplay3.card);
                        Card card6 = GameManager.manager.md2.popCard();
                        GameManager.manager.md2.cardDisplay3.updateCard(card6);
                        GameManager.manager.sendSuccessMessage(17);
                        break;
                    default:
                        break;
                }

                GameManager.manager.takeMarketMenu.SetActive(false);
                return;
            }
        }
    }

    [Command]
    public void CmdTakeMarketCard(string throwerName, int marketCard)
    {
        Debug.Log("Executing CmdTakeMarketCard on the server for player: " + playerName);
        RpcTakeMarketCard(throwerName, marketCard);
    }

    [ClientRpc]
    public void RpcTrashHolster(string throwerName)
    {
        Debug.Log("Trashing holster for: " + playerName);
        foreach(CardPlayer cp in GameManager.manager.players)
        {
            if(cp.name == throwerName)
            {
                foreach(CardDisplay cd in cp.holster.cardList)
                {
                    GameManager.manager.td.addCard(cd);
                }
            }
        }
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
                    damage = cp.checkRingBonus(damage, cp.holster.cardList[selectedCardInt - 1]);
                    damage = cp.checkBonus(damage, selectedCardInt);
                    Debug.Log("Damage after thrower bonuses: " + damage);
                    damage = GameManager.manager.tempPlayer.checkDefensiveBonus(damage, selectedCardInt);
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
                        Debug.Log("ARTIFACT");
                        damage = cp.holster.cardList[selectedCardInt - 1].card.effectAmount;
                        Debug.Log("Original damage: " + damage);
                        damage = cp.checkRingBonus(damage, cp.holster.cardList[selectedCardInt - 1]);
                        damage = cp.checkArtifactBonus(damage, cp.holster.cardList[selectedCardInt - 1]);
                        Debug.Log("Damage after thrower bonuses: " + damage);
                        damage = GameManager.manager.tempPlayer.checkDefensiveBonus(damage, selectedCardInt);
                        Debug.Log("Damage after defensive bonuses: " + damage);

                        // Update response to account for trashing loaded artifact's potion and not the artifact
                        cp.artifactsUsed++;
                        GameManager.manager.td.addCard(cp.holster.cardList[selectedCardInt - 1].aPotion);
                        cp.holster.cardList[selectedCardInt - 1].artifactSlot.transform.parent.gameObject.SetActive(false);

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
                        Debug.Log("VESSEL");
                        if (cp.isTwins && cp.character.character.flipped)
                        {
                            cp.addHealth(4);
                        }
                        //int damage = players[throwerIndex].holster.card1.vPotion1.card.effectAmount + players[throwerIndex].holster.card1.vPotion2.card.effectAmount;
                        damage = cp.holster.cardList[selectedCardInt - 1].vPotion1.card.effectAmount + cp.holster.cardList[selectedCardInt - 1].vPotion2.card.effectAmount;
                        Debug.Log("Original damage: " + damage);
                        damage = cp.checkVesselBonus(damage, cp.holster.cardList[selectedCardInt - 1]);
                        Debug.Log("Damage after thrower bonuses: " + damage);
                        damage = GameManager.manager.tempPlayer.checkDefensiveBonus(damage, selectedCardInt);
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

    [TargetRpc]
    public void RpcSweetbitterMenu(string throwerName)
    {
        Debug.Log("Enabling Sweetbitter menu for: " + playerName);
        GameManager.manager.sweetbitterMenu.SetActive(true);
    }

    [Command]
    public void CmdCheckPlayerAction(string throwerName)
    {
        Debug.Log("Executing CmdSellCard on the server for player: " + playerName);
        RpcCheckPlayerAction(throwerName);
    }

    [Command]
    public void CmdAddTP(string throwerName)
    {
        Debug.Log("Executing CmdSellCard on the server for player: " + playerName);
        RpcAddTP(throwerName);
    }

    [Command]
    public void CmdAddCBB(string throwerName)
    {
        Debug.Log("Executing CmdAddCBB on the server for player: " + playerName);
        RpcAddCBB(throwerName);
    }

    [Command]
    public void CmdAddEI(string throwerName)
    {
        Debug.Log("Executing CmdAddEI on the server for player: " + playerName);
        RpcAddEI(throwerName);
    }

    [Command]
    public void CmdAddPS(string throwerName)
    {
        Debug.Log("Executing CmdAddPS on the server for player: " + playerName);
        RpcAddPS(throwerName);
    }

    [Command]
    public void CmdAddReetsCard(string throwerName)
    {
        Debug.Log("Executing CmdAddPS on the server for player: " + playerName);
        RpcAddReetsCard(throwerName);
    }

    [ClientRpc]
    public void RpcAddPS(string throwerName)
    {
        Debug.Log("Adding The Blacksnake Pip Sling in holster for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == throwerName)
            {
                if (cp.character.character.flipped)
                {
                    if (cp.pips > 1)
                    {
                        cp.subPips(1);
                        cp.addReetsCard();
                    }
                    else
                    {
                        Debug.Log("Did this fire?");
                        GameManager.manager.sendErrorMessage(6);
                    }
                }
                else
                // not flipped
                {
                    if (cp.pips > 2)
                    {
                        cp.subPips(2);
                        cp.addReetsCard();
                    }
                    else
                    {
                        Debug.Log("Did this fire?");
                        GameManager.manager.sendErrorMessage(6);
                    }
                }
            }
        }
    }

    [ClientRpc]
    public void RpcAddPS(string throwerName)
    {
        Debug.Log("Adding The Blacksnake Pip Sling in holster for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == throwerName)
            {
                // send an error message if they don't have enough pips
                if (cp.pips < 3 || cp.character.uniqueCardUsed)
                {
                    GameManager.manager.sendErrorMessage(6);
                    return;
                }
                cp.subPips(3);
                cp.addPipSling();

                foreach (CardDisplay cd in cp.holster.cardList)
                {
                    if (cd.card.cardName == "Blacksnake Pip Sling")
                    {
                        cp.character.uniqueCardUsed = true;
                    }
                }
            }
        }
    }

    [ClientRpc]
    public void RpcAddEI(string throwerName)
    {
        Debug.Log("Adding The Extra Inventory in holster for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == throwerName)
            {
                if (cp.character.uniqueCardUsed)
                {
                    // add error message
                    GameManager.manager.sendErrorMessage(6);
                    return;
                }

                cp.addExtraInventory();

                foreach (CardDisplay cd in cp.holster.cardList)
                {
                    if (cd.card.cardName == "Extra Inventory")
                    {
                        cp.character.uniqueCardUsed = true;
                    }
                }
            }
        }
    }

    [ClientRpc]
    public void RpcAddCBB(string throwerName)
    {
        Debug.Log("Adding The Cherrybomb Badge in holster for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == throwerName)
            {
                if (players[myPlayerIndex].pips < 3 || players[myPlayerIndex].character.uniqueCardUsed)
                {
                    sendErrorMessage(6);
                    return;
                }
                cp.subPips(3);
                cp.addCherryBombBadge();

                foreach (CardDisplay cd in cp.holster.cardList)
                {
                    if (cd.card.cardName == "Cherrybomb Badge")
                    {
                        cp.character.uniqueCardUsed = true;
                    }
                }
            }
        }
    }

    [ClientRpc]
    public void RpcAddTP(string throwerName)
    {
        Debug.Log("Adding The Phylactery in holster for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if(cp.name == throwerName)
            {
                if (cp.pips >= 6 && !cp.character.uniqueCardUsed)
                {
                    cp.addThePhylactery();
                    cp.character.uniqueCardUsed = true;
                }
                else
                {
                    // you are too poor or you did it already
                    Debug.Log("You are poor!");
                    GameManager.manager.sendErrorMessage(6);
                }
            }
        }
    }

    [TargetRpc]
    public void RpcIsadoreMenu(string throwerName)
    {
        Debug.Log("Enabling Isadore menu for: " + playerName);
        GameManager.manager.isadoreMenu.SetActive(true);
    }

    [TargetRpc]
    public void RpcPluotMenu(string throwerName)
    {
        Debug.Log("Enabling Pluot menu for: " + playerName);
        GameManager.manager.ExtraInventoryMenu.SetActive(true);
    }

    [TargetRpc]
    public void RpcReetsMenu(string throwerName)
    {
        Debug.Log("Enabling Reets menu for: " + playerName);
        GameManager.manager.reetsMenu.SetActive(true);

        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if(cp.name == throwerName)
            {
                if (cp.character.character.flipped)
                {
                    GameManager.manager.reetsMenuText.text = "Pay 1P to add top card of deck to Holster?";
                    GameManager.manager.reetsCard.GetComponent<Image>().sprite = GameManager.manager.sprite1;
                }
                else
                {
                    GameManager.manager.reetsMenuText.text = "Pay 2P to add top card of deck to Holster?";
                    GameManager.manager.reetsCard.GetComponent<Image>().sprite = GameManager.manager.sprite2;
                }
            }
        }
    }

    [TargetRpc]
    public void RpcNicklesMenu(string throwerName)
    {
        Debug.Log("Enabling Nickles menu for: " + playerName);
        GameManager.manager.nicklesUI.SetActive(true);
    }

    [TargetRpc]
    public void RpcFlippedNicklesMenu(string throwerName)
    {
        Debug.Log("Enabling flipped Nickles menu for: " + playerName);
        GameManager.manager.flippedNicklesUI.SetActive(true);
    }

    [ClientRpc]
    public void RpcCheckPlayerAction(string throwerName)
    {
        Debug.Log("Checking player action for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == throwerName)
            {
                if (cp.isSweetbitter)
                {
                    if (cp.character.character.flipped)
                    {
                        GameManager.manager.sendErrorMessage(10);
                    }
                    else
                    {
                        // Target RPC for Sweetbitter Menu
                        RpcSweetbitterMenu(throwerName);
                        //sweetbitterMenu.SetActive(true);
                    }
                }

                // change this to flipped and not !flipped after you test this
                if (cp.isIsadore && !cp.character.character.flipped && cp.character.uniqueCardUsed)
                {
                    // Target RPC for Isadore Menu
                    RpcIsadoreMenu(throwerName);
                    //isadoreMenu.SetActive(true);
                }
                else
                {
                    // not able to do action
                    // fix this later
                    //sendErrorMessage(13);
                }

                if (cp.isPluot && cp.character.character.flipped)
                {
                    // prompt ui for adding Extra Inventory into holster
                    // Target RPC for Pluot Menu
                    RpcPluotMenu(throwerName);
                    //ExtraInventoryMenu.SetActive(true);
                }
                else
                {
                    // not able to do action
                    //sendErrorMessage(13);
                }

                if (cp.isReets)
                {
                    // Target RPC for Reets Menu
                    RpcReetsMenu(throwerName);
                    /*
                    reetsMenu.SetActive(true);
                    if (players[myPlayerIndex].character.character.flipped)
                    {
                        reetsMenuText.text = "Pay 1P to add top card of deck to Holster?";
                        reetsCard.GetComponent<Image>().sprite = sprite1;
                    }
                    else
                    {
                        reetsMenuText.text = "Pay 2P to add top card of deck to Holster?";
                        reetsCard.GetComponent<Image>().sprite = sprite2;
                    }
                    */
                }

                if (cp.isNickles && !cp.nicklesAction)
                {
                    if (!cp.character.character.flipped)
                    {
                        // Target RPC for Nickles Menu
                        RpcNicklesMenu(throwerName);

                        // unflipped Nickles UI
                        //nicklesUI.SetActive(true);
                    }
                    else
                    {
                        // Target RPC for flipped Nickles Menu
                        RpcFlippedNicklesMenu(throwerName);

                        // flipped Nickles UI
                        //flippedNicklesUI.SetActive(true);
                    }
                }
                else if (cp.isNickles && cp.nicklesAction)
                {
                    // error message because you already did it once this turn
                    GameManager.manager.sendErrorMessage(15);
                }
            }
        }
    }

                [ClientRpc]
    public void RpcCheckFlip(string throwerName)
    {
        Debug.Log("Checking flip for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == throwerName)
            {
                // insert logic here
                // characters that can flip back for free
                if (cp.character.character.flipped)
                {
                    if (cp.isSaltimbocca)
                    {
                        cp.character.flipCard();
                        cp.character.menu.SetActive(false);
                    }
                    else
                    {
                        GameManager.manager.sendErrorMessage(11);
                        cp.character.menu.SetActive(false);
                    }

                    // pay 2 pips to flip sweetbitter back to front
                    if (cp.isSweetbitter && cp.pips > 2)
                    {
                        cp.subPips(2);
                        cp.character.flipCard();
                        cp.character.menu.SetActive(false);
                    }
                    else
                    {
                        GameManager.manager.sendErrorMessage(11);
                        cp.character.menu.SetActive(false);
                    }
                }

                if (cp.character.canBeFlipped)
                {
                    cp.character.flipCard();
                    cp.character.menu.SetActive(false);
                }
                else
                {
                    // character card flip error
                    //sendErrorMessage(11);
                    cp.character.menu.SetActive(false);
                }
            }
        }
    }

    [TargetRpc]
    public void RpcBRMenuActive()
    {
        GameManager.manager.bottleRocketMenu.SetActive(true);
    }

    [ClientRpc]
    public void RpcCheckCardAction(string throwerName, int selectedCardInt)
    {
        Debug.Log("Checking card action for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == throwerName)
            {
                // Bottle Rocket UI Logic
                if (cp.holster.cardList[selectedCardInt - 1].card.cardName == "BottleRocket")
                {
                    // SetActive the UI
                    //bottleRocketMenu.SetActive(true);
                    Debug.Log("Target RPC, BottleRocket Menu Active");
                    RpcBRMenuActive();
                }
            }
        }
    }

    [ClientRpc]
    public void RpcBottleRocketBonus(string throwerName, int selectedCardInt)
    {
        Debug.Log("Checking Bottle Rocket bonus for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == throwerName)
            {
                if (cp.pips >= 3 || cp.bottleRocketBonus)
                {
                    cp.bottleRocketBonus = true;
                    // add some success message but change what you initially put here lol
                    GameManager.manager.sendSuccessMessage(14);
                    // reset card
                    cp.holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                }
                else
                {
                    GameManager.manager.sendErrorMessage(6);
                    // reset card
                    cp.holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                }
            }
        }
    }

    [Command]
    public void CmdBottleRocketBonus(string throwerName, int selectedCardInt)
    {
        Debug.Log("Executing CmdBottleRocketBonus on the server for player: " + playerName);
        RpcBottleRocketBonus(throwerName, selectedCardInt);
    }

    [Command]
    public void CmdCheckCardAction(string throwerName, int selectedCardInt)
    {
        Debug.Log("Executing CmdCheckCardAction on the server for player: " + playerName);
        RpcCheckCardAction(throwerName, selectedCardInt);
    }

    [Command]
    public void CmdCheckFlip(string throwerName)
    {
        Debug.Log("Executing CmdCheckFlip on the server for player: " + playerName);
        RpcCheckFlip(throwerName);
    }

    [Command]
    public void CmdTakeTrashCard(string throwerName, int cardInt)
    {
        Debug.Log("Executing CmdTakeTrashCard on the server for player: " + playerName);
        RpcTakeTrashCard(throwerName, cardInt);
    }

    [ClientRpc]
    public void RpcTakeTrashCard(string throwerName, int cardInt)
    {
        Debug.Log("Taking trash card for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == throwerName)
            {
                cp.deck.putCardOnTop(GameManager.manager.td.deckList[cardInt]);
                GameManager.manager.td.deckList.RemoveAt(cardInt);
            }
        }
    }

    [TargetRpc]
    public void RpcTMMenuActive()
    {
        // this hopefully should trigger on only the character who got the bonus
        // it does, otherwise mirror is gaslighting me
        GameManager.manager.takeMarketMenu.SetActive(true);
        GameManager.manager.updateTakeMarketMenu();
    }

    [TargetRpc]
    public void RpcTDMenuActive()
    {
        GameManager.manager.trashDeckBonus = true;
        GameManager.manager.trashDeckMenu.SetActive(true);
        GameManager.manager.trashText.text = "Take a potion from the trash and put it on top of your deck!";
        GameManager.manager.td.displayTrash();
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
        Debug.Log("Executing CmdLoadCard on the server for player: " + playerName);
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
                        if (cp.pips >= GameManager.manager.md1.cardDisplay3.card.buyPrice && !cp.isSaltimbocca)
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
                            Debug.Log("Nothing above this triggered");
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

    [TargetRpc]
    public void RpcShieldMenu(string throwerName)
    {
        Debug.Log("Pulling up Sheild Menu for: " + playerName);
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.name == throwerName)
            {
                GameManager.manager.faisalMenu.SetActive(true);
            }
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

    [Command]
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
                if (cp.holster.cardList[selectedCard - 1].card.cardQuality != "Starter")
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

    [Command(requiresAuthority = false)]
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