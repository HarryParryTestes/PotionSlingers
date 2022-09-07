using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using Steamworks;

public class MainMenu : MonoBehaviour
{
    public static MainMenu menu;

    public TMPro.TextMeshProUGUI greeting;
    public string greetingName;

    public int numPlayers;
    private bool privacy = false;
	private GameObject playButton;
	private GameObject loginButton;
	private GameObject registerButton;
	private GameObject startGameButton;
	private GameObject networkMenu;
	// private OldNetworkManager networkManager;
	public GameManager gameManager;
	private MessageQueue msgQueue;

	public List<Character> characters;

	public GameObject lobbyMenu;

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

    void Awake()
    {
        menu = this;
        DontDestroyOnLoad(gameObject);
    }

	void Start()
    {
		//numPlayers = 0;
		playButton = GameObject.Find("PLAY");
		networkManager = GetComponent<MyNetworkManager>();

        if (!SteamManager.Initialized) { return; }

        //Debug.Log(SteamFriends.GetPersonaName());

        /*
		networkManager = GameObject.Find("OldNetworkManager").GetComponent<OldNetworkManager>();
		msgQueue = networkManager.GetComponent<MessageQueue>();

		// adding callbacks
		msgQueue.AddCallback(Constants.SMSG_JOIN, OnResponseJoin);
		msgQueue.AddCallback(Constants.SMSG_CHARACTER, OnResponseCharacter);
		msgQueue.AddCallback(Constants.SMSG_SETNAME, OnResponseSetName);
		msgQueue.AddCallback(Constants.SMSG_READY, OnResponseReady);
		*/
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

	public void OnResponseJoin(ExtendedEventArgs eventArgs)
	{
		ResponseJoinEventArgs args = eventArgs as ResponseJoinEventArgs;
		if (args.status == 0)
		{
			if (args.user_id == 1)
			{
				///Constants.OP1_ID = 2;
				///Constants.OP2_ID = 3;
				//Constants.OP3_ID = 4;
				Debug.Log("Player 1 has joined");
				numPlayers = 1;
			}
			else if (args.user_id == 2)
			{
				//Constants.OP1_ID = 1;
				//Constants.OP2_ID = 3;
				//Constants.OP3_ID = 4;
				Debug.Log("Player 2 has joined");
				numPlayers = 2;
			}
			else if (args.user_id == 3)
			{
				// playerCharDisplay = p3CharCard;
				//Constants.OP1_ID = 1;
				//Constants.OP2_ID = 2;
				//Constants.OP3_ID = 4;
				Debug.Log("Player 3 has joined");
				numPlayers = 3;
			}
			else if (args.user_id == 4)
			{
				// playerCharDisplay = p4CharCard;
				//Constants.OP1_ID = 1;
				//Constants.OP2_ID = 2;
				//Constants.OP3_ID = 3;
				Debug.Log("Player 4 has joined");
				numPlayers = 4;
			}
			else
			{
				Debug.Log("ERROR: Invalid user_id in ResponseJoin: " + args.user_id);
				//messageBoxMsg.text = "Error joining game. Network returned invalid response.";
				//messageBox.SetActive(true);
				return;
			}
			
			Constants.USER_ID = args.user_id;
			Debug.Log("Number of players: " + numPlayers);
		}
	}

	public void OnPlayerNameSet()
	{
		string name = playerUsernameInputField.text;
		authUsername.text = name;
		Debug.Log("Send SetNameReq: " + name);
		// networkManager.SendSetNameRequest(name);
		if (Constants.USER_ID == 1)
		{
			p1Name = name;
			// player1Name.text = p1Name;
		}
		else if(Constants.USER_ID == 2)
		{
			p2Name = name;
			// player2Name.text = p2Name;
		}
		else if (Constants.USER_ID == 3)
		{
			p3Name = name;
			// player3Name.text = p3Name;
		}
		else if (Constants.USER_ID == 4)
		{
			p4Name = name;
			// player4Name.text = p4Name;
		}
	}

	public void OnResponseSetName(ExtendedEventArgs eventArgs)
	{
		ResponseSetNameEventArgs args = eventArgs as ResponseSetNameEventArgs;

			if (args.numPlayers == 1 && args.user_id1 != null)
			{
				player1Name.text = args.name1;
                p1Name = args.name1;
				p1UserId = args.user_id1;
			}
			else if (args.numPlayers == 2 && args.user_id2 != null)
			{
				p2Waiting.SetActive(false);
				player2Name.gameObject.SetActive(true);
				p2NotReadyButton.SetActive(true);
				player1Name.text = args.name1;
				player2Name.text = args.name2;
                p1Name = args.name1;
                p2Name = args.name2;
				p1UserId = args.user_id1;
				p2UserId = args.user_id2;
				numPlayers = 2;
			}
			else if (args.numPlayers == 3)
			{
				p3Waiting.SetActive(false);
				player3Name.gameObject.SetActive(true);
				p3NotReadyButton.SetActive(true);
				player1Name.text = args.name1;
				player2Name.text = args.name2;
				// player3Name.text = args.name3;
                p1Name = args.name1;
                p2Name = args.name2;
                // p3Name = args.name3;
				p1UserId = args.user_id1;
				p2UserId = args.user_id2;
				// p3UserId = args.user_id3;
				numPlayers = 3;
			}
			else if (args.numPlayers == 4)
			{
				p4Waiting.SetActive(false);
				player4Name.gameObject.SetActive(true);
				p4NotReadyButton.SetActive(true);
				player1Name.text = args.name1;
				player2Name.text = args.name2;
				// player3Name.text = args.name3;
				// player4Name.text = args.name4;
                p1Name = args.name1;
                p2Name = args.name2;
                // p3Name = args.name3;
                // p4Name = args.name4;
				p1UserId = args.user_id1;
				p2UserId = args.user_id2;
				// p3UserId = args.user_id3;
				// p4UserId = args.user_id4;
				numPlayers = 4;
			}
	}

	public void OnResponseCharacter(ExtendedEventArgs eventArgs)
	{
		ResponseCharacterEventArgs args = eventArgs as ResponseCharacterEventArgs;

        if (args.numPlayers == 1 && args.user_id1 != null) {
            foreach(Character character in characters) {
				if(character.cardName == args.characterName1)
                {
					Debug.Log(character.cardName);
					p1CharCard.updateCharacter(character);
                }
            }
        }

        else if (args.numPlayers == 2 && args.user_id2 != null) {
            p2CharCard.gameObject.SetActive(true);
            foreach (Character character in characters)
			{
                if(character.cardName == args.characterName1)
                {
					Debug.Log(character.cardName);
					p1CharCard.updateCharacter(character);
                }
				else if (character.cardName == args.characterName2)
				{
					Debug.Log(character.cardName);
					p2CharCard.updateCharacter(character);
				}
			}
        }

        else if (args.numPlayers == 3) {
            p3CharCard.gameObject.SetActive(true);
            foreach (Character character in characters)
			{
                if(character.cardName == args.characterName1)
                {
					Debug.Log(character.cardName);
					p1CharCard.updateCharacter(character);
                }
				else if (character.cardName == args.characterName2)
				{
					Debug.Log(character.cardName);
					p2CharCard.updateCharacter(character);
				}
                // else if (character.cardName == args.characterName3)
				// {
				// 	Debug.Log(character.cardName);
				// 	p3CharCard.updateCharacter(character);
				// }
			}
        }

        else if (args.numPlayers == 4) {
            p4CharCard.gameObject.SetActive(true);
            foreach (Character character in characters)
			{
                if(character.cardName == args.characterName1)
                {
					Debug.Log(character.cardName);
					p1CharCard.updateCharacter(character);
                }
				else if (character.cardName == args.characterName2)
				{
					Debug.Log(character.cardName);
					p2CharCard.updateCharacter(character);
				}
                // else if (character.cardName == args.characterName3)
				// {
				// 	Debug.Log(character.cardName);
				// 	p3CharCard.updateCharacter(character);
				// }
                // else if (character.cardName == args.characterName4)
				// {
				// 	Debug.Log(character.cardName);
				// 	p4CharCard.updateCharacter(character);
				// }
			}
        }

		// if (args.user_id == 1)
		// {
		// 	foreach(Character character in characters){
		// 		if(character.cardName == args.name)
        //         {
		// 			Debug.Log(character.cardName);
		// 			p1CharCard.updateCharacter(character);
        //         }
        //     }
		// }
		// else if (args.user_id == 2)
		// {
		// 	p2CharCard.gameObject.SetActive(true);
		// 	foreach (Character character in characters)
		// 	{
		// 		if (character.cardName == args.name)
		// 		{
		// 			Debug.Log(character.cardName);
		// 			p2CharCard.updateCharacter(character);
		// 		}
		// 	}
		// }
		// else if (args.user_id == 3)
		// {
		// 	p3CharCard.gameObject.SetActive(true);
		// 	foreach (Character character in characters)
		// 	{
		// 		if (character.cardName == args.name)
		// 		{
		// 			p3CharCard.updateCharacter(character);
		// 		}
		// 	}
		// }
		// else if (args.user_id == 4)
		// {
		// 	p4CharCard.gameObject.SetActive(true);
		// 	foreach (Character character in characters)
		// 	{
		// 		if (character.cardName == args.name)
		// 		{
		// 			p4CharCard.updateCharacter(character);
		// 		}
		// 	}
		// }
	
		Debug.Log("Character changed");
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

	public void OnResponseReady(ExtendedEventArgs eventArgs)
	{
		ResponseReadyEventArgs args = eventArgs as ResponseReadyEventArgs;
		/*
		if (args.user_id == 1)
		{
			p1Ready = true;
		}
		else if (args.user_id == 2)
		{
			p2Ready = true;
		}
		else if (args.user_id == 3)
		{
			p3Ready = true;
		}
		else if (args.user_id == 4)
		{
			p4Ready = true;
		}
		else
		{
			Debug.Log("ERROR: Invalid user_id in ResponseReady: " + args.user_id);
			//messageBoxMsg.text = "Error starting game. Network returned invalid response.";
			//messageBox.SetActive(true);
			return;
		}
		*/

		if(args.numPlayers == 1) {
			if(args.player1Ready) {
				p1NotReadyButton.SetActive(false);
				p1ReadyButton.SetActive(true);
				p1Ready = true;
			}
			else {
				p1ReadyButton.SetActive(false);
				p1NotReadyButton.SetActive(true);
				p1Ready = false;
			}
		}

		else if(args.numPlayers == 2) {
			if(args.player1Ready) {
				p1NotReadyButton.SetActive(false);
				p1ReadyButton.SetActive(true);
				p1Ready = true;
			}
			else {
				p1ReadyButton.SetActive(false);
				p1NotReadyButton.SetActive(true);
				p1Ready = false;
			}
			if(args.player2Ready) {
				p2NotReadyButton.SetActive(false);
				p2ReadyButton.SetActive(true);
				p2Ready = true;
			}
			else {
				p2ReadyButton.SetActive(false);
				p2NotReadyButton.SetActive(true);
				p2Ready = false;
			}
		}

		else if(args.numPlayers == 3) {
			Debug.Log("READY ERROR: 3 Players has not been set up yet!");
		}

		else if(args.numPlayers == 4) {
			Debug.Log("READY ERROR: 4 Players has not been set up yet!");
		}

		else {
			Debug.Log("READY ERROR: numPlayers is invalid or incorrect!");
		}

		Debug.Log("Player Ready?: " + p1Ready);
		Debug.Log("Opponent1 Ready?: " + p2Ready);
		Debug.Log("Opponent2 Ready?: " + p3Ready);
		Debug.Log("Opponent3 Ready?: " + p4Ready);

		if((p1Ready && p2Ready) && numPlayers == 2)
        {
			StartNetworkGame();
        }

		if ((p1Ready && p2Ready && p3Ready) && numPlayers == 3)
		{
			StartNetworkGame();
		}

		if ((p1Ready && p2Ready && p3Ready && p4Ready) && numPlayers == 4)
		{
			StartNetworkGame();
		}
		/*
		if (ready && opReady)
		{
			StartNetworkGame();
		}
		*/
	}

	public void StartNetworkGame()
    {
		Debug.Log("Start the game!");
		SceneManager.LoadScene("GameScene");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		gameManager.numPlayers = numPlayers;
		gameManager.init();
	}

	public void StartTutorialGame()
    {
		SceneManager.LoadScene("GameScene");
	}

	public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
