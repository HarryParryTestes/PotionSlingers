using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;
using Steamworks;

[System.Serializable]
public class MainMenu : MonoBehaviour
{
    public static MainMenu menu;

    public TMPro.TextMeshProUGUI greeting;
    public string greetingName;

	public Texture2D texture;
	public Texture2D texture2;

	public int numPlayers;
	public bool flippable = false;
    private bool privacy = false;
	private GameObject playButton;
	private GameObject loginButton;
	private GameObject registerButton;
	private GameObject startGameButton;
	private GameObject networkMenu;
	// private OldNetworkManager networkManager;
	public GameManager gameManager;

	public List<Character> characters;
	public int charSelectIndex = 0;

	public GameObject lobbyMenu;

    public TMPro.TextMeshProUGUI stat1;
    public TMPro.TextMeshProUGUI stat2;
    public TMPro.TextMeshProUGUI stat3;

    public TMPro.TextMeshProUGUI player1Name;
	public TMPro.TextMeshProUGUI player2Name;
	public TMPro.TextMeshProUGUI player3Name;
	public TMPro.TextMeshProUGUI player4Name;
	private GameObject player1Input;
	private GameObject player2Input;

	public CharacterDisplay p1CharCard;
	public CharacterDisplay p2CharCard;
	public CharacterDisplay p3CharCard;
	public CharacterDisplay p4CharCard;

	private Character p1Char;
	private Character p2Char;
	private Character p3Char;
	private Character p4Char;

	private Character playerChar;
	private CharacterDisplay playerCharDisplay;

	public string p1Name = "Player 1";
	public int p1UserId = 0;
    public string p2Name = "Player 2";
	public int p2UserId = 0;
	public string p3Name = "Player 3";
	public int p3UserId = 0;
	public string p4Name = "Player 4";
	public int p4UserId = 0;

	private TMPro.TextMeshProUGUI playerName;
	private TMPro.TextMeshProUGUI opponentName;
	private GameObject playerInput;
	private GameObject opponentInput;

	public TMPro.TMP_InputField playerUsernameInputField;
	public TMPro.TMP_InputField playerPasswordInputField;
	private TMPro.TMP_InputField playerInputField;
	private TMPro.TMP_InputField opponentInputField;

    public TMPro.TextMeshProUGUI publicButton;

    public MyNetworkManager networkManager;

	public TMPro.TextMeshProUGUI authUsername;

	public GameObject p1ReadyButton;
	public GameObject p1NotReadyButton;
	public GameObject p2ReadyButton;
	public GameObject p2NotReadyButton;
	public GameObject p2Waiting;
	public GameObject p3ReadyButton;
	public GameObject p3NotReadyButton;
	public GameObject p3Waiting;
	public GameObject p4ReadyButton;
	public GameObject p4NotReadyButton;
	public GameObject p4Waiting;

	private bool p1Ready = false;
	private bool p2Ready = false;
	private bool p3Ready = false;
	private bool p4Ready = false;

	public CharacterDisplay charSelectChar;
	public CardDisplay uniqueCard;
	public TMPro.TextMeshProUGUI infoText;
	public TMPro.TextMeshProUGUI nameText;
	public TMPro.TextMeshProUGUI uniqueCardText;
	public List<string> infoQuotes;
	public List<UniqueCard> uniqueCards;

	public Slider levelBar;
	public TMPro.TextMeshProUGUI levelText;
	public TMPro.TextMeshProUGUI expText;
	public int level;
	public int points;

	public GameObject titleMenu;
	public GameObject logo;
	public GameObject expMenu;
	public Slider expMenuBar;
	public TMPro.TextMeshProUGUI expMenuLevelText;
	public TMPro.TextMeshProUGUI expMenuExpText;

	public GameObject levelUpText;


