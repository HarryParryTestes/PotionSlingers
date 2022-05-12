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
    public string selectedOpponentCharName;
    public int loadedCardInt;
    public int myPlayerIndex = 0; // used to be currentPlayer
    public int currentPlayerId;
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

    public CharacterDisplay opLeft;
    public CharacterDisplay opTop;
    public CharacterDisplay opRight;
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
        GameObject leftAttack = attackMenu.transform.Find("AttackCharacter (Left)").gameObject;
        GameObject topAttack = attackMenu.transform.Find("AttackCharacter (Top)").gameObject;
        GameObject rightAttack = attackMenu.transform.Find("AttackCharacter (Right)").gameObject;

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
                    players[1].name = mainMenuScript.p2Name;
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
            // myPlayerIndex = 1;
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
                    players[1].name = mainMenuScript.p1Name;
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

                // myPlayerIndex = 1;
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

        // Inits all player's health and essence (HP) cubes.
        for(int i = 0; i < numPlayers; i++) {
            players[i].initHealth();
            // Sets first joined player to be the first current player.
            if(players[i].user_id == mainMenuScript.p1UserId) {
                currentPlayerId = players[i].user_id;
                Debug.Log("CurrentPlayerID is: " + currentPlayerId);
                players[i].setCurrentPlayer();
            }
            else
            {
                players[i].removeCurrentPlayer();
            }
            if(players[i].user_id == Constants.USER_ID)
            {
                myPlayerIndex = i;
            }
        }
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

    public void setSCInt(int num)
    {
        selectedCardInt = num;
    }

    public void setOPInt(int num)
    {
        selectedOpponentInt = num;
    }

    public void setOPName(string characterName)
    {
        selectedOpponentCharName = characterName;
    }

    public void setLoadedInt(string cardName)
    {
        for(int i = 0; i < 4; i++)
        {
            if(players[myPlayerIndex].holster.cardList[i].card.cardName == cardName)
            {
                loadedCardInt = i;
            }
        }
    }

    // DONE: fix this to display properly
    // DISPLAY OPPONENTS
    public void displayOpponents()
    {
        // Displaying opponents to attack for 2 player game.
        if(numPlayers == 2) 
        {
            // For all players that are not this client's player, display their character in attackMenu.
            for (int i = 0; i < numPlayers; i++)
            {
                if(players[i].user_id != Constants.USER_ID) 
                {
                    // Updates middle slot in attackMenu.
                    opTop.updateCharacter(players[i].character.character);
                }
            }
        }

        // Displaying opponents to attack for 3 player game.
        if(numPlayers == 3) 
        {
            int tracker = 0;
            // For all players that are not this client's player, display their character in attackMenu.
            for (int i = 0; i < numPlayers; i++)
            {
                if(players[i].user_id != Constants.USER_ID) 
                {
                    if(tracker == 0) 
                    {
                        // Updates left slot in attackMenu.
                        opLeft.updateCharacter(players[i].character.character);
                    }
                    else if(tracker == 1)
                    {
                        opTop.updateCharacter(players[i].character.character);
                    }
                }
            }
        }

        // Displaying opponents to attack for 4 player game.
        if(numPlayers == 4) 
        {
            int tracker = 0;
            // For all players that are not this client's player, display their character in attackMenu.
            for (int i = 0; i < numPlayers; i++)
            {
                if(players[i].user_id != Constants.USER_ID) 
                {
                    if(tracker == 0) 
                    {
                        // Updates left slot in attackMenu.
                        opLeft.updateCharacter(players[i].character.character);
                    }
                    else if(tracker == 1)
                    {
                        opTop.updateCharacter(players[i].character.character);
                    }
                    else if(tracker == 2)
                    {
                        opRight.updateCharacter(players[i].character.character);
                    }
                }
            }
        }
        
        /*
        int set = 0;
        foreach (Player player in players)
        {
            if(player.user_id != players[myPlayerIndex].user_id)
            {
                if(set == 0)
                {
                    foreach(Character character in characters)
                    {
                        if(player.charName == character.cardName)
                        {
                            opLeft.updateCharacter(character);
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
                            opTop.updateCharacter(character);
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
                            opRight.updateCharacter(character);
                        }
                    }
                }
            }
        }
        */
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
        foreach (CardDisplay cd in players[myPlayerIndex].holster.cardList)
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
            // middle.updateCard(players[myPlayerIndex].deck.placeholder);
            // right.updateCard(players[myPlayerIndex].deck.placeholder);
            loadMenu.transform.Find("Card (Middle)").gameObject.SetActive(false);
            loadMenu.transform.Find("Card (Right)").gameObject.SetActive(false);
        }
        if(set == 2)
        {
            // right.updateCard(players[myPlayerIndex].deck.placeholder);
            loadMenu.transform.Find("Card (Right)").gameObject.SetActive(false);
        }
    }

    // END TURN REQUEST
    public void endTurn()
    {
        // If this client isn't the current player, display error message.
        // Player can't end turn if it isn't their turn.
        // (to change this to maybe disable endTurn button or grey it out?? turn it from button to image when not currentPlayer?)
        if(Constants.USER_ID != currentPlayerId) 
        {
            // "You are not the currentPlayer!"
            sendErrorMessage(7);
        }

        else
        {
            bool currentFound = false;
            int currentIndex = -1;
            for(int i = 0; i < numPlayers; i++)
            {
                if(players[i].user_id == currentPlayerId) {
                    currentIndex = i;
                }
            }

            int newIndex = currentIndex + 1;
            if(newIndex >= numPlayers) {
                newIndex = 0;
            }

            Debug.Log("Request End Turn");
            Debug.Log("Request: Ending turn for PlayerName: " + players[currentIndex].name);
            Debug.Log("Request: Starting turn for PlayerName: " + players[newIndex].name);
            Debug.Log("Request: Ending turn for PlayerID: " + players[currentIndex].user_id);
            Debug.Log("Request: Starting turn for PlayerID: " + players[newIndex].user_id);
            int newId = players[newIndex].user_id;
            // Sends user_id of new current player based on index of new current player
            // in this client's players array.
            bool connected = networkManager.sendEndTurnRequest(newId);

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

    public void onResponseEndTurn(ExtendedEventArgs eventArgs)
    {
        Debug.Log("End Turn!");
        ResponseEndTurnEventArgs args = eventArgs as ResponseEndTurnEventArgs;
        Debug.Log("New Current Player: " + args.newCurrentPlayerId);
        Debug.Log("Previous player: " + args.user_id);

        if(args.user_id == args.newCurrentPlayerId) {
            Debug.Log("ERROR: Current player hasn't been set to the next player!");
        }

        for(int i = 0; i < numPlayers; i++) {
            // Remove currentPlayer highlight from previous player.
            if(players[i].user_id == args.user_id)
            {
                Debug.Log("Response: Ending turn for PlayerName: " + players[i].name);
                players[i].removeCurrentPlayer();
            }
            else if(players[i].user_id == args.newCurrentPlayerId)
            {
                Debug.Log("Request: Starting turn for PlayerName: " + players[i].name);
                currentPlayerId = args.newCurrentPlayerId;
                players[i].setCurrentPlayer();
                onStartTurn(players[i]);
            }
        }

        /*
        // if it didn't come from your own client, change the player
        if (Constants.USER_ID != args.user_id)
        {
            // player 1 just ended turn in 2p game
            if(args.user_id == 1)
            {
                myPlayerIndex = 1;
                // player 2 just ended turn in 2p game
            } else if(args.user_id == 2)
            {
                if(numPlayers > 2)
                {
                    myPlayerIndex = 2;
                }
                else
                {
                    myPlayerIndex = 0;
                }
            } else if (args.user_id == 3)
            {
                if(numPlayers > 3)
                {
                    myPlayerIndex = 3;
                } else
                {
                    myPlayerIndex = 0;
                }
            }
            onStartTurn(players[myPlayerIndex]);
        }
        */
    }

    // THROW POTION REQUEST
    public void throwPotion()
    {
        // If this client isn't the current player, display error message.
        if(Constants.USER_ID != currentPlayerId) {
            // "You are not the currentPlayer!"
            sendErrorMessage(7);
        }

        // This client is the current player.
        else 
        {
            int damage = 0;
            int targetUserId = 0;
            int throwerIndex = -1;
            Debug.Log("GameManager Throw Potion");

            for (int i = 0; i < numPlayers; i++) 
            {
                if(players[i].charName == selectedOpponentCharName) 
                {
                    Debug.Log("Attacking Player: "+players[i].name);
                    targetUserId = players[i].user_id;
                }
                else if(players[i].user_id == Constants.USER_ID) {
                    throwerIndex = i;
                }
            }

            // check card type
            switch (selectedCardInt)
            {
                case 1:
                    if (players[throwerIndex].holster.card1.card.cardType == "Potion")
                    {
                        damage = players[throwerIndex].holster.card1.card.effectAmount;
                        if (players[throwerIndex].ringBonus && players[throwerIndex].potionsThrown == 0)
                        {
                            damage++;
                        }
                        // send protocol to server
                        // also check if they're the current player
                        //if(Constants.USER_ID - 1 == myPlayerIndex)

                        // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                        // Args Potion: throwerId, cardPosition, targetId, damage, isArtifact (T/F), isVessel (T/F)
                        bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, false, false);

                        // MATTEO: Add Potion throw SFX here.

                        // Update this on all clients?
                        // td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
                        // players[myPlayerIndex].potionsThrown++;

                        sendSuccessMessage(2); // Only display on thrower's client.
                        break;
                        //}
                    } 
                    else if(players[throwerIndex].holster.card1.card.cardType == "Vessel")
                    {
                        // Check for two loaded potions in Veseel.
                        if(players[throwerIndex].holster.card1.vPotion1.card.cardName != "placeholder" &&
                            players[throwerIndex].holster.card1.vPotion2.card.cardName != "placeholder")
                        {
                            damage = players[throwerIndex].holster.card1.vPotion1.card.effectAmount + players[throwerIndex].holster.card1.vPotion2.card.effectAmount;
                            // players[myPlayerIndex].deck.putCardOnBottom(players[myPlayerIndex].holster.card1.vPotion1.card);
                            // players[myPlayerIndex].deck.putCardOnBottom(players[myPlayerIndex].holster.card1.vPotion2.card);
                            // players[myPlayerIndex].holster.card1.vPotion1.updateCard(players[0].deck.placeholder);
                            // players[myPlayerIndex].holster.card1.vPotion2.updateCard(players[0].deck.placeholder);
                            // td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
                            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot1.transform.parent.gameObject.SetActive(false);
                            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot2.transform.parent.gameObject.SetActive(false);


                            // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                            bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, false, true);
                            sendSuccessMessage(4);
                            // MATTEO: Add Vessel throw SFX here.

                        } 
                        else
                        {
                            // "Can't throw an unloaded Vessel!"
                            //Debug.Log("Vessel Error");
                            sendErrorMessage(1);
                        }
                    }
                    else if (players[throwerIndex].holster.card1.card.cardType == "Artifact")
                    {
                        if (players[throwerIndex].holster.card1.aPotion.card.cardName != "placeholder")
                        {
                            damage = players[throwerIndex].holster.cardList[selectedCardInt - 1].card.effectAmount;
                            
                            // Update response to account for trashing loaded artifact's potion and not the artifact
                            // td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].aPotion);
                            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.parent.gameObject.SetActive(false);
                            
                            // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                            bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, true, false);
                            sendSuccessMessage(3);
                            // MATTEO: Add Artifact using SFX here.

                        }
                        else
                        {
                            // "Can't use an unloaded Artifact!"
                            sendErrorMessage(0);
                        }
                    }
                    break;

                case 2:
                    if (players[throwerIndex].holster.card2.card.cardType == "Potion")
                    {
                        damage = players[throwerIndex].holster.card2.card.effectAmount;
                        if (players[throwerIndex].ringBonus && players[throwerIndex].potionsThrown == 0)
                        {
                            damage++;
                        }

                        bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, false, false);
                        sendSuccessMessage(2);
                        // MATTEO: Add Potion throw SFX here.

                        break;

                        // send protocol to server
                        // also check if they're the current player
                        // if (Constants.USER_ID - 1 == myPlayerIndex)
                        // {
                        //     bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, false, false);
                        //     // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                        //     // td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
                        //     // players[myPlayerIndex].potionsThrown++;
                        //     sendSuccessMessage(2);
                        //     break;
                        // }
                        // break;
                    }
                    else if (players[throwerIndex].holster.card2.card.cardType == "Vessel")
                    {
                        // check for loaded potions
                        if (players[throwerIndex].holster.card2.vPotion1.card.cardName != "placeholder" &&
                            players[throwerIndex].holster.card2.vPotion2.card.cardName != "placeholder")
                        {
                            damage = players[throwerIndex].holster.card2.vPotion1.card.effectAmount + players[throwerIndex].holster.card2.vPotion2.card.effectAmount;
                            
                            // players[myPlayerIndex].deck.putCardOnBottom(players[myPlayerIndex].holster.card2.vPotion1.card);
                            // players[myPlayerIndex].deck.putCardOnBottom(players[myPlayerIndex].holster.card2.vPotion2.card);
                            // players[myPlayerIndex].holster.card2.vPotion1.updateCard(players[0].deck.placeholder);
                            // players[myPlayerIndex].holster.card2.vPotion2.updateCard(players[0].deck.placeholder);
                            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot1.transform.parent.gameObject.SetActive(false);
                            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot2.transform.parent.gameObject.SetActive(false);
                            // td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);

                            // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                            bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, false, true);
                            sendSuccessMessage(4);
                            // MATTEO: Add Vessel throw SFX here.

                        }
                        else
                        {
                            Debug.Log("Vessel Error");
                            sendErrorMessage(1);
                        }
                    }
                    else if (players[throwerIndex].holster.card2.card.cardType == "Artifact")
                    {
                        if (players[throwerIndex].holster.card2.aPotion.card.cardName != "placeholder")
                        {
                            damage = players[throwerIndex].holster.cardList[selectedCardInt - 1].card.effectAmount;
                            // td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].aPotion);
                            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.parent.gameObject.SetActive(false);

                            // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                            bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, true, false);
                            sendSuccessMessage(3);
                            // MATTEO: Add Artifact using SFX here.

                        }
                        else
                        {
                            sendErrorMessage(0);
                        }
                    }
                    break;
                case 3:
                    if (players[throwerIndex].holster.card3.card.cardType == "Potion")
                    {
                        damage = players[throwerIndex].holster.card3.card.effectAmount;
                        if (players[throwerIndex].ringBonus && players[throwerIndex].potionsThrown == 0)
                        {
                            damage++;
                        }

                        bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, false, false);
                        sendSuccessMessage(2);

                        // MATTEO: Add Potion throw SFX here.


                        break;

                        // send protocol to server
                        // also check if they're the current player
                        // if (Constants.USER_ID - 1 == myPlayerIndex)
                        // {
                        //     // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                        //     // td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
                        //     // players[myPlayerIndex].potionsThrown++;

                        //     bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, false, false);
                        //     sendSuccessMessage(2);
                        //     break;
                        // }
                        // break;
                    }
                    else if (players[throwerIndex].holster.card3.card.cardType == "Vessel")
                    {
                        Debug.Log("Reached vessel");
                        // check for loaded potions
                        if (players[throwerIndex].holster.card3.vPotion1.card.cardName != "placeholder" &&
                            players[throwerIndex].holster.card3.vPotion2.card.cardName != "placeholder")
                        {
                            Debug.Log("Reached cards");
                            damage = players[throwerIndex].holster.card3.vPotion1.card.effectAmount + players[throwerIndex].holster.card3.vPotion2.card.effectAmount;
                            // players[myPlayerIndex].deck.putCardOnBottom(players[myPlayerIndex].holster.card3.vPotion1.card);
                            // players[myPlayerIndex].deck.putCardOnBottom(players[myPlayerIndex].holster.card3.vPotion2.card);
                            // players[myPlayerIndex].holster.card3.vPotion1.updateCard(players[0].deck.placeholder);
                            // players[myPlayerIndex].holster.card3.vPotion2.updateCard(players[0].deck.placeholder);
                            // td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
                            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot1.transform.parent.gameObject.SetActive(false);
                            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot2.transform.parent.gameObject.SetActive(false);
                            
                            bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, false, true);
                            sendSuccessMessage(4);

                            // MATTEO: Add Vessel throw SFX here.

                            // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                        }
                        else
                        {
                            Debug.Log("Vessel Error");
                            sendErrorMessage(1);
                        }
                    }
                    else if (players[throwerIndex].holster.card3.card.cardType == "Artifact")
                    {
                        if (players[throwerIndex].holster.card1.aPotion.card.cardName != "placeholder")
                        {
                            damage = players[throwerIndex].holster.cardList[selectedCardInt - 1].card.effectAmount;
                            // td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].aPotion);
                            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.parent.gameObject.SetActive(false);
                            bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, true, false);
                            sendSuccessMessage(3);

                            // MATTEO: Add Artifact throw SFX here.

                            // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                        }
                        else
                        {
                            sendErrorMessage(0);
                        }
                    }
                    break;
                case 4:
                    if (players[throwerIndex].holster.card4.card.cardType == "Potion")
                    {
                        damage = players[throwerIndex].holster.card4.card.effectAmount;
                        if (players[throwerIndex].ringBonus && players[throwerIndex].potionsThrown == 0)
                        {
                            damage++;
                        }
                        bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, false, false);
                        sendSuccessMessage(2);

                        // MATTEO: Add Potion throw SFX here.

                        break;
                        // send protocol to server
                        // also check if they're the current player
                        // if (Constants.USER_ID - 1 == myPlayerIndex)
                        // {
                        //     // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                        //     // td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
                        //     // players[myPlayerIndex].potionsThrown++;

                        //     bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, false, false);
                        //     sendSuccessMessage(2);
                        //     break;
                        // }
                        // break;
                    }
                    else if (players[throwerIndex].holster.card4.card.cardType == "Vessel")
                    {
                        // check for loaded potions
                        if (players[throwerIndex].holster.card4.vPotion1.card.cardName != "placeholder" &&
                            players[throwerIndex].holster.card4.vPotion2.card.cardName != "placeholder")
                        {
                            damage = players[throwerIndex].holster.card4.vPotion1.card.effectAmount + players[throwerIndex].holster.card4.vPotion2.card.effectAmount;
                            // players[myPlayerIndex].deck.putCardOnBottom(players[myPlayerIndex].holster.card4.vPotion1.card);
                            // players[myPlayerIndex].deck.putCardOnBottom(players[myPlayerIndex].holster.card4.vPotion2.card);
                            // players[myPlayerIndex].holster.card4.vPotion1.updateCard(players[0].deck.placeholder);
                            // players[myPlayerIndex].holster.card4.vPotion2.updateCard(players[0].deck.placeholder);
                            // td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
                            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot1.transform.parent.gameObject.SetActive(false);
                            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].vesselSlot2.transform.parent.gameObject.SetActive(false);

                            bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, false, true);
                            sendSuccessMessage(4);

                            // MATTEO: Add Vessel throw SFX here.

                            // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                        }
                        else
                        {
                            Debug.Log("Vessel Error");
                            sendErrorMessage(1);
                        }
                    }
                    else if (players[throwerIndex].holster.card4.card.cardType == "Artifact")
                    {
                        if (players[throwerIndex].holster.card1.aPotion.card.cardName != "placeholder")
                        {
                            damage = players[throwerIndex].holster.cardList[selectedCardInt - 1].card.effectAmount;
                            // td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].aPotion);
                            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.parent.gameObject.SetActive(false);

                            bool connected = networkManager.SendThrowPotionRequest(Constants.USER_ID, selectedCardInt, targetUserId, damage, true, false);
                            sendSuccessMessage(3);

                            // MATTEO: Add Artifact using SFX here.

                            // bool connected = networkManager.SendThrowPotionRequest(damage, myPlayerIndex + 1, selectedCardInt, selectedOpponentInt);
                        }
                        else
                        {
                            sendErrorMessage(0);
                        }
                    }
                    break;
                default: damage = 0;
                    break;
            }
        }
    }

    // THROW POTION RESPONSE
    public void onResponsePotionThrow(ExtendedEventArgs eventArgs)
    {
        ResponsePotionThrowEventArgs args = eventArgs as ResponsePotionThrowEventArgs;
        // Args Potion: throwerId, cardPosition, targetId, damage, isArtifact (T/F), isVessel (T/F)
        Debug.Log("My User_ID: " + Constants.USER_ID);
        Debug.Log("Thrower ID: " + args.throwerId);
        Debug.Log("Card Position: " + args.cardPosition);
        Debug.Log("Target Opponent ID: " + args.targetId);
        Debug.Log("Damage: " + args.damage);
        Debug.Log("isArtifact?: " + args.isArtifact);
        Debug.Log("isVessel?: " + args.isVessel);

        
        // Loops through players array to update target's health and thrower's holster.
        for(int i = 0; i < numPlayers; i++) {
            // Find Player in players array that got damaged.
            if(players[i].user_id == args.targetId)
            {
                players[i].subHealth(args.damage);
            }
            // Update Holster of player who threw the Potion.
            if(players[i].user_id == args.throwerId) 
            {
                // Threw a Potion (trash thrower's thrown Potion and increase potionsThrown)
                if(args.isVessel == false && args.isArtifact == false)
                {
                    td.addCard(players[i].holster.cardList[args.cardPosition - 1]);
                    players[i].potionsThrown++;
                }
                // Threw a Vessel (put loaded potions in thrower's deck, trash the Vessel card)
                else if(args.isVessel == true && args.isArtifact == false) 
                {
                    // Determines if updating(holster.card1, holster.card2, etc.)
                    switch(args.cardPosition) {
                        case 1:
                            players[i].deck.putCardOnBottom(players[i].holster.card1.vPotion1.card);
                            players[i].deck.putCardOnBottom(players[i].holster.card1.vPotion2.card);
                            players[i].holster.card1.vPotion1.updateCard(players[0].deck.placeholder);
                            players[i].holster.card1.vPotion2.updateCard(players[0].deck.placeholder);
                            break;
                        case 2:
                            players[i].deck.putCardOnBottom(players[i].holster.card2.vPotion1.card);
                            players[i].deck.putCardOnBottom(players[i].holster.card2.vPotion2.card);
                            players[i].holster.card2.vPotion1.updateCard(players[0].deck.placeholder);
                            players[i].holster.card2.vPotion2.updateCard(players[0].deck.placeholder);
                            break;
                        case 3:
                            players[i].deck.putCardOnBottom(players[i].holster.card3.vPotion1.card);
                            players[i].deck.putCardOnBottom(players[i].holster.card3.vPotion2.card);
                            players[i].holster.card3.vPotion1.updateCard(players[0].deck.placeholder);
                            players[i].holster.card3.vPotion2.updateCard(players[0].deck.placeholder);
                            break;
                        case 4:
                            players[i].deck.putCardOnBottom(players[i].holster.card4.vPotion1.card);
                            players[i].deck.putCardOnBottom(players[i].holster.card4.vPotion2.card);
                            players[i].holster.card4.vPotion1.updateCard(players[0].deck.placeholder);
                            players[i].holster.card4.vPotion2.updateCard(players[0].deck.placeholder);
                            break;
                        default: Debug.Log("ERROR (ThrowPotionResponse): Can only update from card1 to card4"); break;
                    }
                    td.addCard(players[i].holster.cardList[args.cardPosition - 1]);
                    players[i].holster.cardList[args.cardPosition - 1].vesselSlot1.transform.parent.gameObject.SetActive(false);
                    players[i].holster.cardList[args.cardPosition - 1].vesselSlot2.transform.parent.gameObject.SetActive(false);
                }
                // Used an Artifact (trashes potion, keeps the Artifact in the holster)
                else if(args.isVessel == false && args.isArtifact == true)
                {
                    td.addCard(players[i].holster.cardList[args.cardPosition - 1].aPotion);
                    players[i].holster.cardList[args.cardPosition - 1].artifactSlot.transform.parent.gameObject.SetActive(false);
                }
            }
        }
        

        /*
        // If the request didn't come from this Client
        if (Constants.USER_ID != args.user_id)
        {
            // p1 request
            if (args.user_id == 1)
            {
                // player 2
                // this is for two players only cause it's the middle CharacterDisplay
                if (args.z == 2 && numPlayers == 2)
                {
                    if(players[myPlayerIndex].holster.cardList[args.y - 1].card.cardName == "Potion")
                    {
                        Debug.Log("Damaged by potion");
                        td.addCard(players[myPlayerIndex].holster.cardList[args.y - 1]);
                        players[0].subHealth(args.w);
                    } else if (players[myPlayerIndex].holster.cardList[args.y - 1].card.cardName == "Artifact")
                    {
                        Debug.Log("Damaged by artifact");
                    }
                    else if (players[myPlayerIndex].holster.cardList[args.y - 1].card.cardName == "Vessel")
                    {
                        Debug.Log("Damaged by vessel");
                    }
                }
            }
            // p2 request
            else if (args.user_id == 2)
            {
                // player 1
                // this is for two players only cause it's the middle CharacterDisplay
                if (args.z == 2 && numPlayers == 2)
                {
                    if (players[myPlayerIndex].holster.cardList[args.y - 1].card.cardName == "Potion")
                    {
                        Debug.Log("Damaged by potion");
                        td.addCard(players[myPlayerIndex].holster.cardList[args.y - 1]);
                        players[1].subHealth(args.w);
                    }
                    else if (players[myPlayerIndex].holster.cardList[args.y - 1].card.cardName == "Artifact")
                    {
                        Debug.Log("Damaged by artifact");
                    }
                    else if (players[myPlayerIndex].holster.cardList[args.y - 1].card.cardName == "Vessel")
                    {
                        Debug.Log("Damaged by vessel");
                    }
                }
            }
        }
        */
    }

    // LOAD REQUEST (DONE - 2 clients)
    public void loadPotion()
    {
        Debug.Log("Load Potion");

        // DONE?: Send potion with loadedCardInt to loaded CardDisplay of card in selectedCardInt
        // test for protocol, must replace parameters later
        // bool connected = networkManager.sendLoadRequest(0, 0);

        // If this client isn't the current player, display error message.
        if(Constants.USER_ID != currentPlayerId) {
            // "You are not the currentPlayer!"
            sendErrorMessage(7);
        }

        // It is this player's turn.
        else
        {
            // if it's an artifact or vessel
            if(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType == "Potion") 
            {
                // Loading a Vessel:
                if(players[myPlayerIndex].holster.cardList[loadedCardInt].card.cardType == "Vessel")
                {
                    // Enable Vessel menu if it wasn't already enabled.
                    Debug.Log("Vessel menu enabled.");
                    // players[myPlayerIndex].holster.cardList[loadedCardInt].vesselSlot1.transform.parent.gameObject.SetActive(true);

                    // Check for existing loaded potion(s) if Vessel menu was already enabled.
                    if(players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion1.card.cardName != "placeholder")
                    {
                        // If Vessel slot 2 is filled.
                        if(players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion2.card.cardName != "placeholder")
                        {
                            Debug.Log("Vessel is fully loaded!");
                            // DONE: Insert error that displays on screen.
                            sendErrorMessage(9);
                        }
                        else 
                        {
                            // Fill Vessel slot 2 with loaded potion.
                            // Card placeholder = players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion2.card;
                            // players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion2.card = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card;
                            // players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion2.updateCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card);

                            bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                            sendSuccessMessage(5);
                            Debug.Log("Potion loaded in Vessel slot 2!");

                            // MATTEO: Add Loading potion SFX here.

                            // // Updates Holster card to be empty.
                            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card = placeholder;
                            // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(placeholder);
                        }
                    }
                    // Vessel slot 1 is unloaded.
                    else
                    {
                        // Card placeholder = players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion1.card;
                        // players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion1.card = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card;
                        // players[myPlayerIndex].holster.cardList[loadedCardInt].vPotion1.updateCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card);

                        bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                        sendSuccessMessage(5);
                        Debug.Log("Potion loaded in Vessel slot 1!");

                        // MATTEO: Add Loading potion SFX here.

                        // // Updates Holster card to be empty.
                        // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card = placeholder;
                        // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(placeholder);
                    }
                }
                
                // Loading an Artifact:
                else if(players[myPlayerIndex].holster.cardList[loadedCardInt].card.cardType == "Artifact")
                {
                    // Enable Artifact menu if it wasn't already enabled.
                    Debug.Log("Artifact menu enabled.");
                    // players[myPlayerIndex].holster.cardList[loadedCardInt].artifactSlot.transform.parent.gameObject.SetActive(true);

                    // Check for existing loaded potion if Artifact menu was already enabled.
                    if(players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.card.cardName != "placeholder")
                    {
                        Debug.Log("Artifact is fully loaded!");
                        // DONE: Insert error that displays on screen.
                        sendErrorMessage(8);
                    }
                    // Artifact slot is unloaded.
                    else
                    {
                        // Card placeholder = players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.card;
                        // players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.card = players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card;
                        // players[myPlayerIndex].holster.cardList[loadedCardInt].aPotion.updateCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card);
                        bool connected = networkManager.sendLoadRequest(selectedCardInt, loadedCardInt);
                        sendSuccessMessage(5);
                        Debug.Log("Potion loaded in Artifact slot!");

                        // MATTEO: Add Loading potion SFX here.

                        // // Updates Holster card to be empty.
                        // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card = placeholder;
                        // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(placeholder);
                    }
                }
            }
        }
    }

    // LOAD RESPONSE (DONE - 2 clients)
    public void onResponseLoad(ExtendedEventArgs eventArgs)
    {
        Debug.Log("Load Response");
        ResponseLoadEventArgs args = eventArgs as ResponseLoadEventArgs;
        // args = (user_id, x = selectedCardInt, y = loadedCardInt)

        // Find relative index in players[] of the player who made loader request.
        int loaderIndex = -1;
        for(int i = 0; i < numPlayers; i++) {
            if(players[i].user_id == args.user_id) {
                loaderIndex = i;
            }
        }

        if(players[loaderIndex].holster.cardList[args.y].card.cardType == "Artifact")
        {
            Card placeholder = players[loaderIndex].holster.cardList[args.y].aPotion.card;
            players[loaderIndex].holster.cardList[args.y].artifactSlot.transform.parent.gameObject.SetActive(true);
            players[loaderIndex].holster.cardList[args.y].aPotion.card = players[loaderIndex].holster.cardList[args.x - 1].card;
            players[loaderIndex].holster.cardList[args.y].aPotion.updateCard(players[loaderIndex].holster.cardList[args.x - 1].card);
            // Updates Holster card to be empty.
            players[loaderIndex].holster.cardList[args.x - 1].card = placeholder;
            players[loaderIndex].holster.cardList[args.x - 1].updateCard(placeholder);
        } 
        else if(players[loaderIndex].holster.cardList[args.y].card.cardType == "Vessel")
        {
            players[loaderIndex].holster.cardList[args.y].vesselSlot1.transform.parent.gameObject.SetActive(true);
            players[loaderIndex].holster.cardList[args.y].vesselSlot2.transform.parent.gameObject.SetActive(true);
            if(players[loaderIndex].holster.cardList[args.y].vPotion1.card.cardName == "placeholder")
            {
                Card placeholder = players[loaderIndex].holster.cardList[args.y].vPotion1.card;
                players[loaderIndex].holster.cardList[args.y].vPotion1.card = players[loaderIndex].holster.cardList[args.x - 1].card;
                players[loaderIndex].holster.cardList[args.y].vPotion1.updateCard(players[loaderIndex].holster.cardList[args.x - 1].card);
                // Updates Holster card to be empty.
                players[loaderIndex].holster.cardList[args.x - 1].card = placeholder;
                players[loaderIndex].holster.cardList[args.x - 1].updateCard(placeholder);
            }
            else if(players[loaderIndex].holster.cardList[args.y].vPotion2.card.cardName == "placeholder")
            {
                Card placeholder = players[loaderIndex].holster.cardList[args.y].vPotion2.card;
                players[loaderIndex].holster.cardList[args.y].vPotion2.card = players[loaderIndex].holster.cardList[args.x - 1].card;
                players[loaderIndex].holster.cardList[args.y].vPotion2.updateCard(players[loaderIndex].holster.cardList[args.x - 1].card);
                // Updates Holster card to be empty.
                players[loaderIndex].holster.cardList[args.x - 1].card = placeholder;
                players[loaderIndex].holster.cardList[args.x - 1].updateCard(placeholder);
            }
        }

        // if(Constants.USER_ID != args.user_id)
        // {
        //     if(players[myPlayerIndex].holster.cardList[args.y].card.cardType == "Artifact")
        //     {
        //         players[myPlayerIndex].holster.cardList[args.y].artifactSlot.transform.parent.gameObject.SetActive(true);
        //         players[myPlayerIndex].holster.cardList[args.y].aPotion.card = players[myPlayerIndex].holster.cardList[args.x - 1].card;
        //         players[myPlayerIndex].holster.cardList[args.y].aPotion.updateCard(players[myPlayerIndex].holster.cardList[args.x - 1].card);
        //     } else if(players[myPlayerIndex].holster.cardList[args.y].card.cardType == "Vessel")
        //     {
        //         players[myPlayerIndex].holster.cardList[args.y].vesselSlot1.transform.parent.gameObject.SetActive(true);
        //         players[myPlayerIndex].holster.cardList[args.y].vesselSlot2.transform.parent.gameObject.SetActive(true);
        //         if(players[myPlayerIndex].holster.cardList[args.y].vPotion1.card.cardName != "placeholder")
        //         {
        //             players[myPlayerIndex].holster.cardList[args.y].vPotion1.card = players[myPlayerIndex].holster.cardList[args.x - 1].card;
        //             players[myPlayerIndex].holster.cardList[args.y].vPotion1.updateCard(players[myPlayerIndex].holster.cardList[args.x - 1].card);
        //         }
        //         else
        //         {
        //             players[myPlayerIndex].holster.cardList[args.y].vPotion2.card = players[myPlayerIndex].holster.cardList[args.x - 1].card;
        //             players[myPlayerIndex].holster.cardList[args.y].vPotion2.updateCard(players[myPlayerIndex].holster.cardList[args.x - 1].card);
        //         }
        //     }
        // }
    }

    // CYCLE REQUEST (DONE - 2 clients)
    public void cycleCard()
    {
        Debug.Log("Cycle Card");

        // If this client isn't the current player, display error message.
        if(Constants.USER_ID != currentPlayerId) {
            // "You are not the currentPlayer!"
            sendErrorMessage(7);
        }

        // It is this player's turn.
        else
        {
            // Cycling a Potion (costs 0 pips to do)
            if(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.cardType == "Potion")
            {
                // players[myPlayerIndex].deck.putCardOnBottom(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card);
                // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].updateCard(players[0].holster.card1.placeholder);
                bool connected = networkManager.sendCycleRequest(selectedCardInt, 0);
                sendSuccessMessage(7);
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
                bool connected = networkManager.sendCycleRequest(selectedCardInt, 1);
                sendSuccessMessage(7);
                // MATTEO: Add Cycle SFX here.
            }
            else
            {
                Debug.Log("ERROR: Card needs to be of type Potion, Artifact, Vessel, Ring");
            }
        }
    }

    // CYCLE RESPONSE (DONE - 2 clients)
    public void onResponseCycle(ExtendedEventArgs eventArgs)
    {
        // args = user_id, x = selectedCardInt or cardPosition, y = pips cost (0 or 1)
        Debug.Log("Cycle Response");
        ResponseCycleEventArgs args = eventArgs as ResponseCycleEventArgs;

        int cardPosition = args.x - 1;

        int cyclerIndex = -1;
        for(int i = 0; i < numPlayers; i++)
        {
            if(args.user_id == players[i].user_id)
            {
                cyclerIndex = i;
            }
        }

        Card placeholder = players[0].holster.card1.placeholder;

        // Cycling a Potion (costs 0 pips to do)
        if(players[cyclerIndex].holster.cardList[cardPosition].card.cardType == "Potion")
        {
            players[cyclerIndex].deck.putCardOnBottom(players[cyclerIndex].holster.cardList[cardPosition].card);
            players[cyclerIndex].holster.cardList[cardPosition].card = placeholder;
            players[cyclerIndex].holster.cardList[cardPosition].updateCard(placeholder);
            
        }

        // Cycling an Artifact (costs 1 pip, also need to drop any loaded potion)
        else if(players[cyclerIndex].holster.cardList[cardPosition].card.cardType == "Artifact")
        {
            // Subtracts a pip from the cycler.
            players[cyclerIndex].subPips(1);

            // Check if Artifact is loaded
            // If loaded, Send card in artifact slot to bottom of deck, and update CardDisplay to placeholder
            if(players[cyclerIndex].holster.cardList[cardPosition].aPotion.card.cardName != "placeholder")
            {

            }

            // Cycle the artifact card itself (CHECK)
            // Update the artifact card's position to have a placeholder card (CHECK)
            players[cyclerIndex].deck.putCardOnBottom(players[cyclerIndex].holster.cardList[cardPosition].card);
            players[cyclerIndex].holster.cardList[cardPosition].card = placeholder;
            players[cyclerIndex].holster.cardList[cardPosition].updateCard(placeholder);

            // If loaded, disable the artifact slot object - SetActive to false (CHECK)
            players[cyclerIndex].holster.cardList[cardPosition].artifactSlot.transform.parent.gameObject.SetActive(false);
        }

        // Cycling an Vessel (costs 1 pip, also need to drop any loaded potions)
        else if(players[cyclerIndex].holster.cardList[cardPosition].card.cardType == "Vessel")
        {
            // Subtracts a pip from the cycler.
            players[cyclerIndex].subPips(1);

            // If loaded with 1 or 2 potions, Send card(s) in vessel slot(s) to bottom of deck, and update CardDisplay(s) to placeholder (CHECK)
            // If Vessel slot 1 is unloaded.
            if(players[cyclerIndex].holster.cardList[cardPosition].vPotion1.card.cardName != "placeholder")
            {
                // If Vessel's Slot 2 is loaded.
                if(players[cyclerIndex].holster.cardList[cardPosition].vPotion2.card.cardName != "placeholder")
                {
                    // Both Vessel slots are loaded.
                    players[cyclerIndex].deck.putCardOnBottom(players[cyclerIndex].holster.cardList[cardPosition].vPotion1.card);
                    players[cyclerIndex].deck.putCardOnBottom(players[cyclerIndex].holster.cardList[cardPosition].vPotion2.card);
                    players[cyclerIndex].holster.cardList[cardPosition].vPotion1.card = placeholder;
                    players[cyclerIndex].holster.cardList[cardPosition].vPotion2.card = placeholder;
                    players[cyclerIndex].holster.cardList[cardPosition].vPotion1.updateCard(placeholder);
                    players[cyclerIndex].holster.cardList[cardPosition].vPotion2.updateCard(placeholder);
                }
                else 
                {
                    // Only Vessel slot 1 is loaded.
                    players[cyclerIndex].deck.putCardOnBottom(players[cyclerIndex].holster.cardList[cardPosition].vPotion1.card);
                    players[cyclerIndex].holster.cardList[cardPosition].vPotion1.card = placeholder;
                    players[cyclerIndex].holster.cardList[cardPosition].vPotion1.updateCard(placeholder);
                }
            }
            // If Vessel slot 1 is unloaded.
            else
            {
                // If Vessel's Slot 2 is loaded.
                if(players[cyclerIndex].holster.cardList[cardPosition].vPotion2.card.cardName != "placeholder")
                {
                    // Only Vessel slot 2 is loaded.
                    players[cyclerIndex].deck.putCardOnBottom(players[cyclerIndex].holster.cardList[cardPosition].vPotion2.card);
                    players[cyclerIndex].holster.cardList[cardPosition].vPotion2.card = placeholder;
                    players[cyclerIndex].holster.cardList[cardPosition].vPotion2.updateCard(placeholder);
                }
                else 
                {
                    // No Vessel slots are loaded.
                }
            }
            
            // Cycle the Vessel card itself (CHECK)
            // Update the Vessel card's position to have a placeholder card (CHECK)
            players[cyclerIndex].deck.putCardOnBottom(players[cyclerIndex].holster.cardList[cardPosition].card);
            players[cyclerIndex].holster.cardList[cardPosition].card = placeholder;
            players[cyclerIndex].holster.cardList[cardPosition].updateCard(placeholder);

            // If loaded, disable the vessel slot objects - SetActive to false (CHECK)
            players[cyclerIndex].holster.cardList[cardPosition].vesselSlot1.transform.parent.gameObject.SetActive(false);
            players[cyclerIndex].holster.cardList[cardPosition].vesselSlot2.transform.parent.gameObject.SetActive(false);
        }

        // Cycling a Ring (costs 1 pip to do)
        else if(players[cyclerIndex].holster.cardList[selectedCardInt - 1].card.cardType == "Ring")
        {
            // Subtracts a pip from the cycler.
            players[cyclerIndex].subPips(1);
            players[cyclerIndex].deck.putCardOnBottom(players[cyclerIndex].holster.cardList[cardPosition].card);
            players[cyclerIndex].holster.cardList[cardPosition].card = placeholder;
            players[cyclerIndex].holster.cardList[cardPosition].updateCard(placeholder);
        }

        // if (Constants.USER_ID != args.user_id)
        // {
        //     players[myPlayerIndex].pips--;
        //     players[myPlayerIndex].deck.putCardOnBottom(players[myPlayerIndex].holster.cardList[args.x - 1].card);
        //     players[myPlayerIndex].holster.cardList[args.x - 1].updateCard(players[0].holster.card1.placeholder);
        // }
    }

    // SELL REQUEST
    public void sellCard()
    {
        Debug.Log("Sell Card");

        // If this client isn't the current player, display error message.
        if(Constants.USER_ID != currentPlayerId) {
            // "You are not the currentPlayer!"
            sendErrorMessage(7);
        }

        // It is this player's turn.
        else
        {
            // players[myPlayerIndex].addPips(players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.sellPrice);
            // td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
            bool connected = networkManager.sendSellRequest(selectedCardInt, players[myPlayerIndex].holster.cardList[selectedCardInt - 1].card.sellPrice);
            sendSuccessMessage(8);
            // MATTEO: Add sell SFX here.
        }
    }

    // SELL RESPONSE
    public void onResponseSell(ExtendedEventArgs eventArgs)
    {
        Debug.Log("ResponseSell");
        ResponseSellEventArgs args = eventArgs as ResponseSellEventArgs;
        Debug.Log("ID: " + args.user_id); // user_id of seller
        Debug.Log("cardInt: " + args.x); // selectedCardInt or cardPosition
        Debug.Log("Sell Price: " + args.y); // # of Pips the seller will get for selling the selected holster card.

        int cardPosition = args.x - 1;
        Card placeholder = players[0].holster.card1.placeholder;

        for(int i = 0; i < numPlayers; i++) {
            if(players[i].user_id == args.user_id)
            {
                players[i].addPips(players[i].holster.cardList[cardPosition].card.sellPrice);
                td.addCard(players[i].holster.cardList[cardPosition]);
                players[i].holster.cardList[cardPosition].card = placeholder;
                players[i].holster.cardList[cardPosition].updateCard(placeholder);
            }
        }

        // if (Constants.USER_ID != args.user_id)
        // {
        //     players[myPlayerIndex].pips += players[myPlayerIndex].holster.cardList[args.x - 1].card.sellPrice;
        //     td.addCard(players[myPlayerIndex].holster.cardList[args.x - 1]);

        // }
    }

