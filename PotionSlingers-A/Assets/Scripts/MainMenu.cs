using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

	private int numPlayers;
	private GameObject playButton;
	private GameObject loginButton;
	private GameObject registerButton;
	private GameObject startGameButton;
	private GameObject networkMenu;
	private NetworkManager networkManager;
	public GameManager gameManager;
	private MessageQueue msgQueue;

	public List<Character> characters;

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

	private string p1Name = "Player 1";
	private string p2Name = "Player 2";
	private string p3Name = "Player 3";
	private string p4Name = "Player 4";

	private TMPro.TextMeshProUGUI playerName;
	private TMPro.TextMeshProUGUI opponentName;
	private GameObject playerInput;
	private GameObject opponentInput;

	public TMPro.TMP_InputField playerUsernameInputField;
	public TMPro.TMP_InputField playerPasswordInputField;
	private TMPro.TMP_InputField playerInputField;
	private TMPro.TMP_InputField opponentInputField;

	public TMPro.TextMeshProUGUI authUsername;

	private bool p1Ready = false;
	private bool p2Ready = false;
	private bool p3Ready = false;
	private bool p4Ready = false;

	void Start()
    {
		numPlayers = 0;
		playButton = GameObject.Find("PLAY");
		networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
		msgQueue = networkManager.GetComponent<MessageQueue>();

		// adding callbacks
		msgQueue.AddCallback(Constants.SMSG_JOIN, OnResponseJoin);
		msgQueue.AddCallback(Constants.SMSG_CHARACTER, OnResponseCharacter);
		msgQueue.AddCallback(Constants.SMSG_SETNAME, OnResponseSetName);
		msgQueue.AddCallback(Constants.SMSG_READY, OnResponseReady);
	}

	public void onPlayClick()
	{
		Debug.Log("Send JoinReq");
		bool connected = networkManager.SendJoinRequest();
		if (!connected)
		{
			//messageBoxMsg.text = "Unable to connect to server.";
			//messageBox.SetActive(true);
		}
	}

	public void onCharacterClick(string character)
    {
		Debug.Log("Send CharReq");
		foreach (Character character2 in characters)
		{
			if (character2.cardName == character)
			{
				playerChar = character2;
				playerCharDisplay.updateCharacter(playerChar);
			}
		}
		bool connected = networkManager.SendCharacterRequest(character);
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
				playerCharDisplay = p3CharCard;
				//Constants.OP1_ID = 1;
				//Constants.OP2_ID = 2;
				//Constants.OP3_ID = 4;
				Debug.Log("Player 3 has joined");
				numPlayers = 3;
			}
			else if (args.user_id == 4)
			{
				playerCharDisplay = p4CharCard;
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
		networkManager.SendSetNameRequest(name);
		if (Constants.USER_ID == 1)
		{
			p1Name = name;
			player1Name.text = p1Name;
		}
		else if(Constants.USER_ID == 2)
		{
			p2Name = name;
			player2Name.text = p2Name;
		}
		else if (Constants.USER_ID == 3)
		{
			p3Name = name;
			player3Name.text = p3Name;
		}
		else if (Constants.USER_ID == 4)
		{
			p4Name = name;
			player4Name.text = p4Name;
		}
	}

	public void OnResponseSetName(ExtendedEventArgs eventArgs)
	{
		ResponseSetNameEventArgs args = eventArgs as ResponseSetNameEventArgs;
		
			if (args.user_id == 1)
			{
				player1Name.text = args.name;
			}
			else if (args.user_id == 2)
			{
				player2Name.text = args.name;
			}
			else if (args.user_id == 3)
			{
				player3Name.text = args.name;
			}
			else if (args.user_id == 4)
			{
				player4Name.text = args.name;
			}
	}

	public void OnResponseCharacter(ExtendedEventArgs eventArgs)
	{
		ResponseCharacterEventArgs args = eventArgs as ResponseCharacterEventArgs;
		if (args.user_id == 1)
		{
			foreach(Character character in characters){
				if(character.cardName == args.name)
                {
					p1CharCard.updateCharacter(character);
                }
            }
		}
		else if (args.user_id == 2)
		{
			foreach (Character character in characters)
			{
				if (character.cardName == args.name)
				{
					p2CharCard.updateCharacter(character);
				}
			}
		}
		else if (args.user_id == 3)
		{
			foreach (Character character in characters)
			{
				if (character.cardName == args.name)
				{
					p3CharCard.updateCharacter(character);
				}
			}
		}
		else if (args.user_id == 4)
		{
			foreach (Character character in characters)
			{
				if (character.cardName == args.name)
				{
					p4CharCard.updateCharacter(character);
				}
			}
		}
	
		Debug.Log("Character changed");
	}

	public void OnReadyClick()
	{
		Debug.Log("Send ReadyReq");
		networkManager.SendReadyRequest();
	}

	public void OnResponseReady(ExtendedEventArgs eventArgs)
	{
		ResponseReadyEventArgs args = eventArgs as ResponseReadyEventArgs;
		// p1 op1
		if (args.user_id == 1)
		{
			p1Ready = true;
		}
		// p2 op1
		if (args.user_id == 2)
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
		gameManager.numPlayers = numPlayers;
		gameManager.init();
	}

	public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
