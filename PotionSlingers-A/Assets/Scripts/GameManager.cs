using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Steamworks;
using Mirror;

public class GameManager : MonoBehaviour
{

    public static SteamLobby lobby;
    public static GameManager manager;
    //private OldNetworkManager networkManager;

    // Later change to be however many players joined when the GameScene loads in.
    // (Maybe transferring numPlayers from MainMenu script to here? Maybe get from server?)
    public int numPlayers;
    public int selectedCardInt;
    public int selectedOpponentInt;
    public string selectedOpponentCharName;
    public int loadedCardInt;
    public int myPlayerIndex = 0; // used to be currentPlayer
    public int currentPlayerId = 0;
    public int nicklesDamage = 0;
    public CardPlayer[] players = new CardPlayer[4];
    public Character[] characters;
    public Dialog dialog;
    public GameObject dialogBox;
    GameObject ob;
    GameObject obTop;
    GameObject obLeft;
    GameObject obRight;
    public TrashDeck td;
    public MarketDeck md1;
    public MarketDeck md2;
    public List<GameObject> successMessages;
    public List<GameObject> errorMessages;
    private MessageQueue msgQueue;

    public GameObject pluotPotionMenu;
    public GameObject ExtraInventoryMenu;
    public GameObject nicklesUI;
    public GameObject flippedNicklesUI;
    public GameObject flippedPipMenu;
    public GameObject reetsMenu;
    public GameObject isadoreMenu;
    public GameObject sweetbitterMenu;
    public GameObject bottleRocketMenu;

    public Holster playerHolster;
    public Deck playerDeck;

    public CardPlayer bolo;
    public CardPlayer cardPlayer;
    public CardPlayer tempPlayer;

    public CardDisplay starterPotionDisplay;

    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;
    public CharacterDisplay opLeft;
    public CharacterDisplay opTop;
    public CharacterDisplay opRight;
    public TMPro.TextMeshProUGUI playerBottomName;
    public TMPro.TextMeshProUGUI playerLeftName;
    public TMPro.TextMeshProUGUI playerTopName;
    public TMPro.TextMeshProUGUI playerRightName;
    public GameObject attackMenu;
    public GameObject loadMenu;
    public GameObject pauseUI;
    public GameObject starterPotionMenu;

    bool paused = false;
    bool starterPotion = false;
    bool usedStarterPotion = false;
    bool isadoreAction = true;

    public TMPro.TextMeshProUGUI reetsMenuText;
    public GameObject reetsCard;

    public Sprite sprite1;
    public Sprite sprite2;

    public Card starterPotionCard;

    GameObject mainMenu;
    MainMenu mainMenuScript;

    GameObject player1Area;
    GameObject player2Area;

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