	void Awake()
    {
		// Don't worry about this for now
		/*
		if (PlayerPrefs.HasKey("fullscreen"))
		{
			
			int resolutionX = PlayerPrefs.GetInt("resolutionX");
			int resolutionY = PlayerPrefs.GetInt("resolutionY");
			Debug.Log("Setting saved resolution of " + resolutionX + "x" + resolutionY);

			bool full = PlayerPrefs.GetString("fullscreen") == "Fullscreen";

			Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
			Screen.SetResolution(resolutionX, resolutionY, full);
			
		}
		*/

		// Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
		menu = this;
        //DontDestroyOnLoad(gameObject);
        if (!SteamManager.Initialized) { return; }
        greetingName = SteamFriends.GetPersonaName().ToString();
        greeting.text = "Hello, "+ greetingName + "!";

		SteamUserStats.GetStat("exp_points", out points);
		SteamUserStats.GetStat("exp_level", out level);

		Debug.Log("Experience Points: " + points);
		Debug.Log("Experience Level: " + level);


		handleMaxValue();
		// float numbers = (float)points / (float)levelBar.maxValue;
		levelBar.value = points;
		expText.text = points + " / " + levelBar.maxValue;
		levelText.text = "Level " + level;
		expMenuBar.value = points;
		expMenuLevelText.text = "Level " + level;
	}

	void Start()
    {
		// Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
		// Screen.fullScreen = true;
		// Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
		//numPlayers = 0;
		playButton = GameObject.Find("PLAY");
		//networkManager = GetComponent<MyNetworkManager>();

        if (!SteamManager.Initialized) { return; }
    }

	void Update()
    {
        if (networkManager == null)
        {
			Debug.Log("Missing network manager");
			networkManager = GameObject.Find("NetworkManager").GetComponent<MyNetworkManager>();

			networkManager.checkCompletedTutorial();
			// add missing references
			// lobby manager
			// scene transition
			// loading screen
			// loading text
			// animator
			networkManager.animator = GameObject.Find("SceneTransition").GetComponent<Animator>();
			networkManager.sceneTransition = GameObject.Find("SceneTransition");
			networkManager.loadingScreen = GameObject.Find("LoadingScreen");
			networkManager.loadingText = GameObject.Find("LOADING");
            // networkManager.lobbyManager = GameObject.Find("OldLobbyMenu").GetComponent<LobbyManager>();

            if (networkManager.completedGame)
            {
				titleMenu.SetActive(false);
				logo.SetActive(false);
				Debug.Log("Experience Menu");
                if (networkManager.completedTutorial)
                {
					initExperienceMenu(50);
					return;
				}
				initExperienceMenu(100);
            }

		}
    }

	public void setSliderValue()
    {
		levelBar.value = points;
		expText.text = points + " / " + levelBar.maxValue;
	}

	public void handleMaxValue()
    {
        switch (level)
        {
			case 1:
				levelBar.maxValue = 100;
				expMenuBar.maxValue = 100;
				break;
			case 2:
				levelBar.maxValue = 200;
				expMenuBar.maxValue = 200;
				break;
			case 3:
				levelBar.maxValue = 300;
				expMenuBar.maxValue = 300;
				break;
			case 4:
				levelBar.maxValue = 400;
				expMenuBar.maxValue = 400;
				break;
			default:
				levelBar.maxValue = 200;
				expMenuBar.maxValue = 200;
				break;

		}
    }

	public void initExperienceMenu(int exp)
    {
		titleMenu.SetActive(false);
		logo.SetActive(false);
		Debug.Log("Experience Menu");
		expMenu.SetActive(true);
		SteamUserStats.GetStat("exp_points", out points);
		SteamUserStats.GetStat("exp_level", out level);

		points += exp;
		if(points >= expMenuBar.maxValue)
        {
			levelUp(level);
        }
		expMenuBar.value = points;
		expMenuExpText.text = points + " / " + levelBar.maxValue;
		Debug.Log("EXP: " + points);
		SteamUserStats.SetStat("exp_points", points);
		SteamUserStats.StoreStats();
	}

