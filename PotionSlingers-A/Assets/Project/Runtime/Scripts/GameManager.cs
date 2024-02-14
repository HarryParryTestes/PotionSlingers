using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Steamworks;
using Mirror;
using DG.Tweening;

public class GameManager : MonoBehaviour
{

    public static SteamLobby lobby;
    public static GameManager manager;
    //private OldNetworkManager networkManager;

    // Later change to be however many players joined when the GameScene loads in.
    // (Maybe transferring numPlayers from MainMenu script to here? Maybe get from server?)
    public int numPlayers = 2;
    public int selectedCardInt;
    public int selectedOpponentInt;
    public int opponentCardInt;
    public int previousDamage;
    public int stage = 1;
    public string selectedOpponentName;
    public string currentPlayerName;
    public int loadedCardInt;
    public int myPlayerIndex = 0; // used to be currentPlayer
    public int currentPlayerId = 0;
    public int nicklesDamage = 0;
    public int numTrashed = 0;
    public CardPlayer[] players = new CardPlayer[4];
    public Character[] characters;
    public Dialog dialog;
    public GameObject dialogBox;
    GameObject ob;
    GameObject obTop;
    GameObject obLeft;
    GameObject obRight;
    public GameObject throwingHand;
    public TrashDeck td;
    public MarketDeck md1;
    public MarketDeck md2;
    public GameObject market;
    public GameObject marketButton;
    public CanvasGroup canvasGroup;
    public List<GameObject> successMessages;
    public List<GameObject> errorMessages;
    public DeckMenuScroll deckMenuScroll;

    public List<Card> starterCards;

    public GameObject tutorialArrow;
    public GameObject tutorialArrow2;
    public GameObject pluotPotionMenu;
    public GameObject ExtraInventoryMenu;
    public GameObject nicklesUI;
    public GameObject flippedNicklesUI;
    public GameObject flippedPipMenu;
    public GameObject reetsMenu;
    public GameObject isadoreMenu;
    public GameObject sweetbitterMenu;
    public GameObject bottleRocketMenu;
    public GameObject trashMarketUI;
    public GameObject trashorDamageMenu;
    public GameObject trashDeckMenu;
    public GameObject takeMarketMenu;
    public GameObject opponentHolsterMenu;
    public GameObject trashBonusMenu;
    public GameObject trashPlayerMenu;
    public GameObject faisalMenu;
    public GameObject scarpettaMenu;
    public GameObject shieldMenu;
    public GameObject bubbleWandMenu;
    public GameObject chooseOpponentMenu;
    public GameObject deckMenu;
    public DeckMenuScroll deckDisplay;

    public TMPro.TextMeshProUGUI trashText;

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
    public TMPro.TextMeshProUGUI opLeftText;
    public TMPro.TextMeshProUGUI opLeftTrashText;
    public CharacterDisplay opTop;
    public TMPro.TextMeshProUGUI opTopText;
    public TMPro.TextMeshProUGUI opTopTrashText;
    public CharacterDisplay opRight;
    public TMPro.TextMeshProUGUI opRightText;
    public TMPro.TextMeshProUGUI opRightTrashText;
    public CharacterDisplay opTrashLeft;
    public CharacterDisplay opTrashTop;
    public CharacterDisplay opTrashRight;
    public TMPro.TextMeshProUGUI playerBottomName;
    public TMPro.TextMeshProUGUI playerLeftName;
    public TMPro.TextMeshProUGUI playerTopName;
    public TMPro.TextMeshProUGUI playerRightName;
    public GameObject attackMenu;
    public GameObject loadMenu;
    public GameObject pauseUI;
    public GameObject starterPotionMenu;

    public System.Random rng = new System.Random();

    public bool paused = false;
    public bool starterPotion = false;
    public bool usedStarterPotion = false;
    public bool isadoreAction = true;
    public bool earlyBirdSpecial = false;
    //public bool trashOrDamage = false;
    public bool trash = false;
    public bool damage = false;
    public bool trashDeckBonus = false;
    public bool replaceStarter = false;
    public bool replace = false;
    public bool mirrorCommand = false;
    public bool snakeBonus = false;
    public bool marketSelected = false;
    public bool holster = false;

    public TMPro.TextMeshProUGUI reetsMenuText;
    public GameObject reetsCard;

    public Sprite sprite1;
    public Sprite sprite2;

    public Card starterPotionCard;

    GameObject mainMenu;
    MainMenu mainMenuScript;

    GameObject player1Area;
    GameObject player2Area;

    public CardDisplay tm1;
    public CardDisplay tm2;
    public CardDisplay tm3;
    public CardDisplay tm4;
    public CardDisplay tm5;
    public CardDisplay tm6;

    public CardDisplay takem1;
    public CardDisplay takem2;
    public CardDisplay takem3;
    public CardDisplay takem4;
    public CardDisplay takem5;
    public CardDisplay takem6;

    public CardDisplay opponentCard1;
    public CardDisplay opponentCard2;
    public CardDisplay opponentCard3;
    public CardDisplay opponentCard4;

    public SaveData saveData;

    public MyNetworkManager game;
    public MyNetworkManager Game
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
        //DontDestroyOnLoad(gameObject);

        if (!Game.tutorial)
        {
            tutorialArrow.SetActive(false);
            tutorialArrow2.SetActive(false);
            numPlayers = 2;
        }

