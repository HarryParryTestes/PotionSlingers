using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Player player1;
    public static Player player2;
    public static GameManager manager;

    //delete once this is production ready
    public CharacterDisplay playerCharacter;
    public CharacterDisplay enemyCharacter;
    public Gamestate State;
    public Holster playerHolster;
    public Holster enemyHolster;

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
        SetUp();
        while(State != Gamestate.Lose && State != Gamestate.Win) {
            updateGameState(State);
        }
        updateGameState(State);
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    void updateGameState(Gamestate newState) {
        State = newState;

        switch (newState) {
            case Gamestate.PlayerTurn:
                handlePlayerTurn();
                break;
            case Gamestate.OpponentTurn:
                handleOpponentTurn();
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

    void handlePlayerTurn() {
        player2.health.subHealth(10);
        if(player2.dead) {
            State = Gamestate.Win;
        } else {
            State = Gamestate.OpponentTurn;
        }
    }

    void handleOpponentTurn() {
        Debug.Log("Opponent took a turn");
        State = Gamestate.PlayerTurn;
    }

    void handleWin() {
        Debug.Log("YOU WON!");
    }
}

public enum Gamestate {
    PlayerTurn,
    OpponentTurn,
    Win,
    Lose
}
