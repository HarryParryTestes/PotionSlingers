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
    public CardDatabase database;
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
    public Image background;
    public Sprite[] backgrounds;

    public List<Card> starterCards;

    public GameObject gameOverScreen;
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
    public GameObject nicklesAttackMenu;
    public GameObject advanceStageUI;
    public DeckMenuScroll deckDisplay;
    public ExpUI expUI;

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

    public float widthRatio;
    public float heightRatio;

    public SaveData saveData;

    public List<string> playersDeck = new List<string>();
    public List<string> playersHolster = new List<string>();

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
        // Debug.Log(Screen.width);
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
                pauseUI.transform.GetChild(0).Find("Card Menu").gameObject.SetActive(false);
                pauseUI.SetActive(false);
            }

        }

        widthRatio = Screen.width / 1920f;
        heightRatio = Screen.height / 1080f;

    }

    public IEnumerator cardDisappear()
    {
        yield return new WaitForSeconds(1f);
        if (!marketSelected)
        {
            md1.cardDisplay1.gameObject.SetActive(false);
            md1.cardDisplay2.gameObject.SetActive(false);
            md1.cardDisplay3.gameObject.SetActive(false);
            md2.cardDisplay1.gameObject.SetActive(false);
            md2.cardDisplay2.gameObject.SetActive(false);
            md2.cardDisplay3.gameObject.SetActive(false);
        }
    }

    public void unblockRaycasts()
    {
        md1.cardDisplay1.GetComponent<CanvasGroup>().blocksRaycasts = true;
        md1.cardDisplay2.GetComponent<CanvasGroup>().blocksRaycasts = true;
        md1.cardDisplay3.GetComponent<CanvasGroup>().blocksRaycasts = true;
        md2.cardDisplay1.GetComponent<CanvasGroup>().blocksRaycasts = true;
        md2.cardDisplay2.GetComponent<CanvasGroup>().blocksRaycasts = true;
        md2.cardDisplay3.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void moveMarket()
    {
        StopCoroutine(cardDisappear());

        if (Game.tutorial && dialog.textBoxCounter < 9)
            return;

        if (!marketSelected)
        {
            marketSelected = true;
            /*
            md1.cardDisplay1.GetComponent<CanvasGroup>().blocksRaycasts = true;
            md1.cardDisplay2.GetComponent<CanvasGroup>().blocksRaycasts = true;
            md1.cardDisplay3.GetComponent<CanvasGroup>().blocksRaycasts = true;
            md2.cardDisplay1.GetComponent<CanvasGroup>().blocksRaycasts = true;
            md2.cardDisplay2.GetComponent<CanvasGroup>().blocksRaycasts = true;
            md2.cardDisplay3.GetComponent<CanvasGroup>().blocksRaycasts = true;
            */

            md1.cardDisplay1.gameObject.SetActive(true);
            md1.cardDisplay2.gameObject.SetActive(true);
            md1.cardDisplay3.gameObject.SetActive(true);
            md2.cardDisplay1.gameObject.SetActive(true);
            md2.cardDisplay2.gameObject.SetActive(true);
            md2.cardDisplay3.gameObject.SetActive(true);
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Market_open");
            if (dialog.textBoxCounter == 10)
            {
                tutorialArrow.GetComponent<ArrowMover>().checkArrow();
                tutorialArrow.SetActive(true);
                tutorialArrow2.GetComponent<SecondArrowMover>().checkArrow();
                tutorialArrow2.SetActive(true);
            }
            // marketButton.SetActive(false);
            canvasGroup.blocksRaycasts = false;
            // marketPosition = marketButton.transform.position;
            // marketButton.transform.parent.DOMove(new Vector3(0, 0, 0), 1f);
            Invoke("unblockRaycasts", 0.9f);

            // market.transform.DOMove(new Vector3(1010, 300, 0), 1f);
            // marketButton.transform.DOMove(new Vector3(960, 300, 0), 1f);
            market.transform.DOMoveY(636f * heightRatio, 1f);
            // marketButton.transform.DOMoveY(300f, 1f);
            Debug.Log("Market moved???");
        }
        else
        {
            // marketPosition = marketButton.transform.position;
            marketSelected = false;
            md1.cardDisplay1.GetComponent<CanvasGroup>().blocksRaycasts = false;
            md1.cardDisplay2.GetComponent<CanvasGroup>().blocksRaycasts = false;
            md1.cardDisplay3.GetComponent<CanvasGroup>().blocksRaycasts = false;
            md2.cardDisplay1.GetComponent<CanvasGroup>().blocksRaycasts = false;
            md2.cardDisplay2.GetComponent<CanvasGroup>().blocksRaycasts = false;
            md2.cardDisplay3.GetComponent<CanvasGroup>().blocksRaycasts = false;
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Market_close");
            // canvasGroup.blocksRaycasts = true;
            market.transform.DOMoveY(-11.5f * heightRatio, 1f);
            // marketButton.transform.DOMoveY(-300f, 1f);
            Debug.Log("Market reset???");
            moveMarketCards();
            StartCoroutine(cardDisappear());
        }

    }

    public void moveMarketCards()
    {
        md1.cardDisplay1.GetComponent<DragCard>().marketBack();
        md1.cardDisplay2.GetComponent<DragCard>().marketBack();
        md1.cardDisplay3.GetComponent<DragCard>().marketBack();
        md2.cardDisplay1.GetComponent<DragCard>().marketBack();
        md2.cardDisplay2.GetComponent<DragCard>().marketBack();
        md2.cardDisplay3.GetComponent<DragCard>().marketBack();
        playerHolster.card1.GetComponent<DragCard>().marketBack();
        playerHolster.card1.aPotion.GetComponent<DragCard>().marketBack();
        playerHolster.card1.vPotion1.GetComponent<DragCard>().marketBack();
        playerHolster.card1.vPotion2.GetComponent<DragCard>().marketBack();
        playerHolster.card2.GetComponent<DragCard>().marketBack();
        playerHolster.card2.aPotion.GetComponent<DragCard>().marketBack();
        playerHolster.card2.vPotion1.GetComponent<DragCard>().marketBack();
        playerHolster.card2.vPotion2.GetComponent<DragCard>().marketBack();
        playerHolster.card3.GetComponent<DragCard>().marketBack();
        playerHolster.card3.aPotion.GetComponent<DragCard>().marketBack();
        playerHolster.card3.vPotion1.GetComponent<DragCard>().marketBack();
        playerHolster.card3.vPotion2.GetComponent<DragCard>().marketBack();
        playerHolster.card4.GetComponent<DragCard>().marketBack();
        playerHolster.card4.aPotion.GetComponent<DragCard>().marketBack();
        playerHolster.card4.vPotion1.GetComponent<DragCard>().marketBack();
        playerHolster.card4.vPotion2.GetComponent<DragCard>().marketBack();
    }

    public void advanceStage()
    {
        Debug.Log("Advancing a stage in story mode");
        saveData = SaveSystem.LoadGameData();
        saveGameManagerValues();
        saveData.newStage = true;

        saveData.stage++;
        saveData.savedGame = true;
        // List<string> playersDeck = new List<string>();
        // List<string> playersHolster = new List<string>();

        saveData.visitedEnemies.Add(saveData.currentEnemyName);

        playersHolster.Clear();
        playersDeck.Clear();

        foreach (CardDisplay cd in players[0].holster.cardList)
        {
            playersHolster.Add(cd.card.name);
        }
        foreach (Card card in players[0].deck.deckList)
        {
            playersDeck.Add(card.name);
        }


        Debug.Log("Now onto stage " + saveData.stage + "!");
        SaveSystem.SaveGameData(saveData);
        // Game.ServerChangeScene("StoryMode");
        advanceStageUI.SetActive(true);
        // StartCoroutine(goToStory());
        // SceneManager.LoadScene("StoryMode");
    }

    public void saveGameManagerValues()
    {
        saveData.playerHolster = playersHolster;
        saveData.playerDeck = playersDeck;
        saveData.playerHealth = players[0].hp;
        saveData.playerCubes = players[0].hpCubes;
        saveData.flipped = players[0].character.character.flipped;
        saveData.canBeFlipped = players[0].character.canBeFlipped;
        saveData.transition = false;
        switch (saveData.currentEnemyName)
        {
            case "Bag o' Snakes":
                saveData.opp1Health = players[1].hp;
                saveData.opp1Cubes = players[1].hpCubes;
                break;
            case "Bag o' Snakes+":
                saveData.opp1Health = players[1].hp;
                saveData.opp1Cubes = players[1].hpCubes;
                saveData.opp2Health = players[2].hp;
                saveData.opp2Cubes = players[2].hpCubes;
                break;
            case "Fingas":
                saveData.opp1Health = players[1].hp;
                saveData.opp1Cubes = players[1].hpCubes;
                break;
            case "Fingas+":
                saveData.opp1Health = players[1].hp;
                saveData.opp1Cubes = players[1].hpCubes;
                saveData.opp2Health = players[2].hp;
                saveData.opp2Cubes = players[2].hpCubes;
                break;
            case "Saltimbocca":
                saveData.opp1Health = players[1].hp;
                saveData.opp1Cubes = players[1].hpCubes;
                break;
            case "Singelotte":
                saveData.opp1Health = players[1].hp;
                saveData.opp1Cubes = players[1].hpCubes;
                break;
            case "Crowpunk":
                saveData.opp1Health = players[1].hp;
                saveData.opp1Cubes = players[1].hpCubes;
                break;
            case "Crowpunk+":
                saveData.opp1Health = players[1].hp;
                saveData.opp1Cubes = players[1].hpCubes;
                saveData.opp2Health = players[2].hp;
                saveData.opp2Cubes = players[2].hpCubes;
                saveData.opp3Health = players[3].hp;
                saveData.opp3Cubes = players[3].hpCubes;
                break;
        }
    }

    public void setStageUI()
    {
        advanceStageUI.SetActive(true);
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

        if (!Game.storyMode && !Game.multiplayer && !Game.tutorial)
        {
            if (players[0].dead)
            {
                Debug.Log("Game over, the player died");
                gameOverScreen.SetActive(true);
                StartCoroutine(goBackToTitle());
                return;
            }
        }

        if (Game.storyMode)
        {
            if (players[0].dead)
            {
                Debug.Log("Game over, the player died");
                gameOverScreen.SetActive(true);
                StartCoroutine(goBackToTitle());
                return;
            }
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

        if ((numPlayers == 2 && numAlive == 1 && numDead == 1) || (numPlayers == 3 && numAlive == 1 && numDead == 2) || (numPlayers == 4 && numAlive == 1 && numDead == 3))
        {
            // The game is over!
            Debug.Log("The game is over!!!");

            // make animated win / loss screen here
            // look up how to use DOTween

            if (Game.storyMode)
            {
                // advance a stage and save the game data
                if (stage == 4)
                {
                    Debug.Log("Singelotte was just beaten... This should give the player an achievement!!!!!");
                    unSpicyCards();
                    SteamUserStats.SetAchievement("BEAT_STAGE_1");
                }

                Debug.Log("Advancing a stage in story mode");
                saveData.newStage = true;
                saveData.visitedEnemies.Add(saveData.currentEnemyName);
                saveGameManagerValues();
                saveData.stage = stage + 1;
                saveData.savedGame = true;
                // List<string> playersDeck = new List<string>();
                // List<string> playersHolster = new List<string>();
                playersHolster.Clear();
                playersDeck.Clear();
                foreach (CardDisplay cd in players[0].holster.cardList)
                {
                    playersHolster.Add(cd.card.name);
                }
                foreach (Card card in players[0].deck.deckList)
                {
                    playersDeck.Add(card.name);
                }
                /*
                saveData.playerHolster = playersHolster;
                saveData.playerDeck = playersDeck;
                saveData.playerHealth = players[0].hp;
                saveData.playerCubes = players[0].hpCubes;
                */
                Debug.Log("Now onto stage " + saveData.stage + "!");
                SaveSystem.SaveGameData(saveData);
                // Game.ServerChangeScene("StoryMode");
                // throw some message up on the screen saying you've completed the stage
                // advanceStageUI.SetActive(true);
                Invoke("setStageUI", 3f);
                // StartCoroutine(goToStory());
                // SceneManager.LoadScene("StoryMode");
                return;
            }

            if (players[0].dead)
            {
                Debug.Log("Game over, the player died");
                gameOverScreen.SetActive(true);
                StartCoroutine(goBackToTitle());
                return;
            }
            Game.completedGame = true;
            StartCoroutine(goBackToTitle());
        }
    }

    public IEnumerator goBackToTitle()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("TitleMenu");
    }

    public IEnumerator goToStory()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("StoryMode");
    }

    public void goToMap()
    {
        // yield return new WaitForSeconds(4);
        SceneManager.LoadScene("StoryMode");
    }

    public void ohFuckGoBack()
    {
        Debug.Log("Going back to title menu");
        Game.tutorial = false;

        if (Game.storyMode && saveData != null)
        {
            saveData.savedGame = true;
            saveData.newStage = false;
            // take this out here, saving gamestate info will occur at the end of each turn
            // saveGameManagerValues();
            /*
            List<string> playersDeck = new List<string>();
            List<string> playersHolster = new List<string>();
            foreach (CardDisplay cd in players[0].holster.cardList)
            {
                playersHolster.Add(cd.card.name);
            }
            foreach (Card card in players[0].deck.deckList)
            {
                playersDeck.Add(card.name);
            }
            */
            /*
            saveData.playerHolster = playersHolster;
            saveData.playerDeck = playersDeck;
            saveData.playerHealth = players[0].hp;
            saveData.playerCubes = players[0].hpCubes; 
            */
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

    void setStoryModeCharacters()
    {
        // changing from stage number to currentEnemyName
        switch (saveData.currentEnemyName)
        {
            case "Fingas":
                background.sprite = backgrounds[0];
                // background.gameObject.transform.localScale = new Vector3(1.5f, 1.4553f, 1.4553f);
                numPlayers = 2;
                players[2].gameObject.AddComponent<ComputerPlayer>();
                players[2].charName = "Fingas";
                players[2].name = "Fingas";
                playerTopName.text = players[2].charName;
                players[2].character.onCharacterClick("Fingas");
                players[2].checkCharacter();
                players[2].holster.gameObject.SetActive(false);
                players[2].deck.gameObject.SetActive(false);

                if (saveData.savedGame && !saveData.newStage)
                {
                    players[2].hpCubes = saveData.opp1Cubes;
                    players[2].hp = saveData.opp1Health;
                    players[2].hBar.image.fillAmount = 0;
                }
                else
                {
                    players[2].hpCubes = 1;
                    players[2].hp = 10;
                    saveData.opp1Cubes = 1;
                    saveData.opp1Health = 10;
                }
                players[2].updateHealthUI();
                players[2].user_id = 1;

                players[1] = players[2];
                players[2] = players[3];
                p3.SetActive(false);
                p4.SetActive(false);
                break;
            case "Fingas+":
                numPlayers = 3;
                // Fingas is stage 1 enemy
                players[1].gameObject.AddComponent<ComputerPlayer>();
                players[1].charName = "Fingas";
                players[1].name = "Fingas";
                playerLeftName.text = players[1].charName;
                players[1].character.onCharacterClick("Fingas");
                players[1].checkCharacter();
                players[1].holster.gameObject.SetActive(false);
                players[1].deck.gameObject.SetActive(false);
                players[1].hpCubes = 1;
                players[1].hp = 10;
                if (saveData.savedGame && !saveData.newStage)
                {
                    players[1].hpCubes = saveData.opp1Cubes;
                    players[1].hp = saveData.opp1Health;
                    players[1].hBar.image.fillAmount = 0;
                }
                else
                {
                    players[1].hpCubes = 1;
                    players[1].hp = 10;
                }

                players[1].updateHealthUI();

                // Crow Punk is stage 1 enemy
                players[2].gameObject.AddComponent<ComputerPlayer>();
                players[2].charName = "Fingas";
                players[2].name = "Fingas";
                playerTopName.text = players[2].charName;
                players[2].character.onCharacterClick("Fingas");
                players[2].checkCharacter();
                players[2].holster.gameObject.SetActive(false);
                players[2].deck.gameObject.SetActive(false);
                players[2].hpCubes = 1;
                players[2].hp = 10;
                if (saveData.savedGame && !saveData.newStage)
                {
                    players[2].hpCubes = saveData.opp2Cubes;
                    players[2].hp = saveData.opp2Health;
                    players[2].hBar.image.fillAmount = 0;
                }
                else
                {
                    players[2].hpCubes = 1;
                    players[2].hp = 10;
                }
                players[2].updateHealthUI();
                p4.SetActive(false);
                break;
            case "Bag o' Snakes+":
                numPlayers = 3;
                // Fingas is stage 1 enemy
                players[1].gameObject.AddComponent<ComputerPlayer>();
                players[1].charName = "Bag o' Snakes";
                players[1].name = "Bag o' Snakes";
                playerLeftName.text = players[1].charName;
                players[1].character.onCharacterClick("Bag o' Snakes");
                players[1].checkCharacter();
                players[1].holster.gameObject.SetActive(false);
                players[1].deck.gameObject.SetActive(false);
                players[1].hpCubes = 1;
                players[1].hp = 10;
                if (saveData.savedGame && !saveData.newStage)
                {
                    players[1].hpCubes = saveData.opp1Cubes;
                    players[1].hp = saveData.opp1Health;
                    players[1].hBar.image.fillAmount = 0;
                }
                else
                {
                    players[1].hpCubes = 1;
                    players[1].hp = 10;
                }

                players[1].updateHealthUI();

                // Crow Punk is stage 1 enemy
                players[2].gameObject.AddComponent<ComputerPlayer>();
                players[2].charName = "Bag o' Snakes";
                players[2].name = "Bag o' Snakes";
                playerTopName.text = players[2].charName;
                players[2].character.onCharacterClick("Bag o' Snakes");
                players[2].checkCharacter();
                players[2].holster.gameObject.SetActive(false);
                players[2].deck.gameObject.SetActive(false);
                players[2].hpCubes = 1;
                players[2].hp = 10;
                if (saveData.savedGame && !saveData.newStage)
                {
                    players[2].hpCubes = saveData.opp2Cubes;
                    players[2].hp = saveData.opp2Health;
                    players[2].hBar.image.fillAmount = 0;
                }
                else
                {
                    players[2].hpCubes = 1;
                    players[2].hp = 10;
                }
                players[2].updateHealthUI();
                p4.SetActive(false);
                break;
            case "Singelotte":
                background.sprite = backgrounds[1];
                numPlayers = 2;
                players[2].gameObject.AddComponent<ComputerPlayer>();
                players[2].charName = "Singelotte";
                players[2].name = "Singelotte";
                playerTopName.text = players[2].charName;
                players[2].character.onCharacterClick("Singelotte");
                players[2].checkCharacter();
                players[2].holster.gameObject.SetActive(false);
                players[2].deck.gameObject.SetActive(false);
                if (saveData.savedGame && !saveData.newStage)
                {
                    players[2].hpCubes = saveData.opp1Cubes;
                    players[2].hp = saveData.opp1Health;
                    players[2].hBar.image.fillAmount = 0;
                }
                else
                {
                    players[2].hpCubes = 1;
                    players[2].hp = 33;
                    saveData.opp1Cubes = 1;
                    saveData.opp1Health = 33;
                }


                players[2].updateHealthUI();
                players[2].user_id = 1;

                players[1] = players[2];
                players[2] = players[3];
                p3.SetActive(false);
                p4.SetActive(false);
                break;
            case "Bag o' Snakes":
                background.sprite = backgrounds[0];
                background.gameObject.transform.localScale = new Vector3(1.5f, 1.4553f, 1.4553f);
                numPlayers = 2;
                players[2].gameObject.AddComponent<ComputerPlayer>();
                players[2].charName = "Bag o' Snakes";
                players[2].name = "Bag o' Snakes";
                playerTopName.text = players[2].charName;
                players[2].character.onCharacterClick("Bag o' Snakes");
                players[2].checkCharacter();
                players[2].holster.gameObject.SetActive(false);
                players[2].deck.gameObject.SetActive(false);

                if (saveData.savedGame && !saveData.newStage)
                {
                    players[2].hpCubes = saveData.opp1Cubes;
                    players[2].hp = saveData.opp1Health;
                    players[2].hBar.image.fillAmount = 0;
                }
                else
                {
                    players[2].hpCubes = 1;
                    players[2].hp = 10;
                    saveData.opp1Cubes = 1;
                    saveData.opp1Health = 10;
                }
                players[2].updateHealthUI();
                players[2].user_id = 1;

                players[1] = players[2];
                players[2] = players[3];
                p3.SetActive(false);
                p4.SetActive(false);
                break;
            case "Crowpunk":
                background.sprite = backgrounds[0];
                background.gameObject.transform.localScale = new Vector3(1.5f, 1.4553f, 1.4553f);
                numPlayers = 2;
                players[2].gameObject.AddComponent<ComputerPlayer>();
                players[2].charName = "Crowpunk";
                players[2].name = "Crowpunk";
                playerTopName.text = players[2].charName;
                players[2].character.onCharacterClick("Crowpunk");
                players[2].checkCharacter();
                players[2].holster.gameObject.SetActive(false);
                players[2].deck.gameObject.SetActive(false);

                if (saveData.savedGame && !saveData.newStage)
                {
                    players[2].hpCubes = saveData.opp1Cubes;
                    players[2].hp = saveData.opp1Health;
                    players[2].hBar.image.fillAmount = 0;
                }
                else
                {
                    players[2].hpCubes = 1;
                    players[2].hp = 10;
                    saveData.opp1Cubes = 1;
                    saveData.opp1Health = 10;
                }
                players[2].updateHealthUI();
                players[2].user_id = 1;

                players[1] = players[2];
                players[2] = players[3];
                p3.SetActive(false);
                p4.SetActive(false);
                break;
            case "Saltimbocca":
                numPlayers = 2;
                players[2].gameObject.AddComponent<ComputerPlayer>();
                players[2].charName = "Saltimbocca";
                players[2].name = "Saltimbocca";
                playerTopName.text = players[2].charName;
                players[2].character.onCharacterClick("Saltimbocca");
                players[2].checkCharacter();
                if (saveData.savedGame && !saveData.newStage)
                {
                    players[2].hpCubes = saveData.opp1Cubes;
                    players[2].hp = saveData.opp1Health;
                    players[2].hBar.image.fillAmount = 0;
                }
                else
                {
                    players[2].hpCubes = 1;
                    players[2].hp = 10;
                    saveData.opp1Cubes = 1;
                    saveData.opp1Health = 10;
                }
                players[2].hpCubes = 1;
                players[2].updateHealthUI();
                players[2].user_id = 1;

                players[1] = players[2];
                players[2] = players[3];
                p3.SetActive(false);
                p4.SetActive(false);
                break;
            case "Crowpunk+":
                numPlayers = 4;
                // Fingas is stage 1 enemy
                players[1].gameObject.AddComponent<ComputerPlayer>();
                players[1].charName = "Fingas";
                players[1].name = "Fingas";
                playerLeftName.text = players[1].charName;
                players[1].character.onCharacterClick("Fingas");
                players[1].checkCharacter();
                players[1].holster.gameObject.SetActive(false);
                players[1].deck.gameObject.SetActive(false);
                players[1].hpCubes = 1;
                players[1].hp = 10;
                if (saveData.savedGame && !saveData.newStage)
                {
                    players[1].hpCubes = saveData.opp1Cubes;
                    players[1].hp = saveData.opp1Health;
                    players[1].hBar.image.fillAmount = 0;
                }
                else
                {
                    players[1].hpCubes = 1;
                    players[1].hp = 10;
                }

                players[1].updateHealthUI();

                // Crow Punk is stage 1 enemy
                players[2].gameObject.AddComponent<ComputerPlayer>();
                players[2].charName = "Crowpunk";
                players[2].name = "Crowpunk";
                playerTopName.text = players[2].charName;
                players[2].character.onCharacterClick("Crowpunk");
                players[2].checkCharacter();
                players[2].holster.gameObject.SetActive(false);
                players[2].deck.gameObject.SetActive(false);
                players[2].hpCubes = 1;
                players[2].hp = 10;
                if (saveData.savedGame && !saveData.newStage)
                {
                    players[2].hpCubes = saveData.opp2Cubes;
                    players[2].hp = saveData.opp2Health;
                    players[2].hBar.image.fillAmount = 0;
                }
                else
                {
                    players[2].hpCubes = 1;
                    players[2].hp = 10;
                }
                players[2].updateHealthUI();

                // Fingas is stage 1 enemy
                players[3].gameObject.AddComponent<ComputerPlayer>();
                players[3].charName = "Fingas";
                players[3].name = "Fingas";
                playerRightName.text = players[3].charName;
                players[3].character.onCharacterClick("Fingas");
                players[3].checkCharacter();
                players[3].holster.gameObject.SetActive(false);
                players[3].deck.gameObject.SetActive(false);
                players[3].hpCubes = 1;
                players[3].hp = 10;
                if (saveData.savedGame && !saveData.newStage)
                {
                    players[3].hpCubes = saveData.opp3Cubes;
                    players[3].hp = saveData.opp3Health;
                    players[3].hBar.image.fillAmount = 0;
                }
                else
                {
                    players[3].hpCubes = 1;
                    players[3].hp = 10;
                }
                players[3].updateHealthUI();
                break;
        }
        /*
        if (saveData.stage == 3)
        {
            numPlayers = 4;
            // Fingas is stage 1 enemy
            players[1].gameObject.AddComponent<ComputerPlayer>();
            players[1].charName = "Fingas";
            players[1].name = "Fingas";
            playerLeftName.text = players[1].charName;
            players[1].character.onCharacterClick("Fingas");
            players[1].checkCharacter();
            players[1].hpCubes = 1;
            players[1].hp = 10;
            if (saveData.savedGame && !saveData.newStage)
            {
                players[1].hpCubes = saveData.opp1Cubes;
                players[1].hp = saveData.opp1Health;
                players[1].hBar.image.fillAmount = 0;
            }
            else
            {
                players[1].hpCubes = 1;
                players[1].hp = 10;
            }

            players[1].updateHealthUI();

            // Crow Punk is stage 1 enemy
            players[2].gameObject.AddComponent<ComputerPlayer>();
            players[2].charName = "CrowPunk";
            players[2].name = "CrowPunk";
            playerTopName.text = players[2].charName;
            players[2].character.onCharacterClick("CrowPunk");
            players[2].checkCharacter();
            players[2].hpCubes = 1;
            players[2].hp = 10;
            if (saveData.savedGame && !saveData.newStage)
            {
                players[2].hpCubes = saveData.opp2Cubes;
                players[2].hp = saveData.opp2Health;
                players[2].hBar.image.fillAmount = 0;
            }
            else
            {
                players[2].hpCubes = 1;
                players[2].hp = 10;
            }
            players[2].updateHealthUI();

            // Fingas is stage 1 enemy
            players[3].gameObject.AddComponent<ComputerPlayer>();
            players[3].charName = "Fingas";
            players[3].name = "Fingas";
            playerRightName.text = players[3].charName;
            players[3].character.onCharacterClick("Fingas");
            players[3].checkCharacter();
            players[3].hpCubes = 1;
            players[3].hp = 10;
            if (saveData.savedGame && !saveData.newStage)
            {
                players[3].hpCubes = saveData.opp3Cubes;
                players[3].hp = saveData.opp3Health;
                players[3].hBar.image.fillAmount = 0;
            }
            else
            {
                players[3].hpCubes = 1;
                players[3].hp = 10;
            }
            players[3].updateHealthUI();
        }
        else if (saveData.stage == 1)
        {
            background.sprite = backgrounds[0];
            background.gameObject.transform.localScale = new Vector3(1.5f, 1.4553f, 1.4553f);
            numPlayers = 2;
            players[2].gameObject.AddComponent<ComputerPlayer>();
            players[2].charName = "Bag o' Snakes";
            players[2].name = "Bag o' Snakes";
            playerTopName.text = players[2].charName;
            players[2].character.onCharacterClick("Bag o' Snakes");
            players[2].checkCharacter();

            if (saveData.savedGame)
            {
                players[2].hpCubes = saveData.opp1Cubes;
                players[2].hp = saveData.opp1Health;
                players[2].hBar.image.fillAmount = 0;
            }
            else
            {
                players[2].hpCubes = 1;
                players[2].hp = 10;
                saveData.opp1Cubes = 1;
                saveData.opp1Health = 10;
            }
            players[2].updateHealthUI();
            players[2].user_id = 1;

            players[1] = players[2];
            players[2] = players[3];
            p3.SetActive(false);
            p4.SetActive(false);
        }
        else if (saveData.stage == 2)
        {
            // background.sprite = backgrounds[0];
            // background.gameObject.transform.localScale = new Vector3(1.5f, 1.4553f, 1.4553f);
            numPlayers = 2;
            players[2].gameObject.AddComponent<ComputerPlayer>();
            players[2].charName = "Saltimbocca";
            players[2].name = "Saltimbocca";
            playerTopName.text = players[2].charName;
            players[2].character.onCharacterClick("Saltimbocca");
            players[2].checkCharacter();
            if (saveData.savedGame && !saveData.newStage)
            {
                players[2].hpCubes = saveData.opp1Cubes;
                players[2].hp = saveData.opp1Health;
                players[2].hBar.image.fillAmount = 0;
            }
            else
            {
                players[2].hpCubes = 1;
                players[2].hp = 10;
                saveData.opp1Cubes = 1;
                saveData.opp1Health = 10;
            }
            players[2].hpCubes = 1;
            players[2].updateHealthUI();
            players[2].user_id = 1;

            players[1] = players[2];
            players[2] = players[3];
            p3.SetActive(false);
            p4.SetActive(false);
        }
        else if (saveData.stage == 4)
        {
            background.sprite = backgrounds[1];
            numPlayers = 2;
            players[2].gameObject.AddComponent<ComputerPlayer>();
            players[2].charName = "Singelotte";
            players[2].name = "Singelotte";
            playerTopName.text = players[2].charName;
            players[2].character.onCharacterClick("Singelotte");
            players[2].checkCharacter();
            if (saveData.savedGame && !saveData.newStage)
            {
                players[2].hpCubes = saveData.opp1Cubes;
                players[2].hp = saveData.opp1Health;
                players[2].hBar.image.fillAmount = 0;
            }
            else
            {
                players[2].hpCubes = 1;
                players[2].hp = 33;
                saveData.opp1Cubes = 1;
                saveData.opp1Health = 33;
            }


            players[2].updateHealthUI();
            players[2].user_id = 1;

            players[1] = players[2];
            players[2] = players[3];
            p3.SetActive(false);
            p4.SetActive(false);
        }
        */
    }

    public void unSpicyCards()
    {
        foreach (Card card in database.cardList)
        {
            card.spicy = false;
        }
    }

    void Start()
    {
        Debug.Log("GameManager started!!!");

        // expUI.DisplayText();

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
                stage = saveData.stage;
                Debug.Log("Contents of saved holster:");
                foreach (string s in saveData.playerHolster)
                {
                    Debug.Log(s);
                }
                Debug.Log("Contents of saved deck:");
                foreach (string s in saveData.playerDeck)
                {
                    Debug.Log(s);
                }
                for (int i = 0; i < 4; i++)
                {
                    foreach (Card card in database.cardList)
                    {
                        if (card.name == saveData.playerHolster[i])
                        {
                            players[0].holster.cardList[i].updateCard(card);
                        }
                    }
                }
                players[0].deck.deckList.Clear();
                players[0].deck.cardDisplay.updateCard(players[0].deck.placeholder);


                /*
                for (int i = saveData.playerDeck.Count - 1; i >= 0; i--)
                {
                    players[0].deck.putCardOnTop()
                }

                foreach (Card card in saveData.playerDeck)
                {
                    players[0].deck.putCardOnTop(card);
                }
                */
                for (int i = 0; i < saveData.playerDeck.Count; i++)
                {
                    foreach (Card card in database.cardList)
                    {
                        if (card.name == saveData.playerDeck[i])
                        {
                            players[0].deck.putCardOnBottom(card);
                        }
                    }
                }

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
                players[0].hpCubes = saveData.playerCubes;
                players[0].hp = saveData.playerHealth;
                if (players[0].hBar != null)
                {
                    Debug.Log("Health bar edit");
                    players[0].hBar.image.fillAmount = 0;
                }
                players[0].currentPlayerHighlight.SetActive(true);
                players[0].updateHealthUI();
                currentPlayerName = players[0].name;
                if (saveData.canBeFlipped)
                {
                    players[0].character.canBeFlipped = true;
                }
                if (saveData.flipped)
                {
                    Debug.Log("Character card should be flipped!");
                    players[0].character.character.flipped = true;
                    players[0].character.flipCard();
                }
                // players[0].deck.loadDeck();

                // numPlayers = 4;
                setStoryModeCharacters();
                if (numPlayers == 3)
                {
                    Debug.Log("Three players!!!");
                    // experimenting
                    players[1].gameObject.transform.parent.parent.position = new Vector3(100f, 840f, 0);
                    players[2].gameObject.transform.parent.position = new Vector3(1260f, 870f, 0);

                    if (Screen.width == 2560)
                    {
                        players[1].gameObject.transform.parent.parent.position = new Vector3(150f, 1180f, 0);
                        players[2].gameObject.transform.parent.position = new Vector3(1625f, 1305f, 0);
                    }
                    else if (Screen.width == 3840)
                    {

                    }
                    p4.SetActive(false);
                }
                // onStartTurn(players[0]);
                foreach (CardDisplay cd in players[0].holster.cardList)
                {
                    if (players[0].deck.deckList.Count >= 1)
                    {
                        if (cd.card.name == "placeholder")
                        {
                            cd.updateCard(players[0].deck.popCard());
                        }
                    }
                }
                /*
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
                return;
                */
            }
            else
            {
                // NEW STORY MODE FILE
                Debug.Log("New story mode file started!!!");
                unSpicyCards();
                md1.shuffle();
                md2.shuffle();
                initDecks();

                SaveData newSaveData = new SaveData(Game.storyModeCharName, stage, saveData.currentEnemyName);
                saveData.savedGame = false;
                saveData = newSaveData;
                // initialize saved cards in case of quit out
                saveData.playerDeck = playersDeck;
                saveData.playerHolster = playersHolster;
                SaveSystem.SaveGameData(saveData);

                players[0].name = SteamFriends.GetPersonaName().ToString();
                players[0].charName = Game.storyModeCharName;
                players[0].character.onCharacterClick(players[0].charName);
                players[0].checkCharacter();
                playerBottomName.text = SteamFriends.GetPersonaName().ToString();
                currentPlayerName = players[0].name;

                setStoryModeCharacters();

                // do numPlayers == 3 check here
            }
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
                players[0].currentPlayerHighlight.SetActive(true);
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

            players[0].currentPlayerHighlight.SetActive(true);

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
                // experimenting
                players[1].gameObject.transform.parent.parent.position = new Vector3(100f, 840f, 0);
                players[2].gameObject.transform.parent.position = new Vector3(1260f, 870f, 0);

                if (Screen.width == 2560)
                {
                    players[1].gameObject.transform.parent.parent.position = new Vector3(150f, 1180f, 0);
                    players[2].gameObject.transform.parent.position = new Vector3(1625f, 1305f, 0);
                }
                else if (Screen.width == 3840)
                {

                }
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
            players[0].currentPlayerHighlight.SetActive(true);
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
        tempPlayer.subHealth(2);
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

    public IEnumerator DeckStealAnimation(CardDisplay cd, CardDisplay cardBeingStolen)
    {
        Debug.Log("Card animation starting");
        GameObject obj = Instantiate(cardBeingStolen.gameObject, cardBeingStolen.gameObject.transform.position, cardBeingStolen.gameObject.transform.rotation, players[myPlayerIndex].gameObject.transform);
        if (players[myPlayerIndex].GetComponent<ComputerPlayer>() != null)
            obj.transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
        obj.transform.DOJump(cd.gameObject.transform.position, 200f, 1, 0.5f, false);
        if (cd.GetComponent<DragCard>() != null)
        {
            obj.transform.DORotate(new Vector3(0, 0, 360f) + cd.GetComponent<DragCard>().cardRotation, 0.5f, RotateMode.FastBeyond360);
            obj.transform.DOScale(2.3f, 0.5f);
        }
        // obj.transform.DORotate(cd.GetComponent<DragCard>().cardRotation, 0.5f);
        else
            obj.transform.DORotate(new Vector3(0, 0, 360f), 0.5f, RotateMode.FastBeyond360);
        // obj.transform.DOJump(new Vector2(1850f * widthRatio, 400f * heightRatio), 400f, 1, 1f, false);
        yield return new WaitForSeconds(0.5f);

        // MATTEO: Add holster fill sfx here!

        cd.updateCard(cardBeingStolen.card);
        cardBeingStolen.updateCard(td.card);
        // cd.updateCard(tempPlayer.holster.cardList[selectedCard - 1].card);
        obj.SetActive(false);
        Destroy(obj);
    }

    public IEnumerator DeckAnimation(CardDisplay cd, CardPlayer player)
    {
        Debug.Log("Card animation starting");
        GameObject obj = Instantiate(player.deck.gameObject, player.deck.gameObject.transform.position, player.deck.gameObject.transform.rotation, player.gameObject.transform);
        if (player.GetComponent<ComputerPlayer>() != null)
            obj.transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
        obj.transform.DOJump(cd.gameObject.transform.position, 200f, 1, 0.5f, false);

        // MATTEO: Add holster fill sfx here!

        Card card = player.deck.popCard();
        if (cd.GetComponent<DragCard>() != null)
            obj.transform.DORotate(cd.GetComponent<DragCard>().cardRotation, 0.5f);
        // obj.transform.DOJump(new Vector2(1850f * widthRatio, 400f * heightRatio), 400f, 1, 1f, false);
        yield return new WaitForSeconds(0.5f);

        cd.updateCard(card);
        obj.SetActive(false);
        Destroy(obj);
    }

    public IEnumerator HolsterFill(CardPlayer player)
    {
        // MATTEO: Add holster fill sfx here!

        int times = 0;

        foreach (CardDisplay cd in player.holster.cardList)
        {
            if (cd.card.name == "placeholder")
            {
                times++;
            }
        }

        if (times > 0)
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Draw");

        foreach (CardDisplay cd in player.holster.cardList)
        {
            if (player.deck.deckList.Count >= 1)
            {
                if (cd.card.name == "placeholder")
                {
                    // cd.updateCard(player.deck.popCard());
                    StartCoroutine(DeckAnimation(cd, player));

                    yield return new WaitForSeconds(0.25f);
                }
            }
        }
        yield return new WaitForSeconds(0.3f);
        // check to see if this changes anything!!!
        Debug.Log("Checking player bonuses!");
        player.setDefaultTurn();
    }

    public void saveGameStuff()
    {
        Debug.Log("Save game stuff");
        playersHolster.Clear();
        playersDeck.Clear();

        // CARDS GET PUT INTO HOLSTER FROM DECK IN THIS METHOD
        // SAVE STORY MODE DATA HERE!
        foreach (CardDisplay cd in players[0].holster.cardList)
        {
            playersHolster.Add(cd.card.name);
        }
        foreach (Card card in players[0].deck.deckList)
        {
            playersDeck.Add(card.name);
        }

        // save the values at the end of each turn!
        saveGameManagerValues();
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

        player.currentPlayerHighlight.SetActive(true);

        // This is where to make the animation that deals the cards from the deck to the holster
        if (player.holster.gameObject.activeInHierarchy)
            StartCoroutine(HolsterFill(player));

        Invoke("saveGameStuff", 1f);

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
        // player.setDefaultTurn();
    }

    public void addDebugCard(CardDisplay cd)
    {
        CardDisplay temp = playerHolster.cardList[selectedCardInt - 1];

        Debug.Log("Adding " + cd.card.cardName + " to the holster!");
        if (temp.card.cardType == "Artifact")
        {
            if (temp.aPotion != null && temp.aPotion.card.cardName != "placeholder")
            {
                // deckList.Add(cd.aPotion.card);
                temp.aPotion.updateCard(playerDeck.placeholder);
                temp.aPotion.gameObject.SetActive(false);
            }
        }

        if (temp.card.cardType == "Vessel")
        {
            if (temp.vPotion1 != null && temp.vPotion1.card.cardName != "placeholder")
            {
                // deckList.Add(cd.vPotion1.card);
                temp.vPotion1.updateCard(playerDeck.placeholder);
                temp.vPotion1.gameObject.SetActive(false);
                temp.vPotion2.gameObject.SetActive(false);

            }

            if (temp.vPotion2 != null && temp.vPotion2.card.cardName != "placeholder")
            {
                // deckList.Add(cd.vPotion2.card);
                temp.vPotion2.updateCard(playerDeck.placeholder);
                temp.vPotion1.gameObject.SetActive(false);
                temp.vPotion2.gameObject.SetActive(false);

            }
        }
        temp.updateCard(cd.card);
    }

    public void setSCInt(int num)
    {
        selectedCardInt = num;
    }

    public void setSCInt(CardDisplay cd)
    {
        for (int i = 0; i < 4; i++)
        {
            if (players[myPlayerIndex].holster.cardList[i] == cd)
            {
                selectedCardInt = i + 1;
            }
        }
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

    public void setLoadedInt(CardDisplay cd)
    {
        for (int i = 0; i < 4; i++)
        {
            if (players[myPlayerIndex].holster.cardList[i] == cd)
            {
                loadedCardInt = i;
            }
        }
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
        for (int i = 0; i < numPlayers; i++)
        {
            if (i == 0)
                continue;

            int cardNumber = rng.Next(1, 5);
            selectedCardInt = cardNumber;

            foreach (CardDisplay cd in players[i].holster.cardList)
            {
                if (cd.card.cardName != "placeholder")
                {
                    GameObject obj = Instantiate(cd.gameObject,
                    cd.gameObject.transform.position,
                    cd.gameObject.transform.rotation,
                    cd.gameObject.transform);

                    StartCoroutine(players[i].MoveToTrash(obj));

                    td.addCard(cd);
                    break;
                }
            }
            sendMessage("Trashed a card from each opponent!");
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
        if (nicklesAttackMenu.activeInHierarchy)
        {
            opponentHolsterMenu.SetActive(false);
            return;
        }

        tempPlayer = cardPlayer;
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
        Debug.Log("MyPlayerIndex = " + myPlayerIndex);

        foreach (CardDisplay cd in players[myPlayerIndex].holster.cardList)
        {
            if (cd.card.cardName == "placeholder")
            {
                // use coroutine to delay the card update until after the animation is done
                StartCoroutine(DeckStealAnimation(cd, tempPlayer.holster.cardList[selectedCard - 1]));
                // cd.updateCard(tempPlayer.holster.cardList[selectedCard - 1].card);
                sendSuccessMessage(20);
                break;
            }
        }
        // tempPlayer.holster.cardList[selectedCard - 1].updateCard(td.card);
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

            GameObject obj = Instantiate(tempPlayer.holster.cardList[selectedCard - 1].gameObject,
                        tempPlayer.holster.cardList[selectedCard - 1].gameObject.transform.position,
                        tempPlayer.holster.cardList[selectedCard - 1].gameObject.transform.rotation,
                        playerHolster.cardList[selectedCardInt - 1].gameObject.transform);

            MoveToTrash(obj, tempPlayer.holster.cardList[selectedCard - 1]);
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
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_EndTurn");

        // checkForEndGame();
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

            players[myPlayerIndex].currentPlayerHighlight.SetActive(false);

            myPlayerIndex++;

            if (myPlayerIndex >= numPlayers)
            {
                Debug.Log("myPlayerIndex rolled over???");
                myPlayerIndex = 0;
            }
            // sendSuccessMessage(18);
            sendMessage(players[myPlayerIndex].name + "'s Turn!");
            currentPlayerName = players[myPlayerIndex].name;



            // check if the CardPlayer gameObject has a ComputerPlayer script attached
            if (players[myPlayerIndex].gameObject.GetComponent<ComputerPlayer>() != null)
            {
                Debug.Log("Computer player!");
                Debug.Log("myPlayerIndex is " + myPlayerIndex);
                onStartTurn(players[myPlayerIndex]);
                players[myPlayerIndex].gameObject.GetComponent<ComputerPlayer>().AICards.Clear();
                if (players[myPlayerIndex].dead)
                {
                    endTurn();
                    Debug.Log("Computer player is dead!");
                    return;
                }

                // MATTEO: I think this is a good place to call the start of turn sounds here!
                if (players[myPlayerIndex].name == "Fingas")
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Fingas_turn");
                }


                // FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Singe_Swing");

                players[myPlayerIndex].gameObject.GetComponent<ComputerPlayer>().StartCoroutine(players[myPlayerIndex].gameObject.GetComponent<ComputerPlayer>().waitASecBro());
            }
            else
            {
                onStartTurn(players[myPlayerIndex]);
            }

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
        nicklesAttackMenu.SetActive(true);
        // chooseOpponentMenu.SetActive(true);
        // displayOpponents();
    }

    public void takeTrashCard(CardDisplay cd)
    {
        if (!trashDeckBonus)
        {
            Debug.Log("Don't take a card reached here");
            return;
        }

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
        trashDeckMenu.SetActive(false);
        sendSuccessMessage(15);
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
            if (players[myPlayerIndex].pluotAction)
            {
                sendMessage("You already performed your action this turn!");
                return;
            }
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

            if (players[myPlayerIndex].reetsCycle)
            {
                sendMessage("You already performed your action this turn!");
                return;
            }

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
            dialog.textInfo = "Potions are the lifeblood of this game, and you will be seeing them a lot!\n\nNot only are they cheap ammunition, but they fuel " +
                "your more powerful artifacts and vessels!";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 6)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "Artifacts are powerful items that only require one potion in order to use." +
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
            dialog.textInfo = "Cards bought from the market appear face-up on top of " +
                "your deck! The order in which you buy things is important!\n\n" +
                "Keep in mind that any unspent Pips do not get saved, " +
                "so use 'em or lose 'em!";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 15)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            tutorialArrow.SetActive(false);
            dialog.textInfo = "Upon the start of your next turn, empty spots in your holster " +
                "will be replaced by the cards on top of your deck!\n\n" +
                "You also get 6 new Pips to start your turn with.";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 20)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "Good job! When a vessel is thrown, the vessel is trashed, and " +
                "the potions are dropped into the bottom of your deck!";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 23)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "Excellent! Now let's buy more cards from the market.\n\n" +
                "Buy some more potions from the market and then end your turn!";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 26)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "While buying cards is all fun and games, selling is where you " +
                "really make your dough!\n\n" +
                "Selling items allows you to increase your Pip total beyond " +
                "6 Pips, allowing you to buy more powerful and expensive " +
                "items!";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 29)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "If you don't want to sell a card but you don't want it in your " +
                "holster, you can cycle it to the bottom of your deck as well!\n\n" +
                "Be aware that cycling non-potion cards costs 1 Pip!\n\n" +
                "Try cycling a card from your holster! Drag a card from " +
                "your holster to your deck!";
            dialog.ActivateText(dialog.dialogBox);
        }
        else if (dialog.textBoxCounter == 31)
        {
            dialog.directions.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true);
            dialog.nameTag.SetActive(true);
            dialog.textInfo = "Now let's talk about you! Your character that is...\n\n" +
                "Every character has a special ability detailed on the front of " +
                "their character card!\n\n" +
                "Additionally, the card can be flipped to reveal an upgraded " +
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
            dialog.textInfo = "Some characters have unique items that can be obtained " +
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

        // MATTEO: we may want to remove the ThrowPotion sounds and replace them with specific vessel / artifact sounds

        throwingHand.SetActive(true);
        throwingHand.GetComponent<Animator>().SetTrigger("Throw");

        // hardcoded logic for two players
        if (numPlayers == 2)
        {
            Debug.Log("Middle person");
            yield return new WaitForSeconds(1f);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_ThrowPotion");
            throwingHand.transform.DOMoveX(1000f, 1f);
            yield return new WaitForSeconds(1f);
            throwingHand.SetActive(false);
            tempPlayer.subHealth(damage, cardQuality);

            yield break;
        }

        if (tempPlayer.user_id == 2 && numPlayers == 3)
        {
            Debug.Log("Right person");
            yield return new WaitForSeconds(1f);
            throwingHand.transform.DOMoveX(1470f, 1f);
            yield return new WaitForSeconds(1f);
            throwingHand.SetActive(false);
            tempPlayer.subHealth(damage, cardQuality);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_ThrowPotion");
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
        /*
        if (snakeBonus)
        {
            Debug.Log("SNAKE!!!");
            // pull the opponent holster UI up here
            opponentHolsterMenu.SetActive(true);
            displayOpponentHolster();
            return;
        }
        */

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
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].transform);

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

                    // Sunday Funnies check
                    if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardName == "Sunday Funnies")
                    {
                        Debug.Log("Sunday Funnies!");
                        players[myPlayerIndex].deck.putCardOnTop(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vPotion1.card);
                        players[myPlayerIndex].deck.putCardOnTop(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vPotion2.card);
                    }
                    else
                    {
                        players[myPlayerIndex].deck.putCardOnBottom(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vPotion1.card);
                        players[myPlayerIndex].deck.putCardOnBottom(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vPotion2.card);
                    }

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

    public IEnumerator MoveToTrash(GameObject obj, CardDisplay cd)
    {
        /*
        var instance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/SFX_ThrowPotion");
        instance.start();
        instance.release();
        */

        obj.transform.SetParent(obj.transform.parent.parent);
        cd.artifactSlot.transform.GetChild(0).gameObject.SetActive(false);
        // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].aPotion.gameObject.SetActive(false);
        obj.transform.DOJump(new Vector2(1850f * widthRatio, 400f * heightRatio), 400f, 1, 1f, false);
        obj.transform.DOScale(0.2f, 1f);
        obj.transform.DORotate(new Vector3(0, 0, 720f), 1f, RotateMode.FastBeyond360);
        yield return new WaitForSeconds(0.1f);
        cd.artifactSlot.transform.GetChild(0).gameObject.SetActive(true);
        cd.artifactSlot.SetActive(false);
        yield return new WaitForSeconds(0.6f);
        // instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        obj.GetComponent<Image>().CrossFadeAlpha(0, 0.2f, false);
        yield return new WaitForSeconds(0.15f);

        Destroy(obj);
        // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).gameObject.SetActive(true);
        // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.SetActive(false);

        // MATTEO : Add trash can thunk sfx here!
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Trash");
        yield return new WaitForSeconds(0.15f);
        td.transform.parent.DOMove(new Vector2(td.transform.parent.position.x, td.transform.parent.position.y - 5), 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public IEnumerator MoveToTrash(GameObject obj)
    {
        obj.transform.SetParent(obj.transform.parent.parent);
        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).gameObject.SetActive(false);
        // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].aPotion.gameObject.SetActive(false);
        obj.transform.DOJump(new Vector2(1850f * widthRatio, 400f * heightRatio), 400f, 1, 1f, false);
        obj.transform.DOScale(0.2f, 1f);
        obj.transform.DORotate(new Vector3(0, 0, 720f), 1f, RotateMode.FastBeyond360);
        yield return new WaitForSeconds(0.1f);
        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).gameObject.SetActive(true);
        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.SetActive(false);
        yield return new WaitForSeconds(0.6f);
        obj.GetComponent<Image>().CrossFadeAlpha(0, 0.2f, false);
        yield return new WaitForSeconds(0.15f);

        Destroy(obj);
        // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).gameObject.SetActive(true);
        // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.SetActive(false);

        // MATTEO : Add trash can thunk sfx here!
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Trash");
        yield return new WaitForSeconds(0.15f);
        td.transform.parent.DOMove(new Vector2(td.transform.parent.position.x, td.transform.parent.position.y - 5), 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public IEnumerator MoveToDeck(GameObject obj)
    {
        obj.transform.SetParent(obj.transform.parent.parent);
        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).gameObject.SetActive(false);
        // obj.transform.DOJump(new Vector2(1850f, 400f), 400f, 1, 1f, false);
        // jump into deck
        obj.transform.DOJump(new Vector2(1600f * widthRatio, 250f * heightRatio), 400f, 1, 1f, false);
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
                md1.cardDisplay1.GetComponent<DragCard>().marketBack();

                break;
            case 2:
                td.addCard(md1.cardDisplay2);
                Card card2 = md1.popCard();
                md1.cardDisplay2.updateCard(card2);
                sendSuccessMessage(9);
                md1.cardDisplay2.GetComponent<DragCard>().marketBack();
                break;
            case 3:
                td.addCard(md1.cardDisplay3);
                Card card3 = md1.popCard();
                md1.cardDisplay3.updateCard(card3);
                sendSuccessMessage(9);
                md1.cardDisplay3.GetComponent<DragCard>().marketBack();
                break;
            case 4:
                td.addCard(md2.cardDisplay1);
                Card card4 = md2.popCard();
                md2.cardDisplay1.updateCard(card4);
                sendSuccessMessage(9);
                md2.cardDisplay1.GetComponent<DragCard>().marketBack();
                break;
            case 5:
                td.addCard(md2.cardDisplay2);
                Card card5 = md2.popCard();
                md2.cardDisplay2.updateCard(card5);
                sendSuccessMessage(9);
                md2.cardDisplay2.GetComponent<DragCard>().marketBack();
                break;
            case 6:
                td.addCard(md2.cardDisplay3);
                Card card6 = md2.popCard();
                md2.cardDisplay3.updateCard(card6);
                sendSuccessMessage(9);
                md2.cardDisplay3.GetComponent<DragCard>().marketBack();
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
                sendMessage("You put a starter potion on top of your deck!");
            }
        }
        //players[myPlayerIndex].deck.putCardOnTop(starterPotionCard);
    }

    public void setStarterPotion()
    {
        starterPotion = true;
    }

    public void preLoadPotion(CardDisplay selectedCard = null)
    {
        if (selectedCard != null)
        {
            Debug.Log("Trying to load deck card!!!");
            if (selectedCard.card.cardType != "Potion")
            {
                Debug.Log("Not a potion, ERROR");
                sendErrorMessage(17);

            }
            else
            {
                loadPotion(selectedCard);
            }
            return;
        }

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

    public void loadPotion(CardDisplay cd)
    {
        players[myPlayerIndex].checkGauntletBonus();

        // If this client isn't the current player, display error message.
        if (players[myPlayerIndex].user_id != myPlayerIndex)
        {
            // "You are not the currentPlayer!"
            sendErrorMessage(7);
            return;
        }

        if (players[myPlayerIndex].deck.GetComponent<CharacterSlot>().gauntletBonus == false)
        {
            Debug.Log("Gauntlet Ring is false and you are trying to be naughty...");
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
            }
            else if (players[myPlayerIndex].holster.cardList[loadedCardInt].card.cardType == "Artifact")
            {
                // Enable Artifact menu if it wasn't already enabled.
                Debug.Log("Artifact menu enabled.");
                players[myPlayerIndex].holster.cardList[loadedCardInt].artifactSlot.transform.parent.gameObject.SetActive(true);
                players[myPlayerIndex].holster.cardList[loadedCardInt].artifactSlot.transform.gameObject.SetActive(true);
                players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.gameObject.SetActive(true);

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
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_Load");
                    usedStarterPotion = true;
                    // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                    sendSuccessMessage(5);
                    //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                    Debug.Log("Starter potion loaded in Artifact slot!");

                    // MATTEO: Add Loading potion SFX here.

                    // // Updates Holster card to be empty.
                    Card card = players[myPlayerIndex].deck.popCard();
                    cd.card = card;
                    cd.updateCard(card);
                    players[myPlayerIndex].deck.updateCardSprite();
                }
            }
            starterPotion = false;
            return;
        }
        else
        {
            // if it's an artifact or vessel
            if (cd.card.cardType == "Potion")
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
                        }
                        else
                        {
                            // Fill Vessel slot 2 with loaded potion.
                            Card placeholder = players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion2.card;
                            players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion2.card = cd.card;
                            players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion2.updateCard(cd.card);
                            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_Load");

                            sendSuccessMessage(5);
                            Debug.Log("Potion loaded in Vessel slot 2!");

                            // MATTEO: Add Loading potion SFX here.

                            // // Updates Holster card to be empty.
                            Card card = players[myPlayerIndex].deck.popCard();
                            cd.card = card;
                            cd.updateCard(card);
                            players[myPlayerIndex].deck.updateCardSprite();
                        }
                    }
                    // Vessel slot 1 is unloaded.
                    else
                    {
                        Card placeholder = players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion1.card;
                        players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion1.card = cd.card;
                        players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion1.updateCard(cd.card);
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_Load");

                        // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                        sendSuccessMessage(5);
                        // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        Debug.Log("Potion loaded in Vessel slot 1!");

                        // MATTEO: Add Loading potion SFX here.

                        // // Updates Holster card to be empty.
                        Card card = players[myPlayerIndex].deck.popCard();
                        cd.card = card;
                        cd.updateCard(card);
                        players[myPlayerIndex].deck.updateCardSprite();
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
                    }
                    // Artifact slot is unloaded.
                    else
                    {
                        Card placeholder = players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.card;
                        players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.card = cd.card;
                        players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.updateCard(cd.card);
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_Load");
                        // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                        sendSuccessMessage(5);
                        Debug.Log("Potion loaded in Artifact slot!");

                        // MATTEO: Add Loading potion SFX here.

                        // // Updates Holster card to be empty.
                        Card card = players[myPlayerIndex].deck.popCard();
                        cd.card = card;
                        cd.updateCard(card);
                        players[myPlayerIndex].deck.updateCardSprite();
                    }
                }
            }
            else
            {
                // add error message
                Debug.Log("That error message...");
                sendErrorMessage(12);
            }
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
                            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_Load");

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
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_Load");

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
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_Load");
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
                }
                else if (players[myPlayerIndex].holster.cardList[loadedCardInt].card.cardType == "Artifact")
                {
                    // Enable Artifact menu if it wasn't already enabled.
                    Debug.Log("Artifact menu enabled.");
                    players[myPlayerIndex].holster.cardList[loadedCardInt].artifactSlot.transform.parent.gameObject.SetActive(true);
                    players[myPlayerIndex].holster.cardList[loadedCardInt].artifactSlot.transform.gameObject.SetActive(true);
                    players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.gameObject.SetActive(true);

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
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_Load");
                        usedStarterPotion = true;
                        // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                        sendSuccessMessage(5);
                        //players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
                        Debug.Log("Starter potion loaded in Artifact slot!");

                        // MATTEO: Add Loading potion SFX here.
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
                            }
                            else
                            {
                                // Fill Vessel slot 2 with loaded potion.
                                Card placeholder = players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion2.card;
                                players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion2.card = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card;
                                players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion2.updateCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card);
                                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_Load");

                                // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                                sendSuccessMessage(5);
                                // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
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
                            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_Load");

                            // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                            sendSuccessMessage(5);
                            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.GetComponent<Hover_Card>().resetCard();
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
                        }
                        // Artifact slot is unloaded.
                        else
                        {
                            Card placeholder = players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.card;
                            players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.card = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card;
                            players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.updateCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card);
                            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_Load");
                            // bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                            sendSuccessMessage(5);
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
            if (playerHolster.cardList[selectedCardInt - 1].card.cardType == "Potion" ||
                playerHolster.cardList[selectedCardInt - 1].card.cardType == "Unique")
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
                        if (players[myPlayerIndex].isReets)
                            players[myPlayerIndex].checkReetsCondition();
                        // bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay1.card.buyPrice, 1);
                        // SALTIMBOCCA LOGIC
                    }
                    else if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].pips >= (md1.cardDisplay1.card.buyPrice - 1))
                    {
                        if (md1.cardDisplay1.card.buyPrice == 1 && players[myPlayerIndex].pips == 0)
                        {
                            sendErrorMessage(6);
                            return;
                        }
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
                        if (players[myPlayerIndex].isReets)
                            players[myPlayerIndex].checkReetsCondition();
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
                        if (players[myPlayerIndex].isReets)
                            players[myPlayerIndex].checkReetsCondition();
                        // bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay2.card.buyPrice, 1);
                    }
                    else if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].pips >= (md1.cardDisplay2.card.buyPrice - 1))
                    {
                        if (md1.cardDisplay2.card.buyPrice == 1 && players[myPlayerIndex].pips == 0)
                        {
                            sendErrorMessage(6);
                            return;
                        }
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
                        if (players[myPlayerIndex].isReets)
                            players[myPlayerIndex].checkReetsCondition();
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
                        if (players[myPlayerIndex].isReets)
                            players[myPlayerIndex].checkReetsCondition();
                        // bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay3.card.buyPrice, 1);
                    }
                    else if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].pips >= (md1.cardDisplay3.card.buyPrice - 1))
                    {
                        if (md1.cardDisplay3.card.buyPrice == 1 && players[myPlayerIndex].pips == 0)
                        {
                            sendErrorMessage(6);
                            return;
                        }
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
                        if (players[myPlayerIndex].isReets)
                            players[myPlayerIndex].checkReetsCondition();
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
                        if (players[myPlayerIndex].isReets)
                            players[myPlayerIndex].checkReetsCondition();
                        // bool connected = networkManager.sendBuyRequest(md2.cardInt, md2.cardDisplay1.card.buyPrice, 0);
                    }
                    else if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].pips >= (md2.cardDisplay1.card.buyPrice - 1))
                    {
                        if (md2.cardDisplay1.card.buyPrice == 1 && players[myPlayerIndex].pips == 0)
                        {
                            sendErrorMessage(6);
                            return;
                        }
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
                        if (players[myPlayerIndex].isReets)
                            players[myPlayerIndex].checkReetsCondition();
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
                        if (players[myPlayerIndex].isReets)
                            players[myPlayerIndex].checkReetsCondition();
                        // bool connected = networkManager.sendBuyRequest(md2.cardInt, md2.cardDisplay2.card.buyPrice, 0);
                    }
                    else if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].pips >= (md2.cardDisplay2.card.buyPrice - 1))
                    {
                        if (md2.cardDisplay2.card.buyPrice == 1 && players[myPlayerIndex].pips == 0)
                        {
                            sendErrorMessage(6);
                            return;
                        }
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
                        if (players[myPlayerIndex].isReets)
                            players[myPlayerIndex].checkReetsCondition();
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
                        if (players[myPlayerIndex].isReets)
                            players[myPlayerIndex].checkReetsCondition();
                        // bool connected = networkManager.sendBuyRequest(md2.cardInt, md2.cardDisplay3.card.buyPrice, 0);
                    }
                    else if (players[myPlayerIndex].isSaltimbocca && players[myPlayerIndex].pips >= (md2.cardDisplay3.card.buyPrice - 1))
                    {
                        if (md2.cardDisplay3.card.buyPrice == 1 && players[myPlayerIndex].pips == 0)
                        {
                            sendErrorMessage(6);
                            return;
                        }
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
                        if (players[myPlayerIndex].isReets)
                            players[myPlayerIndex].checkReetsCondition();
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

            if (players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType == "Unique")
            {
                players[myPlayerIndex].addPips(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.sellPrice);

                GameObject obj2 = Instantiate(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform.position,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform.rotation,
                        players[myPlayerIndex].holster.gameObject.transform);

                StartCoroutine(MoveToTrash(obj2));

                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
                sendSuccessMessage(8);
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


            GameObject obj = Instantiate(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform.position,
                        players[myPlayerIndex].holster.cardList[selectedCardInt - 1].gameObject.transform.rotation,
                        players[myPlayerIndex].holster.gameObject.transform);

            StartCoroutine(MoveToTrash(obj));

            td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
            sendSuccessMessage(8);
            players[myPlayerIndex].checkGauntletBonus();
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
            players[myPlayerIndex].checkGauntletBonus();
        }
    }

    public void trashCardInDeck(CardDisplay cd)
    {
        deckMenuScroll.addCardToTrash(cd);
    }

    public void sendMessage(string message)
    {
        StopCoroutine("sendMessage");

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