    void Awake()
    {
        manager = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key was pressed");
            paused = !paused;
            if (paused)
            {
                pauseUI.SetActive(true);
            } else
            {
                pauseUI.SetActive(false);
            }
            
        }
    }

    public void ohFuckGoBack()
    {
        Debug.Log("Going back to title menu");
        Game.tutorial = false;
        SceneManager.LoadScene("TitleMenu");
        //Game.ServerChangeScene("TitleMenu");
    }

    void Start()
    {
        Debug.Log("GameManager started!!!");

        p3.SetActive(true);
        p4.SetActive(true);

        Debug.Log("Number of players on network: " + Game.GamePlayers.Count);

        initDecks();

        foreach(GamePlayer gp in Game.GamePlayers)
        {
            Debug.Log(gp.playerName);
        }

        numPlayers = Game.GamePlayers.Count;

        // dummy players for tutorial
        if (Game.tutorial)
        {
            Debug.Log("Starting tutorial");
            dialogBox.SetActive(true);
            playerBottomName.text = SteamFriends.GetPersonaName().ToString();
            playerTopName.text = "BOLO";
            p3.SetActive(false);
            p4.SetActive(false);
            return;
            //Debug.Log("Shuffling market decks for tutorial");
        }

        if (Game.GamePlayers.Count == 2)
        {
            p3.SetActive(false);
            p4.SetActive(false);
        }

        if (Game.GamePlayers.Count == 3)
        {
            //p3.SetActive(false);
            p4.SetActive(false);
        }

        // shuffle market decks
        md1.shuffle();
        md2.shuffle();

        //ob = GameObject.Find("CharacterCard");
        //Player playerOb = ob.GetComponent<Player>();
        //Debug.Log("Player 1's character is... " + playerOb.charName);
        for (int i = 0; i < Game.GamePlayers.Count; i++)
        {
            int tracker = 0;
            if (Game.GamePlayers[i].isLocalPlayer)
            {
                Debug.Log("Found local player");
                playerBottomName.text = Game.GamePlayers[i].playerName;
                players[0].name = Game.GamePlayers[i].playerName;
                players[0].charName = Game.GamePlayers[i].charName;
                players[0].character.onCharacterClick(Game.GamePlayers[i].charName);
                players[0].checkCharacter();
                
            }
            else
            {
                if (tracker == 0)
                {
                    playerLeftName.text = Game.GamePlayers[i].playerName;
                    players[1].name = Game.GamePlayers[i].playerName;
                    players[1].charName = Game.GamePlayers[i].charName;
                    players[1].character.onCharacterClick(Game.GamePlayers[i].charName);
                    players[1].checkCharacter();
                    tracker++;
                }
                if (tracker == 1)
                {
                    playerTopName.text = Game.GamePlayers[i].playerName;
                    players[2].name = Game.GamePlayers[i].playerName;
                    players[2].charName = Game.GamePlayers[i].charName;
                    players[2].character.onCharacterClick(Game.GamePlayers[i].charName);
                    players[2].checkCharacter();
                    tracker++;
                }
                if (tracker == 2)
                {
                    playerRightName.text = Game.GamePlayers[i].playerName;
                    players[3].name = Game.GamePlayers[i].playerName;
                    players[3].charName = Game.GamePlayers[i].charName;
                    players[3].character.onCharacterClick(Game.GamePlayers[i].charName);
                    players[3].checkCharacter();
                }

            }
        }
        /*
        menu = GameObject.Find("MainMenuScript").GetComponent<MainMenu>();
        networkManager = GameObject.Find("OldNetworkManager").GetComponent<OldNetworkManager>();
        msgQueue = networkManager.GetComponent<MessageQueue>();
        msgQueue.AddCallback(Constants.SMSG_P_THROW, onResponsePotionThrow);
        msgQueue.AddCallback(Constants.SMSG_END_TURN, onResponseEndTurn);
        msgQueue.AddCallback(Constants.SMSG_BUY, onResponseBuy);
        msgQueue.AddCallback(Constants.SMSG_SELL, onResponseSell);
        msgQueue.AddCallback(Constants.SMSG_CYCLE, onResponseCycle);
        msgQueue.AddCallback(Constants.SMSG_TRASH, onResponseTrash);
        msgQueue.AddCallback(Constants.SMSG_LOAD, onResponseLoad);
        */

        /*
        mainMenu = GameObject.Find("MainMenuScript");
        mainMenuScript = mainMenu.GetComponent<MainMenu>();

        numPlayers = mainMenuScript.getNumPlayers();
        Debug.Log("NumPlayers is: " + numPlayers);
        Debug.Log("P1 ID is: " + mainMenuScript.p1UserId);
        Debug.Log("P2 ID is: " + mainMenuScript.p2UserId);
        */

        //init();
    }

    public void init()
    {
        initDecks();
    }

    public void initDecks()
    {
        //td = GameObject.Find("TrashPile").GetComponent<TrashDeck>();
        md1 = GameObject.Find("PotionPile").GetComponent<MarketDeck>();
        md1.init();
        md2 = GameObject.Find("SpecialCardPile").GetComponent<MarketDeck>();
        md2.init();
    }

    public void checkFlip()
    {
        // TUTORIAL LOGIC
        if(Game.tutorial)
        {
            if(dialog.textBoxCounter != 33)
            {
                cardPlayer.character.canBeFlipped = false;
                sendErrorMessage(11);
            } 
            else
            {
                cardPlayer.character.canBeFlipped = true;
                cardPlayer.character.flipCard();
                sendSuccessMessage(11);
            }
            return;
        }

        // characters that can flip back for free
        if (players[myPlayerIndex].character.character.flipped)
        {
            if (players[myPlayerIndex].isSaltimbocca)
            {
                players[myPlayerIndex].character.flipCard();
                players[myPlayerIndex].character.menu.SetActive(false);
            }
            else
            {
                sendErrorMessage(11);
                players[myPlayerIndex].character.menu.SetActive(false);
            }

            // pay 2 pips to flip sweetbitter back to front
            if (players[myPlayerIndex].isSweetbitter && players[myPlayerIndex].pips > 2)
            {
                players[myPlayerIndex].subPips(2);
                players[myPlayerIndex].character.flipCard();
                players[myPlayerIndex].character.menu.SetActive(false);
            } else
            {
                sendErrorMessage(11);
                players[myPlayerIndex].character.menu.SetActive(false);
            }
        }

        if (players[myPlayerIndex].character.canBeFlipped)
        {
            players[myPlayerIndex].character.flipCard();
            players[myPlayerIndex].character.menu.SetActive(false);
        } else
        {
            // character card flip error
            //sendErrorMessage(11);
            players[myPlayerIndex].character.menu.SetActive(false);
        }
    }

    public void SetPluotBonus(string bonus)
    {
        if (Game.tutorial)
        {
            cardPlayer.pluotBonusType = bonus;
            return;
        }
        players[myPlayerIndex].pluotBonusType = bonus;
    }

    // if there are open spots in the holster, move cards from deck to holster
    public void onStartTurn(CardPlayer player)
    {
        Debug.Log(player.name + "'s turn!");
        usedStarterPotion = false;
        foreach(CardDisplay cd in player.holster.cardList)
        {
            if(player.deck.deckList.Count >= 1)
            {
                if(cd.card.name == "placeholder")
                {
                    cd.updateCard(player.deck.popCard());
                }
            }
        }
        if (player.isPluot)
        {
            pluotPotionMenu.SetActive(true);
        }
        player.setDefaultTurn();
    }

    public void setSCInt(int num)
    {
        selectedCardInt = num;
    }

    public void setOPInt(int num)
    {
        selectedOpponentInt = num;
    }

    public void setOPName(string characterName)
    {
        selectedOpponentCharName = characterName;
    }

    public void setLoadedInt(string cardName)
    {
        // TUTORIAL LOGIC
        if (Game.tutorial)
        {
            for (int i = 0; i < 4; i++)
            {
                if (playerHolster.cardList[i].card.cardName == cardName)
                {
                    loadedCardInt = i;
                }
            }
            return;
        }

        for(int i = 0; i < 4; i++)
        {
            if(players[myPlayerIndex].holster.cardList[i].card.cardName == cardName)
            {
                loadedCardInt = i;
            }
        }
    }

    // DONE: fix this to display properly
    // DISPLAY OPPONENTS
    public void displayOpponents()
    {
        // TUTORIAL LOGIC
        if (Game.tutorial)
        {
            opLeft.gameObject.SetActive(false);
            opRight.gameObject.SetActive(false);
            opTop.onCharacterClick("Bolo");
            return;
        }


        // Displaying opponents to attack for 2 player game.
        if(numPlayers == 2) 
        {
            opLeft.gameObject.SetActive(false);
            opRight.gameObject.SetActive(false);
            // For all players that are not this client's player, display their character in attackMenu.
                    opTop.updateCharacter(players[2].character.character);
        }

        // Displaying opponents to attack for 3 player game.
        if(numPlayers == 3) 
        {
            opLeft.gameObject.SetActive(true);
            opTop.gameObject.SetActive(true);
            opRight.gameObject.SetActive(false);
            int tracker = 0;

            opLeft.updateCharacter(players[1].character.character);
            opTop.updateCharacter(players[2].character.character);

        }

        // Displaying opponents to attack for 4 player game.
        if(numPlayers == 4) 
        {
            opLeft.gameObject.SetActive(true);
            opRight.gameObject.SetActive(true);
            int tracker = 0;

            opLeft.updateCharacter(players[1].character.character);
            opTop.updateCharacter(players[2].character.character);
            opRight.updateCharacter(players[3].character.character);           
        }
    }

    // find potions and display them in LoadItemMenu
    // DONE: fix this to display properly
    public void displayPotions()
    {
        int set = 0;
        loadMenu.transform.Find("Card (Left)").gameObject.SetActive(true);
        loadMenu.transform.Find("Card (Middle)").gameObject.SetActive(true);
        loadMenu.transform.Find("Card (Right)").gameObject.SetActive(true);
        CardDisplay left = loadMenu.transform.Find("Card (Left)").GetComponent<CardDisplay>();
        CardDisplay middle = loadMenu.transform.Find("Card (Middle)").GetComponent<CardDisplay>();
        CardDisplay right = loadMenu.transform.Find("Card (Right)").GetComponent<CardDisplay>();

        // TUTORIAL LOGIC
        if (Game.tutorial)
        {
            foreach (CardDisplay cd in playerHolster.cardList)
            {
                if (cd.card.cardType.ToLower() == "artifact" || cd.card.cardType.ToLower() == "vessel")
                {
                    Debug.Log("CardName is: " + cd.card.cardName);
                    switch (set)
                    {
                        case 0:
                            left.card = cd.card;
                            Debug.Log("Left Card: " + left.card.cardName);
                            left.updateCard(cd.card);
                            set++;
                            break;
                        case 1:
                            middle.card = cd.card;
                            Debug.Log("Middle Card: " + middle.card.cardName);
                            middle.updateCard(cd.card);
                            set++;
                            break;
                        case 2:
                            right.card = cd.card;
                            Debug.Log("Right Card: " + right.card.cardName);
                            right.updateCard(cd.card);
                            break;
                        default:
                            break;
                    }
                }
            }
            if (set == 1)
            {
                // middle.updateCard(players[myPlayerIndex].deck.placeholder);
                // right.updateCard(players[myPlayerIndex].deck.placeholder);
                loadMenu.transform.Find("Card (Middle)").gameObject.SetActive(false);
                loadMenu.transform.Find("Card (Right)").gameObject.SetActive(false);
            }
            if (set == 2)
            {
                // right.updateCard(players[myPlayerIndex].deck.placeholder);
                loadMenu.transform.Find("Card (Right)").gameObject.SetActive(false);
            }
            return;
        }


        foreach (CardDisplay cd in players[myPlayerIndex].holster.cardList)
        {
            // Debug.Log("CardName is: " + cd.card.cardName + ". CardType is: " + cd.card.cardType);
            // if(cd.card.cardType.ToLower() == "potion")
            if(cd.card.cardType.ToLower() == "artifact" || cd.card.cardType.ToLower() == "vessel")
            {
                Debug.Log("CardName is: " + cd.card.cardName);
                switch (set)
                {
                    case 0:
                        left.card = cd.card;
                        Debug.Log("Left Card: " + left.card.cardName);
                        left.updateCard(cd.card);
                        set++;
                        break;
                    case 1:
                        middle.card = cd.card;
                        Debug.Log("Middle Card: " + middle.card.cardName);
                        middle.updateCard(cd.card);
                        set++;
                        break;
                    case 2:
                        right.card = cd.card;
                        Debug.Log("Right Card: " + right.card.cardName);
                        right.updateCard(cd.card);
                        break;
                    default:
                        break;
                }
            }
        }
        if(set == 1)
        {
            // middle.updateCard(players[myPlayerIndex].deck.placeholder);
            // right.updateCard(players[myPlayerIndex].deck.placeholder);
            loadMenu.transform.Find("Card (Middle)").gameObject.SetActive(false);
            loadMenu.transform.Find("Card (Right)").gameObject.SetActive(false);
        }
        if(set == 2)
        {
            // right.updateCard(players[myPlayerIndex].deck.placeholder);
            loadMenu.transform.Find("Card (Right)").gameObject.SetActive(false);
        }
    }

    // END TURN REQUEST
    public void endTurn()
    {

        // TUTORIAL LOGIC
        if (Game.tutorial)
        {
            Debug.Log("Tutorial turn ended");
            onStartTurn(cardPlayer);
            if(dialog.textBoxCounter == 24)
            {
                dialog.textBoxCounter++;
            }
            StartCoroutine(waitThreeSeconds(dialog));
            return;
        }


        // If this client isn't the current player, display error message.
        // Player can't end turn if it isn't their turn.
        // (to change this to maybe disable endTurn button or grey it out?? turn it from button to image when not currentPlayer?)
        if (players[myPlayerIndex].user_id != myPlayerIndex) 
        {
            // "You are not the currentPlayer!"
            sendErrorMessage(7);
        }

        else
        {
            myPlayerIndex++;
            if(myPlayerIndex == numPlayers)
            {
                myPlayerIndex = 0;
            }
            onStartTurn(players[myPlayerIndex]);
            Debug.Log("Request End Turn");

            // ADD IN NETWORKING LATER!

            //bool connected = networkManager.sendEndTurnRequest(myPlayerIndex);

            /*
            bool currentFound = false;
            int currentIndex = -1;
            for(int i = 0; i < numPlayers; i++)
            {
                if(players[i].user_id == currentPlayerId) {
                    currentIndex = i;
                }
            }

            int newIndex = currentIndex + 1;
            if(newIndex >= numPlayers) {
                newIndex = 0;
            }

            Debug.Log("Request End Turn");
            Debug.Log("Request: Ending turn for PlayerName: " + players[currentIndex].name);
            Debug.Log("Request: Starting turn for PlayerName: " + players[newIndex].name);
            Debug.Log("Request: Ending turn for PlayerID: " + players[currentIndex].user_id);
            Debug.Log("Request: Starting turn for PlayerID: " + players[newIndex].user_id);
            int newId = players[newIndex].user_id;
            */
            // Sends user_id of new current player based on index of new current player
            // in this client's players array.
            //bool connected = networkManager.sendEndTurnRequest(newId);

            // MATTEO: Add End Turn SFX here.

        }

        // myPlayerIndex++;
        // if(myPlayerIndex == numPlayers)
        // {
        //     myPlayerIndex = 0;
        // }
        // onStartTurn(players[myPlayerIndex]);
        // Debug.Log("Request End Turn");
        // bool connected = networkManager.sendEndTurnRequest(myPlayerIndex);
    }

    public void onResponseEndTurn(ExtendedEventArgs eventArgs)
    {
        Debug.Log("End Turn!");
        ResponseEndTurnEventArgs args = eventArgs as ResponseEndTurnEventArgs;
        Debug.Log("New Current Player: " + args.newCurrentPlayerId);
        Debug.Log("Previous player: " + args.user_id);

        if(args.user_id == args.newCurrentPlayerId) {
            Debug.Log("ERROR: Current player hasn't been set to the next player!");
        }

        for(int i = 0; i < numPlayers; i++) {
            // Remove currentPlayer highlight from previous player.
            if(players[i].user_id == args.user_id)
            {
                Debug.Log("Response: Ending turn for PlayerName: " + players[i].name);
                players[i].removeCurrentPlayer();
            }
            else if(players[i].user_id == args.newCurrentPlayerId)
            {
                Debug.Log("Request: Starting turn for PlayerName: " + players[i].name);
                currentPlayerId = args.newCurrentPlayerId;
                players[i].setCurrentPlayer();
                onStartTurn(players[i]);
            }
        }

        /*
        // if it didn't come from your own client, change the player
        if (Constants.USER_ID != args.user_id)
        {
            // player 1 just ended turn in 2p game
            if(args.user_id == 1)
            {
                myPlayerIndex = 1;
                // player 2 just ended turn in 2p game
            } else if(args.user_id == 2)
            {
                if(numPlayers > 2)
                {
                    myPlayerIndex = 2;
                }
                else
                {
                    myPlayerIndex = 0;
                }
            } else if (args.user_id == 3)
            {
                if(numPlayers > 3)
                {
                    myPlayerIndex = 3;
                } else
                {
                    myPlayerIndex = 0;
                }
            }
            onStartTurn(players[myPlayerIndex]);
        }
        */
    }

    public void addTP()
    {
        if (players[myPlayerIndex].pips >= 6 && !players[myPlayerIndex].character.uniqueCardUsed)
        {
            players[myPlayerIndex].addThePhylactery();
            players[myPlayerIndex].character.uniqueCardUsed = true;
        } else
        {
            // you are too poor or you did it already
            Debug.Log("You are poor!");
            sendErrorMessage(6);
        }
    }

    public void addEI()
    {
        if (Game.tutorial)
        {
            cardPlayer.addExtraInventory();
            return;
        }

        if (players[myPlayerIndex].character.uniqueCardUsed)
        {
            // add error message
            sendErrorMessage(6);
            return;
        }

        players[myPlayerIndex].addExtraInventory();

        foreach (CardDisplay cd in players[myPlayerIndex].holster.cardList)
        {
            if (cd.card.cardName == "Extra Inventory")
            {
                players[myPlayerIndex].character.uniqueCardUsed = true;
            }
        }
    }

    public void addPS()
    {
        // send an error message if they don't have enough pips
        if (players[myPlayerIndex].pips < 3 || players[myPlayerIndex].character.uniqueCardUsed)
        {
            sendErrorMessage(6);
            return;
        }
        players[myPlayerIndex].subPips(3);
        players[myPlayerIndex].addPipSling();

        foreach(CardDisplay cd in players[myPlayerIndex].holster.cardList)
        {
            if(cd.card.cardName == "Blacksnake Pip Sling")
            {
                players[myPlayerIndex].character.uniqueCardUsed = true;
            }
        }
    }

    public void addCBB()
    {
        if (players[myPlayerIndex].pips < 3 || players[myPlayerIndex].character.uniqueCardUsed)
        {
            sendErrorMessage(6);
            return;
        }
        players[myPlayerIndex].subPips(3);
        players[myPlayerIndex].addCherryBombBadge();

        foreach (CardDisplay cd in players[myPlayerIndex].holster.cardList)
        {
            if (cd.card.cardName == "Cherrybomb Badge")
            {
                players[myPlayerIndex].character.uniqueCardUsed = true;
            }
        }
    }

    public void addReetsCard()
    {
        if (players[myPlayerIndex].character.character.flipped)
        {
            if (players[myPlayerIndex].pips > 1)
            {
                players[myPlayerIndex].subPips(1);
                players[myPlayerIndex].addReetsCard();
            } else
            {
                Debug.Log("Did this fire?");
                sendErrorMessage(6);
            }
        }
        else
        // not flipped
        {
            if (players[myPlayerIndex].pips > 2)
            {
                players[myPlayerIndex].subPips(2);
                players[myPlayerIndex].addReetsCard();
            }
            else
            {
                Debug.Log("Did this fire?");
                sendErrorMessage(6);
            }
        }
        
    }

    public void setNicklesAttack(int damage)
    {
        nicklesDamage = damage;
        displayOpponents();
    }

    // any time ACTION is pressed on the player in the game scene
    // it will go through the logic in this method

    public void checkPlayerAction()
    {
        // TUTORIAL LOGIC
        if (Game.tutorial)
        {
            if (cardPlayer.character.character.flipped)
            {
                // do Pluot action (I'll code this in later)
                ExtraInventoryMenu.SetActive(true);

            } else
            {
                // display error message
                sendErrorMessage(10);
            }
            return;
        }

        if (players[myPlayerIndex].isSweetbitter)
        {
            if (players[myPlayerIndex].character.character.flipped)
            {
                sendErrorMessage(10);
            } else
            {
                sweetbitterMenu.SetActive(true);
            }
        }

        // change this to flipped and not !flipped after you test this
        if (players[myPlayerIndex].isIsadore && !players[myPlayerIndex].character.character.flipped && players[myPlayerIndex].character.uniqueCardUsed)
        {
            isadoreMenu.SetActive(true);
        } else
        {
            // not able to do action
            // fix this later
            //sendErrorMessage(13);
        }

        if (players[myPlayerIndex].isPluot && players[myPlayerIndex].character.character.flipped)
        {
            // prompt ui for adding Extra Inventory into holster
            ExtraInventoryMenu.SetActive(true);
        }
        else
        {
            // not able to do action
            //sendErrorMessage(13);
        }

        if (players[myPlayerIndex].isReets)
        {
            //Image image = reetsMenu.GetComponent<Image>();
            reetsMenu.SetActive(true);
            if (players[myPlayerIndex].character.character.flipped)
            {
                reetsMenuText.text = "Pay 1P to add top card of deck to Holster?";
                reetsCard.GetComponent<Image>().sprite = sprite1;
            } else
            {
                reetsMenuText.text = "Pay 2P to add top card of deck to Holster?";
                reetsCard.GetComponent<Image>().sprite = sprite2;
            }
        }

        if (players[myPlayerIndex].isNickles && !players[myPlayerIndex].nicklesAction)
        {
            if (!players[myPlayerIndex].character.character.flipped)
            {
                // unflipped Nickles UI
                nicklesUI.SetActive(true);
            }
            else
            {
                // flipped Nickles UI
                flippedNicklesUI.SetActive(true);
            }
        } else if(players[myPlayerIndex].isNickles && players[myPlayerIndex].nicklesAction)
        {
            // error message because you already did it once this turn
            sendErrorMessage(15);
        }
    }

    public void put4CardsInHolster()
    {
        foreach(CardDisplay cd in players[myPlayerIndex].holster.cardList)
        {
            cd.updateCard(md1.popCard());
        }
    }

    /* If you're wondering why there's two of these
     * it's because one takes in a GameObject and the
     * other is overloaded to take in a Dialog to handle
     * the tutorial text
     */

    public IEnumerator waitThreeSeconds(Dialog dialog)
    {
        if (dialog.textBoxCounter == 24)
        {
            yield break;
        }
        yield return new WaitForSeconds(3);
        dialog.textBoxCounter++;
        if (dialog.textBoxCounter == 3)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "Potions are the lifeblood of this game, and you will be seeing\nthem a lot!\n\nNot only are they cheap ammunition, but they fuel " +
                "your more\npowerful artifacts and vessels!";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 6)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "Artifacts are powerful items that only require one potion\nin order to use." +
                "\n\nTry using that artifact on me!\n\n" +
                "Click THROW on the artifact card and choose your foe!";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 8)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "Artifacts can be used as many times as you want per turn!\n\n" +
                "Whenever an artifact is used, the potion loaded into it is trashed!";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 12)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "Cards bought from the market appear face-up on top of\n" +
                "your deck! The order in which you buy things is important!\n\n" +
                "Keep in mind that any unspent Pips do not get saved,\n" +
                "so use 'em or lose 'em!";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 15)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "Upon the start of your next turn, empty spots in your holster\n" +
                "will be replaced by the cards on top of your deck!\n\n" +
                "You also get 6 new Pips to start your turn with.";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 20)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "Good job! When a vessel is thrown, the vessel is trashed, and\n" +
                "the potions are cycled into the bottom of your deck!";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 23)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "Excellent! Now let's buy more cards from the market.\n\n" +
                "Buy some more cards from the market and then end your turn!";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 26)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "While buying cards is all fun and games, selling is where you\n" +
                "really make the dough!\n\n" +
                "Selling items allows you to increase your Pip total beyond\n" +
                "6 Pips, allowing you to buy more powerful and\n" +
                "expensive items!";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 29)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "If you don't want to sell a card but you don't want it in your\n" +
                "holster, you can cycle it to the bottom of your deck as well!\n\n" +
                "Be aware that cycling non-potion cards costs 1 Pip!\n\n" +
                "Try cycling a card from your holster!";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 31)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "Now let's talk about you! Your character that is...\n\n" +
                "Every character has a special ability detailed on the front of\n" +
                "their character card!\n\n" +
                "Additionally, the card can be flipped to reveal an upgraded\n" +
                "version of your character!";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 34)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "Try using your character's upgraded ability!\n\n" +
                "Click on your character card and click ACTION to use it!";
            dialog.ActivateText(dialog.dialogBox);
        }
    }

    // THROW POTION REQUEST
    public void throwPotion()
    {
        
        // TUTORIAL LOGIC
        if (Game.tutorial)
        {
            if (playerHolster.cardList[selectedCardInt - 1].card.cardType == "Potion")
            {
                int damage = playerHolster.cardList[selectedCardInt - 1].card.effectAmount;
                damage = cardPlayer.checkBonus(damage, selectedCardInt);
                sendSuccessMessage(2); // Only display on thrower's client.
                playerHolster.cardList[selectedCardInt - 1].updateCard(bolo.deck.placeholder);
                td.addCard(playerHolster.cardList[selectedCardInt - 1]);
                playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                bolo.subHealth(damage);
                StartCoroutine(waitThreeSeconds(dialog));
            } else if (playerHolster.cardList[selectedCardInt - 1].card.cardType == "Artifact")
            {
                if (playerHolster.cardList[selectedCardInt - 1].aPotion.card.cardName != "placeholder")
                {
                    int damage = playerHolster.cardList[selectedCardInt - 1].card.effectAmount;
                    damage = cardPlayer.checkBonus(damage, selectedCardInt);

                    // Update response to account for trashing loaded artifact's potion and not the artifact
                    td.addCard(playerHolster.cardList[selectedCardInt - 1].aPotion);
                    playerHolster.cardList[selectedCardInt - 1].artifactSlot.transform.parent.gameObject.SetActive(false);

                    // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                    // bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, true, false);
                    bolo.subHealth(damage);
                    sendSuccessMessage(3);
                    StartCoroutine(waitThreeSeconds(dialog));
                    playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                    // MATTEO: Add Artifact using SFX here.

                }
                else
                {
                    // "Can't use an unloaded Artifact!"
                    sendErrorMessage(1);
                }
            } else if (playerHolster.cardList[selectedCardInt - 1].card.cardType == "Vessel")
            {
                if (playerHolster.cardList[selectedCardInt - 1].vPotion1.card.cardName != "placeholder" &&
                            playerHolster.cardList[selectedCardInt - 1].vPotion2.card.cardName != "placeholder")
                {
                    //int damage = players[throwerIndex].holster.card1.vPotion1.card.effectAmount + players[throwerIndex].holster.card1.vPotion2.card.effectAmount;
                    int damage = playerHolster.cardList[selectedCardInt - 1].vPotion1.card.effectAmount + playerHolster.cardList[selectedCardInt - 1].vPotion2.card.effectAmount;
                    //damage = players[throwerIndex].checkBonus(damage, selectedCardInt);
                    playerDeck.putCardOnBottom(playerHolster.cardList[selectedCardInt - 1].vPotion1.card);
                    playerDeck.putCardOnBottom(playerHolster.cardList[selectedCardInt - 1].vPotion2.card);
                    playerHolster.card1.vPotion1.updateCard(playerHolster.cardList[selectedCardInt - 1].placeholder);
                    playerHolster.card1.vPotion2.updateCard(playerHolster.cardList[selectedCardInt - 1].placeholder);
                    td.addCard(playerHolster.cardList[selectedCardInt - 1]);
                    playerHolster.cardList[selectedCardInt - 1].vesselSlot1.transform.parent.gameObject.SetActive(false);
                    playerHolster.cardList[selectedCardInt - 1].vesselSlot2.transform.parent.gameObject.SetActive(false);
                    // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                    // bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, false, true);
                    bolo.subHealth(damage);
                    sendSuccessMessage(4);
                    StartCoroutine(waitThreeSeconds(dialog));
                    playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();

                    // MATTEO: Add Vessel throw SFX here.

                }
                else
                {
                    // "Can't throw an unloaded Vessel!"
                    //Debug.Log("Vessel Error");
                    sendErrorMessage(2);
                }
            }
            return;
        }

        // if you're saltimbocca and you're flipped
        if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].character.character.flipped)
        {
            int damage = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.buyPrice;

            foreach (CardPlayer cp in players)
            {
                if (cp.character.character.cardName == selectedOpponentCharName)
                {
                    cp.subHealth(damage);
                    td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
                    // maybe add a notif idk
                    sendSuccessMessage(2);
                    return;
                }
            }
        }

        if(nicklesDamage > 0)
        {
            foreach (CardPlayer cp in players)
            {
                if(cp.character.character.cardName == selectedOpponentCharName)
                {
                    players[myPlayerIndex].subPips(nicklesDamage);
                    cp.subHealth(nicklesDamage);
                    nicklesDamage = 0;
                    // add a notification here??? up to you future denzill
                    
                    return;
                }
            }
            return;
        }


        // If this client isn't the current player, display error message.
        if (players[myPlayerIndex].user_id != myPlayerIndex) {
            // "You are not the currentPlayer!"
            sendErrorMessage(7);
        }

        // This client is the current player.
        else 
        {
            int damage = 0;
            //int targetUserId = 0;
            string targetUser = "";
            //int throwerIndex = -1;
            int throwerIndex = myPlayerIndex;
            Debug.Log("GameManager Throw Potion");

            /*
            for (int i = 0; i < Game.GamePlayers.Count; i++) 
            {
                if(players[i].charName == selectedOpponentCharName) 
                {
                    Debug.Log("Attacking Player: "+players[i].name);
                    targetUserId = players[i].user_id;
                }
                else if(players[i].user_id == currentPlayerId) {
                    throwerIndex = i;
                }
            }
            */

            foreach (CardPlayer cp in players)
            {
                if (cp.character.character.cardName == selectedOpponentCharName)
                {
                    Debug.Log("Name matched");
                    targetUser = cp.charName;
                    tempPlayer = cp;
                    // idiot, don't put this here
                    // return;
                }
            }


            if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType == "Potion")
            {
                Debug.Log("POTION");
                if (players[myPlayerIndex].isTwins)
                {
                    if (!players[myPlayerIndex].character.character.flipped)
                    {
                        players[myPlayerIndex].addHealth(1);
                    } else
                    {
                        players[myPlayerIndex].addHealth(2);
                    }
                }
                damage = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.effectAmount;
                damage = players[myPlayerIndex].checkBonus(damage, selectedCardInt);

                sendSuccessMessage(2); // Only display on thrower's client.
                players[myPlayerIndex].potionsThrown++;
                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].placeholder);
                td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
                if (players[myPlayerIndex].blackRainBonus)
                {
                    put4CardsInHolster();
                    players[myPlayerIndex].blackRainBonus = false;
                }
                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                tempPlayer.subHealth(damage);

            } else if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType == "Artifact")
            {
                if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].aPotion.card.cardName != "placeholder")
                {
                    damage = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.effectAmount;
                    //damage = players[throwerIndex].checkBonus(damage, selectedCardInt);
                    damage = players[myPlayerIndex].checkArtifactBonus(damage, players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);

                    // Update response to account for trashing loaded artifact's potion and not the artifact
                    players[myPlayerIndex].artifactsUsed++;
                    td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].aPotion);
                    players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.parent.gameObject.SetActive(false);

                    // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                    // bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, true, false);
                    tempPlayer.subHealth(damage);

                    // ISADORE LOGIC
                    if (players[myPlayerIndex].isIsadore && players[myPlayerIndex].artifactsUsed == 2)
                    {
                        players[myPlayerIndex].character.canBeFlipped = true;
                        // add success message for "You can now flip your card!" or something
                        sendSuccessMessage(13);

                    } else
                    {
                        sendSuccessMessage(3);
                    }
                    
                    players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                    // MATTEO: Add Artifact using SFX here.

                }
                else
                {
                    // "Can't use an unloaded Artifact!"
                    sendErrorMessage(1);
                }

            } else if(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType == "Vessel")
            {
                if (playerHolster.cardList[selectedCardInt - 1].vPotion1.card.cardName != "placeholder" &&
                            playerHolster.cardList[selectedCardInt - 1].vPotion2.card.cardName != "placeholder")
                {
                    if (players[myPlayerIndex].isTwins && players[myPlayerIndex].character.character.flipped)
                    {
                        players[myPlayerIndex].addHealth(4);
                    }
                    //int damage = players[throwerIndex].holster.card1.vPotion1.card.effectAmount + players[throwerIndex].holster.card1.vPotion2.card.effectAmount;
                    damage = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vPotion1.card.effectAmount + players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vPotion2.card.effectAmount;
                    damage = players[myPlayerIndex].checkVesselBonus(damage, players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);

                    // TODO: fix bonus damage
                    //damage = players[throwerIndex].checkBonus(damage, selectedCardInt);
                    players[myPlayerIndex].deck.putCardOnBottom(playerHolster.cardList[selectedCardInt - 1].vPotion1.card);
                    players[myPlayerIndex].deck.putCardOnBottom(playerHolster.cardList[selectedCardInt - 1].vPotion2.card);
                    players[myPlayerIndex].holster.card1.vPotion1.updateCard(playerHolster.cardList[selectedCardInt - 1].placeholder);
                    players[myPlayerIndex].holster.card1.vPotion2.updateCard(playerHolster.cardList[selectedCardInt - 1].placeholder);
                    td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
                    players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot1.transform.parent.gameObject.SetActive(false);
                    players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot2.transform.parent.gameObject.SetActive(false);
                    // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                    // bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, false, true);
                    tempPlayer.subHealth(damage);
                    sendSuccessMessage(4);
                    players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();

                    // MATTEO: Add Vessel throw SFX here.

                }
                else
                {
                    // "Can't throw an unloaded Vessel!"
                    //Debug.Log("Vessel Error");
                    sendErrorMessage(2);
                }
            }

        }
    }

    public void addStarterPotion()
    {
        players[myPlayerIndex].deck.putCardOnTop(starterPotionCard);
    }

    // THROW POTION RESPONSE
    public void onResponsePotionThrow(ExtendedEventArgs eventArgs)
    {
        ResponsePotionThrowEventArgs args = eventArgs as ResponsePotionThrowEventArgs;
        // Args Potion: throwerId, cardPosition, targetId, damage, isArtifact (T/F), isVessel (T/F)
        Debug.Log("My User_ID: " + Constants.USER_ID);
        Debug.Log("Thrower ID: " + args.throwerId);
        Debug.Log("Card Position: " + args.cardPosition);
        Debug.Log("Target Opponent ID: " + args.targetId);
        Debug.Log("Damage: " + args.damage);
        Debug.Log("isArtifact?: " + args.isArtifact);
        Debug.Log("isVessel?: " + args.isVessel);

        
        // Loops through players array to update target's health and thrower's holster.
        for(int i = 0; i < numPlayers; i++) {
            // Find Player in players array that got damaged.
            if(players[i].user_id == args.targetId)
            {
                players[i].subHealth(args.damage);
            }
            // Update Holster of player who threw the Potion.
            if(players[i].user_id == args.throwerId) 
            {
                // Threw a Potion (trash thrower's thrown Potion and increase potionsThrown)
                if(args.isVessel == false && args.isArtifact == false)
                {
                    td.addCard(players[i].holster.cardList[args.cardPosition - 1]);
                    players[i].potionsThrown++;
                }
                // Threw a Vessel (put loaded potions in thrower's deck, trash the Vessel card)
                else if(args.isVessel == true && args.isArtifact == false) 
                {
                    // Determines if updating(holster.card1, holster.card2, etc.)
                    switch(args.cardPosition) {
                        case 1:
                            players[i].deck.putCardOnBottom(players[i].holster.card1.vPotion1.card);
                            players[i].deck.putCardOnBottom(players[i].holster.card1.vPotion2.card);
                            players[i].holster.card1.vPotion1.updateCard(players[0].deck.placeholder);
                            players[i].holster.card1.vPotion2.updateCard(players[0].deck.placeholder);
                            break;
                        case 2:
                            players[i].deck.putCardOnBottom(players[i].holster.card2.vPotion1.card);
                            players[i].deck.putCardOnBottom(players[i].holster.card2.vPotion2.card);
                            players[i].holster.card2.vPotion1.updateCard(players[0].deck.placeholder);
                            players[i].holster.card2.vPotion2.updateCard(players[0].deck.placeholder);
                            break;
                        case 3:
                            players[i].deck.putCardOnBottom(players[i].holster.card3.vPotion1.card);
                            players[i].deck.putCardOnBottom(players[i].holster.card3.vPotion2.card);
                            players[i].holster.card3.vPotion1.updateCard(players[0].deck.placeholder);
                            players[i].holster.card3.vPotion2.updateCard(players[0].deck.placeholder);
                            break;
                        case 4:
                            players[i].deck.putCardOnBottom(players[i].holster.card4.vPotion1.card);
                            players[i].deck.putCardOnBottom(players[i].holster.card4.vPotion2.card);
                            players[i].holster.card4.vPotion1.updateCard(players[0].deck.placeholder);
                            players[i].holster.card4.vPotion2.updateCard(players[0].deck.placeholder);
                            break;
                        default: Debug.Log("ERROR (ThrowPotionResponse): Can only update from card1 to card4"); break;
                    }
                    td.addCard(players[i].holster.cardList[args.cardPosition - 1]);
                    players[i].holster.cardList[args.cardPosition - 1].vesselSlot1.transform.parent.gameObject.SetActive(false);
                    players[i].holster.cardList[args.cardPosition - 1].vesselSlot2.transform.parent.gameObject.SetActive(false);
                }
                // Used an Artifact (trashes potion, keeps the Artifact in the holster)
                else if(args.isVessel == false && args.isArtifact == true)
                {
                    td.addCard(players[i].holster.cardList[args.cardPosition - 1].aPotion);
                    players[i].holster.cardList[args.cardPosition - 1].artifactSlot.transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }

    public void setStarterPotion()
    {
        starterPotion = true;
    }

    // LOAD REQUEST (DONE - 2 clients)
    public void loadPotion()
    {
        Debug.Log("Load Potion");

        // TUTORIAL LOGIC
        if (Game.tutorial)
        {
            if (playerHolster.cardList[selectedCardInt - 1].card.cardType == "Potion")
            {
                // Loading a Vessel:
                if (playerHolster.cardList[loadedCardInt].card.cardType == "Vessel")
                {
                    // Enable Vessel menu if it wasn't already enabled.
                    Debug.Log("Vessel menu enabled.");
                    playerHolster.cardList[loadedCardInt].vesselSlot1.transform.parent.gameObject.SetActive(true);

                    // Check for existing loaded potion(s) if Vessel menu was already enabled.
                    if (playerHolster.cardList[loadedCardInt].vPotion1.card.cardName != "placeholder")
                    {
                        // If Vessel slot 2 is filled.
                        if (playerHolster.cardList[loadedCardInt].vPotion2.card.cardName != "placeholder")
                        {
                            Debug.Log("Vessel is fully loaded!");
                            // DONE: Insert error that displays on screen.
                            sendErrorMessage(9);
                        }
                        else
                        {
                            // Fill Vessel slot 2 with loaded potion.
                            Card placeholder = playerHolster.cardList[loadedCardInt].vPotion2.card;
                            playerHolster.cardList[loadedCardInt].vPotion2.card = playerHolster.cardList[selectedCardInt - 1].card;
                            playerHolster.cardList[loadedCardInt].vPotion2.updateCard(playerHolster.cardList[selectedCardInt - 1].card);

                            // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                            sendSuccessMessage(5);
                            StartCoroutine(waitThreeSeconds(dialog));
                            playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                            Debug.Log("Potion loaded in Vessel slot 2!");

                            // MATTEO: Add Loading potion SFX here.

                            // // Updates Holster card to be empty.
                            playerHolster.cardList[selectedCardInt - 1].card = placeholder;
                            playerHolster.cardList[selectedCardInt - 1].updateCard(placeholder);
                        }
                    }
                    // Vessel slot 1 is unloaded.
                    else
                    {
                        Card placeholder = playerHolster.cardList[loadedCardInt].vPotion1.card;
                        playerHolster.cardList[loadedCardInt].vPotion1.card = playerHolster.cardList[selectedCardInt - 1].card;
                        playerHolster.cardList[loadedCardInt].vPotion1.updateCard(playerHolster.cardList[selectedCardInt - 1].card);

                        // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                        sendSuccessMessage(5);
                        StartCoroutine(waitThreeSeconds(dialog));
                        playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        Debug.Log("Potion loaded in Vessel slot 1!");

                        // MATTEO: Add Loading potion SFX here.

                        // // Updates Holster card to be empty.
                        playerHolster.cardList[selectedCardInt - 1].card = placeholder;
                        playerHolster.cardList[selectedCardInt - 1].updateCard(placeholder);
                    }
                }

                // Loading an Artifact:
                else if (playerHolster.cardList[loadedCardInt].card.cardType == "Artifact")
                {
                    // Enable Artifact menu if it wasn't already enabled.
                    Debug.Log("Artifact menu enabled.");
                    playerHolster.cardList[loadedCardInt].artifactSlot.transform.parent.gameObject.SetActive(true);

                    // Check for existing loaded potion if Artifact menu was already enabled.
                    if (playerHolster.cardList[loadedCardInt].aPotion.card.cardName != "placeholder")
                    {
                        Debug.Log("Artifact is fully loaded!");
                        // DONE: Insert error that displays on screen.
                        sendErrorMessage(8);
                    }
                    // Artifact slot is unloaded.
                    else
                    {
                        Card placeholder = playerHolster.cardList[loadedCardInt].aPotion.card;
                        playerHolster.cardList[loadedCardInt].aPotion.card = playerHolster.cardList[selectedCardInt - 1].card;
                        playerHolster.cardList[loadedCardInt].aPotion.updateCard(playerHolster.cardList[selectedCardInt - 1].card);
                        // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                        sendSuccessMessage(5);
                        StartCoroutine(waitThreeSeconds(dialog));
                        playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        Debug.Log("Potion loaded in Artifact slot!");

                        // MATTEO: Add Loading potion SFX here.

                        // // Updates Holster card to be empty.
                        playerHolster.cardList[selectedCardInt - 1].card = placeholder;
                        playerHolster.cardList[selectedCardInt - 1].updateCard(placeholder);
                    }
                }
            }
            return;
        }



            // DONE?: Send potion with loadedCardInt to loaded CardDisplay of card in selectedCardInt
            // test for protocol, must replace parameters later
            // bool connected = networkManager.sendLoadRequest(0, 0);

            // If this client isn't the current player, display error message.
            if (players[myPlayerIndex].user_id != myPlayerIndex) {
            // "You are not the currentPlayer!"
            sendErrorMessage(7);
        }

        // It is this player's turn.
        else
        {
            // Isadore logic
            if (starterPotion && !usedStarterPotion)
            {
                // Loading a Vessel:
                if (players[myPlayerIndex].holster.cardList[loadedCardInt].card.cardType == "Vessel")
                {
                    // TODO: Make another error message
                    Debug.Log("This error message???");
                    sendErrorMessage(12);
                }
                else if (players[myPlayerIndex].holster.cardList[loadedCardInt].card.cardType == "Artifact")
                {
                    // Enable Artifact menu if it wasn't already enabled.
                    Debug.Log("Artifact menu enabled.");
                    players[myPlayerIndex].holster.cardList[loadedCardInt].artifactSlot.transform.parent.gameObject.SetActive(true);

                    // Check for existing loaded potion if Artifact menu was already enabled.
                    if (players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.card.cardName != "placeholder")
                    {
                        Debug.Log("Artifact is fully loaded!");
                        // DONE: Insert error that displays on screen.
                        sendErrorMessage(8);
                    }
                    // Artifact slot is unloaded.
                    else
                    {
                        Card placeholder = players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.card;
                        players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.card = starterPotionDisplay.card;
                        players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.updateCard(starterPotionDisplay.card);
                        usedStarterPotion = true;
                        // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                        sendSuccessMessage(5);
                        //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        Debug.Log("Starter potion loaded in Artifact slot!");

                        // MATTEO: Add Loading potion SFX here.

                        // // Updates Holster card to be empty.
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card = placeholder;
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(placeholder);
                    }
                }
                starterPotion = false;
                return;
            } else
            {
                // if it's an artifact or vessel
                if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType == "Potion")
                {
                    // Loading a Vessel:
                    if (players[myPlayerIndex].holster.cardList[loadedCardInt].card.cardType == "Vessel")
                    {
                        // Enable Vessel menu if it wasn't already enabled.
                        Debug.Log("Vessel menu enabled.");
                        players[myPlayerIndex].holster.cardList[loadedCardInt].vesselSlot1.transform.parent.gameObject.SetActive(true);

                        // Check for existing loaded potion(s) if Vessel menu was already enabled.
                        if (players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion1.card.cardName != "placeholder")
                        {
                            // If Vessel slot 2 is filled.
                            if (players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion2.card.cardName != "placeholder")
                            {
                                Debug.Log("Vessel is fully loaded!");
                                // DONE: Insert error that displays on screen.
                                sendErrorMessage(9);
                            }
                            else
                            {
                                // Fill Vessel slot 2 with loaded potion.
                                Card placeholder = players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion2.card;
                                players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion2.card = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card;
                                players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion2.updateCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card);

                                // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                                sendSuccessMessage(5);
                                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                                Debug.Log("Potion loaded in Vessel slot 2!");

                                // MATTEO: Add Loading potion SFX here.

                                // // Updates Holster card to be empty.
                                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card = placeholder;
                                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(placeholder);
                            }
                        }
                        // Vessel slot 1 is unloaded.
                        else
                        {
                            Card placeholder = players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion1.card;
                            players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion1.card = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card;
                            players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion1.updateCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card);

                            // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                            sendSuccessMessage(5);
                            players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                            Debug.Log("Potion loaded in Vessel slot 1!");

                            // MATTEO: Add Loading potion SFX here.

                            // // Updates Holster card to be empty.
                            players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card = placeholder;
                            players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(placeholder);
                        }
                    }

                    // Loading an Artifact:
                    else if (players[myPlayerIndex].holster.cardList[loadedCardInt].card.cardType == "Artifact")
                    {
                        // Enable Artifact menu if it wasn't already enabled.
                        Debug.Log("Artifact menu enabled.");
                        players[myPlayerIndex].holster.cardList[loadedCardInt].artifactSlot.transform.parent.gameObject.SetActive(true);

                        // Check for existing loaded potion if Artifact menu was already enabled.
                        if (players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.card.cardName != "placeholder")
                        {
                            Debug.Log("Artifact is fully loaded!");
                            // DONE: Insert error that displays on screen.
                            sendErrorMessage(8);
                        }
                        // Artifact slot is unloaded.
                        else
                        {
                            Card placeholder = players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.card;
                            players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.card = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card;
                            players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.updateCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card);
                            // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                            sendSuccessMessage(5);
                            players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                            Debug.Log("Potion loaded in Artifact slot!");

                            // MATTEO: Add Loading potion SFX here.

                            // // Updates Holster card to be empty.
                            players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card = placeholder;
                            players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(placeholder);
                        }
                    }
                } else
                {
                    // add error message
                    Debug.Log("That error message...");
                    sendErrorMessage(12);
                }
            }
        }
    }

    // LOAD RESPONSE (DONE - 2 clients)
    public void onResponseLoad(ExtendedEventArgs eventArgs)
    {
        Debug.Log("Load Response");
        ResponseLoadEventArgs args = eventArgs as ResponseLoadEventArgs;
        // args = (user_id, x = selectedCardInt, y = loadedCardInt)

        // Find relative index in players[] of the player who made loader request.
        int loaderIndex = -1;
        for(int i = 0; i < numPlayers; i++) {
            if(players[i].user_id == args.user_id) {
                loaderIndex = i;
            }
        }

        if(players[loaderIndex].holster.cardList[args.y].card.cardType == "Artifact")
        {
            Card placeholder = players[loaderIndex].holster.cardList[args.y].aPotion.card;
            players[loaderIndex].holster.cardList[args.y].artifactSlot.transform.parent.gameObject.SetActive(true);
            players[loaderIndex].holster.cardList[args.y].aPotion.card = players[loaderIndex].holster.cardList[args.x - 1].card;
            players[loaderIndex].holster.cardList[args.y].aPotion.updateCard(players[loaderIndex].holster.cardList[args.x - 1].card);
            // Updates Holster card to be empty.
            players[loaderIndex].holster.cardList[args.x - 1].card = placeholder;
            players[loaderIndex].holster.cardList[args.x - 1].updateCard(placeholder);
        } 
        else if(players[loaderIndex].holster.cardList[args.y].card.cardType == "Vessel")
        {
            players[loaderIndex].holster.cardList[args.y].vesselSlot1.transform.parent.gameObject.SetActive(true);
            players[loaderIndex].holster.cardList[args.y].vesselSlot2.transform.parent.gameObject.SetActive(true);
            if(players[loaderIndex].holster.cardList[args.y].vPotion1.card.cardName == "placeholder")
            {
                Card placeholder = players[loaderIndex].holster.cardList[args.y].vPotion1.card;
                players[loaderIndex].holster.cardList[args.y].vPotion1.card = players[loaderIndex].holster.cardList[args.x - 1].card;
                players[loaderIndex].holster.cardList[args.y].vPotion1.updateCard(players[loaderIndex].holster.cardList[args.x - 1].card);
                // Updates Holster card to be empty.
                players[loaderIndex].holster.cardList[args.x - 1].card = placeholder;
                players[loaderIndex].holster.cardList[args.x - 1].updateCard(placeholder);
            }
            else if(players[loaderIndex].holster.cardList[args.y].vPotion2.card.cardName == "placeholder")
            {
                Card placeholder = players[loaderIndex].holster.cardList[args.y].vPotion2.card;
                players[loaderIndex].holster.cardList[args.y].vPotion2.card = players[loaderIndex].holster.cardList[args.x - 1].card;
                players[loaderIndex].holster.cardList[args.y].vPotion2.updateCard(players[loaderIndex].holster.cardList[args.x - 1].card);
                // Updates Holster card to be empty.
                players[loaderIndex].holster.cardList[args.x - 1].card = placeholder;
                players[loaderIndex].holster.cardList[args.x - 1].updateCard(placeholder);
            }
        }

        // if(Constants.USER_ID != args.user_id)
        // {
        //     if(players[myPlayerIndex].holster.cardList[args.y].card.cardType == "Artifact")
        //     {
        //         players[myPlayerIndex].holster.cardList[args.y].artifactSlot.transform.parent.gameObject.SetActive(true);
        //         players[myPlayerIndex].holster.cardList[args.y].aPotion.card = players[myPlayerIndex].holster.cardList[args.x - 1].card;
        //         players[myPlayerIndex].holster.cardList[args.y].aPotion.updateCard(players[myPlayerIndex].holster.cardList[args.x - 1].card);
        //     } else if(players[myPlayerIndex].holster.cardList[args.y].card.cardType == "Vessel")
        //     {
        //         players[myPlayerIndex].holster.cardList[args.y].vesselSlot1.transform.parent.gameObject.SetActive(true);
        //         players[myPlayerIndex].holster.cardList[args.y].vesselSlot2.transform.parent.gameObject.SetActive(true);
        //         if(players[myPlayerIndex].holster.cardList[args.y].vPotion1.card.cardName != "placeholder")
        //         {
        //             players[myPlayerIndex].holster.cardList[args.y].vPotion1.card = players[myPlayerIndex].holster.cardList[args.x - 1].card;
        //             players[myPlayerIndex].holster.cardList[args.y].vPotion1.updateCard(players[myPlayerIndex].holster.cardList[args.x - 1].card);
        //         }
        //         else
        //         {
        //             players[myPlayerIndex].holster.cardList[args.y].vPotion2.card = players[myPlayerIndex].holster.cardList[args.x - 1].card;
        //             players[myPlayerIndex].holster.cardList[args.y].vPotion2.updateCard(players[myPlayerIndex].holster.cardList[args.x - 1].card);
        //         }
        //     }
        // }
    }

    // CYCLE REQUEST (DONE - 2 clients)
    public void cycleCard()
    {
        Debug.Log("Cycle Card");

        // TUTORIAL LOGIC
        if (Game.tutorial)
        {
            if (playerHolster.cardList[selectedCardInt - 1].card.cardType == "Potion")
            {
                cardPlayer.deck.putCardOnBottom(playerHolster.cardList[selectedCardInt - 1].card);
                playerHolster.cardList[selectedCardInt - 1].updateCard(playerHolster.card1.placeholder);
                sendSuccessMessage(7);
                playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                StartCoroutine(waitThreeSeconds(dialog));
            } else if(cardPlayer.pips < 1)
            {
                sendErrorMessage(5);
            }
            else if (playerHolster.cardList[selectedCardInt - 1].card.cardType == "Artifact" ||
                    playerHolster.cardList[selectedCardInt - 1].card.cardType == "Vessel" ||
                    playerHolster.cardList[selectedCardInt - 1].card.cardType == "Ring")
            {
                cardPlayer.deck.putCardOnBottom(playerHolster.cardList[selectedCardInt - 1].card);
                sendSuccessMessage(7);
                playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
            }
            return;
        }

        // If this client isn't the current player, display error message.
        if(players[myPlayerIndex].user_id != myPlayerIndex) {
            // "You are not the currentPlayer!"
            sendErrorMessage(7);
        }

        // It is this player's turn.
        else
        {
            // Cycling a Potion (costs 0 pips to do)
            if(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType == "Potion")
            {
                // players[myPlayerIndex].deck.putCardOnBottom(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card);
                // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(players[0].holster.card1.placeholder);
                // bool connected = networkManager.sendCycleRequest(selectedCardInt, 0);
                sendSuccessMessage(7);
                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                // MATTEO: Add Cycle SFX here.
                
            }
            // If Player has no pips to cycle an Artifact, Vessel, or Ring.
            else if (players[myPlayerIndex].pips < 1)
            {
                // "You don't have enough Pips to cycle this!"
                sendErrorMessage(5);
            }
            // Player has enough pips to cycle an Artifact, Vessel, or Ring.
            else if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType == "Artifact" ||
                    players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType == "Vessel" ||
                    players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType == "Ring")
            {
                // bool connected = networkManager.sendCycleRequest(selectedCardInt, 1);
                players[myPlayerIndex].deck.putCardOnBottom(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card);
                sendSuccessMessage(7);
                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                // MATTEO: Add Cycle SFX here.
            }
            else
            {
                Debug.Log("ERROR: Card needs to be of type Potion, Artifact, Vessel, Ring");
            }
        }
    }

    // CYCLE RESPONSE (DONE - 2 clients)
    public void onResponseCycle(ExtendedEventArgs eventArgs)
    {
        // args = user_id, x = selectedCardInt or cardPosition, y = pips cost (0 or 1)
        Debug.Log("Cycle Response");
        ResponseCycleEventArgs args = eventArgs as ResponseCycleEventArgs;

        int cardPosition = args.x - 1;

        int cyclerIndex = -1;
        for(int i = 0; i < numPlayers; i++)
        {
            if(args.user_id == players[i].user_id)
            {
                cyclerIndex = i;
            }
        }

        Card placeholder = players[0].holster.card1.placeholder;

        // Cycling a Potion (costs 0 pips to do)
        if(players[cyclerIndex].holster.cardList[cardPosition].card.cardType == "Potion")
        {
            players[cyclerIndex].deck.putCardOnBottom(players[cyclerIndex].holster.cardList[cardPosition].card);
            players[cyclerIndex].holster.cardList[cardPosition].card = placeholder;
            players[cyclerIndex].holster.cardList[cardPosition].updateCard(placeholder);
            
        }

        // Cycling an Artifact (costs 1 pip, also need to drop any loaded potion)
        else if(players[cyclerIndex].holster.cardList[cardPosition].card.cardType == "Artifact")
        {
            // Subtracts a pip from the cycler.
            players[cyclerIndex].subPips(1);

            // Check if Artifact is loaded
            // If loaded, Send card in artifact slot to bottom of deck, and update CardDisplay to placeholder
            if(players[cyclerIndex].holster.cardList[cardPosition].aPotion.card.cardName != "placeholder")
            {

            }

            // Cycle the artifact card itself (CHECK)
            // Update the artifact card's position to have a placeholder card (CHECK)
            players[cyclerIndex].deck.putCardOnBottom(players[cyclerIndex].holster.cardList[cardPosition].card);
            players[cyclerIndex].holster.cardList[cardPosition].card = placeholder;
            players[cyclerIndex].holster.cardList[cardPosition].updateCard(placeholder);

            // If loaded, disable the artifact slot object - SetActive to false (CHECK)
            players[cyclerIndex].holster.cardList[cardPosition].artifactSlot.transform.parent.gameObject.SetActive(false);
        }

        // Cycling an Vessel (costs 1 pip, also need to drop any loaded potions)
        else if(players[cyclerIndex].holster.cardList[cardPosition].card.cardType == "Vessel")
        {
            // Subtracts a pip from the cycler.
            players[cyclerIndex].subPips(1);

            // If loaded with 1 or 2 potions, Send card(s) in vessel slot(s) to bottom of deck, and update CardDisplay(s) to placeholder (CHECK)
            // If Vessel slot 1 is unloaded.
            if(players[cyclerIndex].holster.cardList[cardPosition].vPotion1.card.cardName != "placeholder")
            {
                // If Vessel's Slot 2 is loaded.
                if(players[cyclerIndex].holster.cardList[cardPosition].vPotion2.card.cardName != "placeholder")
                {
                    // Both Vessel slots are loaded.
                    players[cyclerIndex].deck.putCardOnBottom(players[cyclerIndex].holster.cardList[cardPosition].vPotion1.card);
                    players[cyclerIndex].deck.putCardOnBottom(players[cyclerIndex].holster.cardList[cardPosition].vPotion2.card);
                    players[cyclerIndex].holster.cardList[cardPosition].vPotion1.card = placeholder;
                    players[cyclerIndex].holster.cardList[cardPosition].vPotion2.card = placeholder;
                    players[cyclerIndex].holster.cardList[cardPosition].vPotion1.updateCard(placeholder);
                    players[cyclerIndex].holster.cardList[cardPosition].vPotion2.updateCard(placeholder);
                }
                else 
                {
                    // Only Vessel slot 1 is loaded.
                    players[cyclerIndex].deck.putCardOnBottom(players[cyclerIndex].holster.cardList[cardPosition].vPotion1.card);
                    players[cyclerIndex].holster.cardList[cardPosition].vPotion1.card = placeholder;
                    players[cyclerIndex].holster.cardList[cardPosition].vPotion1.updateCard(placeholder);
                }
            }
            // If Vessel slot 1 is unloaded.
            else
            {
                // If Vessel's Slot 2 is loaded.
                if(players[cyclerIndex].holster.cardList[cardPosition].vPotion2.card.cardName != "placeholder")
                {
                    // Only Vessel slot 2 is loaded.
                    players[cyclerIndex].deck.putCardOnBottom(players[cyclerIndex].holster.cardList[cardPosition].vPotion2.card);
                    players[cyclerIndex].holster.cardList[cardPosition].vPotion2.card = placeholder;
                    players[cyclerIndex].holster.cardList[cardPosition].vPotion2.updateCard(placeholder);
                }
                else 
                {
                    // No Vessel slots are loaded.
                }
            }
            
            // Cycle the Vessel card itself (CHECK)
            // Update the Vessel card's position to have a placeholder card (CHECK)
            players[cyclerIndex].deck.putCardOnBottom(players[cyclerIndex].holster.cardList[cardPosition].card);
            players[cyclerIndex].holster.cardList[cardPosition].card = placeholder;
            players[cyclerIndex].holster.cardList[cardPosition].updateCard(placeholder);

            // If loaded, disable the vessel slot objects - SetActive to false (CHECK)
            players[cyclerIndex].holster.cardList[cardPosition].vesselSlot1.transform.parent.gameObject.SetActive(false);
            players[cyclerIndex].holster.cardList[cardPosition].vesselSlot2.transform.parent.gameObject.SetActive(false);
        }

        // Cycling a Ring (costs 1 pip to do)
        else if(players[cyclerIndex].holster.cardList[selectedCardInt - 1].card.cardType == "Ring")
        {
            // Subtracts a pip from the cycler.
            players[cyclerIndex].subPips(1);
            players[cyclerIndex].deck.putCardOnBottom(players[cyclerIndex].holster.cardList[cardPosition].card);
            players[cyclerIndex].holster.cardList[cardPosition].card = placeholder;
            players[cyclerIndex].holster.cardList[cardPosition].updateCard(placeholder);
        }

        // if (Constants.USER_ID != args.user_id)
        // {
        //     players[myPlayerIndex].pips--;
        //     players[myPlayerIndex].deck.putCardOnBottom(players[myPlayerIndex].holster.cardList[args.x - 1].card);
        //     players[myPlayerIndex].holster.cardList[args.x - 1].updateCard(players[0].holster.card1.placeholder);
        // }
    }

