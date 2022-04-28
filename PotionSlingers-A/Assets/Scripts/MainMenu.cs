using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

	private GameObject playButton;
	private GameObject loginButton;
	private GameObject registerButton;
	private GameObject startGameButton;
	private GameObject networkMenu;
	private NetworkManager networkManager;
	public static GameManager gameManager;
	private MessageQueue msgQueue;

	public List<Character> characters;

	public TMPro.TextMeshProUGUI player1Name;
	public TMPro.TextMeshProUGUI player2Name;
	public TMPro.TextMeshProUGUI player3Name;
	public TMPro.TextMeshProUGUI player4Name;
	private GameObject player1Input;
	private GameObject player2Input;

	public CharacterDisplay p1CharCard;
	private CharacterDisplay boloCard;

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
	private bool opReady = false;

	void Start()
    {
		playButton = GameObject.Find("PLAY");
		networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
		msgQueue = networkManager.GetComponent<MessageQueue>();

		// adding callbacks
		msgQueue.AddCallback(Constants.SMSG_JOIN, OnResponseJoin);
		msgQueue.AddCallback(Constants.SMSG_CHARACTER, OnResponseCharacter);
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
				/*
				player1Name.text = p1Name;
				opponentName = player2Name;
				playerInput = player1Input;
				opponentInput = player2Input;
				playerInputField = player1InputField;
				opponentInputField = player2InputField;
				*/
				Debug.Log("Player 1 has joined");
			}
			else if (args.user_id == 2)
			{
				/*
				player2Name.text = p2Name;
				opponentName = player1Name;
				playerInput = player2Input;
				opponentInput = player1Input;
				playerInputField = player2InputField;
				opponentInputField = player1InputField;
				*/

				Debug.Log("Player 2 has joined");
			}
			else if (args.user_id == 3)
			{
				Debug.Log("Player 3 has joined");
			}
			else if (args.user_id == 4)
			{
				Debug.Log("Player 4 has joined");
			}
			else
			{
				Debug.Log("ERROR: Invalid user_id in ResponseJoin: " + args.user_id);
				//messageBoxMsg.text = "Error joining game. Network returned invalid response.";
				//messageBox.SetActive(true);
				return;
			}
			Constants.USER_ID = args.user_id;
			Constants.OP_ID = 3 - args.user_id;

			if (args.op_id > 0)
			{
				if (args.op_id == Constants.OP_ID)
				{
					opponentName.text = args.op_name;
					opReady = args.op_ready;
				}
				else
				{
					Debug.Log("ERROR: Invalid op_id in ResponseJoin: " + args.op_id);
					//messageBoxMsg.text = "Error joining game. Network returned invalid response.";
					//messageBox.SetActive(true);
					return;
				}
			}
			else
			{
				opponentName.text = "Waiting for opponent";
			}

			//playerInput.SetActive(true);
			//opponentName.gameObject.SetActive(true);
			//playerName.gameObject.SetActive(false);
			//opponentInput.SetActive(false);

			//rootMenuPanel.SetActive(false);
			//networkMenuPanel.SetActive(true);
		}
		else
		{
			//messageBoxMsg.text = "Server is full.";
			//messageBox.SetActive(true);
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
			
		}
		else if (args.user_id == 3)
		{
			
		}
		else if (args.user_id == 4)
		{
			
		}
	
		Debug.Log("Character changed");
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
