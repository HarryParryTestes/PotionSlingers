using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager manager;
    private NetworkManager networkManager;
    public int numPlayers = 2;
    public int selectedCardInt;
    public int selectedOpponentInt;
    int currentPlayer = 0;
    public Player[] players = new Player[4];
    public Character[] characters;
    GameObject ob;
    GameObject ob2;
    GameObject ob3;
    GameObject ob4;
    TrashDeck td;
    MarketDeck md1;
    MarketDeck md2;
    public List<GameObject> successMessages;
    public List<GameObject> errorMessages;
    private MessageQueue msgQueue;

    public CharacterDisplay op1;
    public CharacterDisplay op2;
    public CharacterDisplay op3;

    void Awake()
    {
        manager = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        msgQueue = networkManager.GetComponent<MessageQueue>();
        msgQueue.AddCallback(Constants.SMSG_P_THROW, onResponsePotionThrow);
        init();
    }

    public void init()
    {
        initPlayers();
        initDecks();
    }

    void initPlayers()
    {
        if(numPlayers == 2)
        {
            ob = GameObject.Find("CharacterCard");
            ob2 = GameObject.Find("CharacterCard (Top)");
            // player 1 setup
            if (Constants.USER_ID == 1)
            {
                players[0] = ob.GetComponent<Player>();
                players[1] = ob2.GetComponent<Player>();
            }
            // player 2 setup
            else if(Constants.USER_ID == 2)
            {
                players[0] = ob2.GetComponent<Player>();
                players[1] = ob.GetComponent<Player>();
            }
        }
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

    void initDecks()
    {
        td = GameObject.Find("TrashPile").GetComponent<TrashDeck>();
        md1 = GameObject.Find("PotionPile").GetComponent<MarketDeck>();
        md1.init();
        md2 = GameObject.Find("SpecialCardPile").GetComponent<MarketDeck>();
        md2.init();
    }

    // if there are open spots in the holster, move cards from deck to holster
    public void onStartTurn(Player player)
    {
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
        player.setDefaultTurn();
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

    public void throwPotion()
    {
        int damage = 0;
        Debug.Log("GameManager Throw Potion");

        // check card type
        switch (selectedCardInt)
        {
            case 1:
                if (players[currentPlayer].holster.card1.card.cardType == "Potion")
                {
                    damage = players[currentPlayer].holster.card1.card.effectAmount;
                    td.addCard(players[currentPlayer].holster.cardList[selectedCardInt - 1]);
                    if (players[currentPlayer].ringBonus)
                    {
                        damage += players[currentPlayer].bonusAmount;
                    }
                    // send protocol to server
                    bool connected = networkManager.SendThrowPotionRequest(currentPlayer + 1, selectedCardInt, selectedOpponentInt);
                    sendSuccessMessage(2);
                    break;
                } else if(players[currentPlayer].holster.card1.card.cardType == "Vessel")
                {

                }
                else if (players[currentPlayer].holster.card1.card.cardType == "Artifact")
                {

                }
                break;

            case 2:
                if (players[currentPlayer].holster.card2.card.cardType == "Potion")
                {
                    damage = players[currentPlayer].holster.card2.card.effectAmount;
                    td.addCard(players[currentPlayer].holster.card2);
                    sendSuccessMessage(2);
                    break;
                }
                else if (players[currentPlayer].holster.card2.card.cardType == "Vessel")
                {

                }
                else if (players[currentPlayer].holster.card2.card.cardType == "Artifact")
                {

                }
                break;
            case 3:
                if (players[currentPlayer].holster.card3.card.cardType == "Potion")
                {
                    damage = players[currentPlayer].holster.card3.card.effectAmount;
                    td.addCard(players[currentPlayer].holster.card3);
                    sendSuccessMessage(2);
                    break;
                }
                else if (players[currentPlayer].holster.card3.card.cardType == "Vessel")
                {

                }
                else if (players[currentPlayer].holster.card3.card.cardType == "Artifact")
                {

                }
                break;
            case 4:
                if (players[currentPlayer].holster.card4.card.cardType == "Potion")
                {
                    damage = players[currentPlayer].holster.card4.card.effectAmount;
                    td.addCard(players[currentPlayer].holster.card4);
                    sendSuccessMessage(2);
                    break;
                }
                else if (players[currentPlayer].holster.card4.card.cardType == "Vessel")
                {

                }
                else if (players[currentPlayer].holster.card4.card.cardType == "Artifact")
                {

                }
                break;
            default: damage = 0;
                break;
        }
    }

    public void setSCInt(int num)
    {
        selectedCardInt = num;
    }

    public void setOPInt(int num)
    {
        selectedOpponentInt = num;
    }

    // find potions and display them in LoadItemMenu
    public void displayPotions()
    {
        int set = 0;
        CardDisplay left = GameObject.Find("Card (Left)").GetComponent<CardDisplay>();
        CardDisplay middle = GameObject.Find("Card (Middle)").GetComponent<CardDisplay>();
        CardDisplay right = GameObject.Find("Card (Right)").GetComponent<CardDisplay>();
        foreach (CardDisplay cd in players[currentPlayer].holster.cardList)
        {
           if(cd.card.cardType == "Potion")
            {
                switch (set)
                {
                    case 0:
                        left.updateCard(cd.card);
                        set++;
                        break;
                    case 1:
                        middle.updateCard(cd.card);
                        set++;
                        break;
                    case 2:
                        right.updateCard(cd.card);
                        break;
                    default:
                        break;
                }
            }
        }
        if(set == 1)
        {
            middle.updateCard(players[currentPlayer].deck.placeholder);
            right.updateCard(players[currentPlayer].deck.placeholder);
        }
        if(set == 2)
        {
            right.updateCard(players[currentPlayer].deck.placeholder);
        }
    }

    public void displayOpponents()
    {
        int set = 0;
        foreach (Player player in players)
        {
            if(player.user_id != players[currentPlayer].user_id)
            {
                if(set == 0)
                {
                    foreach(Character character in characters)
                    {
                        if(player.charName == character.cardName)
                        {
                            op1.updateCharacter(character);
                            set++;
                        }
                    }
                }

                if (set == 1)
                {
                    foreach (Character character in characters)
                    {
                        if (player.charName == character.cardName)
                        {
                            op2.updateCharacter(character);
                            set++;
                        }
                    }
                }

                if (set == 2)
                {
                    foreach (Character character in characters)
                    {
                        if (player.charName == character.cardName)
                        {
                            op3.updateCharacter(character);
                        }
                    }
                }
            }
        }
    }

    // subtract pips, update deck display and market display
    public void topMarketBuy()
    {
        Debug.Log("Top Market Buy");
        switch (md1.cardInt)
        {
            case 1:
                if(players[currentPlayer].pips >= md1.cardDisplay1.card.buyPrice)
                {
                    players[currentPlayer].pips -= md1.cardDisplay1.card.buyPrice;
                    players[currentPlayer].deck.putCardOnTop(md1.cardDisplay1.card);
                    Card card = md1.popCard();
                    md1.cardDisplay1.updateCard(card);
                    sendSuccessMessage(1);
                } else
                {
                    sendErrorMessage(6);
                }
                break;
            case 2:
                if (players[currentPlayer].pips >= md1.cardDisplay2.card.buyPrice)
                {
                    players[currentPlayer].pips -= md1.cardDisplay2.card.buyPrice;
                    players[currentPlayer].deck.putCardOnTop(md1.cardDisplay2.card);
                    Card card = md1.popCard();
                    md1.cardDisplay2.updateCard(card);
                    sendSuccessMessage(1);
                }
                else
                {
                    sendErrorMessage(6);
                }
                break;
            case 3:
                if (players[currentPlayer].pips >= md1.cardDisplay3.card.buyPrice)
                {
                    players[currentPlayer].pips -= md1.cardDisplay3.card.buyPrice;
                    players[currentPlayer].deck.putCardOnTop(md1.cardDisplay3.card);
                    Card card = md1.popCard();
                    md1.cardDisplay3.updateCard(card);
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

    public void bottomMarketBuy()
    {
        Debug.Log("Bottom Market Buy");
        switch (md2.cardInt)
        {
            case 1:
                if (players[currentPlayer].pips >= md2.cardDisplay1.card.buyPrice)
                {
                    players[currentPlayer].pips -= md2.cardDisplay1.card.buyPrice;
                    players[currentPlayer].deck.putCardOnTop(md2.cardDisplay1.card);
                    Card card = md2.popCard();
                    md2.cardDisplay1.updateCard(card);
                    sendSuccessMessage(1);
                }
                else
                {
                    sendErrorMessage(6);
                }
                break;
            case 2:
                if (players[currentPlayer].pips >= md2.cardDisplay2.card.buyPrice)
                {
                    players[currentPlayer].pips -= md2.cardDisplay2.card.buyPrice;
                    players[currentPlayer].deck.putCardOnTop(md2.cardDisplay2.card);
                    Card card = md2.popCard();
                    md2.cardDisplay2.updateCard(card);
                    sendSuccessMessage(1);
                }
                else
                {
                    sendErrorMessage(6);
                }
                break;
            case 3:
                if (players[currentPlayer].pips >= md2.cardDisplay3.card.buyPrice)
                {
                    players[currentPlayer].pips -= md2.cardDisplay3.card.buyPrice;
                    players[currentPlayer].deck.putCardOnTop(md2.cardDisplay3.card);
                    Card card = md2.popCard();
                    md2.cardDisplay3.updateCard(card);
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

    public void sellCard()
    {
        Debug.Log("Sell Card");
        players[currentPlayer].pips += players[currentPlayer].holster.cardList[selectedCardInt - 1].card.sellPrice;
        td.addCard(players[currentPlayer].holster.cardList[selectedCardInt - 1]);
        sendSuccessMessage(8);
    }

    public void trashCard()
    {
        Debug.Log("Trash Card");
        td.addCard(players[currentPlayer].holster.cardList[selectedCardInt - 1]);
        sendSuccessMessage(9);
    }

    public void cycleCard()
    {
        Debug.Log("Cycle Card");
        if(players[currentPlayer].holster.cardList[selectedCardInt - 1].card.cardType == "Potion")
        {
            players[currentPlayer].deck.putCardOnBottom(players[currentPlayer].holster.cardList[selectedCardInt - 1].card);
            players[currentPlayer].holster.cardList[selectedCardInt - 1].updateCard(players[0].holster.card1.placeholder);
            sendSuccessMessage(7);
            
        }
        else if (players[currentPlayer].pips < 1)
        {
            sendErrorMessage(5);
        }
        else
        {
            players[currentPlayer].deck.putCardOnBottom(players[currentPlayer].holster.cardList[selectedCardInt - 1].card);
            players[currentPlayer].holster.cardList[selectedCardInt - 1].updateCard(players[0].holster.card1.placeholder);
            sendSuccessMessage(7);
        }
    }

    public void onResponsePotionThrow(ExtendedEventArgs eventArgs)
    {
        ResponsePotionThrowEventArgs args = eventArgs as ResponsePotionThrowEventArgs;
        Debug.Log("User ID: " + args.user_id);
        Debug.Log("Current Player? " + args.x);
        Debug.Log("Card Int: " + args.y);
        Debug.Log("Opponent ID: " + args.z);

        // if request didn't come from player
        if (Constants.USER_ID != args.user_id)
        {
            Debug.Log("Change this client");
            td.addCard(players[args.user_id - 1].holster.cardList[args.y - 1]);
        }
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

    IEnumerator waitThreeSeconds(GameObject gameObj)
    {
        yield return new WaitForSeconds(3);
        gameObj.SetActive(false);
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