	public void levelUp(int level)
    {
		Debug.Log("You leveled up!!!");
		levelUpText.SetActive(true);
		points -= (int)expMenuBar.maxValue;
		level++;
		Debug.Log("EXP: " + points);
		Debug.Log("LEVEL: " + level);

        switch (level)
        {
			case 1:
				levelBar.maxValue = 100;
				expMenuBar.maxValue = 100;
				break;
			case 2:
				levelBar.maxValue = 200;
				expMenuBar.maxValue = 200;
				break;
			case 3:
				levelBar.maxValue = 300;
				expMenuBar.maxValue = 300;
				break;
			case 4:
				levelBar.maxValue = 400;
				expMenuBar.maxValue = 400;
				break;
			case 5:
				levelBar.maxValue = 500;
				expMenuBar.maxValue = 500;
				break;
			case 6:
				levelBar.maxValue = 600;
				expMenuBar.maxValue = 600;
				break;
			case 7:
				levelBar.maxValue = 800;
				expMenuBar.maxValue = 800;
				break;
			case 8:
				levelBar.maxValue = 1000;
				expMenuBar.maxValue = 1000;
				break;
			default:
				levelBar.maxValue = 200;
				expMenuBar.maxValue = 200;
				break;
		}

		levelBar.value = points;
		expText.text = points + " / " + levelBar.maxValue;
		levelText.text = "Level " + level;
		expMenuBar.value = points;
		expMenuLevelText.text = "Level " + level;

		SteamUserStats.SetStat("exp_points", points);
		SteamUserStats.SetStat("exp_level", level);
		SteamUserStats.StoreStats();
	}

	public void createCPU()
	{
		LobbyManager.instance.createCPU();
	}

	public void checkStats()
    {
        int potions;
        int artifacts;
        int pips;
        SteamUserStats.GetStat("potions_thrown", out potions);
        SteamUserStats.GetStat("artifacts_used", out artifacts);
        SteamUserStats.GetStat("pips_spent", out pips);
        stat1.text = "Potions thrown:  " + potions;
        stat2.text = "Artifacts used:  " + artifacts;
        stat3.text = "Pips spent:  " + pips;
    }

	public void selectCharNameRight()
	{
		charSelectIndex += 1;
		if (charSelectIndex > 8)
		{
			charSelectIndex = 0;
		}

		if(charSelectIndex == 1)
        {
			// Isadore's item
			uniqueCard.gameObject.SetActive(true);
			uniqueCard.updateUniqueCard(uniqueCards[1]);
			uniqueCardText.gameObject.SetActive(true);
			uniqueCardText.text = "Special Item:\nThe CherryBomb\nBadge";
        }
		else if (charSelectIndex == 2)
        {
			uniqueCard.gameObject.SetActive(true);
			uniqueCard.updateUniqueCard(uniqueCards[0]);
			uniqueCardText.gameObject.SetActive(true);
			uniqueCardText.text = "Special Item:\nThe Blacksnake\nPip Sling";
		}
		else if (charSelectIndex == 3)
		{
			// Pluot's item
			uniqueCard.gameObject.SetActive(true);
			uniqueCard.updateUniqueCard(uniqueCards[2]);
			uniqueCardText.gameObject.SetActive(true);
			uniqueCardText.text = "Special Item:\nThe Extra\nInventory";
		}
		else if (charSelectIndex == 7)
		{
			// Sweetbitter's item
			uniqueCard.gameObject.SetActive(true);
			uniqueCard.updateUniqueCard(uniqueCards[3]);
			uniqueCardText.gameObject.SetActive(true);
			uniqueCardText.text = "Special Item:\nThe Phlactery";
		}
		else
        {
			uniqueCard.gameObject.SetActive(false);
			uniqueCardText.gameObject.SetActive(false);
		}
		charSelectChar.onCharacterClick(characters[charSelectIndex].cardName);
		nameText.text = characters[charSelectIndex].cardName;
		infoText.text = infoQuotes[charSelectIndex];
		
		/*
		charName = MainMenu.menu.characters[charIndex].cardName;
		LobbyManager.instance.localGamePlayerScript.charIndex = charIndex;
		LobbyManager.instance.localGamePlayerScript.charName = MainMenu.menu.characters[charIndex].cardName;
		LobbyManager.instance.localGamePlayerScript.CmdChangeCharacter(MainMenu.menu.characters[charIndex].cardName);
		LobbyManager.instance.UpdateUI();
		*/
	}

