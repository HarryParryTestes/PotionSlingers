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
	public static GameManagerTest gameManager;
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

	private bool ready = false;
	private bool op1Ready = false;
	private bool op2Ready = false;
	private bool op3Ready = false;

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
		bool connected = networkManager.SendCharacterRequest(character);
    }

	public void OnResponseJoin(ExtendedEventArgs eventArgs)
	{
		ResponseJoinEventArgs args = eventArgs as ResponseJoinEventArgs;
		if (args.status == 0)
		{
			if (args.user_id == 1)
			{
				Constants.OP1_ID = 2;
				Constants.OP2_ID = 3;
				Constants.OP3_ID = 4;
				Debug.Log("Player 1 has joined");
				numPlayers++;
			}
			else if (args.user_id == 2)
			{
				Constants.OP1_ID = 1;
				Constants.OP2_ID = 3;
				Constants.OP3_ID = 4;
				Debug.Log("Player 2 has joined");
				numPlayers++;
			}
			else if (args.user_id == 3)
			{
				Constants.OP1_ID = 1;
				Constants.OP2_ID = 2;
				Constants.OP3_ID = 4;
				Debug.Log("Player 3 has joined");
				numPlayers++;
			}
			else if (args.user_id == 4)
			{
				Constants.OP1_ID = 1;
				Constants.OP2_ID = 2;
				Constants.OP3_ID = 3;
				Debug.Log("Player 4 has joined");
				numPlayers++;
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
		// if user_id is opponent
		if (args.user_id != Constants.USER_ID)
		{
			// p1 op1
			if (args.user_id == Constants.OP1_ID  && args.user_id == 2)
			{
				player2Name.text = args.name;
			}
			// p2 op1
			if (args.user_id == Constants.OP1_ID && args.user_id == 1)
			{
				player1Name.text = args.name;
			}
			// p1 op2
			if (args.user_id == Constants.OP2_ID && args.user_id == 3)
			{
				player3Name.text = args.name;
			}
			// p2 op2
			if (args.user_id == Constants.OP2_ID && args.user_id == 3)
			{
				player3Name.text = args.name;
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
	}

	public void OnResponseCharacter(ExtendedEventArgs eventArgs)
	{
		ResponseCharacterEventArgs args = eventArgs as ResponseCharacterEventArgs;
		if (args.user_id == 1)
		{
			foreach(Character character in characters){
				if(character.cardName == args.name)
                {
					p1Char = character;
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
					p2Char = character;
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
		if (Constants.USER_ID == -1) // Haven't joined, but got ready message
		{
			op1Ready = true;
		}
		else
		{
			if (args.user_id == Constants.OP1_ID)
			{
				op1Ready = true;
			}
			else if (args.user_id == Constants.OP2_ID)
			{
				op2Ready = true;
			}
			else if (args.user_id == Constants.OP3_ID)
			{
				op3Ready = true;
			}
			else if (args.user_id == Constants.USER_ID)
			{
				ready = true;
			}
			else
			{
				Debug.Log("ERROR: Invalid user_id in ResponseReady: " + args.user_id);
				//messageBoxMsg.text = "Error starting game. Network returned invalid response.";
				//messageBox.SetActive(true);
				return;
			}
		}

		Debug.Log("Player Ready?: " + ready);
		Debug.Log("Opponent1 Ready?: " + op1Ready);
		Debug.Log("Opponent2 Ready?: " + op2Ready);
		Debug.Log("Opponent3 Ready?: " + op3Ready);

		if((ready && op1Ready) && numPlayers == 2)
        {
			StartNetworkGame();
        }

		if ((ready && op1Ready && op2Ready) && numPlayers == 3)
		{
			StartNetworkGame();
		}

		if ((ready && op1Ready && op2Ready && op3Ready) && numPlayers == 4)
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