// TOP MARKET REQUEST
// subtract pips, update deck display and market display
    public void topMarketBuy()
    {
        Debug.Log("Top Market Buy");

        // TUTORIAL LOGIC
        if (Game.tutorial)
        {
            switch (md1.cardInt)
            {
                case 1:
                    if (cardPlayer.pips >= md1.cardDisplay1.card.buyPrice)
                    {
                        // players[myPlayerIndex].pips -= md1.cardDisplay1.card.buyPrice;
                        cardPlayer.subPips(md1.cardDisplay1.card.buyPrice);
                        playerDeck.putCardOnTop(md1.cardDisplay1.card);
                        Card card = md1.popCard();
                        md1.cardDisplay1.updateCard(card);
                        StartCoroutine(waitThreeSeconds(dialog));
                        // playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        md1.cardDisplay1.gameObject.GetComponent<Market_Hover>().resetCard();
                        sendSuccessMessage(1);
                        // bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay1.card.buyPrice, 1);
                    }
                    else
                    {
                        sendErrorMessage(6);
                    }
                    break;
                case 2:
                    if (cardPlayer.pips >= md1.cardDisplay2.card.buyPrice)
                    {
                        cardPlayer.subPips(md1.cardDisplay2.card.buyPrice);
                        playerDeck.putCardOnTop(md1.cardDisplay2.card);
                        Card card = md1.popCard();
                        md1.cardDisplay2.updateCard(card);
                        StartCoroutine(waitThreeSeconds(dialog));
                        // playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        md1.cardDisplay2.gameObject.GetComponent<Market_Hover>().resetCard();
                        sendSuccessMessage(1);
                        // bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay2.card.buyPrice, 1);
                    }
                    else
                    {
                        sendErrorMessage(6);
                    }
                    break;
                case 3:
                    if (cardPlayer.pips >= md1.cardDisplay3.card.buyPrice)
                    {
                        // players[myPlayerIndex].pips -= md1.cardDisplay3.card.buyPrice;
                        // players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay3.card);
                        // Card card = md1.popCard();
                        // md1.cardDisplay3.updateCard(card);
                        cardPlayer.subPips(md1.cardDisplay3.card.buyPrice);
                        playerDeck.putCardOnTop(md1.cardDisplay3.card);
                        Card card = md1.popCard();
                        md1.cardDisplay3.updateCard(card);
                        StartCoroutine(waitThreeSeconds(dialog));
                        // playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        md1.cardDisplay3.gameObject.GetComponent<Market_Hover>().resetCard();
                        sendSuccessMessage(1);
                        // bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay3.card.buyPrice, 1);
                    }
                    else
                    {
                        sendErrorMessage(6);
                    }
                    break;
                default:
                    Debug.Log("MarketDeck Error!");
                    break;
            }
            return;
        }

        // If this client isn't the current player, display error message.
        if (players[myPlayerIndex].user_id != myPlayerIndex) {
            // "You are not the currentPlayer!"
            sendErrorMessage(7);
        }

        // It is this player's turn.
        else
        {
            // cardInt based on position of card in Top Market (position 1, 2, or 3)
            switch (md1.cardInt)
            {
                case 1:
                    if(players[myPlayerIndex].pips >= md1.cardDisplay1.card.buyPrice && !players[myPlayerIndex].isSaltimbocca)
                    {
                        players[myPlayerIndex].subPips(md1.cardDisplay1.card.buyPrice);
                        players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay1.card);
                        Card card = md1.popCard();
                        md1.cardDisplay1.updateCard(card);
                        //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        md1.cardDisplay1.gameObject.GetComponent<Market_Hover>().resetCard();
                        sendSuccessMessage(1);
                        // bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay1.card.buyPrice, 1);
                        // SALTIMBOCCA LOGIC
                    } else if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].pips >= (md1.cardDisplay1.card.buyPrice - 1))
                    {
                        if (md1.cardDisplay1.card.buyPrice - 1 == 0)
                        {
                            players[myPlayerIndex].subPips(md1.cardDisplay1.card.buyPrice);
                        }
                        else
                        {
                            players[myPlayerIndex].subPips(md1.cardDisplay1.card.buyPrice - 1);
                        }
                        players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay1.card);
                        Card card = md1.popCard();
                        md1.cardDisplay1.updateCard(card);
                        //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        md1.cardDisplay1.gameObject.GetComponent<Market_Hover>().resetCard();
                        sendSuccessMessage(1);
                    }
                    else
                    {
                        sendErrorMessage(6);
                    }
                    break;
                case 2:
                    if (players[myPlayerIndex].pips >= md1.cardDisplay2.card.buyPrice && !players[myPlayerIndex].isSaltimbocca)
                    {
                        players[myPlayerIndex].subPips(md1.cardDisplay2.card.buyPrice);
                        players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay2.card);
                        Card card = md1.popCard();
                        md1.cardDisplay2.updateCard(card);
                        //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        md1.cardDisplay2.gameObject.GetComponent<Market_Hover>().resetCard();
                        sendSuccessMessage(1);
                        // bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay2.card.buyPrice, 1);
                    }
                    else if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].pips >= (md1.cardDisplay2.card.buyPrice - 1))
                    {
                        if (md1.cardDisplay2.card.buyPrice - 1 == 0)
                        {
                            players[myPlayerIndex].subPips(md1.cardDisplay2.card.buyPrice);
                        }
                        else
                        {
                            players[myPlayerIndex].subPips(md1.cardDisplay2.card.buyPrice - 1);
                        }
                        players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay2.card);
                        Card card = md1.popCard();
                        md1.cardDisplay2.updateCard(card);
                        //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        md1.cardDisplay2.gameObject.GetComponent<Market_Hover>().resetCard();
                        sendSuccessMessage(1);
                    }
                    else
                    {
                        sendErrorMessage(6);
                    }
                    break;
                case 3:
                    if (players[myPlayerIndex].pips >= md1.cardDisplay3.card.buyPrice && !players[myPlayerIndex].isSaltimbocca)
                    {
                        players[myPlayerIndex].subPips(md1.cardDisplay3.card.buyPrice);
                        players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay3.card);
                        Card card = md1.popCard();
                        md1.cardDisplay3.updateCard(card);
                        //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        md1.cardDisplay3.gameObject.GetComponent<Market_Hover>().resetCard();
                        sendSuccessMessage(1);
                        // bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay3.card.buyPrice, 1);
                    }
                    else if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].pips >= (md1.cardDisplay3.card.buyPrice - 1))
                    {
                        if (md1.cardDisplay3.card.buyPrice - 1 == 0)
                        {
                            players[myPlayerIndex].subPips(md1.cardDisplay3.card.buyPrice);
                        }
                        else
                        {
                            players[myPlayerIndex].subPips(md1.cardDisplay3.card.buyPrice - 1);
                        }
                        players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay3.card);
                        Card card = md1.popCard();
                        md1.cardDisplay3.updateCard(card);
                        //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        md1.cardDisplay3.gameObject.GetComponent<Market_Hover>().resetCard();
                        sendSuccessMessage(1);
                    }
                    else
                    {
                        sendErrorMessage(6);
                    }
                    break;
                default:
                    Debug.Log("MarketDeck Error!");
                    break;
            }
        }
    }

