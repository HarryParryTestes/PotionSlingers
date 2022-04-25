using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //[SerializeField] UnityEvent throwPotion<Player, CardDisplay, Player>;
    
    int numPlayers = 0;
    int currentPlayer = 0;
    public Player[] players = new Player[4];
    GameObject ob;
    GameObject ob2;
    GameObject ob3;
    GameObject ob4;

    void Start()
    {
        initPlayers();
    }

    void initPlayers()
    {
        ob = GameObject.Find("CharacterCard");
        players[0] = ob.GetComponent<Player>();
        ob2 = GameObject.Find("CharacterCard (Top)");
        players[1] = ob2.GetComponent<Player>();

        if(numPlayers > 2)
        {
            ob3 = GameObject.Find("CharacterCard (Left)");
            players[2] = ob3.GetComponent<Player>();
            
            if(numPlayers > 3)
            {
                ob4 = GameObject.Find("CharacterCard (Right)");
                players[3] = ob4.GetComponent<Player>();
            }
        }
    }

    public void onStartTurn(Player player)
    {
        if(player.holster.card1.card.name == "placeholder")
        {
            player.holster.card1.updateCard(player.deck.popCard());
        }
        if (player.holster.card2.card.name == "placeholder")
        {
            player.holster.card2.updateCard(player.deck.popCard());
        }
        if (player.holster.card3.card.name == "placeholder")
        {
            player.holster.card3.updateCard(player.deck.popCard());
        }
        if (player.holster.card4.card.name == "placeholder")
        {
            player.holster.card4.updateCard(player.deck.popCard());
        }
    }

    public void endTurn()
    {
        //currentPlayer++;
        //if(currentPlayer == numPlayers)
        //{
            //currentPlayer = 0;
        //}
        onStartTurn(players[currentPlayer]);
    }

    public void throwPotion(Player player, CardDisplay cd, Player opponent)
    {
        Debug.Log("GameManager Throw Potion");
    }

    public void throwPotion()
    {
        Debug.Log("GameManager Throw Potion");
    }

    /*
    //delete once this is production ready
    public CharacterDisplay playerCharacter;
    public CharacterDisplay enemyCharacter;
    public Gamestate State;
    public Holster playerHolster;
    public Holster enemyHolster;
    public Text notificationText;
    public Text playerHealth;
    public Text playerCubes;
    public Text oppHealth;
    public Text oppCubes;

    void Awake() {
        manager = this;
        GameObject go = new GameObject("Player");
        player1 = go.AddComponent<Player>();

        GameObject go2 = new GameObject("Player");
        player2 = go2.AddComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        notificationText = GameObject.Find("Notification").GetComponent<Text>();
        

        healthSetUp();
        SetUp();

        updatePlayerText();
        //while(State != Gamestate.Lose && State != Gamestate.Win) {
            //updateGameState(State);
        //}
        updateGameState(State);
    }

    // Update is called once per frame
    void Update()
    {
        //updateGameState(State);
    }

    void healthSetUp() {
        playerHealth = GameObject.Find("pHealth").GetComponent<Text>();
        playerCubes = GameObject.Find("pEssenceCubes").GetComponent<Text>();
        oppHealth = GameObject.Find("Health").GetComponent<Text>();
        oppCubes = GameObject.Find("EssenceCubes").GetComponent<Text>();
    }

    void updatePlayerText() {
        playerHealth.text = player1.health.hp.ToString();
        playerCubes.text = player1.health.essenceCubes.ToString();
        oppHealth.text = player2.health.hp.ToString();
        oppCubes.text = player2.health.essenceCubes.ToString();
    }


    void SetUp() {
        //player1 = new Player();
        player1.character = playerCharacter;
        player1.holster = playerHolster;

        //player2 = new Player();
        player2.character = enemyCharacter;
        player2.holster = enemyHolster;

        Debug.Log("Player Character is: " + player1.character.character.cardName);
        Debug.Log("Enemy Character is: " + player2.character.character.cardName);

        Debug.Log("Player Cards:\n");
        Debug.Log(player1.holster.card1.card.cardName+"\n");
        Debug.Log(player1.holster.card2.card.cardName+"\n");
        Debug.Log(player1.holster.card3.card.cardName+"\n");
        Debug.Log(player1.holster.card4.card.cardName+"\n");

        Debug.Log("Enemy Cards:\n");
        Debug.Log(player2.holster.card1.card.cardName+"\n");
        Debug.Log(player2.holster.card2.card.cardName+"\n");
        Debug.Log(player2.holster.card3.card.cardName+"\n");
        Debug.Log(player2.holster.card4.card.cardName+"\n");

        State = Gamestate.PlayerTurn;
        Debug.Log("State is: " + State);
    }

    void updateGameState(Gamestate newState) {
        State = newState;

        switch (newState) {
            case Gamestate.PlayerTurn:
                StartCoroutine(handlePlayerTurn());
                break;
            case Gamestate.OpponentTurn:
                StartCoroutine(handleOpponentTurn());
                break;
            case Gamestate.Win:
                handleWin();
                break;
            case Gamestate.Lose:
                break;
            default:
                Debug.Log("That state action shouldn't have happened...");
                break;
        }
    }

    IEnumerator handlePlayerTurn() {
        Debug.Log("You just took a turn");

        yield return new WaitForSeconds(2f);
        Debug.Log("You finished waiting 2 seconds");

        player2.health.subHealth(10);
        Debug.Log("Opponent's health is: " + player2.health.hp);
        if(player2.health.dead) {
            State = Gamestate.Win;
            updateGameState(State);
        } 

        updatePlayerText();
    }

    public void endTurn() {
        Debug.Log("You just ended your turn.");
        State = Gamestate.OpponentTurn;
        Debug.Log("State is: " + State);

        updateGameState(State);
    }

    IEnumerator handleOpponentTurn() {
        Debug.Log("Opponent took a turn");

        yield return new WaitForSeconds(2f);

        if(player1.health.dead) {
            State = Gamestate.Lose;
        } else {
            State = Gamestate.PlayerTurn;
        }

        updatePlayerText();
        updateGameState(State);
    }

    void handleWin() {
        Debug.Log("YOU WON!");
        notificationText.text = "YOU WIN!";
    }

    void handleLoss() {
        Debug.Log("YOU LOST!");
        notificationText.text = "YOU LOSE!";
    }
    */
}

public enum Gamestate {
    PlayerTurn,
    OpponentTurn,
    Win,
    Lose
}
