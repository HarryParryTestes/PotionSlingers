using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager manager;
    private NetworkManager networkManager;

    // Later change to be however many players joined when the GameScene loads in.
    // (Maybe transferring numPlayers from MainMenu script to here? Maybe get from server?)
    public int numPlayers;
    public int selectedCardInt;
    public int selectedOpponentInt;
    public int loadedCardInt;
    public int currentPlayer = 0;
    public Player[] players = new Player[4];
    public Character[] characters;
    GameObject ob;
    GameObject obTop;
    GameObject obLeft;
    GameObject obRight;
    public TrashDeck td;
    MarketDeck md1;
    MarketDeck md2;
    public List<GameObject> successMessages;
    public List<GameObject> errorMessages;
    private MessageQueue msgQueue;

    public CharacterDisplay op1;
    public CharacterDisplay op2;
    public CharacterDisplay op3;
    public TMPro.TextMeshProUGUI playerBottomName;
    public TMPro.TextMeshProUGUI playerLeftName;
    public TMPro.TextMeshProUGUI playerTopName;
    public TMPro.TextMeshProUGUI playerRightName;
    public GameObject attackMenu;
    public GameObject loadMenu;

    GameObject mainMenu;
    MainMenu mainMenuScript;

    GameObject player1Area;
    GameObject player2Area;

    void Awake()
    {
        manager = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //menu = GameObject.Find("MainMenuScript").GetComponent<MainMenu>();
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        msgQueue = networkManager.GetComponent<MessageQueue>();
        msgQueue.AddCallback(Constants.SMSG_P_THROW, onResponsePotionThrow);
        msgQueue.AddCallback(Constants.SMSG_END_TURN, onResponseEndTurn);
        msgQueue.AddCallback(Constants.SMSG_BUY, onResponseBuy);
        msgQueue.AddCallback(Constants.SMSG_SELL, onResponseSell);
        msgQueue.AddCallback(Constants.SMSG_CYCLE, onResponseCycle);
        msgQueue.AddCallback(Constants.SMSG_TRASH, onResponseTrash);
        msgQueue.AddCallback(Constants.SMSG_LOAD, onResponseLoad);

        mainMenu = GameObject.Find("MainMenuScript");
        mainMenuScript = mainMenu.GetComponent<MainMenu>();

        numPlayers = mainMenuScript.getNumPlayers();
        Debug.Log("NumPlayers is: " + numPlayers);
        Debug.Log("P1 ID is: " + mainMenuScript.p1UserId);
        Debug.Log("P2 ID is: " + mainMenuScript.p2UserId);

        init();
    }

    public void init()
    {
        initPlayers();
        initDecks();
    }

    void initPlayers()
    {
        // Player1 = Client1 set up
        ob = GameObject.Find("CharacterCard");
        obTop = GameObject.Find("CharacterCard (Top)");
        obLeft = GameObject.Find("CharacterCard (Left)");
        obRight = GameObject.Find("CharacterCard (Right)");

        CharacterDisplay bottomCharacter = ob.GetComponent<CharacterDisplay>();
        CharacterDisplay topCharacter = obTop.GetComponent<CharacterDisplay>();
        CharacterDisplay leftCharacter = obLeft.GetComponent<CharacterDisplay>();
        CharacterDisplay rightCharacter = obRight.GetComponent<CharacterDisplay>();

        // Children objects of attackMenu
        GameObject leftAttack = attackMenu.transform.Find("CharacterCard (Left)").gameObject;
        GameObject topAttack = attackMenu.transform.Find("CharacterCard (Top)").gameObject;
        GameObject rightAttack = attackMenu.transform.Find("CharacterCard (Right)").gameObject;

        // Took this if + else if statements out of if(numPlayers == 2) check
        // because it wasn't running for some reason.
        // TO FIX: Need to fix to act differently depending on number of players!

        // Player 1 setup (attempt to be less hard coded)
        if (Constants.USER_ID == mainMenuScript.p1UserId)
        {
            players[0] = ob.GetComponent<Player>();
            // Sets mainPlayer area belonging to this client's user.
            players[0].user_id = Constants.USER_ID;

            players[0].name = mainMenuScript.p1Name;
            players[0].charName = mainMenuScript.p1CharCard.character.cardName;
            playerBottomName.text = players[0].name;

            // Updating PlayerArea CharacterDisplay to belong to P1's chosen character
            bottomCharacter.character = mainMenuScript.p1CharCard.character;
            bottomCharacter.updateCharacter(bottomCharacter.character);
            players[0].character = bottomCharacter;

            switch(numPlayers) {
                case 2: 
                    players[1] = obTop.GetComponent<Player>();
                    players[1].user_id = mainMenuScript.p2UserId;
                    players[1].charName = mainMenuScript.p2CharCard.character.cardName;
                    playerTopName.text = mainMenuScript.p2Name;
                    topCharacter.character = mainMenuScript.p2CharCard.character;
                    topCharacter.updateCharacter(topCharacter.character);
                    players[1].character = topCharacter;
                    obLeft.transform.parent.gameObject.SetActive(false);
                    obRight.transform.parent.gameObject.SetActive(false);
                    leftAttack.SetActive(false);
                    rightAttack.SetActive(false);
                    break;
                case 3:
                    // TO DO: Finish configuring 
                    // players[1] = obLeft.GetComponent<Player>();
                    // players[2] = obRight.GetComponent<Player>();
                    // players[1].user_id = mainMenuScript.p2UserId;
                    // players[2].user_id = mainMenuScript.p3UserId;
                    // playerLeftName.text = mainMenuScript.p2Name;
                    // playerRightName.text = mainMenuScript.p3Name;
                    break;
                case 4:
                    // TO DO: Finish configuring 
                    // players[1] = obLeft.GetComponent<Player>();
                    // players[2] = obTop.GetComponent<Player>();
                    // players[3] = obRight.GetComponent<Player>();
                    // players[1].user_id = mainMenuScript.p2UserId;
                    // players[2].user_id = mainMenuScript.p3UserId;
                    // players[3].user_id = mainMenuScript.p3UserId;
                    break;
                default: Debug.Log("ERROR in initPlayers()"); break;
            }
            
        }
        // Player 2 = Client 2 setup
        else if(Constants.USER_ID == mainMenuScript.p2UserId)
        {
            players[0] = ob.GetComponent<Player>();
            // Sets mainPlayer area belonging to this client's user.
            players[0].user_id = Constants.USER_ID;

            players[0].name = mainMenuScript.p2Name;
            players[0].charName = mainMenuScript.p2CharCard.character.cardName;
            playerBottomName.text = players[0].name;

            // Updating PlayerArea CharacterDisplay to belong to P2's chosen character
            bottomCharacter.character = mainMenuScript.p2CharCard.character;
            bottomCharacter.updateCharacter(bottomCharacter.character);
            players[0].character = bottomCharacter;

            switch(numPlayers) {
                case 2: 
                    players[1] = obTop.GetComponent<Player>();
                    players[1].user_id = mainMenuScript.p1UserId;
                    players[1].charName = mainMenuScript.p1CharCard.character.cardName;
                    playerTopName.text = mainMenuScript.p1Name;
                    topCharacter.character = mainMenuScript.p1CharCard.character;
                    topCharacter.updateCharacter(topCharacter.character);
                    players[1].character = topCharacter;
                    obLeft.transform.parent.gameObject.SetActive(false);
                    obRight.transform.parent.gameObject.SetActive(false);
                    leftAttack.SetActive(false);
                    rightAttack.SetActive(false);
                    break;
                case 3:
                    // TO DO: Finish configuring 
                    break;
                case 4:
                    // TO DO: Finish configuring 
                    break;
                default: Debug.Log("ERROR in initPlayers()"); break;
            }
        }

        /*
        if(numPlayers == 2)
        {
            // gotta fix this
            /*
            GameObject obj = GameObject.Find("EnemyArea (Right Side)");
            GameObject obj2 = GameObject.Find("EnemyArea (Left Side)");
            obj.SetActive(false);
            obj2.SetActive(false);
            /

            // player 1 setup
            if (Constants.USER_ID == 1)
            {
                // we're good
                playerBottomName.text = mainMenuScript.p1Name;
                playerTopName.text = mainMenuScript.p2Name;
            }
            // Player 2 = Client2 setup
            else if(Constants.USER_ID == 2)
            {
                // switching players around for p2 to be at bottom

                // this could be wrong so I'm scrapping it for now
                /*
                players[3] = players[0];
                players[0] = players[1];
                players[1] = players[3];
                /

                // currentPlayer = 1;
                playerBottomName.text = mainMenuScript.p2Name;
                playerTopName.text = mainMenuScript.p1Name;
                players[0] = obTop.GetComponent<Player>();
                players[1] = ob.GetComponent<Player>();
            }
        }
        */

        else if (numPlayers == 3)
        {
            // TODO

        } else if(numPlayers == 4)
        {
            // TODO
        }

        //ob = GameObject.Find("CharacterCard");
        //players[0] = ob.GetComponent<Player>();
        //ob2 = GameObject.Find("CharacterCard (Top)");
        //players[1] = ob2.GetComponent<Player>();

        /*
        foreach (CardDisplay cd in players[2].holster.cardList)
        {
            cd.updateCard(players[0].deck.placeholder);
        }

        foreach (CardDisplay cd in players[3].holster.cardList)
        {
            cd.updateCard(players[0].deck.placeholder);
        }
        */
    }

    void initDecks()
    {
        //td = GameObject.Find("TrashPile").GetComponent<TrashDeck>();
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
        currentPlayer++;
        if(currentPlayer == numPlayers)
        {
            currentPlayer = 0;
        }
        onStartTurn(players[currentPlayer]);
        Debug.Log("Request End Turn");
        bool connected = networkManager.sendEndTurnRequest(currentPlayer);
    }

    public void onResponseEndTurn(ExtendedEventArgs eventArgs)
    {
        Debug.Log("End Turn!");
        ResponseEndTurnEventArgs args = eventArgs as ResponseEndTurnEventArgs;
        Debug.Log("Current Player: " + args.w);
        Debug.Log("User ID: " + args.user_id);

        // if it didn't come from your own client, change the player
        if (Constants.USER_ID != args.user_id)
        {
            // player 1 just ended turn in 2p game
            if(args.user_id == 1)
            {
                currentPlayer = 1;
                // player 2 just ended turn in 2p game
            } else if(args.user_id == 2)
            {
                if(numPlayers > 2)
                {
                    currentPlayer = 2;
                }
                else
                {
                    currentPlayer = 0;
                }
            } else if (args.user_id == 3)
            {
                if(numPlayers > 3)
                {
                    currentPlayer = 3;
                } else
                {
                    currentPlayer = 0;
                }
            }
            onStartTurn(players[currentPlayer]);
        }
    }

    public void onResponseBuy(ExtendedEventArgs eventArgs)
    {
        Debug.Log("ResponseBuy");
        ResponseBuyEventArgs args = eventArgs as ResponseBuyEventArgs;
        Debug.Log("ID: " + args.user_id);
        Debug.Log("cardInt: " + args.x);
        Debug.Log("Price: " + args.y);
        Debug.Log("T or B: " + args.z);
        if (Constants.USER_ID != args.user_id)
        {
            // request coming fom p1
            if(args.user_id == 1)
            {
                // if top market
                if(args.z == 1)
                {
                    switch (args.x)
                    {
                        case 1:
                            players[currentPlayer].pips -= md1.cardDisplay1.card.buyPrice;
                            players[currentPlayer].deck.putCardOnTop(md1.cardDisplay1.card);
                            Card card = md1.popCard();
                            md1.cardDisplay1.updateCard(card);
                            break;
                        case 2:
                            players[currentPlayer].pips -= md1.cardDisplay2.card.buyPrice;
                            players[currentPlayer].deck.putCardOnTop(md1.cardDisplay2.card);
                            Card card2 = md1.popCard();
                            md1.cardDisplay2.updateCard(card2);
                            break;
                        case 3:
                            players[currentPlayer].pips -= md1.cardDisplay3.card.buyPrice;
                            players[currentPlayer].deck.putCardOnTop(md1.cardDisplay3.card);
                            Card card3 = md1.popCard();
                            md1.cardDisplay3.updateCard(card3);
                            break;
                    }
                }
                else
                {
                    switch (args.x)
                    {
                        case 1:
                            players[currentPlayer].pips -= md2.cardDisplay1.card.buyPrice;
                            players[currentPlayer].deck.putCardOnTop(md2.cardDisplay1.card);
                            Card card4 = md2.popCard();
                            md2.cardDisplay1.updateCard(card4);
                            break;
                        case 2:
                            players[currentPlayer].pips -= md2.cardDisplay2.card.buyPrice;
                            players[currentPlayer].deck.putCardOnTop(md2.cardDisplay2.card);
                            Card card5 = md2.popCard();
                            md2.cardDisplay2.updateCard(card5);
                            break;
                        case 3:
                            players[currentPlayer].pips -= md2.cardDisplay3.card.buyPrice;
                            players[currentPlayer].deck.putCardOnTop(md2.cardDisplay3.card);
                            Card card6 = md2.popCard();
                            md2.cardDisplay3.updateCard(card6);
                            break;
                    }
                }
            }
        } else if(args.user_id == 2)
        {
            // if top market
            if (args.z == 1)
            {
                switch (args.x)
                {
                    case 1:
                        players[currentPlayer].pips -= md1.cardDisplay1.card.buyPrice;
                        players[currentPlayer].deck.putCardOnTop(md1.cardDisplay1.card);
                        Card card = md1.popCard();
                        md1.cardDisplay1.updateCard(card);
                        break;
                    case 2:
                        players[currentPlayer].pips -= md1.cardDisplay2.card.buyPrice;
                        players[currentPlayer].deck.putCardOnTop(md1.cardDisplay2.card);
                        Card card2 = md1.popCard();
                        md1.cardDisplay2.updateCard(card2);
                        break;
                    case 3:
                        players[currentPlayer].pips -= md1.cardDisplay3.card.buyPrice;
                        players[currentPlayer].deck.putCardOnTop(md1.cardDisplay3.card);
                        Card card3 = md1.popCard();
                        md1.cardDisplay3.updateCard(card3);
                        break;
                }
            }
            else
            {
                switch (args.x)
                {
                    case 1:
                        players[currentPlayer].pips -= md2.cardDisplay1.card.buyPrice;
                        players[currentPlayer].deck.putCardOnTop(md2.cardDisplay1.card);
                        Card card4 = md2.popCard();
                        md2.cardDisplay1.updateCard(card4);
                        break;
                    case 2:
                        players[currentPlayer].pips -= md2.cardDisplay2.card.buyPrice;
                        players[currentPlayer].deck.putCardOnTop(md2.cardDisplay2.card);
                        Card card5 = md2.popCard();
                        md2.cardDisplay2.updateCard(card5);
                        break;
                    case 3:
                        players[currentPlayer].pips -= md2.cardDisplay3.card.buyPrice;
                        players[currentPlayer].deck.putCardOnTop(md2.cardDisplay3.card);
                        Card card6 = md2.popCard();
                        md2.cardDisplay3.updateCard(card6);
                        break;
                }
            }
        }
    }

    public void onResponseSell(ExtendedEventArgs eventArgs)
    {
        Debug.Log("ResponseSell");
        ResponseSellEventArgs args = eventArgs as ResponseSellEventArgs;
        Debug.Log("ID: " + args.user_id);
        Debug.Log("cardInt: " + args.x);
        Debug.Log("Sell Price: " + args.y);

        if (Constants.USER_ID != args.user_id)
        {
            players[currentPlayer].pips += players[currentPlayer].holster.cardList[args.x - 1].card.sellPrice;
            td.addCard(players[currentPlayer].holster.cardList[args.x - 1]);

        }
    }

    public void onResponseCycle(ExtendedEventArgs eventArgs)
    {
        // TODO
        Debug.Log("Cycle Response");
        ResponseCycleEventArgs args = eventArgs as ResponseCycleEventArgs;

        if (Constants.USER_ID != args.user_id)
        {
            players[currentPlayer].pips--;
            players[currentPlayer].deck.putCardOnBottom(players[currentPlayer].holster.cardList[args.x - 1].card);
            players[currentPlayer].holster.cardList[args.x - 1].updateCard(players[0].holster.card1.placeholder);
        }
    }

    public void onResponseTrash(ExtendedEventArgs eventArgs)
    {
        // args.x is cardInt
        Debug.Log("Trash Response");
        ResponseTrashEventArgs args = eventArgs as ResponseTrashEventArgs;

        if(Constants.USER_ID != args.user_id)
        {
            td.addCard(players[currentPlayer].holster.cardList[args.x - 1]);
        }
    }

    public void onResponseLoad(ExtendedEventArgs eventArgs)
    {
        Debug.Log("Load Response");
        ResponseLoadEventArgs args = eventArgs as ResponseLoadEventArgs;

        // TODO
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
                    if (players[currentPlayer].ringBonus && players[currentPlayer].potionsThrown == 0)
                    {
                        damage++;
                    }
                    // send protocol to server
                    // also check if they're the current player
                    //if(Constants.USER_ID - 1 == currentPlayer)
                    //{
                    bool connected = networkManager.SendThrowPotionRequest(damage, currentPlayer + 1, selectedCardInt, selectedOpponentInt);
                    td.addCard(players[currentPlayer].holster.cardList[selectedCardInt - 1]);
                    players[currentPlayer].potionsThrown++;
                    sendSuccessMessage(2);
                    break;
                    //}
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
                    if (players[currentPlayer].ringBonus && players[currentPlayer].potionsThrown == 0)
                    {
                        damage++;
                    }
                    // send protocol to server
                    // also check if they're the current player
                    if (Constants.USER_ID - 1 == currentPlayer)
                    {
                        bool connected = networkManager.SendThrowPotionRequest(damage, currentPlayer + 1, selectedCardInt, selectedOpponentInt);
                        td.addCard(players[currentPlayer].holster.cardList[selectedCardInt - 1]);
                        players[currentPlayer].potionsThrown++;
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
                }
                break;
            case 3:
                if (players[currentPlayer].holster.card3.card.cardType == "Potion")
                {
                    damage = players[currentPlayer].holster.card3.card.effectAmount;
                    if (players[currentPlayer].ringBonus && players[currentPlayer].potionsThrown == 0)
                    {
                        damage++;
                    }
                    // send protocol to server
                    // also check if they're the current player
                    if (Constants.USER_ID - 1 == currentPlayer)
                    {
                        bool connected = networkManager.SendThrowPotionRequest(damage, currentPlayer + 1, selectedCardInt, selectedOpponentInt);
                        td.addCard(players[currentPlayer].holster.cardList[selectedCardInt - 1]);
                        players[currentPlayer].potionsThrown++;
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
                }
                break;
            case 4:
                if (players[currentPlayer].holster.card4.card.cardType == "Potion")
                {
                    damage = players[currentPlayer].holster.card4.card.effectAmount;
                    if (players[currentPlayer].ringBonus && players[currentPlayer].potionsThrown == 0)
                    {
                        damage++;
                    }
                    // send protocol to server
                    // also check if they're the current player
                    if (Constants.USER_ID - 1 == currentPlayer)
                    {
                        bool connected = networkManager.SendThrowPotionRequest(damage, currentPlayer + 1, selectedCardInt, selectedOpponentInt);
                        td.addCard(players[currentPlayer].holster.cardList[selectedCardInt - 1]);
                        players[currentPlayer].potionsThrown++;
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
    // TODO: fix this to display properly
    public void displayPotions()
    {
        int set = 0;
        loadMenu.transform.Find("Card (Left)").gameObject.SetActive(true);
        loadMenu.transform.Find("Card (Middle)").gameObject.SetActive(true);
        loadMenu.transform.Find("Card (Right)").gameObject.SetActive(true);
        CardDisplay left = loadMenu.transform.Find("Card (Left)").GetComponent<CardDisplay>();
        CardDisplay middle = loadMenu.transform.Find("Card (Middle)").GetComponent<CardDisplay>();
        CardDisplay right = loadMenu.transform.Find("Card (Right)").GetComponent<CardDisplay>();
        foreach (CardDisplay cd in players[currentPlayer].holster.cardList)
        {
            // Debug.Log("CardName is: " + cd.card.cardName + ". CardType is: " + cd.card.cardType);
            // if(cd.card.cardType.ToLower() == "potion")
            if(cd.card.cardType.ToLower() == "artifact" || cd.card.cardType.ToLower() == "vessel")
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
        if(set == 1)
        {
            // middle.updateCard(players[currentPlayer].deck.placeholder);
            // right.updateCard(players[currentPlayer].deck.placeholder);
            loadMenu.transform.Find("Card (Middle)").gameObject.SetActive(false);
            loadMenu.transform.Find("Card (Right)").gameObject.SetActive(false);
        }
        if(set == 2)
        {
            // right.updateCard(players[currentPlayer].deck.placeholder);
            loadMenu.transform.Find("Card (Right)").gameObject.SetActive(false);
        }
    }

    public void loadPotion()
    {
        Debug.Log("Load Potion");

        // TODO: Send potion with loadedCardInt to loaded CardDisplay of card in selectedCardInt

        // if it's an artifact or vessel
        if(players[currentPlayer].holster.cardList[selectedCardInt - 1].card.cardType == "Artifact" ||
            players[currentPlayer].holster.cardList[selectedCardInt - 1].card.cardType == "Vessel")
        {
            // do something

            // test for protocol, must replace parameters later
            bool connected = networkManager.sendLoadRequest(0, 0);
        }
    }

    public void setLoadedInt(int cardInt)
    {
        loadedCardInt = cardInt;
    }

    // TODO: fix this to display properly
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
                    bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay1.card.buyPrice, 1);
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
                    bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay2.card.buyPrice, 1);
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
                    bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay3.card.buyPrice, 1);
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
                    bool connected = networkManager.sendBuyRequest(md2.cardInt, md2.cardDisplay1.card.buyPrice, 0);
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
                    bool connected = networkManager.sendBuyRequest(md2.cardInt, md2.cardDisplay2.card.buyPrice, 0);
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
                    bool connected = networkManager.sendBuyRequest(md2.cardInt, md2.cardDisplay3.card.buyPrice, 0);
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
        bool connected = networkManager.sendSellRequest(selectedCardInt, players[currentPlayer].holster.cardList[selectedCardInt - 1].card.sellPrice);
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
            bool connected = networkManager.sendCycleRequest(selectedCardInt, 0);
            sendSuccessMessage(7);
            
        }
        else if (players[currentPlayer].pips < 1)
        {
            sendErrorMessage(5);
        }
        else
        {
            players[currentPlayer].pips--;
            players[currentPlayer].deck.putCardOnBottom(players[currentPlayer].holster.cardList[selectedCardInt - 1].card);
            players[currentPlayer].holster.cardList[selectedCardInt - 1].updateCard(players[0].holster.card1.placeholder);
            bool connected = networkManager.sendCycleRequest(selectedCardInt, 1);
            sendSuccessMessage(7);
        }
    }

    public void onResponsePotionThrow(ExtendedEventArgs eventArgs)
    {
        ResponsePotionThrowEventArgs args = eventArgs as ResponsePotionThrowEventArgs;
        Debug.Log("Constant: " + Constants.USER_ID);
        Debug.Log("Damage: " + args.w);
        Debug.Log("User ID: " + args.user_id);
        Debug.Log("Current Player? " + args.x);
        Debug.Log("Card Int: " + args.y);
        Debug.Log("Opponent ID: " + args.z);

        // if request didn't come from player
        if (Constants.USER_ID != args.user_id)
        {
            // p1 request
            if (args.user_id == 1)
            {
                // player 2
                if (args.z == 1)
                {
                    Debug.Log("Change this client");
                    td.addCard(players[currentPlayer].holster.cardList[args.y - 1]);
                    players[0].subHealth(args.w);
                }
            }
            // p2 request
            else if (args.user_id == 2)
            {
                // player 1
                if (args.z == 1)
                {
                    Debug.Log("Change this client");
                    td.addCard(players[currentPlayer].holster.cardList[args.y - 1]);
                    players[1].subHealth(args.w);
                }
            }
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