	public void selectCharNameLeft()
	{
		charSelectIndex -= 1;
		if (charSelectIndex < 0)
		{
			charSelectIndex = 8;
		}

		if (charSelectIndex == 1)
		{
			// Isadore's item
			uniqueCard.gameObject.SetActive(true);
			uniqueCard.updateUniqueCard(uniqueCards[1]);
			uniqueCardText.gameObject.SetActive(true);
		}
		else if (charSelectIndex == 2)
		{
			// Nickel's item
			uniqueCard.gameObject.SetActive(true);
			uniqueCard.updateUniqueCard(uniqueCards[0]);
			uniqueCardText.gameObject.SetActive(true);
			uniqueCardText.text = "Special Item:\nThe Blacksnake\nPip Sling";
		}
		else if (charSelectIndex == 3)
		{
			// Pluot's item
			uniqueCard.gameObject.SetActive(true);
			uniqueCard.updateUniqueCard(uniqueCards[2]);
			uniqueCardText.gameObject.SetActive(true);
			uniqueCardText.text = "Special Item:\nThe Extra\nInventory";
		}
		else if (charSelectIndex == 7)
		{
			// Sweetbitter's item
			uniqueCard.gameObject.SetActive(true);
			uniqueCard.updateUniqueCard(uniqueCards[3]);
			uniqueCardText.gameObject.SetActive(true);
			uniqueCardText.text = "Special Item:\nThe Phylactery";
		}
		else
		{
			uniqueCardText.gameObject.SetActive(false);
			uniqueCard.gameObject.SetActive(false);
		}
		charSelectChar.onCharacterClick(characters[charSelectIndex].cardName);
		nameText.text = characters[charSelectIndex].cardName;
		infoText.text = infoQuotes[charSelectIndex];
		
		/*
		charName = MainMenu.menu.characters[charIndex].cardName;
		LobbyManager.instance.localGamePlayerScript.charIndex = charIndex;
		LobbyManager.instance.localGamePlayerScript.charName = MainMenu.menu.characters[charIndex].cardName;
		LobbyManager.instance.localGamePlayerScript.CmdChangeCharacter(MainMenu.menu.characters[charIndex].cardName);
		LobbyManager.instance.UpdateUI();
		*/
	}

	public void checkSaveGame()
    {
		SaveSystem.checkGameData();
    }

	public void changePrivacy()
    {
        privacy = !privacy;
    }

    public void changeButton()
    {
        if (!privacy)
        {
            publicButton.text = "PUBLIC";
        }
        else
        {
            publicButton.text = "FRIENDS ONLY";
        }
    }