// TOP MARKET REQUEST
// subtract pips, update deck display and market display
    public void topMarketBuy()
    {
        Debug.Log("Top Market Buy");

        // If this client isn't the current player, display error message.
        if(Constants.USER_ID != currentPlayerId) {
            // "You are not the currentPlayer!"
            sendErrorMessage(7);
        }

        // It is this player's turn.
        else
        {
            // cardInt based on position of card in Top Market (position 1, 2, or 3)
            switch (md1.cardInt)
            {
                case 1:
                    if(players[myPlayerIndex].pips >= md1.cardDisplay1.card.buyPrice)
                    {
                        // players[myPlayerIndex].pips -= md1.cardDisplay1.card.buyPrice;
                        // players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay1.card);
                        // Card card = md1.popCard();
                        // md1.cardDisplay1.updateCard(card);
                        sendSuccessMessage(1);
                        bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay1.card.buyPrice, 1);
                    } else
                    {
                        sendErrorMessage(6);
                    }
                    break;
                case 2:
                    if (players[myPlayerIndex].pips >= md1.cardDisplay2.card.buyPrice)
                    {
                        // players[myPlayerIndex].pips -= md1.cardDisplay2.card.buyPrice;
                        // players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay2.card);
                        // Card card = md1.popCard();
                        // md1.cardDisplay2.updateCard(card);
                        sendSuccessMessage(1);
                        bool connected = networkManager.sendBuyRequest(md1.cardInt, md1.cardDisplay2.card.buyPrice, 1);
                    }
                    else
                    {
                        sendErrorMessage(6);
                    }
                    break;
                case 3:
                    if (players[myPlayerIndex].pips >= md1.cardDisplay3.card.buyPrice)
                    {
                        // players[myPlayerIndex].pips -= md1.cardDisplay3.card.buyPrice;
                        // players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay3.card);
                        // Card card = md1.popCard();
                        // md1.cardDisplay3.updateCard(card);
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
    }

// BOTTOM MARKET REQUEST
    public void bottomMarketBuy()
    {
        Debug.Log("Bottom Market Buy");

        // If this client isn't the current player, display error message.
        if(Constants.USER_ID != currentPlayerId) {
            // "You are not the currentPlayer!"
            sendErrorMessage(7);
        }

        // It is this player's turn.
        else
        {
            switch (md2.cardInt)
            {
                // cardInt based on position of card in Top Market (position 1, 2, or 3)
                case 1:
                    if (players[myPlayerIndex].pips >= md2.cardDisplay1.card.buyPrice)
                    {
                        // players[myPlayerIndex].pips -= md2.cardDisplay1.card.buyPrice;
                        // players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay1.card);
                        // Card card = md2.popCard();
                        // md2.cardDisplay1.updateCard(card);
                        sendSuccessMessage(1);
                        bool connected = networkManager.sendBuyRequest(md2.cardInt, md2.cardDisplay1.card.buyPrice, 0);
                    }
                    else
                    {
                        sendErrorMessage(6);
                    }
                    break;
                case 2:
                    if (players[myPlayerIndex].pips >= md2.cardDisplay2.card.buyPrice)
                    {
                        // players[myPlayerIndex].pips -= md2.cardDisplay2.card.buyPrice;
                        // players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay2.card);
                        // Card card = md2.popCard();
                        // md2.cardDisplay2.updateCard(card);
                        sendSuccessMessage(1);
                        bool connected = networkManager.sendBuyRequest(md2.cardInt, md2.cardDisplay2.card.buyPrice, 0);
                    }
                    else
                    {
                        sendErrorMessage(6);
                    }
                    break;
                case 3:
                    if (players[myPlayerIndex].pips >= md2.cardDisplay3.card.buyPrice)
                    {
                        // players[myPlayerIndex].pips -= md2.cardDisplay3.card.buyPrice;
                        // players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay3.card);
                        // Card card = md2.popCard();
                        // md2.cardDisplay3.updateCard(card);
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
    }

    // BUY RESPONSE
    public void onResponseBuy(ExtendedEventArgs eventArgs)
    {
        Debug.Log("ResponseBuy");
        ResponseBuyEventArgs args = eventArgs as ResponseBuyEventArgs;
        Debug.Log("ID: " + args.user_id); // User_id of player who made purchase
        Debug.Log("cardInt: " + args.x); // Card position in market (1, 2, or 3)
        Debug.Log("Price: " + args.y); // Int price of card purchased
        Debug.Log("T or B: " + args.z); // Top (1) or Bottom (0) market that card was purchased from.

        
        for(int i = 0; i < numPlayers; i++)
        {
            // Find player who made the Buy request.
            if(players[i].user_id == args.user_id)
            {
                // Subtracts pips from Player who made purchase (buy request)
                players[i].subPips(args.y);

                // If purchase was made from the top market
                if(args.z == 1)
                {
                    // Update based on card position in the top market (1, 2, or 3)
                    switch (args.x)
                    {
                        case 1:
                            players[i].deck.putCardOnTop(md1.cardDisplay1.card);
                            Card card = md1.popCard();
                            md1.cardDisplay1.updateCard(card);
                            break;
                        case 2:
                            players[i].deck.putCardOnTop(md1.cardDisplay2.card);
                            Card card2 = md1.popCard();
                            md1.cardDisplay2.updateCard(card2);
                            break;
                        case 3:
                            players[i].deck.putCardOnTop(md1.cardDisplay3.card);
                            Card card3 = md1.popCard();
                            md1.cardDisplay3.updateCard(card3);
                            break;
                    }
                }
                // If purchase was made from the bottom market
                else
                {
                    // Update based on card position in the bottom market (1, 2, or 3)
                    switch (args.x)
                    {
                        case 1:
                            players[i].deck.putCardOnTop(md2.cardDisplay1.card);
                            Card card4 = md2.popCard();
                            md2.cardDisplay1.updateCard(card4);
                            break;
                        case 2:
                            players[i].deck.putCardOnTop(md2.cardDisplay2.card);
                            Card card5 = md2.popCard();
                            md2.cardDisplay2.updateCard(card5);
                            break;
                        case 3:
                            players[i].deck.putCardOnTop(md2.cardDisplay3.card);
                            Card card6 = md2.popCard();
                            md2.cardDisplay3.updateCard(card6);
                            break;
                    }
                }
            }
        }
        
        /*
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
                            players[myPlayerIndex].pips -= md1.cardDisplay1.card.buyPrice;
                            players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay1.card);
                            Card card = md1.popCard();
                            md1.cardDisplay1.updateCard(card);
                            break;
                        case 2:
                            players[myPlayerIndex].pips -= md1.cardDisplay2.card.buyPrice;
                            players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay2.card);
                            Card card2 = md1.popCard();
                            md1.cardDisplay2.updateCard(card2);
                            break;
                        case 3:
                            players[myPlayerIndex].pips -= md1.cardDisplay3.card.buyPrice;
                            players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay3.card);
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
                            players[myPlayerIndex].pips -= md2.cardDisplay1.card.buyPrice;
                            players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay1.card);
                            Card card4 = md2.popCard();
                            md2.cardDisplay1.updateCard(card4);
                            break;
                        case 2:
                            players[myPlayerIndex].pips -= md2.cardDisplay2.card.buyPrice;
                            players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay2.card);
                            Card card5 = md2.popCard();
                            md2.cardDisplay2.updateCard(card5);
                            break;
                        case 3:
                            players[myPlayerIndex].pips -= md2.cardDisplay3.card.buyPrice;
                            players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay3.card);
                            Card card6 = md2.popCard();
                            md2.cardDisplay3.updateCard(card6);
                            break;
                    }
                }
            }
        } 
        else if(args.user_id == 2)
        {
            // if top market
            if (args.z == 1)
            {
                switch (args.x)
                {
                    case 1:
                        players[myPlayerIndex].pips -= md1.cardDisplay1.card.buyPrice;
                        players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay1.card);
                        Card card = md1.popCard();
                        md1.cardDisplay1.updateCard(card);
                        break;
                    case 2:
                        players[myPlayerIndex].pips -= md1.cardDisplay2.card.buyPrice;
                        players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay2.card);
                        Card card2 = md1.popCard();
                        md1.cardDisplay2.updateCard(card2);
                        break;
                    case 3:
                        players[myPlayerIndex].pips -= md1.cardDisplay3.card.buyPrice;
                        players[myPlayerIndex].deck.putCardOnTop(md1.cardDisplay3.card);
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
                        players[myPlayerIndex].pips -= md2.cardDisplay1.card.buyPrice;
                        players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay1.card);
                        Card card4 = md2.popCard();
                        md2.cardDisplay1.updateCard(card4);
                        break;
                    case 2:
                        players[myPlayerIndex].pips -= md2.cardDisplay2.card.buyPrice;
                        players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay2.card);
                        Card card5 = md2.popCard();
                        md2.cardDisplay2.updateCard(card5);
                        break;
                    case 3:
                        players[myPlayerIndex].pips -= md2.cardDisplay3.card.buyPrice;
                        players[myPlayerIndex].deck.putCardOnTop(md2.cardDisplay3.card);
                        Card card6 = md2.popCard();
                        md2.cardDisplay3.updateCard(card6);
                        break;
                }
            }
        }
        */
    }

    // TRASH REQUEST
    public void trashCard()
    {
        Debug.Log("Trash Card");

        // If this client isn't the current player, display error message.
        if(Constants.USER_ID != currentPlayerId) {
            // "You are not the currentPlayer!"
            sendErrorMessage(7);
        }

        // It is this player's turn.
        else
        {
            td.addCard(players[myPlayerIndex].holster.cardList[selectedCardInt - 1]);
            // SEND TRASH REQUEST (int x, int y)
            bool connected = networkManager.sendTrashRequest(selectedCardInt, 0);
            sendSuccessMessage(9);
        }
    }

    // TRASH RESPONSE
    public void onResponseTrash(ExtendedEventArgs eventArgs)
    {
        // args.x is cardInt
        Debug.Log("Trash Response");
        ResponseTrashEventArgs args = eventArgs as ResponseTrashEventArgs;

        if(Constants.USER_ID != args.user_id)
        {
            td.addCard(players[myPlayerIndex].holster.cardList[args.x - 1]);
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

}

public enum Gamestate {
    PlayerTurn,
    OpponentTurn,
    Win,
    Lose
}