        if (Game.tutorial)
        {
            Debug.Log("Happened in Awake");
            dialogBox.SetActive(true);
            dialog.initDialog();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key was pressed");
            // paused = !paused;
            if (pauseUI.activeInHierarchy == false)
            {
                pauseUI.SetActive(true);
            }
            else
            {
                pauseUI.transform.GetChild(0).Find("Debug Menu").gameObject.SetActive(false);
                pauseUI.transform.GetChild(0).Find("Graphics Menu").gameObject.SetActive(false);
                pauseUI.transform.GetChild(0).Find("DialogText").gameObject.SetActive(true);
                pauseUI.transform.GetChild(0).Find("RESUME").gameObject.SetActive(true);
                pauseUI.transform.GetChild(0).Find("AUDIO").gameObject.SetActive(true);
                pauseUI.transform.GetChild(0).Find("GRAPHICS").gameObject.SetActive(true);
                pauseUI.transform.GetChild(0).Find("DEBUG").gameObject.SetActive(true);
                pauseUI.transform.GetChild(0).Find("MAIN MENU").gameObject.SetActive(true);
                pauseUI.SetActive(false);
            }

        }
    }

    public IEnumerator Button()
    {
        yield return new WaitForSeconds(1f);
        if (!marketSelected)
            marketButton.SetActive(true);
    }

    public void moveMarket()
    {
        if (Game.tutorial && dialog.textBoxCounter < 9)
            return;

        if (!marketSelected)
        {
            marketSelected = true;
            if (dialog.textBoxCounter == 10)
            {
                tutorialArrow.GetComponent<ArrowMover>().checkArrow();
                tutorialArrow.SetActive(true);
                tutorialArrow2.GetComponent<SecondArrowMover>().checkArrow();
                tutorialArrow2.SetActive(true);
            }
            marketButton.SetActive(false);
            canvasGroup.blocksRaycasts = false;
            // marketPosition = marketButton.transform.position;
            // marketButton.transform.parent.DOMove(new Vector3(0, 0, 0), 1f);

            // market.transform.DOMove(new Vector3(1010, 300, 0), 1f);
            // marketButton.transform.DOMove(new Vector3(960, 300, 0), 1f);
            market.transform.DOMoveY(636f, 1f);
            // marketButton.transform.DOMoveY(300f, 1f);
            Debug.Log("Market moved???");
        }
        else
        {
            // marketPosition = marketButton.transform.position;
            marketSelected = false;
            StartCoroutine(Button());
            // canvasGroup.blocksRaycasts = true;
            market.transform.DOMoveY(-11.5f, 1f);
            // marketButton.transform.DOMoveY(-300f, 1f);
            Debug.Log("Market reset???");
        }

    }

    public void checkForEndGame()
    {
        Debug.Log("Checking if the game is over");

        if (Game.tutorial)
        {
            // do achievement check in here
            // you probably want to make new UI for this so this is placeholder stuff

            // hold on partner! don't do this yet
            // GameManager.manager.pauseUI.SetActive(true);
            dialog.endTutorialDialog();
            return;
        }

        int numAlive = 0;
        int numDead = 0;

        foreach (CardPlayer cp in players)
        {
            if (cp.dead)
            {
                numDead++;
            }
            else if (cp.gameObject.activeInHierarchy)
            {
                numAlive++;
            }
        }

        Debug.Log("Dead: " + numDead);
        Debug.Log("Alive" + numAlive);

        if (numAlive == 1 && numDead >= 1)
        {
            // The game is over!
            Debug.Log("The game is over!!!");

            // make animated win / loss screen here
            // look up how to use DOTween

            if (Game.storyMode)
            {
                // advance a stage and save the game data
                stage++;
                SaveSystem.SaveGameData(saveData);
            }
        }
    }

    public void ohFuckGoBack()
    {
        Debug.Log("Going back to title menu");
        Game.tutorial = false;

        if (saveData != null)
        {
            saveData.savedGame = true;
            SaveSystem.SaveGameData(saveData);
            Debug.Log("Game data saved");
            SceneManager.LoadScene("TitleMenu");
            return;
        }

        SteamMatchmaking.LeaveLobby((CSteamID)Game.steamLobby.current_lobbyID);
        Game.StopHost();
        SceneManager.LoadScene("TitleMenu");
        //Game.ServerChangeScene("TitleMenu");
    }

    void Start()
    {
        Debug.Log("GameManager started!!!");

        // just testing the new health bar
        // players[0].subHealth(5);

        if (Game.storyMode)
        {
            Debug.Log("Story mode!!!");
            saveData = SaveSystem.LoadGameData();

            // THIS IS WHERE YOU WILL MANIPULATE GAMESTATE WITH SAVE DATA
            if (saveData.savedGame)
            {
                Debug.Log("SAVED GAME!!!");


                // maybe take this out, we'll see
                md1.shuffle();
                md2.shuffle();
                initDecks();

                // check this
                myPlayerIndex = 0;
                Debug.Log(saveData.playerCharName + "!!!");
                Debug.Log(saveData.stage + "!!!");
                stage = saveData.stage;
                // players[0].name = saveData.playerName;
                players[0].charName = saveData.playerCharName;
                players[0].character.onCharacterClick(players[0].charName);
                players[0].checkCharacter();
                // playerBottomName.text = players[0].name;
                playerBottomName.text = SteamFriends.GetPersonaName().ToString();
                players[0].name = playerBottomName.text;
                currentPlayerName = players[0].name;
                // players[0].deck.loadDeck();


                players[2].gameObject.AddComponent<ComputerPlayer>();
                // players[2].name = saveData.oppCharName;
                // players[2].charName = saveData.oppCharName;
                // players[2].character.onCharacterClick(players[2].charName);
                // players[2].checkCharacter();
                // players[2].name = "Reets";
                // playerTopName.text = players[2].charName;

                players[1] = players[2];
                // hardcode this lol
                // playerTopName.text = Game.singlePlayerNames[1];
                players[1].user_id = 1;
                players[2].user_id = 1;

                players[2] = players[3];
                p3.SetActive(false);
                p4.SetActive(false);
                // return;
            }
            else
            {
                // NEW STORY MODE FILE
                Debug.Log("New story mode file started!!!");
                md1.shuffle();
                md2.shuffle();
                initDecks();

                SaveData newSaveData = new SaveData(Game.storyModeCharName, stage);
                saveData.savedGame = false;
                saveData = newSaveData;
                SaveSystem.SaveGameData(saveData);

                myPlayerIndex = 0;
                Debug.Log(saveData.playerCharName + "!!!");
                Debug.Log(saveData.stage + "!!!");
                stage = saveData.stage;
                // players[0].name = saveData.playerName;
                players[0].charName = saveData.playerCharName;
                players[0].character.onCharacterClick(players[0].charName);
                players[0].checkCharacter();
                // playerBottomName.text = players[0].name;
                playerBottomName.text = SteamFriends.GetPersonaName().ToString();
                players[0].name = playerBottomName.text;
                currentPlayerName = players[0].name;
                // players[0].deck.loadDeck();

                // Fingas is stage 1 enemy
                players[1].gameObject.AddComponent<ComputerPlayer>();
                players[1].charName = "Fingas";
                players[1].name = "Fingas";
                playerLeftName.text = players[1].charName;
                players[1].character.onCharacterClick("Fingas");
                players[1].checkCharacter();
                players[1].hpCubes = 1;
                players[1].updateHealthUI();

                // Crow Punk is stage 1 enemy
                players[2].gameObject.AddComponent<ComputerPlayer>();
                players[2].charName = "CrowPunk";
                players[2].name = "CrowPunk";
                playerTopName.text = players[2].charName;
                players[2].character.onCharacterClick("CrowPunk");
                players[2].checkCharacter();
                players[2].hpCubes = 1;
                players[2].updateHealthUI();

                // Fingas is stage 1 enemy
                players[3].gameObject.AddComponent<ComputerPlayer>();
                players[3].charName = "Fingas";
                players[3].name = "Fingas";
                playerRightName.text = players[3].charName;
                players[3].character.onCharacterClick("Fingas");
                players[3].checkCharacter();
                players[3].hpCubes = 1;
                players[3].updateHealthUI();

                /*
                players[1] = players[2];
                // hardcode this lol
                // playerTopName.text = Game.singlePlayerNames[1];
                players[1].user_id = 1;
                players[2].user_id = 1;

                players[2] = players[3];
                p3.SetActive(false);
                p4.SetActive(false);
                */
            }
            /*
            md1.shuffle();
            md2.shuffle();
            initDecks();
            */

            numPlayers = 4;

            if (numPlayers == 2)
            {
                // hardcoding 2-player duel, we'll be able to change the character and gamestate in here
                // when implementing character selection, get info from MyNetworkManager.singlePlayerNames[0]
                // Be sure to change the hardcoded values!!!
                // playerTopName.text = Game.singlePlayerNames[1];
                /*
                int num = rng.Next(0, 2);


                players[0].name = SteamFriends.GetPersonaName().ToString();
                players[0].charName = Game.storyModeCharName;
                players[0].character.onCharacterClick(players[0].charName);
                players[0].checkCharacter();
                playerBottomName.text = SteamFriends.GetPersonaName().ToString();

                if (num == 0)
                {
                    players[2].gameObject.AddComponent<ComputerPlayer>();
                    players[2].charName = "Reets";
                    players[2].character.onCharacterClick("Reets");
                    players[2].checkCharacter();
                    players[2].name = "Reets";
                    playerTopName.text = players[2].charName;
                }

                if (num == 1)
                {
                    players[2].gameObject.AddComponent<ComputerPlayer>();
                    players[2].charName = "Bolo";
                    players[2].character.onCharacterClick("Bolo");
                    players[2].checkCharacter();
                    players[2].name = "Bolo";
                    playerTopName.text = players[2].charName;
                }

                // players[1] = players[2];
                // hardcode this lol
                // playerTopName.text = Game.singlePlayerNames[1];
                //players[1].user_id = 1;
                //players[2].user_id = 1;
                */
                // players[2] = players[3];
                // p3.SetActive(false);
                // p4.SetActive(false);
            }

            numPlayers = 4;
            return;
        }

        // check for quickplay
        if (Game.quickplay)
        {
            Debug.Log("Quickplay started");
            md1.shuffle();
            md2.shuffle();
            initDecks();
            numPlayers = 4;
            // add computer player and make CPU names
            for (int i = 0; i < numPlayers; i++)
            {


                playerBottomName.text = SteamFriends.GetPersonaName().ToString();
                cardPlayer.name = SteamFriends.GetPersonaName().ToString();
                // change CardDisplay here with Game.singlePlayerNames
                // Debug.Log("Changing #" + i + 1 + " player's name to " + Game.singlePlayerNames[i]);
                players[0].charName = "Pluot";
                players[0].character.onCharacterClick("Pluot");
                players[0].checkCharacter();
                currentPlayerName = playerBottomName.text;
                // everyone except player 1
                if (i > 0)
                {
                    players[i].gameObject.AddComponent<ComputerPlayer>();
                    Debug.Log("Computer Player added to player " + (i + 1));
                    players[i].name = "CPU" + i;
                    switch (i)
                    {
                        case 1:
                            players[i].charName = "Reets";
                            playerLeftName.text = players[i].charName;
                            break;
                        case 2:
                            players[i].charName = "Isadore";
                            playerTopName.text = players[i].charName;
                            break;
                        case 3:
                            players[i].charName = "Saltimbocca";
                            playerRightName.text = players[i].charName;
                            break;
                    }

                    players[i].checkCharacter();
                    // CardPlayer in players mutated to be ComputerPlayer
                    //players[i] = players[i].gameObject.GetComponent<ComputerPlayer>();
                }
            }
            return;
        }

        // single player stuff
        if (!Game.tutorial && !Game.multiplayer && !Game.quickplay)
        {
            Debug.Log("So that happened...");
            // changing this to 4 just to test for now, remember to take this out

            Debug.Log(Game.GamePlayers.Count);

            md1.shuffle();
            md2.shuffle();
            initDecks();

            // take this out for now, maybe put it back in
            /*
            for (int j = 0; j < Game.GamePlayers.Count; j++)
            {
                Debug.Log("Name: " + Game.charNames[j]);
                // Debug.Log("Bool: " + Game.charBools[j]);
            }
            */

            // this should fix things
            numPlayers = Game.numPlayers;
            // initDecks();
            // md1.shuffle();
            // md1.initCardDisplays();
            // md2.shuffle();
            // md2.initCardDisplays();

            if (Game.numPlayers == 2)
            {
                players[1] = players[2];
                // hardcode this lol
                playerTopName.text = Game.singlePlayerNames[1];
                players[1].user_id = 1;
                players[2].user_id = 1;

                players[2] = players[3];
                p3.SetActive(false);
                p4.SetActive(false);
            }

            if (Game.numPlayers == 3)
            {
                p4.SetActive(false);
            }

            // initialize the ComputerPlayer classes and add them to the players that need them
            // make the class and then do AddComponent<ClassName>()
            // i'm gonna change this to 
            for (int i = 0; i < numPlayers; i++)
            {
                // try this out, if it doesn't work take this out
                // players[i].character.onCharacterClick(Game.GamePlayers[i].charName);

                // get a NRE when I try to access Game.GamePlayers in singleplayer mode
                // players[i].charName = Game.GamePlayers[i].charName;
                // for player 1, the user

                // should happen regardless if human or CPU

                Debug.Log("Changing #" + (i + 1) + " player's name to " + Game.singlePlayerNames[i]);
                players[i].charName = Game.singlePlayerNames[i];
                players[i].character.onCharacterClick(Game.singlePlayerNames[i]);
                players[i].checkCharacter();

                if (i == 0)
                {
                    playerBottomName.text = SteamFriends.GetPersonaName().ToString();
                    cardPlayer.name = SteamFriends.GetPersonaName().ToString();
                    // change CardDisplay here with Game.singlePlayerNames
                    Debug.Log("Changing #" + (i + 1) + " player's name to " + Game.singlePlayerNames[i]);
                    players[i].charName = Game.singlePlayerNames[i];
                    players[i].character.onCharacterClick(Game.singlePlayerNames[i]);
                    currentPlayerName = playerBottomName.text;
                }
                // everyone except player 1
                if (i > 0)
                {
                    players[i].gameObject.AddComponent<ComputerPlayer>();
                    Debug.Log("Computer Player added to player " + (i + 1));
                    players[i].name = "CPU" + i;
                    switch (i)
                    {
                        case 1:
                            playerLeftName.text = players[i].charName;
                            break;
                        case 2:
                            playerTopName.text = players[i].charName;
                            break;
                        case 3:
                            playerRightName.text = players[i].charName;
                            break;
                    }

                    players[i].checkCharacter();
                    // CardPlayer in players mutated to be ComputerPlayer
                    //players[i] = players[i].gameObject.GetComponent<ComputerPlayer>();
                }
            }

            return;
        }

        // END OF SINGLEPLAYER SETUP

        if (!Game.tutorial)
            StartCoroutine(shuffleDecks());

        p3.SetActive(true);
        p4.SetActive(true);

        Debug.Log("Number of players on network: " + Game.GamePlayers.Count);

        foreach (GamePlayer gp in Game.GamePlayers)
        {
            Debug.Log(gp.playerName);
        }

        // dummy players for tutorial
        if (Game.tutorial)
        {
            Debug.Log("Starting tutorial");
            Debug.Log("Happened in Start");
            dialogBox.SetActive(true);
            dialog.initDialog();
            playerBottomName.text = SteamFriends.GetPersonaName().ToString();
            cardPlayer.name = SteamFriends.GetPersonaName().ToString();
            currentPlayerName = playerBottomName.text;
            playerTopName.text = "BOLO";
            players[2].hpCubes = 1;
            players[2].updateHealthUI();
            p3.SetActive(false);
            p4.SetActive(false);
            return;
            //Debug.Log("Shuffling market decks for tutorial");
        }

        // END OF TUTORIAL SETUP

        initDecks();

        if (Game.GamePlayers.Count == 2)
        {
            players[1] = players[2];
            players[2] = players[3];
            p3.SetActive(false);
            p4.SetActive(false);
        }

        if (Game.GamePlayers.Count == 3)
        {
            //p3.SetActive(false);
            p4.SetActive(false);
        }

        // add in Mirror shuffle command
        //Game.GamePlayers[0].CmdShuffleDecks();

        //ob = GameObject.Find("CharacterCard");
        //Player playerOb = ob.GetComponent<Player>();
        //Debug.Log("Player 1's character is... " + playerOb.charName);
        int tracker = 0;
        currentPlayerName = Game.GamePlayers[0].playerName;
        numPlayers = Game.GamePlayers.Count;
        for (int i = 0; i < Game.GamePlayers.Count; i++)
        {
            if (Game.GamePlayers[i].isLocalPlayer)
            {
                /*
                if(numPlayers == 2)
                {
                    if(tracker == 0)
                    {
                        players[0].user_id = 1;
                        players[1].user_id = 0;
                    }
                    if (tracker == 1)
                    {
                        players[0].user_id = 1;
                        players[1].user_id = 0;
                    }
                }

                if(numPlayers == 3)
                {

                }

                if(numPlayers == 4)
                {

                }
                */
                Debug.Log("Found local player");
                players[0].currentPlayerHighlight.SetActive(true);
                // maybe use this?
                // players[i].currentPlayerHighlight.SetActive(true);
                playerBottomName.text = Game.GamePlayers[i].playerName;
                Debug.Log("Player name before becoming cardPlayer name: " + Game.GamePlayers[i].playerName);
                players[0].name = Game.GamePlayers[i].playerName;
                players[0].charName = Game.GamePlayers[i].charName;
                players[0].user_id = i;
                Game.GamePlayers[i].hp = players[0].hp;
                Game.GamePlayers[i].essenceCubes = players[0].hpCubes;
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
                    players[1].user_id = i;
                    players[1].currentPlayerHighlight.SetActive(false);
                    Game.GamePlayers[i].hp = players[1].hp;
                    Game.GamePlayers[i].essenceCubes = players[1].hpCubes;
                    players[1].character.onCharacterClick(Game.GamePlayers[i].charName);
                    players[1].checkCharacter();
                    tracker++;

                }
                if (tracker == 1)
                {
                    players[2].currentPlayerHighlight.SetActive(false);
                    playerTopName.text = Game.GamePlayers[i].playerName;
                    players[2].name = Game.GamePlayers[i].playerName;
                    players[2].charName = Game.GamePlayers[i].charName;
                    players[2].user_id = i;
                    Game.GamePlayers[i].hp = players[2].hp;
                    Game.GamePlayers[i].essenceCubes = players[2].hpCubes;
                    players[2].character.onCharacterClick(Game.GamePlayers[i].charName);
                    players[2].checkCharacter();
                    tracker++;
                }
                if (tracker == 2)
                {
                    players[3].currentPlayerHighlight.SetActive(false);
                    playerRightName.text = Game.GamePlayers[i].playerName;
                    players[3].name = Game.GamePlayers[i].playerName;
                    players[3].charName = Game.GamePlayers[i].charName;
                    players[3].user_id = i;
                    Game.GamePlayers[i].hp = players[3].hp;
                    Game.GamePlayers[i].essenceCubes = players[3].hpCubes;
                    players[3].character.onCharacterClick(Game.GamePlayers[i].charName);
                    players[3].checkCharacter();
                }

            }
        }
        /*
        menu = GameObject.Find("MainMenuScript").GetComponent<MainMenu>();
        networkManager = GameObject.Find("OldNetworkManager").GetComponent<OldNetworkManager>();
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
        // td = GameObject.Find("TrashPile").GetComponent<TrashDeck>();
        // md1 = GameObject.Find("PotionPile").GetComponent<MarketDeck>();
        md1.init();
        // md2 = GameObject.Find("SpecialCardPile").GetComponent<MarketDeck>();
        md2.init();
        Debug.Log("Decks shuffled");
    }

    public void updateTrashMarketMenu()
    {
        tm1.updateCard(md1.cardDisplay1.card);
        tm2.updateCard(md1.cardDisplay2.card);
        tm3.updateCard(md1.cardDisplay3.card);
        tm4.updateCard(md2.cardDisplay1.card);
        tm5.updateCard(md2.cardDisplay2.card);
        tm6.updateCard(md2.cardDisplay3.card);
    }

    public void updateTakeMarketMenu()
    {
        takem1.updateCard(md1.cardDisplay1.card);
        takem2.updateCard(md1.cardDisplay2.card);
        takem3.updateCard(md1.cardDisplay3.card);
        takem4.updateCard(md2.cardDisplay1.card);
        takem5.updateCard(md2.cardDisplay2.card);
        takem6.updateCard(md2.cardDisplay3.card);
    }

    public void setTrashBool()
    {
        trash = true;
    }

    public void setDamageBool()
    {
        damage = true;
    }

    public void checkFlip()
    {
        // TUTORIAL LOGIC
        if (Game.tutorial)
        {
            if (dialog.textBoxCounter != 33)
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

        if (Game.multiplayer)
        {
            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == currentPlayerName)
                {
                    Debug.Log("Starting Mirror CmdCheckFlip");
                    // do the Mirror Command
                    gp.CmdCheckFlip(currentPlayerName);
                }
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
                players[myPlayerIndex].character.canBeFlipped = false;
                sendSuccessMessage(11);
                return;
            }

            if (players[myPlayerIndex].isIsadore)
            {
                players[myPlayerIndex].character.flipCard();
                players[myPlayerIndex].character.menu.SetActive(false);
                players[myPlayerIndex].character.canBeFlipped = false;
                sendSuccessMessage(11);
                return;
            }

            if (players[myPlayerIndex].isPluot)
            {
                players[myPlayerIndex].character.flipCard();
                players[myPlayerIndex].character.menu.SetActive(false);
                players[myPlayerIndex].character.canBeFlipped = false;
                sendSuccessMessage(11);
                return;
            }

            // pay 2 pips to flip sweetbitter back to front
            if (players[myPlayerIndex].isSweetbitter && players[myPlayerIndex].pips >= 2)
            {
                players[myPlayerIndex].subPips(2);
                players[myPlayerIndex].character.flipCard();
                players[myPlayerIndex].character.menu.SetActive(false);
                return;
            }

            if (players[myPlayerIndex].isSweetbitter && players[myPlayerIndex].pips < 2)
            {
                sendErrorMessage(11);
                players[myPlayerIndex].character.menu.SetActive(false);
                return;
            }
        }

        if (players[myPlayerIndex].isScarpetta && players[myPlayerIndex].pipsUsedThisTurn == 0 && players[myPlayerIndex].potionsThrown == 0 && players[myPlayerIndex].artifactsUsed == 0 && players[myPlayerIndex].character.character.flipped == false)
        {
            players[myPlayerIndex].character.canBeFlipped = true;
            players[myPlayerIndex].character.flipCard();
            players[myPlayerIndex].character.menu.SetActive(false);
            return;
        }

        if (players[myPlayerIndex].isScarpetta && (players[myPlayerIndex].pipsUsedThisTurn > 0 || players[myPlayerIndex].potionsThrown > 0 || players[myPlayerIndex].artifactsUsed > 0))
        {
            Debug.Log("Reached Scarpetta here");
            sendErrorMessage(11);
            players[myPlayerIndex].character.menu.SetActive(false);
            return;
        }


        /*
        else
        {
            // character card flip error
            sendErrorMessage(11);
            players[myPlayerIndex].character.menu.SetActive(false);
            return;
        }
        */

        if (players[myPlayerIndex].character.canBeFlipped)
        {
            players[myPlayerIndex].character.flipCard();
            sendSuccessMessage(11);
            if (players[myPlayerIndex].isBolo || players[myPlayerIndex].isNickles || players[myPlayerIndex].isReets || players[myPlayerIndex].isScarpetta || players[myPlayerIndex].isTwins)
            {
                players[myPlayerIndex].character.canBeFlipped = false;
            }

            players[myPlayerIndex].character.menu.SetActive(false);
        }
        else
        {
            // character card flip error
            sendErrorMessage(11);
            Debug.Log("Reached here");
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

        if (Game.multiplayer)
        {
            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == currentPlayerName)
                {
                    Debug.Log("Starting Mirror CmdSetPluotBonus");
                    // do the Mirror Command
                    gp.CmdSetPluotBonus(currentPlayerName, bonus);
                }
            }
            return;
        }
        players[myPlayerIndex].pluotBonusType = bonus;
    }

    // if there are open spots in the holster, move cards from deck to holster
    public void onStartTurn(CardPlayer player)
    {
        Debug.Log(player.name + "'s turn!");
        sendSuccessMessage(18, player.name);
        earlyBirdSpecial = false;
        usedStarterPotion = false;
        trash = false;
        damage = false;
        trashDeckBonus = false;

        // CARDS GET PUT INTO HOLSTER FROM DECK IN THIS METHOD
        // SAVE STORY MODE DATA HERE!

        foreach (CardDisplay cd in player.holster.cardList)
        {
            if (player.deck.deckList.Count >= 1)
            {
                if (cd.card.name == "placeholder")
                {
                    cd.updateCard(player.deck.popCard());
                }
            }
        }
        // this should make it not trigger every turn
        if (player.isPluot && player.name == currentPlayerName && player.gameObject.GetComponent<ComputerPlayer>() == null)
        {
            if (Game.multiplayer)
            {
                foreach (GamePlayer gp in Game.GamePlayers)
                {
                    if (gp.playerName == currentPlayerName)
                    {
                        gp.RpcPluotPotionMenu(currentPlayerName);
                    }
                }
                return;
            }
            pluotPotionMenu.SetActive(true);
        }
        player.setDefaultTurn();
    }

    public void setSCInt(int num)
    {
        selectedCardInt = num;
    }

    public void setSCInt(string cardName)
    {

        for (int i = 0; i < 4; i++)
        {
            if (players[myPlayerIndex].holster.cardList[i].card.cardName == cardName)
            {
                selectedCardInt = i + 1;
                break;
            }
        }
        // selectedCardInt = num;
    }

    public void setOPInt(int num)
    {
        selectedOpponentInt = num;
    }

    public void setOPName(string username)
    {
        foreach (CardPlayer cp in players)
        {
            if (cp.name == username)
            {
                Debug.Log("Opponent found");
                tempPlayer = cp;
                selectedOpponentName = cp.name;
                return;
            }
        }

        /*
        selectedOpponentCharName = character.character.name;
        foreach (CardPlayer cp in players)
        {
            if (cp.charName == character.character.name)
            {
                Debug.Log("Opponent found");
                tempPlayer = cp;
                break;
            }
        }
        */

        /*
        if (multiplayer)
        {

        }
        else
        {
            Debug.Log("Singleplayer");
            foreach(CardPlayer cp in players)
            {
                if(cp.charName == characterName)
                {
                    tempPlayer = cp;
                }
            }
        }
        */
    }

    public void setLoadedInt(string cardName)
    {
        // TUTORIAL LOGIC
        // temporarily take this out

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


        for (int i = 0; i < 4; i++)
        {
            if (players[myPlayerIndex].holster.cardList[i].card.cardName == cardName)
            {
                loadedCardInt = i;
                break;
            }
        }
    }

    public void displayDeck()
    {
        deckMenuScroll.displayCards();
        // deckDisplay.displayCards();
    }

    public void opponentTrashesHolster(CharacterDisplay cd)
    {
        if (Game.multiplayer)
        {
            foreach (GamePlayer gp in Game.GamePlayers)
            {
                if (gp.name == cd.character.cardName)
                {
                    // TargetRpc
                    Debug.Log("Trash Holster ClientRpc");
                    gp.RpcTrashHolster(gp.name);
                }
            }
            return;
        }
        // local singleplayer logic goes here 

    }

    public void everyoneTrashesOneCard()
    {
        if (Game.multiplayer)
        {
            foreach (GamePlayer gp in Game.GamePlayers)
            {
                if (gp.playerName != currentPlayerName)
                {
                    gp.CmdEverybodyTrashOneCard(gp.playerName);
                }
            }
            return;
        }
        // single player logic goes here
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
            opTopText.text = "Bolo";
            return;
        }

        if (trashPlayerMenu.activeInHierarchy)
        {
            Debug.Log("Trash Player Menu");
            // Displaying opponents to attack for 2 player game.
            if (numPlayers == 2)
            {
                if (Game.multiplayer)
                {
                    opTrashTop.updateCharacter(players[1].character.character);
                    opTopTrashText.text = players[1].name;
                    opTrashLeft.gameObject.SetActive(false);
                    opTrashRight.gameObject.SetActive(false);
                }
                else
                {
                    // changing this to 1 from 2 should fix it
                    opTrashTop.updateCharacter(players[1].character.character);
                    opTopTrashText.text = players[1].name;
                    opTrashLeft.gameObject.SetActive(false);
                    opTrashRight.gameObject.SetActive(false);
                }

                // For all players that are not this client's player, display their character in attackMenu.
            }

            // Displaying opponents to attack for 3 player game.
            if (numPlayers == 3)
            {
                opTrashLeft.gameObject.SetActive(true);
                opTrashTop.gameObject.SetActive(true);
                opTrashRight.gameObject.SetActive(false);
                int tracker = 0;

                opTrashLeft.updateCharacter(players[1].character.character);
                opLeftTrashText.text = players[1].name;
                opTrashTop.updateCharacter(players[2].character.character);
                opTopTrashText.text = players[2].name;

            }

            // Displaying opponents to attack for 4 player game.
            if (numPlayers == 4)
            {
                opTrashLeft.gameObject.SetActive(true);
                opTrashRight.gameObject.SetActive(true);
                int tracker = 0;

                opTrashLeft.updateCharacter(players[1].character.character);
                opLeftTrashText.text = players[1].name;
                opTrashTop.updateCharacter(players[2].character.character);
                opTopTrashText.text = players[2].name;
                opTrashRight.updateCharacter(players[3].character.character);
                opRightTrashText.text = players[3].name;
            }
            return;
        }

        if (Game.storyMode)
        {
            numPlayers = 2;
        }

        // Displaying opponents to attack for 2 player game.
        if (numPlayers == 2)
        {
            if (Game.multiplayer)
            {
                opTop.updateCharacter(players[1].character.character);
                opTopText.text = players[1].name;
                opLeft.gameObject.SetActive(false);
                opRight.gameObject.SetActive(false);
            }
            else
            {
                // changing this to 1 from 2 should fix it
                opTop.updateCharacter(players[1].character.character);
                opTopText.text = players[1].name;
                opLeft.gameObject.SetActive(false);
                opRight.gameObject.SetActive(false);
            }

            // For all players that are not this client's player, display their character in attackMenu.
        }

        // Displaying opponents to attack for 3 player game.
        if (numPlayers == 3)
        {
            opLeft.gameObject.SetActive(true);
            opTop.gameObject.SetActive(true);
            opRight.gameObject.SetActive(false);
            int tracker = 0;

            opLeft.updateCharacter(players[1].character.character);
            opLeftText.text = players[1].name;
            opTop.updateCharacter(players[2].character.character);
            opTopText.text = players[2].name;

        }

        // Displaying opponents to attack for 4 player game.
        if (numPlayers == 4)
        {
            opLeft.gameObject.SetActive(true);
            opRight.gameObject.SetActive(true);
            int tracker = 0;

            opLeft.updateCharacter(players[1].character.character);
            opLeftText.text = players[1].name;
            opTop.updateCharacter(players[2].character.character);
            opTopText.text = players[2].name;
            opRight.updateCharacter(players[3].character.character);
            opRightText.text = players[3].name;
        }
    }

    public void displayOpponentHolster()
    {
        opponentCard1.updateCard(tempPlayer.holster.cardList[0].card);
        opponentCard2.updateCard(tempPlayer.holster.cardList[1].card);
        opponentCard3.updateCard(tempPlayer.holster.cardList[2].card);
        opponentCard4.updateCard(tempPlayer.holster.cardList[3].card);
    }

    public void turnHolsterOff()
    {
        holster = false;
    }

    public void displayOpponentHolster(CardPlayer cardPlayer)
    {
        holster = true;
        opponentCard1.updateCard(cardPlayer.holster.cardList[0].card);
        opponentCard2.updateCard(cardPlayer.holster.cardList[1].card);
        opponentCard3.updateCard(cardPlayer.holster.cardList[2].card);
        opponentCard4.updateCard(cardPlayer.holster.cardList[3].card);
    }

    public void stealCard(string opponentName, int selectedCard)
    {
        Debug.Log("Player receiving card: " + currentPlayerName);
        Debug.Log("Stealing card from " + opponentName);
        Debug.Log("CARDVALUE = " + selectedCard);

        foreach (CardPlayer cp in players)
        {
            if (cp.name == currentPlayerName)
            {
                foreach (CardDisplay cd in cp.holster.cardList)
                {
                    if (cd.card.cardName == "placeholder")
                    {
                        foreach (CardPlayer cp2 in players)
                        {
                            if (cp2.name == opponentName)
                            {
                                Debug.Log("Code reaches here");
                                cd.updateCard(cp2.holster.cardList[selectedCard - 1].card);
                                Debug.Log("Code reaches here");
                                cp2.holster.cardList[selectedCard - 1].updateCard(td.card);
                                Debug.Log("Code reaches here");
                                sendSuccessMessage(20);
                                Debug.Log("Code reaches here");
                                return;
                            }
                        }
                    }
                }
            }
        }

        /*
        foreach(CardDisplay cd in players[myPlayerIndex].holster.cardList)
        {
            if (cd.card.cardName == "placeholder")
            {
                foreach(CardPlayer cp in players)
                {
                    if(cp.name == opponentName)
                    {
                        Debug.Log("Code reaches here");
                        cd.updateCard(cp.holster.cardList[selectedCard - 1].card);
                        Debug.Log("Code reaches here");
                        cp.holster.cardList[selectedCard - 1].updateCard(td.card);
                        Debug.Log("Code reaches here");
                        sendSuccessMessage(20);
                        Debug.Log("Code reaches here");
                        return;
                    }
                }
                Debug.Log("Code reaches here");
                cd.updateCard(tempPlayer.holster.cardList[selectedCard - 1].card);
                Debug.Log("Code reaches here");
                tempPlayer.holster.cardList[selectedCard - 1].updateCard(td.card);
                Debug.Log("Code reaches here");
                sendSuccessMessage(20);
                Debug.Log("Code reaches here");
                // break;
                return;
            }
        }
        */
    }

    public void replaceOpponentCardWithStarter(int selectedCard)
    {
        if (holster)
            return;

        if (snakeBonus)
        {
            if (Game.multiplayer)
            {
                foreach (GamePlayer gp in Game.GamePlayers)
                {
                    // if the steam usernames match
                    if (gp.playerName == currentPlayerName)
                    {
                        Debug.Log("Starting Mirror CmdStealCard");
                        // do the Mirror Command
                        gp.CmdStealCard(tempPlayer.name, selectedCard);
                    }
                }
            }
            else
            {
                stealCard(tempPlayer.name, selectedCard);
            }

            snakeBonus = false;
            opponentHolsterMenu.SetActive(false);
            return;
        }

        if (replaceStarter)
        {
            if (tempPlayer.holster.cardList[selectedCard - 1].card.cardType == "Potion")
            {
                tempPlayer.holster.cardList[selectedCard - 1].updateCard(starterCards[2]);
            }

            if (tempPlayer.holster.cardList[selectedCard - 1].card.cardType == "Artifact")
            {
                tempPlayer.holster.cardList[selectedCard - 1].updateCard(starterCards[0]);
            }

            if (tempPlayer.holster.cardList[selectedCard - 1].card.cardType == "Vessel")
            {
                tempPlayer.holster.cardList[selectedCard - 1].updateCard(starterCards[1]);
            }

            if (tempPlayer.holster.cardList[selectedCard - 1].card.cardType == "Ring")
            {
                tempPlayer.holster.cardList[selectedCard - 1].updateCard(starterCards[3]);
            }
        }
        else
        {
            Debug.Log("Did not replace starter");
            if (Game.multiplayer)
            {
                foreach (GamePlayer gp in Game.GamePlayers)
                {
                    // if the steam usernames match
                    if (gp.playerName == tempPlayer.name)
                    {
                        Debug.Log("Starting Mirror CmdTrashCard");
                        // do the Mirror Command
                        gp.CmdTrashCard(tempPlayer.name, selectedCard);
                    }
                }
                return;
            }
            td.addCard(tempPlayer.holster.cardList[selectedCard - 1]);
        }
        replaceStarter = false;
        opponentHolsterMenu.SetActive(false);
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

        if (Game.multiplayer)
        {
            Debug.Log("Reached this part in displayPotions");
            foreach (CardPlayer cp in players)
            {
                if (cp.name == currentPlayerName)
                {
                    foreach (CardDisplay cd in cp.holster.cardList)
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
                                    set++;
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
                    if (set == 3)
                    {
                        loadMenu.transform.Find("Card (Left)").gameObject.SetActive(true);
                        loadMenu.transform.Find("Card (Middle)").gameObject.SetActive(true);
                        loadMenu.transform.Find("Card (Right)").gameObject.SetActive(true);
                    }
                }
            }
            return;
        }


        foreach (CardDisplay cd in players[myPlayerIndex].holster.cardList)
        {
            // Debug.Log("CardName is: " + cd.card.cardName + ". CardType is: " + cd.card.cardType);
            // if(cd.card.cardType.ToLower() == "potion")
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
                        set++;
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
        if (set == 3)
        {
            loadMenu.transform.Find("Card (Right)").gameObject.SetActive(true);
        }
    }

    // END TURN REQUEST
    public void endTurn()
    {
        // reset the market at the end of your turn
        if (marketSelected)
        {
            moveMarket();
        }
        // TUTORIAL LOGIC
        if (Game.tutorial)
        {
            if ((dialog.textBoxCounter != 14 && dialog.textBoxCounter != 24) && dialog.textBoxCounter < 39)
            {
                sendErrorMessage(18);
                playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                return;
            }
            Debug.Log("Tutorial turn ended");
            onStartTurn(cardPlayer);
            if (dialog.textBoxCounter == 24)
            {
                dialog.textBoxCounter++;
            }
            StartCoroutine(waitThreeSeconds(dialog));
            return;
        }

        // If this client isn't the current player, display error message.
        // Player can't end turn if it isn't their turn.
        // (to change this to maybe disable endTurn button or grey it out?? turn it from button to image when not currentPlayer?)
        /*
        if (players[myPlayerIndex].user_id != myPlayerIndex) 
        {
            // "You are not the currentPlayer!"
            sendErrorMessage(7);
            return;
        }
        */

        Debug.Log("Player's user_id: " + players[myPlayerIndex].user_id);
        Debug.Log("myPlayerIndex: " + myPlayerIndex);

        if (Game.multiplayer)
        {
            /*
            if (numPlayers == 2 && Game.GamePlayers[0].playerName == currentPlayerName && players[myPlayerIndex].user_id != myPlayerIndex)
            {
                // "You are not the currentPlayer!"
                sendErrorMessage(7);
                return;
            }

            if (numPlayers == 2 && Game.GamePlayers[1].playerName == currentPlayerName && players[myPlayerIndex].user_id == myPlayerIndex)
            {
                // "You are not the currentPlayer!"
                sendErrorMessage(7);
                return;
            }
            */
            // end turn mirror command
            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == currentPlayerName)
                {
                    Debug.Log("Starting Mirror CmdEndTurn");
                    // do the Mirror Command
                    gp.CmdEndTurn(gp.playerName);
                }
            }
        }
        else
        {
            Debug.Log("Local command");
            if (players[myPlayerIndex].user_id != myPlayerIndex)
            {
                // "You are not the currentPlayer!"
                sendErrorMessage(7);
                return;
            }
            // Logic to check for end of turn effect ring
            foreach (CardDisplay cd in players[myPlayerIndex].holster.cardList)
            {
                if (cd.card.cardName == "Vengeful Ring of the Cursed Mutterings")
                {
                    if (players[myPlayerIndex].doubleRingBonus)
                    {
                        dealDamageToAll(4);
                    }
                    else
                    {
                        dealDamageToAll(2);
                    }
                }

                // check for Blacksnake Pip Sling
                if (cd.card.cardName == "Blacksnake Pip Sling")
                {
                    players[myPlayerIndex].deck.putCardOnBottom(cd.card);
                    cd.updateCard(cd.placeholder);
                }

                // check for gambling ring
                if (cd.card.cardName == "RingofGamblingMopoji" && players[myPlayerIndex].pipsUsedThisTurn == 0)
                {
                    players[myPlayerIndex].pipCount = rng.Next(1, 11);
                    if (players[myPlayerIndex].doubleRingBonus)
                    {
                        Debug.Log("Double ring bonus");
                        players[myPlayerIndex].pipCount *= 2;
                    }
                    Debug.Log("New pip count: " + players[myPlayerIndex].pipCount);
                }
            }
            // taking this out for now
            // players[myPlayerIndex].currentPlayerHighlight.SetActive(false);

            int potions;
            int artifacts;
            int pips;


            // if character's name matches your Steam username
            if (SteamFriends.GetPersonaName().ToString() == players[myPlayerIndex].name)
            {
                SteamUserStats.GetStat("potions_thrown", out potions);
                SteamUserStats.GetStat("artifacts_used", out artifacts);
                SteamUserStats.GetStat("pips_spent", out pips);


                potions += players[myPlayerIndex].potionsThrown;
                artifacts += players[myPlayerIndex].artifactsUsed;
                pips += players[myPlayerIndex].pipsUsedThisTurn;

                if (potions >= 10)
                {
                    SteamUserStats.SetAchievement("THROW_10_POTIONS");
                }

                if (artifacts >= 10)
                {
                    SteamUserStats.SetAchievement("USE_10_ARTIFACTS");
                }

                if (pips >= 100)
                {
                    SteamUserStats.SetAchievement("SPEND_100_PIPS");
                }

                SteamUserStats.SetStat("potions_thrown", potions);
                SteamUserStats.SetStat("artifacts_used", artifacts);
                SteamUserStats.SetStat("pips_spent", pips);

                SteamUserStats.StoreStats();
            }

            myPlayerIndex++;

            if (myPlayerIndex >= numPlayers)
            {
                Debug.Log("myPlayerIndex rolled over???");
                myPlayerIndex = 0;
            }
            sendSuccessMessage(18);
            currentPlayerName = players[myPlayerIndex].name;

            // check if the CardPlayer gameObject has a ComputerPlayer script attached
            if (players[myPlayerIndex].gameObject.GetComponent<ComputerPlayer>() != null)
            {
                Debug.Log("Computer player!");
                Debug.Log("myPlayerIndex is " + myPlayerIndex);
                onStartTurn(players[myPlayerIndex]);
                players[myPlayerIndex].gameObject.GetComponent<ComputerPlayer>().AICards.Clear();
                players[myPlayerIndex].gameObject.GetComponent<ComputerPlayer>().StartCoroutine(players[myPlayerIndex].gameObject.GetComponent<ComputerPlayer>().waitASecBro());
            }
            else
            {
                onStartTurn(players[myPlayerIndex]);
            }

            // Debug.Log("Request End Turn");

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

    public void addTP()
    {
        if (Game.multiplayer)
        {

            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == currentPlayerName)
                {
                    Debug.Log("Starting Mirror CmdAddTP");
                    // do the Mirror Command
                    gp.CmdAddTP(currentPlayerName);
                }
            }
            return;
        }

        if (players[myPlayerIndex].pips >= 6 && !players[myPlayerIndex].character.uniqueCardUsed)
        {
            players[myPlayerIndex].addThePhylactery();
            players[myPlayerIndex].character.uniqueCardUsed = true;
        }
        else
        {
            // you are too poor or you did it already
            Debug.Log("You are poor!");
            sendErrorMessage(6);
        }
    }

    public void addEI()
    {
        if (Game.multiplayer)
        {

            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == currentPlayerName)
                {
                    Debug.Log("Starting Mirror CmdAddEI");
                    // do the Mirror Command
                    gp.CmdAddEI(currentPlayerName);
                }
            }
            return;
        }

        if (Game.tutorial)
        {
            cardPlayer.addExtraInventory();
            StartCoroutine(waitThreeSeconds(dialog));
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
        if (Game.multiplayer)
        {

            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == currentPlayerName)
                {
                    Debug.Log("Starting Mirror CmdAddPS");
                    // do the Mirror Command
                    gp.CmdAddPS(currentPlayerName);
                }
            }
            return;
        }

        // send an error message if they don't have enough pips
        if (players[myPlayerIndex].pips < 3 || players[myPlayerIndex].character.uniqueCardUsed)
        {
            sendErrorMessage(6);
            return;
        }
        players[myPlayerIndex].subPips(3);
        players[myPlayerIndex].addPipSling();

        foreach (CardDisplay cd in players[myPlayerIndex].holster.cardList)
        {
            if (cd.card.cardName == "Blacksnake Pip Sling")
            {
                players[myPlayerIndex].character.uniqueCardUsed = true;
            }
        }
    }

    public void addCBB()
    {
        if (Game.multiplayer)
        {

            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == currentPlayerName)
                {
                    Debug.Log("Starting Mirror CmdAddCBB");
                    // do the Mirror Command
                    gp.CmdAddCBB(currentPlayerName);
                }
            }
            return;
        }

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
        if (Game.multiplayer)
        {

            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == currentPlayerName)
                {
                    Debug.Log("Starting Mirror CmdAddReetsCard");
                    // do the Mirror Command
                    gp.CmdAddReetsCard(currentPlayerName);
                }
            }
            return;
        }

        if (players[myPlayerIndex].character.character.flipped)
        {
            if (players[myPlayerIndex].pips >= 1)
            {
                // players[myPlayerIndex].subPips(1);
                players[myPlayerIndex].addReetsCard();
            }
            else
            {
                Debug.Log("Did this fire?");
                sendErrorMessage(6);
            }
        }
        else
        // not flipped
        {
            if (players[myPlayerIndex].pips >= 2)
            {
                // players[myPlayerIndex].subPips(2);
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
        // why did you never check their money? Dumbass lol
        if (damage > players[myPlayerIndex].pips)
        {
            Debug.Log("Nickles action failed");
            sendErrorMessage(6);
            return;
        }

        Debug.Log("Nickles action succeeded");
        players[myPlayerIndex].nicklesAction = true;
        nicklesDamage = damage;
        chooseOpponentMenu.SetActive(true);
        displayOpponents();
    }

    public void takeTrashCard(CardDisplay cd)
    {
        int i;
        for (i = 0; i < td.deckList.Count; i++)
        {
            if (td.deckList[i].cardName == cd.card.cardName)
            {
                Debug.Log("Card found in trash");
                break;
            }
        }

        if (Game.multiplayer)
        {
            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == currentPlayerName)
                {
                    Debug.Log("Starting Mirror CmdTakeTrashCard");
                    // do the Mirror Command
                    gp.CmdTakeTrashCard(currentPlayerName, i);
                }
            }
            return;
        }
        players[myPlayerIndex].deck.putCardOnTop(td.deckList[i]);
        td.deckList.RemoveAt(i);
    }

    public void buyTrashCard(CardDisplay cd)
    {
        int i;
        int price = 99;
        for (i = 0; i < td.deckList.Count; i++)
        {
            if (td.deckList[i].cardName == cd.card.cardName)
            {
                Debug.Log("Card found in trash");
                price = cd.card.buyPrice;
                break;
            }
        }

        if (price > players[myPlayerIndex].pips || !players[myPlayerIndex].isScarpetta)
        {
            sendErrorMessage(6);
        }

        if (Game.multiplayer)
        {
            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == currentPlayerName)
                {
                    Debug.Log("Starting Mirror CmdBuyTrashCard");
                    // do the Mirror Command
                    gp.CmdBuyTrashCard(currentPlayerName, i);
                }
            }
            return;
        }
        players[myPlayerIndex].subPips(price);
        players[myPlayerIndex].deck.putCardOnTop(td.deckList[i]);
        td.deckList.RemoveAt(i);
    }

    // any time ACTION is pressed on the player in the game scene
    // it will go through the logic in this method

    public void checkPlayerAction()
    {
        // TUTORIAL LOGIC
        if (Game.tutorial)
        {
            if (cardPlayer.character.character.flipped && dialog.textBoxCounter == 35)
            {
                // do Pluot action (I'll code this in later)
                ExtraInventoryMenu.SetActive(true);

            }
            else
            {
                // display error message
                sendErrorMessage(10);
            }
            return;
        }

        if (Game.multiplayer)
        {

            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == currentPlayerName)
                {
                    Debug.Log("Starting Mirror CmdCheckPlayerAction");
                    // do the Mirror Command
                    gp.CmdCheckPlayerAction(currentPlayerName);
                }
            }
            return;
        }

        if (players[myPlayerIndex].isScarpetta)
        {
            if (players[myPlayerIndex].character.character.flipped)
            {
                sendErrorMessage(10);
            }
            else if (players[myPlayerIndex].pips >= 2)
            {
                players[myPlayerIndex].subPips(2);
                scarpettaMenu.SetActive(true);
            }
        }

        if (players[myPlayerIndex].isSweetbitter)
        {
            if (players[myPlayerIndex].character.character.flipped)
            {
                sendErrorMessage(10);
            }
            else
            {
                sweetbitterMenu.SetActive(true);
            }
        }

        if (players[myPlayerIndex].isIsadore && players[myPlayerIndex].character.character.flipped && !players[myPlayerIndex].character.uniqueCardUsed)
        {
            isadoreMenu.SetActive(true);
        }
        else if (players[myPlayerIndex].isIsadore && (!players[myPlayerIndex].character.character.flipped || players[myPlayerIndex].character.uniqueCardUsed))
        {
            // not able to do action
            // fix this later
            sendErrorMessage(13);
        }

        if (players[myPlayerIndex].isPluot)
        {
            if (players[myPlayerIndex].character.character.flipped)
            {
                // prompt ui for adding Extra Inventory into holster
                ExtraInventoryMenu.SetActive(true);
            }
            else
            {
                sendErrorMessage(13);
            }

        }

        if (players[myPlayerIndex].isReets)
        {
            //Image image = reetsMenu.GetComponent<Image>();
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
        }
        else if (players[myPlayerIndex].isNickles && players[myPlayerIndex].nicklesAction)
        {
            // error message because you already did it once this turn
            sendErrorMessage(15);
        }
    }

    public void put4CardsInHolster()
    {
        foreach (CardDisplay cd in players[myPlayerIndex].holster.cardList)
        {
            cd.updateCard(md1.popCard());
        }
    }

    /* If you're wondering why there's two of these
     * it's because one takes in a GameObject and the
     * other is overloaded to take in a Dialog to handle
     * the tutorial text
     */

    public IEnumerator shuffleDecks()
    {
        yield return new WaitForSeconds(3);
        Game.GamePlayers[0].CmdShuffleDecks();
    }

    public IEnumerator waitThreeSeconds(Dialog dialog)
    {
        if (dialog.textBoxCounter == 24)
        {
            yield break;
        }
        yield return new WaitForSeconds(2);
        dialog.textBoxCounter++;
        if (dialog.textBoxCounter == 3)
        {
            dialog.directions.gameObject.SetActive(false);
            // tutorialArrow.SetActive(false);
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
                "Drag the artifact card over your foe and let 'em have it!";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 8)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "Artifacts can be used as many times as you want per turn!\n\n" +
                "Whenever an artifact is used, the loaded potion will be trashed!";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 12)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            tutorialArrow.SetActive(false);
            tutorialArrow2.SetActive(false);
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
            tutorialArrow.SetActive(false);
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
                "the potions are dropped into the bottom of your deck!";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 23)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "Excellent! Now let's buy more cards from the market.\n\n" +
                "Buy some more potions from the market and then end your\nturn!";
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
                "6 Pips, allowing you to buy more powerful and expensive\n" +
                "items!";
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
                "Try cycling a card from your holster! Drag a card from\n" +
                "your holster to your deck!";
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
            dialog.textInfo = "Try using your character's upgraded action!\n\n" +
                "Click on your character card and click ACTION to use it!";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 36)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "Some characters have unique items that can be obtained\n" +
                "by using the character's flipped action!\n\n" +
                "Try getting them all with each specific character!";
            dialog.ActivateText(dialog.dialogBox);
        }
    }

    IEnumerator waitThreeSecondsHand(int damage, string cardQuality = "")
    {
        if (tutorialArrow.activeInHierarchy)
        {
            tutorialArrow.SetActive(false);
        }
        if (tutorialArrow2.activeInHierarchy)
        {
            tutorialArrow2.SetActive(false);
        }

        throwingHand.SetActive(true);
        throwingHand.GetComponent<Animator>().SetTrigger("Throw");

        // hardcoded logic for two players
        if (numPlayers == 2)
        {
            Debug.Log("Middle person");
            yield return new WaitForSeconds(0.9f);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_ThrowPotion");
            throwingHand.transform.DOMoveX(1000f, 1f);
            yield return new WaitForSeconds(1f);
            throwingHand.SetActive(false);
            tempPlayer.subHealth(damage, cardQuality);
            
            yield break;
        }

        if (tempPlayer.user_id == 2)
        {
            Debug.Log("Middle person");
            yield return new WaitForSeconds(1f);
            throwingHand.transform.DOMoveX(1000f, 1f);
            yield return new WaitForSeconds(1f);
            throwingHand.SetActive(false);
        }
        else if (tempPlayer.user_id == 1)
        {
            Debug.Log("Left person");
            yield return new WaitForSeconds(1f);
            throwingHand.transform.DOMoveX(470f, 1f);
            yield return new WaitForSeconds(1f);
            throwingHand.SetActive(false);
        }
        else if (tempPlayer.user_id == 3)
        {
            Debug.Log("Right person");
            yield return new WaitForSeconds(1f);
            throwingHand.transform.DOMoveX(1470f, 1f);
            yield return new WaitForSeconds(1f);
            throwingHand.SetActive(false);
        }

        if (Game.tutorial)
        {
            tempPlayer.subHealth(damage, cardQuality);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_ThrowPotion");
            StartCoroutine(waitThreeSeconds(dialog));
        }
        else
        {
            tempPlayer.subHealth(damage, cardQuality);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_ThrowPotion");
        }
    }

    // THROW POTION REQUEST
    public void throwPotion()
    {
        if (snakeBonus)
        {
            Debug.Log("SNAKE!!!");
            // pull the opponent holster UI up here
            opponentHolsterMenu.SetActive(true);
            displayOpponentHolster();
            return;
        }

        string cardQuality = "None";
        if (!players[myPlayerIndex].nicklesAction)
            cardQuality = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardQuality;

        // TUTORIAL LOGIC
        if (Game.tutorial)
        {
            if ((dialog.textBoxCounter != 2 && dialog.textBoxCounter != 7 && dialog.textBoxCounter != 17
                && dialog.textBoxCounter != 18 && dialog.textBoxCounter != 19) && dialog.textBoxCounter < 39)
            {
                sendErrorMessage(19);
                playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                return;
            }
            if (playerHolster.cardList[selectedCardInt - 1].card.cardType == "Potion")
            {
                Debug.Log("Tutorial throw");

                if (dialog.textBoxCounter == 17 || dialog.textBoxCounter == 18)
                {
                    sendErrorMessage(19);
                    playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                }
                int damage = playerHolster.cardList[selectedCardInt - 1].card.effectAmount;
                damage = cardPlayer.checkBonus(damage, playerHolster.cardList[selectedCardInt - 1]);
                sendSuccessMessage(2); // Only display on thrower's client.
                GameObject obj = Instantiate(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject,
                        playerHolster.cardList[selectedCardInt - 1].gameObject.transform.position,
                        playerHolster.cardList[selectedCardInt - 1].gameObject.transform.rotation,
                        playerHolster.cardList[selectedCardInt - 1].gameObject.transform);

                StartCoroutine(MoveToTrash(obj));

                playerHolster.cardList[selectedCardInt - 1].updateCard(bolo.deck.placeholder);
                td.addCard(playerHolster.cardList[selectedCardInt - 1]);
                // playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();

                // THROWING ANIMATION
                StartCoroutine(waitThreeSecondsHand(damage));

                // PASSING THIS LOGIC TO COROUTINE
                /*
                bolo.subHealth(damage);
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_ThrowPotion");
                StartCoroutine(waitThreeSeconds(dialog));
                */
            }
            else if (playerHolster.cardList[selectedCardInt - 1].card.cardType == "Artifact")
            {
                if (playerHolster.cardList[selectedCardInt - 1].aPotion.card.cardName != "placeholder")
                {
                    int damage = playerHolster.cardList[selectedCardInt - 1].card.effectAmount;
                    damage = cardPlayer.checkArtifactBonus(damage, playerHolster.cardList[selectedCardInt - 1]);

                    // Update response to account for trashing loaded artifact's potion and not the artifact
                    GameObject obj = Instantiate(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).gameObject,
                        playerHolster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).position,
                        playerHolster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).rotation,
                        playerHolster.cardList[selectedCardInt - 1].artifactSlot.transform);

                    StartCoroutine(MoveToTrash(obj));
                    td.addCard(playerHolster.cardList[selectedCardInt - 1].aPotion);
                    // playerHolster.cardList[selectedCardInt - 1].artifactSlot.transform.parent.gameObject.SetActive(false);

                    // THROWING ANIMATION
                    StartCoroutine(waitThreeSecondsHand(damage));
                    //tempPlayer.subHealth(damage);
                    sendSuccessMessage(3);
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_ThrowArtifact");
                    // StartCoroutine(waitThreeSeconds(dialog));
                    // playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                    // MATTEO: Add Artifact using SFX here.

                }
                else
                {
                    // "Can't use an unloaded Artifact!"
                    sendErrorMessage(1);
                }
            }
            else if (playerHolster.cardList[selectedCardInt - 1].card.cardType == "Vessel")
            {
                if (playerHolster.cardList[selectedCardInt - 1].vPotion1.card.cardName != "placeholder" &&
                            playerHolster.cardList[selectedCardInt - 1].vPotion2.card.cardName != "placeholder")
                {
                    //int damage = players[throwerIndex].holster.card1.vPotion1.card.effectAmount + players[throwerIndex].holster.card1.vPotion2.card.effectAmount;
                    int damage = playerHolster.cardList[selectedCardInt - 1].vPotion1.card.effectAmount + playerHolster.cardList[selectedCardInt - 1].vPotion2.card.effectAmount;
                    //damage = players[throwerIndex].checkBonus(damage, selectedCardInt);
                    GameObject obj = Instantiate(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot1.transform.GetChild(0).gameObject,
                        playerHolster.cardList[selectedCardInt - 1].vesselSlot1.transform.GetChild(0).position,
                        playerHolster.cardList[selectedCardInt - 1].vesselSlot1.transform.GetChild(0).rotation,
                        playerHolster.cardList[selectedCardInt - 1].vesselSlot1.transform);

                    StartCoroutine(MoveToTrash(obj));

                    GameObject obj2 = Instantiate(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot2.transform.GetChild(0).gameObject,
                        playerHolster.cardList[selectedCardInt - 1].vesselSlot2.transform.GetChild(0).position,
                        playerHolster.cardList[selectedCardInt - 1].vesselSlot2.transform.GetChild(0).rotation,
                        playerHolster.cardList[selectedCardInt - 1].vesselSlot2.transform);

                    StartCoroutine(MoveToTrash(obj2));

                    playerDeck.putCardOnBottom(playerHolster.cardList[selectedCardInt - 1].vPotion1.card);
                    playerDeck.putCardOnBottom(playerHolster.cardList[selectedCardInt - 1].vPotion2.card);
                    playerHolster.card1.vPotion1.updateCard(playerHolster.cardList[selectedCardInt - 1].placeholder);
                    playerHolster.card1.vPotion2.updateCard(playerHolster.cardList[selectedCardInt - 1].placeholder);
                    td.addCard(playerHolster.cardList[selectedCardInt - 1]);
                    // playerHolster.cardList[selectedCardInt - 1].vesselSlot1.transform.parent.gameObject.SetActive(false);
                    // playerHolster.cardList[selectedCardInt - 1].vesselSlot2.transform.parent.gameObject.SetActive(false);
                    // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                    // bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, false, true);
                    // THROWING ANIMATION
                    StartCoroutine(waitThreeSecondsHand(damage));
                    //tempPlayer.subHealth(damage);
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_ThrowPotion");
                    sendSuccessMessage(4);
                    // StartCoroutine(waitThreeSeconds(dialog));
                    // playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();

                    // MATTEO: Add Vessel throw SFX here.

                }
                else
                {
                    // "Can't throw an unloaded Vessel!"
                    //Debug.Log("Vessel Error");
                    Debug.Log("Did this happen?");
                    sendErrorMessage(2);
                }
            }
            return;
        }

        if (nicklesDamage > 0 && !Game.multiplayer)
        {
            foreach (CardPlayer cp in players)
            {
                if (cp.name == selectedOpponentName)
                {
                    Debug.Log("Nickles damage action damaged for: " + nicklesDamage);
                    players[myPlayerIndex].subPips(nicklesDamage);
                    cp.subHealth(nicklesDamage);
                    nicklesDamage = 0;
                    sendSuccessMessage(21);
                    // add a notification here??? up to you future denzill

                    return;
                }
            }
            return;
        }


        /*
        foreach (CardPlayer cp in players)
        {
            if (cp.gameObject.activeInHierarchy)
            {
                if (cp.name == selectedOpponentName)
                {
                    Debug.Log("Name matched");
                    tempPlayer = cp;
                    Debug.Log("Enemy's charName: " + tempPlayer.charName);
                }
            }
        }
        */

        // if you're saltimbocca and you're flipped
        if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].character.character.flipped && !Game.multiplayer)
        {
            int damage = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.buyPrice;

            foreach (CardPlayer cp in players)
            {
                if (cp.name == selectedOpponentName)
                {
                    cp.subHealth(damage);
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_ThrowPotion");
                    td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
                    // maybe add a notif idk
                    sendSuccessMessage(2);
                    return;
                }
            }
        }

        if (Game.multiplayer)
        {

            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == currentPlayerName)
                {
                    Debug.Log("Starting Mirror CmdThrowCard");
                    // do the Mirror Command
                    gp.CmdThrowCard(currentPlayerName, tempPlayer.name, selectedCardInt);
                }
            }
        }
        // This client is the current player.
        else
        {
            // If this client isn't the current player, display error message.
            if (players[myPlayerIndex].user_id != myPlayerIndex)
            {
                // "You are not the currentPlayer!"
                sendErrorMessage(7);
                return;
            }
            int damage = 0;
            Debug.Log("GameManager Throw Potion");

            if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType == "Potion")
            {
                Debug.Log("POTION");
                if (players[myPlayerIndex].isTwins)
                {
                    if (!players[myPlayerIndex].character.character.flipped)
                    {
                        players[myPlayerIndex].addHealth(1);
                    }
                    else
                    {
                        players[myPlayerIndex].addHealth(2);
                    }
                }
                damage = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.effectAmount;
                Debug.Log("Original damage: " + damage);
                damage = players[myPlayerIndex].checkRingBonus(damage, players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
                damage = players[myPlayerIndex].checkBonus(damage, players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
                Debug.Log("Damage after thrower bonuses: " + damage);
                damage = tempPlayer.checkDefensiveBonus(damage, selectedCardInt);
                Debug.Log("Damage after defensive bonuses: " + damage);

                sendSuccessMessage(2); // Only display on thrower's client.
                players[myPlayerIndex].potionsThrown++;
                //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].placeholder);

                foreach (CardDisplay cd in players[myPlayerIndex].holster.cardList)
                {
                    if (cd.card.cardName == "Extra Inventory")
                    {
                        Debug.Log("Extra Inventory!");
                        players[myPlayerIndex].deck.putCardOnBottom(cd.card);
                        cd.updateCard(players[myPlayerIndex].holster.cardList[0].placeholder);
                        break;
                    }
                }
                GameObject obj = Instantiate(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform.position,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform.rotation,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform);

                StartCoroutine(MoveToTrash(obj));

                if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardName != "placeholder")
                {
                    td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
                }

                //td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);

                if (players[myPlayerIndex].blackRainBonus)
                {
                    put4CardsInHolster();
                    players[myPlayerIndex].blackRainBonus = false;
                }

                // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();

                if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
                {
                    players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                }
                else if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>() != null)
                {
                    players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>().resetCard();
                }

                // may need to add this back in
                if (myPlayerIndex == 0)
                    StartCoroutine(waitThreeSecondsHand(damage, cardQuality));
                else
                    tempPlayer.subHealth(damage, cardQuality);
                /*
                tempPlayer.subHealth(damage, cardQuality);
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_ThrowPotion");
                */

            }
            else if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType == "Artifact")
            {
                if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].aPotion.card.cardName != "placeholder")
                {
                    damage = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.effectAmount;
                    damage = players[myPlayerIndex].checkRingBonus(damage, players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
                    damage = players[myPlayerIndex].checkArtifactBonus(damage, players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
                    damage = tempPlayer.checkDefensiveBonus(damage, selectedCardInt);

                    // Update response to account for trashing loaded artifact's potion and not the artifact
                    // if the artifact being used is different from the last one they used
                    if (players[myPlayerIndex].lastArtifactUsed != players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardName)
                    {
                        players[myPlayerIndex].uniqueArtifactsUsed++;
                    }
                    players[myPlayerIndex].artifactsUsed++;
                    players[myPlayerIndex].lastArtifactUsed = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardName;

                    // code that animates the loaded cards
                    GameObject obj = Instantiate(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).gameObject,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).position,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).rotation,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform);
                    // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).gameObject.SetActive(false);

                    StartCoroutine(MoveToTrash(obj));
                    td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].aPotion);

                    // MATTEO: taking this out for now, may want to add back in
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_ThrowArtifact");
                    if (myPlayerIndex == 0)
                        StartCoroutine(waitThreeSecondsHand(damage, cardQuality));
                    else
                        tempPlayer.subHealth(damage, cardQuality);
                    // tempPlayer.subHealth(damage, cardQuality);

                    // ISADORE LOGIC
                    // change this logic to check for unique artifacts
                    if (players[myPlayerIndex].isIsadore && players[myPlayerIndex].uniqueArtifactsUsed == 2)
                    {
                        players[myPlayerIndex].character.canBeFlipped = true;
                        // add success message for "You can now flip your card!" or something
                        sendSuccessMessage(13);

                    }
                    else
                    {
                        sendSuccessMessage(3);
                    }

                    // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                    if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
                    {
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                    }
                    else if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>() != null)
                    {
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>().resetCard();
                    }
                    // MATTEO: Add Artifact using SFX here.

                }
                else
                {
                    // "Can't use an unloaded Artifact!"
                    sendErrorMessage(1);
                }

            }
            else if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType == "Vessel")
            {
                if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vPotion1.card.cardName != "placeholder" &&
                            players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vPotion2.card.cardName != "placeholder")
                {
                    if (players[myPlayerIndex].isTwins && players[myPlayerIndex].character.character.flipped)
                    {
                        players[myPlayerIndex].addHealth(4);
                    }
                    //int damage = players[throwerIndex].holster.card1.vPotion1.card.effectAmount + players[throwerIndex].holster.card1.vPotion2.card.effectAmount;
                    damage = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vPotion1.card.effectAmount + players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vPotion2.card.effectAmount;
                    damage = players[myPlayerIndex].checkRingBonus(damage, players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
                    damage = players[myPlayerIndex].checkVesselBonus(damage, players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
                    damage = tempPlayer.checkDefensiveBonus(damage, selectedCardInt);

                    players[myPlayerIndex].vesselsThrown++;

                    // TODO: fix bonus damage
                    //damage = players[throwerIndex].checkBonus(damage, selectedCardInt);
                    players[myPlayerIndex].deck.putCardOnBottom(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vPotion1.card);
                    players[myPlayerIndex].deck.putCardOnBottom(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vPotion2.card);
                    // code that animates the loaded cards
                    GameObject obj = Instantiate(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot1.transform.GetChild(0).gameObject,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot1.transform.GetChild(0).position,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot1.transform.GetChild(0).rotation,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot1.transform);
                    StartCoroutine(MoveToDeck(obj));
                    GameObject obj2 = Instantiate(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot2.transform.GetChild(0).gameObject,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot2.transform.GetChild(0).position,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot2.transform.GetChild(0).rotation,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot2.transform);
                    StartCoroutine(MoveToDeck(obj2));

                    players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vPotion1.updateCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].placeholder);
                    players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vPotion2.updateCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].placeholder);

                    GameObject obj3 = Instantiate(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform.position,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform.rotation,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform);

                    StartCoroutine(MoveToTrash(obj3));
                    td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
                    players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot1.transform.GetChild(0).gameObject.SetActive(false);
                    players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot2.transform.GetChild(0).gameObject.SetActive(false);
                    // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                    // bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, false, true);
                    if (myPlayerIndex == 0)
                        StartCoroutine(waitThreeSecondsHand(damage, cardQuality));
                    else
                        tempPlayer.subHealth(damage, cardQuality);
                    // MATTEO: taking this out for now, may want to add back in
                    // FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_ThrowPotion");
                    sendSuccessMessage(4);
                    // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                    if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
                    {
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                    }
                    else if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>() != null)
                    {
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>().resetCard();
                    }

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

    public IEnumerator MoveToTrash(GameObject obj)
    {
        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).gameObject.SetActive(false);
        obj.transform.DOJump(new Vector2(1850f, 400f), 400f, 1, 1f, false);
        obj.transform.DOScale(0.2f, 1f);
        obj.transform.DORotate(new Vector3(0, 0, 720f), 1f, RotateMode.FastBeyond360);
        yield return new WaitForSeconds(1f);

        Destroy(obj);
        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).gameObject.SetActive(true);
        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.SetActive(false);
        td.transform.parent.DOMove(new Vector2(td.transform.parent.position.x, td.transform.parent.position.y - 5), 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public IEnumerator MoveToDeck(GameObject obj)
    {
        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).gameObject.SetActive(false);
        // obj.transform.DOJump(new Vector2(1850f, 400f), 400f, 1, 1f, false);
        // jump into deck
        obj.transform.DOJump(new Vector2(1600f, 250f), 400f, 1, 1f, false);
        obj.transform.DOScale(0.2f, 1f);
        obj.transform.DORotate(new Vector3(0, 0, 720f), 1f, RotateMode.FastBeyond360);
        yield return new WaitForSeconds(0.85f);
        // set sibling index to be behind deck
        obj.transform.SetParent(transform.root);
        obj.transform.SetSiblingIndex(20);
        yield return new WaitForSeconds(0.15f);
        Destroy(obj);
        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.gameObject.SetActive(true);
        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.parent.gameObject.SetActive(false);
        players[myPlayerIndex].deck.transform.DOMove(new Vector2(players[myPlayerIndex].deck.transform.position.x,
            players[myPlayerIndex].deck.transform.position.y - 5), 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public void popAllMarketCards()
    {
        td.addCard(md1.cardDisplay1);
        Card card1 = md1.popCard();
        md1.cardDisplay1.updateCard(card1);

        td.addCard(md1.cardDisplay2);
        Card card2 = md1.popCard();
        md1.cardDisplay2.updateCard(card2);

        td.addCard(md1.cardDisplay3);
        Card card3 = md1.popCard();
        md1.cardDisplay3.updateCard(card3);

        td.addCard(md2.cardDisplay1);
        Card card4 = md2.popCard();
        md2.cardDisplay1.updateCard(card4);

        td.addCard(md2.cardDisplay2);
        Card card5 = md2.popCard();
        md2.cardDisplay2.updateCard(card5);

        td.addCard(md2.cardDisplay3);
        Card card6 = md2.popCard();
        md2.cardDisplay3.updateCard(card6);

    }

    public void trashMarket(int marketCard)
    {
        if (Game.multiplayer)
        {
            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == currentPlayerName)
                {
                    Debug.Log("Starting Mirror CmdTrashMarketCard");
                    // do the Mirror Command
                    gp.CmdTrashMarketCard(gp.playerName, marketCard);
                }
            }
            return;
        }

        switch (marketCard)
        {
            case 1:
                td.addCard(md1.cardDisplay1);
                Card card1 = md1.popCard();
                md1.cardDisplay1.updateCard(card1);
                sendSuccessMessage(9);
                break;
            case 2:
                td.addCard(md1.cardDisplay2);
                Card card2 = md1.popCard();
                md1.cardDisplay2.updateCard(card2);
                sendSuccessMessage(9);
                break;
            case 3:
                td.addCard(md1.cardDisplay3);
                Card card3 = md1.popCard();
                md1.cardDisplay3.updateCard(card3);
                sendSuccessMessage(9);
                break;
            case 4:
                td.addCard(md2.cardDisplay1);
                Card card4 = md2.popCard();
                md2.cardDisplay1.updateCard(card4);
                sendSuccessMessage(9);
                break;
            case 5:
                td.addCard(md2.cardDisplay2);
                Card card5 = md2.popCard();
                md2.cardDisplay2.updateCard(card5);
                sendSuccessMessage(9);
                break;
            case 6:
                td.addCard(md2.cardDisplay3);
                Card card6 = md2.popCard();
                md2.cardDisplay3.updateCard(card6);
                sendSuccessMessage(9);
                break;
            default:
                break;
        }

        numTrashed--;

        if (numTrashed > 0)
        {
            trashMarketUI.SetActive(true);
            updateTrashMarketMenu();
        }
        else
        {
            trashMarketUI.SetActive(false);
        }

    }

    public void takeMarket(int marketCard)
    {
        if (Game.multiplayer)
        {
            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == currentPlayerName)
                {
                    Debug.Log("Starting Mirror CmdTakeMarketCard");
                    // do the Mirror Command
                    gp.CmdTakeMarketCard(gp.playerName, marketCard);
                }
            }
            return;
        }

        switch (marketCard)
        {
            case 1:
                players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay1.card);
                Card card1 = md1.popCard();
                md1.cardDisplay1.updateCard(card1);
                sendSuccessMessage(17);
                break;
            case 2:
                players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay2.card);
                Card card2 = md1.popCard();
                md1.cardDisplay2.updateCard(card2);
                sendSuccessMessage(17);
                break;
            case 3:
                players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay3.card);
                Card card3 = md1.popCard();
                md1.cardDisplay3.updateCard(card3);
                sendSuccessMessage(17);
                break;
            case 4:
                players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay1.card);
                Card card4 = md2.popCard();
                md2.cardDisplay1.updateCard(card4);
                sendSuccessMessage(17);
                break;
            case 5:
                players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay2.card);
                Card card5 = md2.popCard();
                md2.cardDisplay2.updateCard(card5);
                sendSuccessMessage(17);
                break;
            case 6:
                players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay3.card);
                Card card6 = md2.popCard();
                md2.cardDisplay3.updateCard(card6);
                sendSuccessMessage(17);
                break;
            default:
                break;
        }

        takeMarketMenu.SetActive(false);

    }

    public void addStarterPotion()
    {
        if (Game.multiplayer)
        {
            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (currentPlayerName == gp.playerName)
                {
                    Debug.Log("Starting Mirror CmdAddStarterPotion");
                    // do the Mirror Command
                    gp.CmdAddStarterPotion(currentPlayerName);
                }
            }
            return;
        }
        foreach (CardPlayer cp in players)
        {
            if (cp.name == currentPlayerName)
            {
                cp.deck.putCardOnTop(starterPotionCard);
            }
        }
        //players[myPlayerIndex].deck.putCardOnTop(starterPotionCard);
    }

    public void setStarterPotion()
    {
        starterPotion = true;
    }

    public void preLoadPotion()
    {
        int cards = 0;

        // commenting this out to test something, make sure to add it back in
        // /*
        if (Game.multiplayer)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].name == currentPlayerName)
                {
                    myPlayerIndex = i;
                }
            }
        }
        // */

        foreach (CardDisplay cd in players[myPlayerIndex].holster.cardList)
        {
            if (cd.card.cardType == "Artifact" || cd.card.cardType == "Vessel")
            {
                cards++;
            }
        }

        if (cards == 0)
        {
            sendErrorMessage(20);
            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().cardMenu.gameObject.SetActive(false);
            return;
        }

        if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType != "Potion")
        {
            Debug.Log("Not a potion, ERROR");
            sendErrorMessage(17);
            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().cardMenu.gameObject.SetActive(false);

        }
        else
        {
            // OLD LOGIC

            /*
            loadMenu.SetActive(true);
            displayPotions();
            */

            Debug.Log(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CardDisplay>().card.cardName +
                " is being loaded into " + players[myPlayerIndex].holster.cardList[loadedCardInt].gameObject.GetComponent<CardDisplay>().card.cardName);
            loadPotion();
        }
    }

    public void preThrowPotion()
    {

        if (Game.multiplayer)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].name == currentPlayerName)
                {
                    Debug.Log("myPlayerIndex changed???");
                    myPlayerIndex = i;
                }
            }
        }

        if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType != "Potion" &&
            players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType != "Artifact" &&
            players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType != "Vessel")
        {
            Debug.Log("Throwing ring, ERROR");
            sendErrorMessage(16);
            if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
            {
                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().cardMenu.gameObject.SetActive(false);
            }

        }
        else
        {
            chooseOpponentMenu.SetActive(true);
            displayOpponents();
        }
    }

    // LOAD REQUEST (DONE - 2 clients)
    public void loadPotion()
    {
        Debug.Log("Load Potion");



        // TUTORIAL LOGIC
        if (Game.tutorial)
        {
            if ((dialog.textBoxCounter != 5 && dialog.textBoxCounter != 17 && dialog.textBoxCounter != 18) && dialog.textBoxCounter < 39)
            {
                sendErrorMessage(12);
                // playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                return;
            }
            if (playerHolster.cardList[selectedCardInt - 1].card.cardType == "Potion")
            {
                // Loading a Vessel:
                if (playerHolster.cardList[loadedCardInt].card.cardType == "Vessel")
                {
                    if (dialog.textBoxCounter == 5)
                    {
                        sendErrorMessage(19);
                        // playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        return;
                    }

                    // Enable Vessel menu if it wasn't already enabled.
                    Debug.Log("Vessel menu enabled.");
                    playerHolster.cardList[loadedCardInt].vesselSlot1.transform.parent.gameObject.SetActive(true);
                    playerHolster.cardList[loadedCardInt].vesselSlot1.transform.gameObject.SetActive(true);
                    playerHolster.cardList[loadedCardInt].vesselSlot2.transform.gameObject.SetActive(true);

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
                            // playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();

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
                        // playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
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
                    if (dialog.textBoxCounter == 17 || dialog.textBoxCounter == 18)
                    {
                        sendErrorMessage(19);
                        // playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        return;
                    }
                    // Enable Artifact menu if it wasn't already enabled.
                    Debug.Log("Artifact menu enabled.");
                    playerHolster.cardList[loadedCardInt].artifactSlot.transform.parent.gameObject.SetActive(true);
                    playerHolster.cardList[loadedCardInt].artifactSlot.transform.gameObject.SetActive(true);

                    // Check for existing loaded potion if Artifact menu was already enabled.
                    if (playerHolster.cardList[loadedCardInt].aPotion.card.cardName != "placeholder")
                    {
                        Debug.Log("Artifact is fully loaded!");
                        // DONE: Insert error that displays on screen.
                        sendErrorMessage(8);
                        // playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
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
                        // playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
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

        if (Game.multiplayer)
        {
            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (currentPlayerName == gp.playerName)
                {
                    Debug.Log("Starting Mirror CmdLoadCard");
                    // do the Mirror Command
                    gp.CmdLoadCard(currentPlayerName, selectedCardInt, loadedCardInt);
                }
            }
        }
        // It is this player's turn.
        else
        {
            // If this client isn't the current player, display error message.
            if (players[myPlayerIndex].user_id != myPlayerIndex)
            {
                // "You are not the currentPlayer!"
                sendErrorMessage(7);
                return;
            }

            // Isadore logic
            if (starterPotion && !usedStarterPotion)
            {
                // Loading a Vessel:
                if (players[myPlayerIndex].holster.cardList[loadedCardInt].card.cardType == "Vessel")
                {
                    // TODO: Make another error message
                    Debug.Log("This error message???");
                    sendErrorMessage(12);
                    if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
                    {
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                    }
                    else
                    {
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>().resetCard();
                    }
                }
                else if (players[myPlayerIndex].holster.cardList[loadedCardInt].card.cardType == "Artifact")
                {
                    // Enable Artifact menu if it wasn't already enabled.
                    Debug.Log("Artifact menu enabled.");
                    players[myPlayerIndex].holster.cardList[loadedCardInt].artifactSlot.transform.parent.gameObject.SetActive(true);
                    players[myPlayerIndex].holster.cardList[loadedCardInt].artifactSlot.transform.gameObject.SetActive(true);

                    // Check for existing loaded potion if Artifact menu was already enabled.
                    if (players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.card.cardName != "placeholder")
                    {
                        Debug.Log("Artifact is fully loaded!");
                        // DONE: Insert error that displays on screen.
                        sendErrorMessage(8);
                        if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
                        {
                            players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        }
                        else
                        {
                            players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>().resetCard();
                        }
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

                        if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
                        {
                            players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        }
                        else
                        {
                            players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>().resetCard();
                        }

                        // MATTEO: Add Loading potion SFX here.

                        // // Updates Holster card to be empty.
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card = placeholder;
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(placeholder);
                    }
                }
                starterPotion = false;
                return;
            }
            else
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
                        players[myPlayerIndex].holster.cardList[loadedCardInt].vesselSlot1.transform.gameObject.SetActive(true);
                        players[myPlayerIndex].holster.cardList[loadedCardInt].vesselSlot2.transform.gameObject.SetActive(true);

                        // Check for existing loaded potion(s) if Vessel menu was already enabled.
                        if (players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion1.card.cardName != "placeholder")
                        {
                            // If Vessel slot 2 is filled.
                            if (players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion2.card.cardName != "placeholder")
                            {
                                Debug.Log("Vessel is fully loaded!");
                                // DONE: Insert error that displays on screen.
                                sendErrorMessage(9);
                                if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
                                {
                                    players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                                }
                                else
                                {
                                    players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>().resetCard();
                                }
                            }
                            else
                            {
                                // Fill Vessel slot 2 with loaded potion.
                                Card placeholder = players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion2.card;
                                players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion2.card = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card;
                                players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion2.updateCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card);

                                // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                                sendSuccessMessage(5);
                                // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                                if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
                                {
                                    players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                                }
                                else if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>() != null)
                                {
                                    players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>().resetCard();
                                }
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
                            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                            if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
                            {
                                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                            }
                            else if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>() != null)
                            {
                                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>().resetCard();
                            }
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
                        players[myPlayerIndex].holster.cardList[loadedCardInt].artifactSlot.transform.gameObject.SetActive(true);

                        // Check for existing loaded potion if Artifact menu was already enabled.
                        if (players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.card.cardName != "placeholder")
                        {
                            Debug.Log("Artifact is fully loaded!");
                            // DONE: Insert error that displays on screen.
                            sendErrorMessage(8);
                            // playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                            if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
                            {
                                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                            }
                            else if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>() != null)
                            {
                                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>().resetCard();
                            }
                        }
                        // Artifact slot is unloaded.
                        else
                        {
                            Card placeholder = players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.card;
                            players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.card = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card;
                            players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.updateCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card);
                            // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                            sendSuccessMessage(5);
                            if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
                            {
                                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                            }
                            else if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>() != null)
                            {
                                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>().resetCard();
                            }
                            Debug.Log("Potion loaded in Artifact slot!");

                            // MATTEO: Add Loading potion SFX here.

                            // // Updates Holster card to be empty.
                            players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card = placeholder;
                            players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(placeholder);
                        }
                    }
                }
                else
                {
                    // add error message
                    Debug.Log("That error message...");
                    sendErrorMessage(12);
                    if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
                    {
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                    }
                    else
                    {
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>().resetCard();
                    }
                }
            }
        }
    }

    // CYCLE REQUEST (DONE - 2 clients)
    public void cycleCard()
    {
        Debug.Log("Cycle Card");

        // TUTORIAL LOGIC
        if (Game.tutorial)
        {
            if (dialog.textBoxCounter != 30 && dialog.textBoxCounter < 39)
            {
                sendErrorMessage(19);
                //playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                return;
            }
            if (playerHolster.cardList[selectedCardInt - 1].card.cardType == "Potion")
            {
                // wasn't working, taking it out for now
                Debug.Log("Animation triggered");
                GameObject obj = Instantiate(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform.position,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform.rotation,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform);

                StartCoroutine(MoveToDeck(obj));

                cardPlayer.deck.putCardOnBottom(playerHolster.cardList[selectedCardInt - 1].card);
                playerHolster.cardList[selectedCardInt - 1].updateCard(playerHolster.card1.placeholder);
                sendSuccessMessage(7);
                //playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                StartCoroutine(waitThreeSeconds(dialog));
            }
            else if (cardPlayer.pips < 1)
            {
                sendErrorMessage(5);
                //playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
            }
            else if (playerHolster.cardList[selectedCardInt - 1].card.cardType == "Artifact" ||
                    playerHolster.cardList[selectedCardInt - 1].card.cardType == "Vessel" ||
                    playerHolster.cardList[selectedCardInt - 1].card.cardType == "Ring")
            {
                cardPlayer.subPips(1);
                cardPlayer.deck.putCardOnBottom(playerHolster.cardList[selectedCardInt - 1].card);
                playerHolster.cardList[selectedCardInt - 1].updateCard(playerHolster.card1.placeholder);
                sendSuccessMessage(7);
                //playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                StartCoroutine(waitThreeSeconds(dialog));
            }
            return;
        }

        if (Game.multiplayer)
        {
            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (currentPlayerName == gp.playerName)
                {
                    Debug.Log("Starting Mirror CmdCycleCard");
                    // do the Mirror Command
                    gp.CmdCycleCard(currentPlayerName, selectedCardInt);
                }
            }
        }
        // It is this player's turn.
        else
        {
            // If this client isn't the current player, display error message.
            if (players[myPlayerIndex].user_id != myPlayerIndex)
            {
                // "You are not the currentPlayer!"
                sendErrorMessage(7);
                // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
                {
                    //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                }
                else
                {
                    //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>().resetCard();
                }
                return;
            }

            // Cycling a Potion (costs 0 pips to do)
            if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType == "Potion")
            {
                // wasn't working, taking it out for now
                
                Debug.Log("Animation triggered");
                GameObject obj = Instantiate(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform.position,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform.rotation,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform);

                StartCoroutine(MoveToDeck(obj));
                

                players[myPlayerIndex].deck.putCardOnBottom(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card);
                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(players[0].holster.card1.placeholder);
                // bool connected = networkManager.sendCycleRequest(selectedCardInt, 0);
                sendSuccessMessage(7);
                // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
                {
                    //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                }
                else
                {
                    //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>().resetCard();
                }
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
                players[myPlayerIndex].subPips(1);
                GameObject obj = Instantiate(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform.position,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform.rotation,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform);

                StartCoroutine(MoveToDeck(obj));
                players[myPlayerIndex].deck.putCardOnBottom(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card);
                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(players[0].holster.card1.placeholder);

                /*
                if(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType == "Artifact")
                {
                    GameObject obj2 = Instantiate(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).gameObject,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).position,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).rotation,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform);
                    // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).gameObject.SetActive(false);

                    StartCoroutine(MoveToDeck(obj2));
                }
                */

                sendSuccessMessage(7);

                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.parent.gameObject.SetActive(false);
                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot1.transform.parent.gameObject.SetActive(false);
                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot2.transform.parent.gameObject.SetActive(false);

                // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
                {
                    //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                }
                else
                {
                    //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>().resetCard();
                }
                // MATTEO: Add Cycle SFX here.
            }
            else
            {
                Debug.Log("ERROR: Card needs to be of type Potion, Artifact, Vessel, Ring");
            }
        }
    }

    // TOP MARKET REQUEST
    // subtract pips, update deck display and market display
    public void topMarketBuy()
    {
        Debug.Log("Top Market Buy");

        // TUTORIAL LOGIC
        if (Game.tutorial)
        {
            if ((dialog.textBoxCounter != 10 && dialog.textBoxCounter != 11 &&
                dialog.textBoxCounter != 24) && dialog.textBoxCounter < 38)
            {
                Debug.Log("You weren't supposed to do that, add UI for tutorial error");
                sendErrorMessage(19);
                return;
            }
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
                        //md1.cardDisplay1.gameObject.GetComponent<Market_Hover>().resetCard();
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
                        sendSuccessMessage(1);
                        // bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay1.card.buyPrice, 1);
                    }
                    else
                    {
                        sendErrorMessage(6);
                        //md1.cardDisplay1.gameObject.GetComponent<Market_Hover>().resetCard();
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
                        //md1.cardDisplay2.gameObject.GetComponent<Market_Hover>().resetCard();
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
                        sendSuccessMessage(1);
                        // bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay2.card.buyPrice, 1);
                    }
                    else
                    {
                        sendErrorMessage(6);
                        //md1.cardDisplay2.gameObject.GetComponent<Market_Hover>().resetCard();
                    }
                    break;
                case 3:
                    if (cardPlayer.pips >= md1.cardDisplay3.card.buyPrice)
                    {
                        cardPlayer.subPips(md1.cardDisplay3.card.buyPrice);
                        playerDeck.putCardOnTop(md1.cardDisplay3.card);
                        Card card = md1.popCard();
                        md1.cardDisplay3.updateCard(card);
                        StartCoroutine(waitThreeSeconds(dialog));
                        // playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        //md1.cardDisplay3.gameObject.GetComponent<Market_Hover>().resetCard();
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
                        sendSuccessMessage(1);
                        // bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay3.card.buyPrice, 1);
                    }
                    else
                    {
                        sendErrorMessage(6);
                        //md1.cardDisplay3.gameObject.GetComponent<Market_Hover>().resetCard();
                    }
                    break;
                default:
                    Debug.Log("MarketDeck Error!");
                    break;
            }
            return;
        }


        // It is this player's turn.
        else
        {
            if (Game.multiplayer)
            {
                foreach (GamePlayer gp in Game.GamePlayers)
                {
                    // if the steam usernames match
                    if (gp.playerName == currentPlayerName)
                    {
                        Debug.Log("Starting Mirror CmdBuyTopCard");
                        // do the Mirror Command
                        gp.CmdBuyTopCard(gp.playerName, md1.cardInt);
                    }
                }
                return;
            }
            // If this client isn't the current player, display error message.
            if (players[myPlayerIndex].user_id != myPlayerIndex || (players[myPlayerIndex].isScarpetta && players[myPlayerIndex].character.character.flipped))
            {
                // "You are not the currentPlayer!"
                sendErrorMessage(7);
                return;
            }
            // cardInt based on position of card in Top Market (position 1, 2, or 3)
            switch (md1.cardInt)
            {
                case 1:
                    if (players[myPlayerIndex].pips >= md1.cardDisplay1.card.buyPrice && !players[myPlayerIndex].isSaltimbocca)
                    {
                        // All rings cost 4 logic
                        if (md1.cardDisplay1.card.cardType == "Ring" && players[myPlayerIndex].doubleRingBonus)
                        {
                            md1.cardDisplay1.card.buyPrice = 4;
                        }

                        // if The Early Bird Special was drawn this turn
                        if (md1.cardDisplay1.card.cardName == "EarlyBirdSpecial" && earlyBirdSpecial)
                        {
                            // it buys for 3 pips
                            md1.cardDisplay1.card.buyPrice = 3;
                        }
                        players[myPlayerIndex].subPips(md1.cardDisplay1.card.buyPrice);
                        players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay1.card);
                        Card card = md1.popCard();
                        md1.cardDisplay1.updateCard(card);
                        //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        //md1.cardDisplay1.gameObject.GetComponent<Market_Hover>().resetCard();
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
                        sendSuccessMessage(1);
                        // bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay1.card.buyPrice, 1);
                        // SALTIMBOCCA LOGIC
                    }
                    else if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].pips >= (md1.cardDisplay1.card.buyPrice - 1))
                    {
                        // All rings cost 4 logic
                        if (md1.cardDisplay1.card.cardType == "Ring" && players[myPlayerIndex].doubleRingBonus)
                        {
                            md1.cardDisplay1.card.buyPrice = 4;
                        }

                        // if The Early Bird Special was drawn this turn
                        if (md1.cardDisplay1.card.cardName == "EarlyBirdSpecial" && earlyBirdSpecial)
                        {
                            // it buys for 3 pips
                            md1.cardDisplay1.card.buyPrice = 3;
                        }

                        if (md1.cardDisplay1.card.buyPrice == 1)
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
                        //md1.cardDisplay1.gameObject.GetComponent<Market_Hover>().resetCard();
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
                        sendSuccessMessage(1);
                    }
                    else
                    {
                        sendErrorMessage(6);
                        //md1.cardDisplay1.gameObject.GetComponent<Market_Hover>().resetCard();
                    }
                    break;
                case 2:
                    if (players[myPlayerIndex].pips >= md1.cardDisplay2.card.buyPrice && !players[myPlayerIndex].isSaltimbocca)
                    {
                        // All rings cost 4 logic
                        if (md1.cardDisplay2.card.cardType == "Ring" && players[myPlayerIndex].doubleRingBonus)
                        {
                            md1.cardDisplay2.card.buyPrice = 4;
                        }

                        // if The Early Bird Special was drawn this turn
                        if (md1.cardDisplay2.card.cardName == "EarlyBirdSpecial" && earlyBirdSpecial)
                        {
                            // it buys for 3 pips
                            md1.cardDisplay2.card.buyPrice = 3;
                        }
                        players[myPlayerIndex].subPips(md1.cardDisplay2.card.buyPrice);
                        players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay2.card);
                        Card card = md1.popCard();
                        md1.cardDisplay2.updateCard(card);
                        //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        //md1.cardDisplay2.gameObject.GetComponent<Market_Hover>().resetCard();
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
                        sendSuccessMessage(1);
                        // bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay2.card.buyPrice, 1);
                    }
                    else if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].pips >= (md1.cardDisplay2.card.buyPrice - 1))
                    {
                        // All rings cost 4 logic
                        if (md1.cardDisplay2.card.cardType == "Ring" && players[myPlayerIndex].doubleRingBonus)
                        {
                            md1.cardDisplay2.card.buyPrice = 4;
                        }

                        // if The Early Bird Special was drawn this turn
                        if (md1.cardDisplay2.card.cardName == "EarlyBirdSpecial" && earlyBirdSpecial)
                        {
                            // it buys for 3 pips
                            md1.cardDisplay2.card.buyPrice = 3;
                        }

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
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
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
                        // All rings cost 4 logic
                        if (md1.cardDisplay3.card.cardType == "Ring" && players[myPlayerIndex].doubleRingBonus)
                        {
                            md1.cardDisplay3.card.buyPrice = 4;
                        }

                        // if The Early Bird Special was drawn this turn
                        if (md1.cardDisplay3.card.cardName == "EarlyBirdSpecial" && earlyBirdSpecial)
                        {
                            // it buys for 3 pips
                            md1.cardDisplay3.card.buyPrice = 3;
                        }
                        players[myPlayerIndex].subPips(md1.cardDisplay3.card.buyPrice);
                        players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay3.card);
                        Card card = md1.popCard();
                        md1.cardDisplay3.updateCard(card);
                        //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
                        sendSuccessMessage(1);
                        // bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay3.card.buyPrice, 1);
                    }
                    else if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].pips >= (md1.cardDisplay3.card.buyPrice - 1))
                    {
                        // All rings cost 4 logic
                        if (md1.cardDisplay3.card.cardType == "Ring" && players[myPlayerIndex].doubleRingBonus)
                        {
                            md1.cardDisplay3.card.buyPrice = 4;
                        }

                        // if The Early Bird Special was drawn this turn
                        if (md1.cardDisplay3.card.cardName == "EarlyBirdSpecial" && earlyBirdSpecial)
                        {
                            // it buys for 3 pips
                            md1.cardDisplay3.card.buyPrice = 3;
                        }

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
                        // md1.cardDisplay3.gameObject.GetComponent<Market_Hover>().resetCard();
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
                        sendSuccessMessage(1);
                    }
                    else
                    {
                        sendErrorMessage(6);
                        //md1.cardDisplay3.gameObject.GetComponent<Market_Hover>().resetCard();
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

        if (Game.tutorial)
        {
            if (dialog.textBoxCounter < 38)
            {
                Debug.Log("You weren't supposed to do that, add UI for tutorial error");
                sendErrorMessage(19);
                return;
            }
        }

        if (Game.multiplayer)
        {
            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == currentPlayerName)
                {
                    Debug.Log("Starting Mirror CmdBuyBottomCard");
                    // do the Mirror Command
                    gp.CmdBuyBottomCard(gp.playerName, md2.cardInt);
                }
            }
            return;
        }

        // It is this player's turn.
        else
        {
            // If this client isn't the current player, display error message.
            if (players[myPlayerIndex].user_id != myPlayerIndex || players[myPlayerIndex].isScarpetta)
            {
                // "You are not the currentPlayer!"
                sendErrorMessage(7);
                return;
            }

            switch (md2.cardInt)
            {
                // cardInt based on position of card in Top Market (position 1, 2, or 3)
                case 1:
                    if (players[myPlayerIndex].pips >= md2.cardDisplay1.card.buyPrice && !players[myPlayerIndex].isSaltimbocca)
                    {
                        // All rings cost 4 logic
                        if (md2.cardDisplay1.card.cardType == "Ring" && players[myPlayerIndex].doubleRingBonus)
                        {
                            md2.cardDisplay1.card.buyPrice = 4;
                        }
                        // if The Early Bird Special was drawn this turn
                        if (md2.cardDisplay1.card.cardName == "EarlyBirdSpecial" && earlyBirdSpecial)
                        {
                            // it buys for 3 pips
                            md2.cardDisplay1.card.buyPrice = 3;
                        }
                        players[myPlayerIndex].subPips(md2.cardDisplay1.card.buyPrice);
                        players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay1.card);
                        Card card = md2.popCard();
                        md2.cardDisplay1.updateCard(card);
                        //md2.cardDisplay1.gameObject.GetComponent<Market_Hover>().resetCard();
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
                        sendSuccessMessage(1);
                        // bool connected = networkManager.sendBuyRequest(md2.cardInt, md2.cardDisplay1.card.buyPrice, 0);
                    }
                    else if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].pips >= (md2.cardDisplay1.card.buyPrice - 1))
                    {
                        // All rings cost 4 logic
                        if (md2.cardDisplay1.card.cardType == "Ring" && players[myPlayerIndex].doubleRingBonus)
                        {
                            md2.cardDisplay1.card.buyPrice = 4;
                        }

                        // if The Early Bird Special was drawn this turn
                        if (md2.cardDisplay1.card.cardName == "EarlyBirdSpecial" && earlyBirdSpecial)
                        {
                            // it buys for 3 pips
                            md2.cardDisplay1.card.buyPrice = 3;
                        }

                        if (md2.cardDisplay1.card.buyPrice - 1 == 0)
                        {
                            players[myPlayerIndex].subPips(md2.cardDisplay1.card.buyPrice);
                        }
                        else
                        {
                            players[myPlayerIndex].subPips(md2.cardDisplay1.card.buyPrice - 1);
                        }
                        players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay1.card);
                        Card card = md2.popCard();
                        md2.cardDisplay1.updateCard(card);
                        //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        //md2.cardDisplay1.gameObject.GetComponent<Market_Hover>().resetCard();
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
                        sendSuccessMessage(1);
                    }
                    else
                    {
                        sendErrorMessage(6);
                        //md2.cardDisplay1.gameObject.GetComponent<Market_Hover>().resetCard();
                    }
                    break;
                case 2:
                    if (players[myPlayerIndex].pips >= md2.cardDisplay2.card.buyPrice && !players[myPlayerIndex].isSaltimbocca)
                    {
                        // All rings cost 4 logic
                        if (md2.cardDisplay2.card.cardType == "Ring" && players[myPlayerIndex].doubleRingBonus)
                        {
                            md2.cardDisplay2.card.buyPrice = 4;
                        }

                        // if The Early Bird Special was drawn this turn
                        if (md2.cardDisplay2.card.cardName == "EarlyBirdSpecial" && earlyBirdSpecial)
                        {
                            // it buys for 3 pips
                            md2.cardDisplay2.card.buyPrice = 3;
                        }
                        players[myPlayerIndex].subPips(md2.cardDisplay2.card.buyPrice);
                        players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay2.card);
                        Card card = md2.popCard();
                        md2.cardDisplay2.updateCard(card);
                        //md2.cardDisplay2.gameObject.GetComponent<Market_Hover>().resetCard();
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
                        sendSuccessMessage(1);
                        // bool connected = networkManager.sendBuyRequest(md2.cardInt, md2.cardDisplay2.card.buyPrice, 0);
                    }
                    else if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].pips >= (md2.cardDisplay2.card.buyPrice - 1))
                    {
                        // All rings cost 4 logic
                        if (md2.cardDisplay2.card.cardType == "Ring" && players[myPlayerIndex].doubleRingBonus)
                        {
                            md2.cardDisplay2.card.buyPrice = 4;
                        }

                        // if The Early Bird Special was drawn this turn
                        if (md2.cardDisplay2.card.cardName == "EarlyBirdSpecial" && earlyBirdSpecial)
                        {
                            // it buys for 3 pips
                            md2.cardDisplay2.card.buyPrice = 3;
                        }

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
                        //md2.cardDisplay2.gameObject.GetComponent<Market_Hover>().resetCard();
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
                        sendSuccessMessage(1);
                    }
                    else
                    {
                        sendErrorMessage(6);
                        //md2.cardDisplay2.gameObject.GetComponent<Market_Hover>().resetCard();
                    }
                    break;
                case 3:
                    if (players[myPlayerIndex].pips >= md2.cardDisplay3.card.buyPrice && !players[myPlayerIndex].isSaltimbocca)
                    {
                        // All rings cost 4 logic
                        if (md2.cardDisplay3.card.cardType == "Ring" && players[myPlayerIndex].doubleRingBonus)
                        {
                            md2.cardDisplay3.card.buyPrice = 4;
                        }
                        // if The Early Bird Special was drawn this turn
                        if (md2.cardDisplay3.card.cardName == "EarlyBirdSpecial" && earlyBirdSpecial)
                        {
                            // it buys for 3 pips
                            md2.cardDisplay3.card.buyPrice = 3;
                        }
                        players[myPlayerIndex].subPips(md2.cardDisplay3.card.buyPrice);
                        players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay3.card);
                        Card card = md2.popCard();
                        md2.cardDisplay3.updateCard(card);
                        //md2.cardDisplay3.gameObject.GetComponent<Market_Hover>().resetCard();
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
                        sendSuccessMessage(1);
                        // bool connected = networkManager.sendBuyRequest(md2.cardInt, md2.cardDisplay3.card.buyPrice, 0);
                    }
                    else if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].pips >= (md2.cardDisplay3.card.buyPrice - 1))
                    {
                        // All rings cost 4 logic
                        if (md2.cardDisplay3.card.cardType == "Ring" && players[myPlayerIndex].doubleRingBonus)
                        {
                            md2.cardDisplay3.card.buyPrice = 4;
                        }

                        // if The Early Bird Special was drawn this turn
                        if (md2.cardDisplay3.card.cardName == "EarlyBirdSpecial" && earlyBirdSpecial)
                        {
                            // it buys for 3 pips
                            md2.cardDisplay3.card.buyPrice = 3;
                        }

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
                        //md2.cardDisplay3.gameObject.GetComponent<Market_Hover>().resetCard();
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
                        sendSuccessMessage(1);
                    }
                    else
                    {
                        sendErrorMessage(6);
                        //md2.cardDisplay3.gameObject.GetComponent<Market_Hover>().resetCard();
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
        if (Game.multiplayer)
        {
            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == currentPlayerName)
                {
                    Debug.Log("Starting Mirror CmdCheckCardAction");
                    // do the Mirror Command
                    gp.CmdCheckCardAction(gp.playerName, selectedCardInt);
                }
            }
            return;
        }

        // Bottle Rocket UI Logic
        if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardName == "BottleRocket")
        {
            // SetActive the UI
            bottleRocketMenu.SetActive(true);
        }
    }

    public void setShieldBonus()
    {
        if (Game.multiplayer)
        {
            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == tempPlayer.name)
                {
                    Debug.Log("Starting Mirror CmdShieldBonus");
                    // do the Mirror Command
                    gp.CmdShieldBonus(gp.playerName, opponentCardInt);
                }
            }
            return;
        }

        foreach (CardPlayer cp in players)
        {
            if (cp.name == tempPlayer.name)
            {
                td.addCard(cp.holster.cardList[opponentCardInt - 1].aPotion);
            }
        }
    }

    public void setBubbleWandBonus()
    {
        if (Game.multiplayer)
        {
            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == tempPlayer.name)
                {
                    Debug.Log("Starting Mirror CmdBubbleWandBonus");
                    // do the Mirror Command
                    gp.CmdBubbleWandBonus(gp.playerName, opponentCardInt);
                }
            }
            return;
        }

        foreach (CardPlayer cp in players)
        {
            if (cp.name == tempPlayer.name)
            {
                td.addCard(cp.holster.cardList[opponentCardInt - 1].aPotion);
                // correct this logic in a little bit
            }
        }
    }

    public void setBottleRocketBonus()
    {
        if (Game.multiplayer)
        {
            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == currentPlayerName)
                {
                    Debug.Log("Starting Mirror CmdBottleRocketBonus");
                    // do the Mirror Command
                    gp.CmdBottleRocketBonus(gp.playerName, selectedCardInt);
                }
            }
            return;
        }

        if (players[myPlayerIndex].pips >= 3 || players[myPlayerIndex].bottleRocketBonus)
        {
            players[myPlayerIndex].bottleRocketBonus = true;
            // add some success message but change what you initially put here lol
            sendSuccessMessage(14);
            // reset card
            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
            if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
            {
                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
            }
            else
            {
                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>().resetCard();
            }
        }
        else
        {
            sendErrorMessage(6);
            // reset card
            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
            if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
            {
                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
            }
            else
            {
                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>().resetCard();
            }
        }
    }

    public void deal3ToAll()
    {
        foreach (CardPlayer cp in players)
        {
            if (cp.name != currentPlayerName)
            {
                cp.subHealth(3);
            }
        }
    }

    public void deal1ToAll()
    {
        foreach (CardPlayer cp in players)
        {
            if (cp.name != currentPlayerName)
            {
                cp.subHealth(1);
            }
        }
    }

    public void dealDamageToAll(int damage)
    {
        foreach (CardPlayer cp in players)
        {
            if (cp.name != currentPlayerName)
            {
                cp.subHealth(damage);
            }
        }
    }

    public void sellCardCommand()
    {
        if (Game.tutorial)
        {
            sellCard();
        }
        else
        {
            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == players[myPlayerIndex].name)
                {
                    Debug.Log("Starting Mirror CmdSellCard");
                    // do the Mirror Command
                    gp.CmdSellCard(gp.playerName, selectedCardInt);
                }
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
            if (dialog.textBoxCounter != 28 && dialog.textBoxCounter < 39)
            {
                sendErrorMessage(19);
                // playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                return;
            }
            cardPlayer.addPips(playerHolster.cardList[selectedCardInt - 1].card.sellPrice);
            td.addCard(playerHolster.cardList[selectedCardInt - 1]);
            // playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
            StartCoroutine(waitThreeSeconds(dialog));
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
            sendSuccessMessage(8);
            return;
        }
        // It is this player's turn.
        else
        {
            if (Game.multiplayer)
            {
                foreach (GamePlayer gp in Game.GamePlayers)
                {
                    // if the steam usernames match
                    if (gp.playerName == currentPlayerName)
                    {
                        Debug.Log("Starting Mirror CmdSellCard");
                        // do the Mirror Command
                        gp.CmdSellCard(gp.playerName, selectedCardInt);
                    }
                }
                return;
            }
            // If this client isn't the current player, display error message.
            if (players[myPlayerIndex].user_id != myPlayerIndex)
            {
                // "You are not the currentPlayer!"
                sendErrorMessage(7);
                return;
            }
            // LOGIC FOR BOLO SELLING ABILITY
            // Bolo not flipped and he's selling something that's not a potion
            if (players[myPlayerIndex].isBolo && !players[myPlayerIndex].character.character.flipped && players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType != "Potion")
            {
                players[myPlayerIndex].addPips(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.sellPrice + 1);
                // Bolo flipped selling anything
            }
            else if (players[myPlayerIndex].isBolo && players[myPlayerIndex].character.character.flipped)
            {
                players[myPlayerIndex].addPips(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.sellPrice + 1);
            }
            else
            {
                // everyone else
                players[myPlayerIndex].addPips(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.sellPrice);
            }
            td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);

            // bool connected = networkManager.sendSellRequest(selectedCardInt, players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.sellPrice);
            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
            if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
            {
                //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
            }
            else
            {
                //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>().resetCard();
            }
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
            sendSuccessMessage(8);
            // MATTEO: Add sell SFX here.
        }
    }

    // TRASH REQUEST
    public void trashCard()
    {
        Debug.Log("Trash Card");

        if (Game.tutorial)
        {
            if (dialog.textBoxCounter != 22 && dialog.textBoxCounter < 39)
            {
                sendErrorMessage(19);
                playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                return;
            }
            // add animation here
            GameObject obj = Instantiate(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject,
                        playerHolster.cardList[selectedCardInt - 1].gameObject.transform.position,
                        playerHolster.cardList[selectedCardInt - 1].gameObject.transform.rotation,
                        playerHolster.cardList[selectedCardInt - 1].gameObject.transform);

            StartCoroutine(MoveToTrash(obj));
            td.addCard(playerHolster.cardList[selectedCardInt - 1]);
            StartCoroutine(waitThreeSeconds(dialog));
            // playerHolster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
            sendSuccessMessage(9);
            return;
        }

        if (Game.multiplayer)
        {
            foreach (GamePlayer gp in Game.GamePlayers)
            {
                // if the steam usernames match
                if (gp.playerName == currentPlayerName)
                {
                    Debug.Log("Starting Mirror CmdTrashCard");
                    // do the Mirror Command
                    gp.CmdTrashCard(currentPlayerName, selectedCardInt);
                }
            }
        }
        // It is this player's turn.
        else
        {
            // If this client isn't the current player, display error message.
            if (players[myPlayerIndex].user_id != myPlayerIndex)
            {
                // "You are not the currentPlayer!"
                sendErrorMessage(7);
                return;
            }

            // Savory Layer Cake logic
            if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardName == "SavoryLayerCake")
            {
                // Heals for +3 HP if trashed
                players[myPlayerIndex].addHealth(3);
            }
            if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardQuality != "Starter")
            {
                players[myPlayerIndex].cardsTrashed++;
            }
            Debug.Log("Card Trashed");
            GameObject obj = Instantiate(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform.position,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform.rotation,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform);

            StartCoroutine(MoveToTrash(obj));
            td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);

            if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].cardsTrashed == 4)
            {
                sendSuccessMessage(15);
                players[myPlayerIndex].character.canBeFlipped = true;
            }
            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
            if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>() != null)
            {
                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
            }
            else if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>() != null)
            {
                players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<CPUHoverCard>().resetCard();
            }
            sendSuccessMessage(9);
        }
    }

    public void trashCardInDeck(CardDisplay cd)
    {
        deckMenuScroll.addCardToTrash(cd);
    }

    public void sendMessage(string message)
    {
        GameObject textbox = successMessages[21];
        textbox.SetActive(true);
        textbox.GetComponent<TMPro.TextMeshProUGUI>().text = message;
        StartCoroutine(waitThreeSeconds(textbox));
    }

    public void sendSuccessMessage(int notif)
    {
        foreach (GameObject ob in successMessages)
        {
            ob.SetActive(false);
        }

        foreach (GameObject ob in errorMessages)
        {
            ob.SetActive(false);
        }

        GameObject message = successMessages[notif - 1];
        if (notif == 18)
        {
            Debug.Log("Player name: " + players[myPlayerIndex].name);
            string thing = players[myPlayerIndex].name + "'s Turn!";
            message.GetComponent<TMPro.TextMeshProUGUI>().text = thing;
        }
        message.SetActive(true);
        StartCoroutine(waitThreeSeconds(message));
    }

    public void sendSuccessMessage(int notif, string name)
    {
        foreach (GameObject ob in successMessages)
        {
            ob.SetActive(false);
        }

        foreach (GameObject ob in errorMessages)
        {
            ob.SetActive(false);
        }

        GameObject message = successMessages[notif - 1];
        if (notif == 18)
        {
            Debug.Log("Player name: " + name);
            string thing = name + "'s Turn!";
            message.GetComponent<TMPro.TextMeshProUGUI>().text = thing;
        }
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
        yield return new WaitForSeconds(2);
        gameObj.SetActive(false);
    }

}

public enum Gamestate
{
    PlayerTurn,
    OpponentTurn,
    Win,
    Lose
}