	public void CreatePublicLobby()
	{
		SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, networkManager.maxConnections);
	}

    public void CreateNewLobby()
    {
        ELobbyType newLobbyType;

        Debug.Log("CreateNewLobby: friendsOnlyToggle is OFF. Making lobby public.");
        newLobbyType = ELobbyType.k_ELobbyTypePublic;


        //Debug.Log("CreateNewLobby: player created a lobby name of: " + lobbyNameInputField.text);
        // didPlayerNameTheLobby = true;
        //lobbyName = lobbyNameInputField.text;

        SteamLobby.instance.CreateNewLobby(newLobbyType);
    }

	public void StartTutorial()
	{
		networkManager.tutorial = true;
        networkManager.multiplayer = false;
		networkManager.quickplay = false;
		networkManager.storyMode = false;
		networkManager.savedGame = false;

		StartCoroutine(networkManager.LoadLevel());
		// networkManager.ServerChangeScene("GameScene");
	}

	public void StartStoryMode()
	{
		networkManager.tutorial = false;
		networkManager.multiplayer = false;
		networkManager.quickplay = false;
		networkManager.storyMode = true;
		networkManager.savedGame = false;

		StartCoroutine(networkManager.LoadLevel());
		// networkManager.ServerChangeScene("GameScene");
	}

	public void StartSavedStoryMode()
	{
		networkManager.tutorial = false;
		networkManager.multiplayer = false;
		networkManager.quickplay = false;
		networkManager.storyMode = true;
		networkManager.savedGame = true;

		StartCoroutine(networkManager.LoadLevel());
		// networkManager.ServerChangeScene("GameScene");
	}

	public void StartGame()
	{
		networkManager.multiplayer = true;
        networkManager.tutorial = false;
		networkManager.quickplay = false;
		networkManager.storyMode = false;
		networkManager.savedGame = false;
		StartCoroutine(networkManager.LoadLevel());
		// networkManager.ServerChangeScene("GameScene");
	}

    public void StartSingleplayerGame()
    {
		// maybe use static reference to Game instead of networkManager?
        networkManager.multiplayer = false;
        networkManager.tutorial = false;
		networkManager.quickplay = false;
		networkManager.storyMode = false;
		networkManager.savedGame = false;

		/*
		foreach (PlayerListItem item in LobbyManager.instance.playerListItems)
        {
			//networkManager.charNames.Add(item.charName);
			// add gameplayers
			networkManager.GamePlayers.Add(item.GetComponent<GamePlayer>());
        }
        */

		// now try adding names
		/*
		foreach (GamePlayer player in networkManager.GamePlayers)
        {
			networkManager.charNames.Add(player.charName);
		}
		*/

		// networkManager.ServerChangeScene("GameScene");
		networkManager.StartSingleplayerGame();
    }

	public void StartQuickplayGame()
    {
		networkManager.multiplayer = false;
		networkManager.tutorial = false;
		networkManager.quickplay = true;
		networkManager.storyMode = false;
		networkManager.savedGame = false;

		StartCoroutine(networkManager.LoadLevel());

		// networkManager.ServerChangeScene("GameScene");
	}

	public int getNumPlayers() {
		return numPlayers;
	}

	public void onPlayClick()
	{
		Debug.Log("Send JoinReq");
		/*
		bool connected = networkManager.SendJoinRequest();
		if (!connected)
		{
			messageBoxMsg.text = "Unable to connect to server.";
			messageBox.SetActive(true);
		}
		*/
	}

	public void changeFlip()
    {
		/*
		Pluot		Playstyle: Aggressive
		Difficulty: *  	Proficiency: Potions
		Pluot’s strategy is simple;  collect potions
		and throw them for a lot of damage! Her
		item allows you to be even more efficient! 

		Reets		Playstyle: Technical  
		Difficulty: * * *  	Proficiency: Inventory
		Reets always has the right tools on hand!
		He can cycle through his items in his
		Holster and Deck much faster than the

		rest.
		
		Saltimbocca
		Playstyle: Strategic 
		Difficulty: * * 
		Proficiency: Thrifting


		With intimidation and an eye for quality,
		Bocca wants the best items and wants them
		cheap. Preferably also sharp,  heavy, and 
		aerodynamic.

		Scarpetta
Playstyle:  ? ? ?     
Difficulty: ? ? ?  
Proficiency: Trash


Scarpetta loves trash. When others may
scoff at as useless junk, he revels in an 
ever-increasing pile of treasures. Refuse,
reuse, recycle!

		 */

		flippable = !flippable;
		charSelectChar.onCharacterClick(characters[0].cardName);
		nameText.text = "Bolo";
		// Bolo
		infoQuotes[0] = "Bolo\nPlaystyle: Strategic\nDifficulty: * * *\nProficiency: Sales\n\n\n" +
			"Bolo is an adept salesman. He will always\nbe able to upsell his items, and can even\ntrade useless baubles for powerful\ntreasure!";

		// Isadore
		infoQuotes[1] = "Isadore\t\tPlaystyle: Aggressive\nDifficulty: * *\tProficiency: Artifacts\nIsadore is a master of artifacts, and is\nextremely" +
			" versatile with them. Flip her to\nequip her badge, and punish her foes\neven further!";

		//Nickles
		infoQuotes[2] = "Nickles\t\tPlaystyle: Flexible\nDifficulty: * *\tProficiency: Pips\nNickles can use " +
            "leftover pips as an extra\nresource. Use his unique item to rustle\nup a couple extra pips to sling or even\nto spend!";

		// Pluot
		infoQuotes[3] = "Pluot\t\t\tPlaystyle: Aggressive\nDifficulty: *\t\tProficiency: Potions\nPluot's strategy is simple; collect potions\n" +
			"and throw them for a lot of damage! Her\nitem allows you to be even more efficient!";

		// Reets
		infoQuotes[4] = "Reets\nPlaystyle: Technical\nDifficulty: * * *\nProficiency: Inventory\n\n\nReets always has the right tools on hand!\n" +
            "He can cycle through his items in his\nHolster and Deck much faster than the\nrest.";

		// Saltimbocca
		infoQuotes[5] = "Saltimbocca\nPlaystyle: Strategic\nDifficulty: * *\nProficiency: Thrifting\n\n\n" +
			"With intimidation and an eye for quality,\nBocca wants the best items and wants 'em\ncheap. Preferably also sharp, heavy, and\naerodynamic.";

		// Scarpetta
		infoQuotes[6] = "Scarpetta\nPlaystyle:  ? ? ?\nDifficulty: ? ? ?\nProficiency: Trash\n\n\nScarpetta loves trash. While others may\n" +
            "scoff at as useless junk, he revels in an\never-increasing pile of treasures. Refuse,\nreuse, recycle!";

		// Sweetbitter
		infoQuotes[7] = "Sweetbitter\t\tPlaystyle: Technical\nDifficulty: * * * *  Proficiency: Resilience\nSweetbitter has a strange object that\n" +
            "prevents her from dying. She also has a\nnefarious plan. She just has to collect a\nfew things...";

        // Twins
        infoQuotes[8] = "Twins\nPlaystyle: Flexible\nDifficulty: *\nProficiency: Self Healing\n\n\n" +
            "The Twins focus on keeping their HP\nhigh, at the expense of everyone else! Flip\nthem over to have their vessels heal too!";

		infoText.text = "Bolo\nPlaystyle: Strategic\nDifficulty: * * *\nProficiency: Sales\n\n\n" +
			"Bolo is an adept salesman. He will always\nbe able to upsell his items, and can even\ntrade useless baubles for powerful\ntreasure!";
	}

	public void onCharacterClick(string character)
    {
		Debug.Log("Send CharReq");
		foreach (Character character2 in MainMenu.menu.characters)
		{
			if (character2.cardName == character)
			{
				Debug.Log(character + " chosen");
				//playerCharDisplay.updateCharacter(character2);
			}
		}
		// bool connected = networkManager.SendCharacterRequest(character);
    }

	public void OnPlayerNameSet()
	{
		string name = playerUsernameInputField.text;
		authUsername.text = name;
		Debug.Log("Send SetNameReq: " + name);
		// networkManager.SendSetNameRequest(name);
	}

	public void OnReadyClick()
	{
		Debug.Log("Send ReadyReq");
		// networkManager.SendReadyRequest(1);
	}

	public void OnNotReadyClick()
	{
		Debug.Log("Send ReadyReq");
		// networkManager.SendReadyRequest(0);
	}

	public void StartNetworkGame()
    {
		Debug.Log("Start the game!");
		SceneManager.LoadScene("TownCenter");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	public void StartTutorialGame()
    {
		SceneManager.LoadScene("TownCenter");
	}

	public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