// BOTTOM MARKET REQUEST
    public void bottomMarketBuy()
    {
        Debug.Log("Bottom Market Buy");

        // If this client isn't the current player, display error message.
        if(players[myPlayerIndex].user_id != myPlayerIndex) {
            // "You are not the currentPlayer!"
            sendErrorMessage(7);
        }

        // It is this player's turn.
        else
        {
            switch (md2.cardInt)
            {
                // cardInt based on position of card in Top Market (position 1, 2, or 3)
                case 1:
                    if (players[myPlayerIndex].pips >= md2.cardDisplay1.card.buyPrice && !players[myPlayerIndex].isSaltimbocca)
                    {
                        players[myPlayerIndex].subPips(md2.cardDisplay1.card.buyPrice);
                        players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay1.card);
                        Card card = md2.popCard();
                        md2.cardDisplay1.updateCard(card);
                        md2.cardDisplay1.gameObject.GetComponent<Market_Hover>().resetCard();
                        sendSuccessMessage(1);
                        // bool connected = networkManager.sendBuyRequest(md2.cardInt, md2.cardDisplay1.card.buyPrice, 0);
                    }
                    else if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].pips >= (md2.cardDisplay1.card.buyPrice - 1))
                    {
                        if(md2.cardDisplay1.card.buyPrice - 1 == 0)
                        {
                            players[myPlayerIndex].subPips(md2.cardDisplay1.card.buyPrice);
                        } else
                        {
                            players[myPlayerIndex].subPips(md2.cardDisplay1.card.buyPrice - 1);
                        }  
                        players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay1.card);
                        Card card = md2.popCard();
                        md2.cardDisplay1.updateCard(card);
                        //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        md2.cardDisplay1.gameObject.GetComponent<Market_Hover>().resetCard();
                        sendSuccessMessage(1);
                    }
                    else
                    {
                        sendErrorMessage(6);
                    }
                    break;
                case 2:
                    if (players[myPlayerIndex].pips >= md2.cardDisplay2.card.buyPrice && !players[myPlayerIndex].isSaltimbocca)
                    {
                        players[myPlayerIndex].subPips(md2.cardDisplay2.card.buyPrice);
                        players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay2.card);
                        Card card = md2.popCard();
                        md2.cardDisplay2.updateCard(card);
                        md2.cardDisplay2.gameObject.GetComponent<Market_Hover>().resetCard();
                        sendSuccessMessage(1);
                        // bool connected = networkManager.sendBuyRequest(md2.cardInt, md2.cardDisplay2.card.buyPrice, 0);
                    }
                    else if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].pips >= (md2.cardDisplay2.card.buyPrice - 1))
                    {
                        if (md2.cardDisplay2.card.buyPrice - 1 == 0)
                        {
                            players[myPlayerIndex].subPips(md2.cardDisplay2.card.buyPrice);
                        }
                        else
                        {
                            players[myPlayerIndex].subPips(md2.cardDisplay2.card.buyPrice - 1);
                        }
                        players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay2.card);
                        Card card = md2.popCard();
                        md2.cardDisplay2.updateCard(card);
                        //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        md2.cardDisplay2.gameObject.GetComponent<Market_Hover>().resetCard();
                        sendSuccessMessage(1);
                    }
                    else
                    {
                        sendErrorMessage(6);
                    }
                    break;
                case 3:
                    if (players[myPlayerIndex].pips >= md2.cardDisplay3.card.buyPrice && !players[myPlayerIndex].isSaltimbocca)
                    {
                        players[myPlayerIndex].subPips(md2.cardDisplay3.card.buyPrice);
                        players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay3.card);
                        Card card = md2.popCard();
                        md2.cardDisplay3.updateCard(card);
                        md2.cardDisplay3.gameObject.GetComponent<Market_Hover>().resetCard();
                        sendSuccessMessage(1);
                        // bool connected = networkManager.sendBuyRequest(md2.cardInt, md2.cardDisplay3.card.buyPrice, 0);
                    }
                    else if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].pips >= (md2.cardDisplay3.card.buyPrice - 1))
                    {
                        if (md2.cardDisplay3.card.buyPrice - 1 == 0)
                        {
                            players[myPlayerIndex].subPips(md2.cardDisplay3.card.buyPrice);
                        }
                        else
                        {
                            players[myPlayerIndex].subPips(md2.cardDisplay3.card.buyPrice - 1);
                        }
                        players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay3.card);
                        Card card = md2.popCard();
                        md2.cardDisplay3.updateCard(card);
                        //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        md2.cardDisplay3.gameObject.GetComponent<Market_Hover>().resetCard();
                        sendSuccessMessage(1);
                    }
                    else
                    {
                        sendErrorMessage(6);
                    }
                    break;
                default:
                    Debug.Log("MarketDeck Error!");
                    break;
            }
        }
    }

    // this method is for cards with special text like paying pips to do something with the card or putting the hat card on your character
    public void checkCardAction()
    {
        // Bottle Rocket UI Logic
        if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardName == "BottleRocket")
        {
            // SetActive the UI
            bottleRocketMenu.SetActive(true);
        }
    }

    public void setBottleRocketBonus()
    {
        if (players[myPlayerIndex].pips >= 3 || players[myPlayerIndex].bottleRocketBonus)
        {
            players[myPlayerIndex].bottleRocketBonus = true;
            // add some success message but change what you initially put here lol
            sendSuccessMessage(14);
            // reset card
            playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
        } else
        {
            sendErrorMessage(6);
            // reset card
            playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
        }
    }

    // BUY RESPONSE
    public void onResponseBuy(ExtendedEventArgs eventArgs)
    {
        Debug.Log("ResponseBuy");
        ResponseBuyEventArgs args = eventArgs as ResponseBuyEventArgs;
        Debug.Log("ID: " + args.user_id); // User_id of player who made purchase
        Debug.Log("cardInt: " + args.x); // Card position in market (1, 2, or 3)
        Debug.Log("Price: " + args.y); // Int price of card purchased
        Debug.Log("T or B: " + args.z); // Top (1) or Bottom (0) market that card was purchased from.

        
        for(int i = 0; i < numPlayers; i++)
        {
            // Find player who made the Buy request.
            if(players[i].user_id == args.user_id)
            {
                // Subtracts pips from Player who made purchase (buy request)
                players[i].subPips(args.y);

                // If purchase was made from the top market
                if(args.z == 1)
                {
                    // Update based on card position in the top market (1, 2, or 3)
                    switch (args.x)
                    {
                        case 1:
                            players[i].deck.putCardOnTop(md1.cardDisplay1.card);
                            Card card = md1.popCard();
                            md1.cardDisplay1.updateCard(card);
                            break;
                        case 2:
                            players[i].deck.putCardOnTop(md1.cardDisplay2.card);
                            Card card2 = md1.popCard();
                            md1.cardDisplay2.updateCard(card2);
                            break;
                        case 3:
                            players[i].deck.putCardOnTop(md1.cardDisplay3.card);
                            Card card3 = md1.popCard();
                            md1.cardDisplay3.updateCard(card3);
                            break;
                    }
                }
                // If purchase was made from the bottom market
                else
                {
                    // Update based on card position in the bottom market (1, 2, or 3)
                    switch (args.x)
                    {
                        case 1:
                            players[i].deck.putCardOnTop(md2.cardDisplay1.card);
                            Card card4 = md2.popCard();
                            md2.cardDisplay1.updateCard(card4);
                            break;
                        case 2:
                            players[i].deck.putCardOnTop(md2.cardDisplay2.card);
                            Card card5 = md2.popCard();
                            md2.cardDisplay2.updateCard(card5);
                            break;
                        case 3:
                            players[i].deck.putCardOnTop(md2.cardDisplay3.card);
                            Card card6 = md2.popCard();
                            md2.cardDisplay3.updateCard(card6);
                            break;
                    }
                }
            }
        }
    }

    public void deal3ToAll()
    {
        for (int i = 0; i < Game.GamePlayers.Count; i++)
        {
            if(i != myPlayerIndex)
            {
                players[i].subHealth(3);
            }
        }
    }

    public void deal1ToAll()
    {
        for (int i = 0; i < Game.GamePlayers.Count; i++)
        {
            if (i != myPlayerIndex)
            {
                players[i].subHealth(1);
            }
        }
    }

    // SELL REQUEST
    public void sellCard()
    {
        Debug.Log("Sell Card");

        // TUTORIAL LOGIC
        if (Game.tutorial)
        {
            cardPlayer.addPips(playerHolster.cardList[selectedCardInt - 1].card.sellPrice);
            td.addCard(playerHolster.cardList[selectedCardInt - 1]);
            playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
            StartCoroutine(waitThreeSeconds(dialog));
            sendSuccessMessage(8);
            return;
        }

        // If this client isn't the current player, display error message.
        if(players[myPlayerIndex].user_id != myPlayerIndex) {
            // "You are not the currentPlayer!"
            sendErrorMessage(7);
        }

        // It is this player's turn.
        else
        {
            // LOGIC FOR BOLO SELLING ABILITY
            // Bolo not flipped and he's selling something that's not a potion
            if (players[myPlayerIndex].isBolo && !players[myPlayerIndex].character.character.flipped && players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType != "Potion")
            {
                players[myPlayerIndex].addPips(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.sellPrice + 1);
                // Bolo flipped selling anything
            } else if(players[myPlayerIndex].isBolo && players[myPlayerIndex].character.character.flipped)
            {
                players[myPlayerIndex].addPips(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.sellPrice + 1);
            } else
            {
                // everyone else
                players[myPlayerIndex].addPips(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.sellPrice);
            }
            td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
            // bool connected = networkManager.sendSellRequest(selectedCardInt, players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.sellPrice);
            players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
            sendSuccessMessage(8);
            // MATTEO: Add sell SFX here.
        }
    }

    // SELL RESPONSE
    public void onResponseSell(ExtendedEventArgs eventArgs)
    {
        Debug.Log("ResponseSell");
        ResponseSellEventArgs args = eventArgs as ResponseSellEventArgs;
        Debug.Log("ID: " + args.user_id); // user_id of seller
        Debug.Log("cardInt: " + args.x); // selectedCardInt or cardPosition
        Debug.Log("Sell Price: " + args.y); // # of Pips the seller will get for selling the selected holster card.

        int cardPosition = args.x - 1;
        Card placeholder = players[0].holster.card1.placeholder;

        for(int i = 0; i < numPlayers; i++) {
            if(players[i].user_id == args.user_id)
            {
                players[i].addPips(players[i].holster.cardList[cardPosition].card.sellPrice);
                td.addCard(players[i].holster.cardList[cardPosition]);
                players[i].holster.cardList[cardPosition].card = placeholder;
                players[i].holster.cardList[cardPosition].updateCard(placeholder);
            }
        }

        // if (Constants.USER_ID != args.user_id)
        // {
        //     players[myPlayerIndex].pips += players[myPlayerIndex].holster.cardList[args.x - 1].card.sellPrice;
        //     td.addCard(players[myPlayerIndex].holster.cardList[args.x - 1]);

        // }
    }

    // TRASH REQUEST
    public void trashCard()
    {
        Debug.Log("Trash Card");

        if (Game.tutorial)
        {
            td.addCard(playerHolster.cardList[selectedCardInt - 1]);
            StartCoroutine(waitThreeSeconds(dialog));
            playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
            sendSuccessMessage(9);
            return;
        }

        // If this client isn't the current player, display error message.
        if(players[myPlayerIndex].user_id != myPlayerIndex) {
            // "You are not the currentPlayer!"
            sendErrorMessage(7);
        }

        // It is this player's turn.
        else
        {
            td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
            // SEND TRASH REQUEST (int x, int y)
            // bool connected = networkManager.sendTrashRequest(selectedCardInt, 0);
            players[myPlayerIndex].cardsTrashed++;
            if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].cardsTrashed == 4)
            {
                sendSuccessMessage(4);
                players[myPlayerIndex].character.canBeFlipped = true;
            }
            players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
            sendSuccessMessage(9);
        }
    }

    // TRASH RESPONSE
    public void onResponseTrash(ExtendedEventArgs eventArgs)
    {
        // args.x is cardInt, args.y is 0
        Debug.Log("Trash Response");
        ResponseTrashEventArgs args = eventArgs as ResponseTrashEventArgs;

        int cardPosition = args.x - 1;
        Card placeholder = players[0].holster.card1.placeholder;

        for(int i = 0; i < numPlayers; i++) {
            if(players[i].user_id == args.user_id)
            {
                td.addCard(players[i].holster.cardList[cardPosition]);
                players[i].holster.cardList[cardPosition].card = placeholder;
                players[i].holster.cardList[cardPosition].updateCard(placeholder);
            }
        }

        // if(Constants.USER_ID != args.user_id)
        // {
        //     td.addCard(players[myPlayerIndex].holster.cardList[args.x - 1]);
        // }
    }

    public void sendSuccessMessage(int notif)
    {
        GameObject message = successMessages[notif - 1];
        message.SetActive(true);
        StartCoroutine(waitThreeSeconds(message));
    }

    public void sendErrorMessage(int notif)
    {
        GameObject message = errorMessages[notif - 1];
        message.SetActive(true);
        StartCoroutine(waitThreeSeconds(message));
    }

    /* If you're wondering why there's two of these
     * it's because one takes in a GameObject and the
     * other is overloaded to take in a Dialog to handle
     * the tutorial text
     */

    IEnumerator waitThreeSeconds(GameObject gameObj)
    {
        yield return new WaitForSeconds(3);
        gameObj.SetActive(false);
    }

}

public enum Gamestate {
    PlayerTurn,
    OpponentTurn,
    Win,
    Lose
}
