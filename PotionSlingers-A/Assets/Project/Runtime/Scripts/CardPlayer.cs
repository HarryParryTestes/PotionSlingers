using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardPlayer : MonoBehaviour
{
    public int hp;
    public string name;
    public string charName;
    public int user_id;
    public int hpCubes;
    public int takenHPCubes;    //HP Cubes that have been taken from opponents
    public Deck deck;
    public Holster holster;
    public int pips;
    public int pipCount = 6;
    public int pipsUsedThisTurn = 0;
    public bool dead;           //Does the player still have health left?
    public int potionsThrown = 0;
    public int vesselsThrown = 0;
    public int artifactsUsed = 0;
    public int uniqueArtifactsUsed = 0;
    public int tricks = 0;
    public CharacterDisplay character;
    public CanvasGroup canvasGroup;
    public Animator animator;
    public bool cubed = false;
    public bool ringBonus;
    public bool doubleRingBonus = false;
    public int bonusAmount;
    public int cardsTrashed = 0;
    //public HealthController health;
    public HealthBar hBar;
    public GameObject username;
    public GameObject bar;
    public GameObject playerHP;
    public GameObject playerHPCubes;
    public GameObject playerPips;
    public GameObject currentPlayerHighlight;
    public GameObject damageSign;
    public GameObject damageAmount;
    public GameObject healSign;
    public GameObject healAmount;
    public GameObject hpBar;
    public GameObject pipsSign;
    public GameObject healthText;
    public GameObject hitAnimation;
    public GameObject hoverBox;
    public List<Sprite> hitImages;
    private System.Random rng = new System.Random();
    public string lastArtifactUsed = "";


    // TODO: Refactor methods that use UniqueCard and replace them all with Card
    // also, make ScriptableObjects that are regular Cards and not UniqueCards
    public List<Card> uniqueCards;

    // possible bonuses
    public bool throwBonus;
    public bool vesselBonus;
    public bool artifactBonus;
    public bool reetsCycle = false;
    public bool pluotAction = false;
    public bool isPluot = false;
    public bool isBolo = false;
    public bool isNickles = false;
    public bool isIsadore = false;
    public bool isReets = false;
    public bool isSaltimbocca = false;
    public bool isScarpetta = false;
    public bool isSweetbitter = false;
    public bool isTwins = false;

    public bool nicklesAction = false;
    // None, Hot, Cold, Wet, Dry
    public string pluotBonusType = "None";
    public bool pluotHot = false;
    public bool pluotCold = false;
    public bool pluotWet = false;
    public bool pluotDry = false;
    public bool bottleRocketBonus = false;
    public bool blackRainBonus = false;
    public bool opponentPreventedDamage = false;
    public bool healBonus = false;
    public bool phialBonus = false;
    public bool hotCoffee = false;

    public MyNetworkManager game;
    public MyNetworkManager Game
    {
        get
        {
            if (game != null)
            {
                return game;
            }
            return game = MyNetworkManager.singleton as MyNetworkManager;
        }
    }

    /*
    public CardPlayer(int user_id, string name)
    {
        this.user_id = user_id;
        this.name = name;
    }
    */

    void Start()
    {
        pipCount = 6;
    }

    void Update()
    {
        // yes damage
        if (GameManager.manager.damage && name == GameManager.manager.currentPlayerName)
        {
            foreach (CardPlayer player in GameManager.manager.players)
            {
                if (player.name == GameManager.manager.selectedOpponentName)
                {
                    player.subHealth(2);
                    GameManager.manager.damage = false;
                }
            }
        }

        // no damage
        if (GameManager.manager.trash && name == GameManager.manager.currentPlayerName)
        {
            GameManager.manager.trashMarketUI.SetActive(true);
            GameManager.manager.updateTrashMarketMenu();
            GameManager.manager.trash = false;

        }
    }

    public void displayDeck()
    {
        GameManager.manager.deckDisplay.displayCards(deck);
    }

    public void checkCharacter()
    {
        // switch (character.character.cardName)
        switch (charName)
        {
            case "Pluot":
                Debug.Log("I AM PLUOT");
                isPluot = true;
                break;
            case "Bolo":
                Debug.Log("I AM BOLO");
                isBolo = true;
                break;
            case "Nickles":
                Debug.Log("I AM NICKLES");
                isNickles = true;
                break;
            case "Isadore":
                Debug.Log("I AM ISADORE");
                isIsadore = true;
                // this.character.updateCharacter(character2);
                if (this.gameObject.name == "CharacterCard (Left)")
                    this.transform.localScale = new Vector3(2f, 2.6f, 0);
                if (this.gameObject.name == "CharacterCard (Top)")
                    this.transform.localScale = new Vector3(7.5f, 10f, 0);
                if (this.gameObject.name == "CharacterCard (Right)")
                    this.transform.localScale = new Vector3(5.2f, 6.3f, 0);
                break;
            case "Reets":
                Debug.Log("I AM REETS");
                isReets = true;
                if (this.gameObject.name == "CharacterCard (Right)")
                {
                    this.transform.localScale = new Vector3(7f, 8f, 0);
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 30, this.transform.position.z);
                }
                
                if (this.gameObject.name == "CharacterCard (Top)")
                    this.transform.localScale = new Vector3(11f, 12.5f, 0);
                if (this.gameObject.name == "CharacterCard (Left)")
                {
                    this.transform.localScale = new Vector3(3f, 3.5f, 0);
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 30, this.transform.position.z);
                }
                    
                break;
            case "Saltimbocca":
                Debug.Log("I AM SALTIMBOCCA");
                isSaltimbocca = true;
                if (this.gameObject.name == "CharacterCard (Right)")
                {
                    this.transform.localScale = new Vector3(5.4f, 7.9f, 0);
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 20, this.transform.position.z);
                }
                    
                if (this.gameObject.name == "CharacterCard (Top)")
                    this.transform.localScale = new Vector3(9.1f, 12.5f, 0);
                if (this.gameObject.name == "CharacterCard (Left)")
                {
                    this.transform.localScale = new Vector3(2.3f, 3.2f, 0);
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 20, this.transform.position.z);
                }
                    
                break;
            case "Scarpetta":
                Debug.Log("I AM SCARPETTA");
                isScarpetta = true;
                break;
            case "Sweetbitter":
                Debug.Log("I AM SWEETBITTER");
                isSweetbitter = true;
                break;
            case "Twins":
                Debug.Log("I AM TWINS");
                isTwins = true;
                break;
            case "Crowpunk":
                this.transform.localScale = new Vector3(16.5f, 11f, 0);
                this.GetComponent<Image>().raycastPadding = new Vector4(36f, 2f, 26f, 2f);
                break;
            case "Bag o' Snakes":
                Debug.Log("Bag of Snakes!!!");
                // this.transform.localScale = new Vector3(16.5f, 11f, 0);
                break;
            case "Singelotte":
                Debug.Log("Singelotte!!!");
                this.transform.localScale = new Vector3(29f, 19.33f, 0);
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 60, this.transform.position.z);
                this.GetComponent<Image>().raycastPadding = new Vector4(44f, 32f, 43f, 0.5f);
                break;
            case "Fingas":
                Debug.Log("Fingas!!!");
                // this.transform.localScale = new Vector3(16.5f, 11f, 0);
                break;

            default:
                Debug.Log("Failed to set any bools");
                break;
        }
        if (animator != null)
            playIdle();
    }

    public void onCharacterClick(string character)
    {
        Debug.Log("Send CharReq");
        foreach (Character character2 in MainMenu.menu.characters)
        {
            if (character2.cardName == character)
            {
                Debug.Log(character + " chosen");
                this.character.updateCharacter(character2);
                checkCharacter();
            }
        }
        if (character == "")
        {
            this.character.onCharacterClick("Bolo");
        }


    }

    public void initHealth()
    {
        hp = 10;
        hpCubes = 3;
        updateHealthUI();
    }

    public void playIdle()
    {
        switch (charName)
        {
            case "Pluot":
                Debug.Log("I AM PLUOT");
                isPluot = true;
                break;
            case "Bolo":
                animator.Play("BoloIdle");
                break;
            case "Nickles":
                Debug.Log("I AM NICKLES");
                isNickles = true;
                break;
            case "Isadore":
                animator.Play("IsadoreIdle");
                break;
            case "Reets":
                animator.Play("ReetsIdle");
                break;
            case "Saltimbocca":
                animator.Play("SaltIdle");
                break;
            case "Scarpetta":
                Debug.Log("I AM SCARPETTA");
                isScarpetta = true;
                break;
            case "Sweetbitter":
                Debug.Log("I AM SWEETBITTER");
                isSweetbitter = true;
                break;
            case "Twins":
                Debug.Log("I AM TWINS");
                isTwins = true;
                break;
            case "Crowpunk":
                Debug.Log("CrowPunk animation");
                animator.Play("CrowIdle");
                break;

            case "Fingas":
                Debug.Log("Fingas animation");
                animator.Play("Fingas_idle");
                break;

            case "Bag o' Snakes":
                Debug.Log("Bag of Snakes animation");
                animator.Play("BagIdle");
                break;

            case "Singelotte":
                Debug.Log("Singelotte animation");
                animator.Play("SingeIdle");
                break;

            default:
                Debug.Log("Failed to set any bools");
                break;
        }
    }

    public void playHit()
    {
        switch (charName)
        {
            case "Pluot":
                Debug.Log("I AM PLUOT");
                // isPluot = true;
                break;
            case "Bolo":
                animator.Play("BoloHit");
                break;
            case "Nickles":
                Debug.Log("I AM NICKLES");
                // isNickles = true;
                break;
            case "Isadore":
                animator.Play("IsadoreHit");
                break;
            case "Reets":
                animator.Play("ReetsHit");
                break;
            case "Saltimbocca":
                animator.Play("SaltHit");
                break;
            case "Scarpetta":
                Debug.Log("I AM SCARPETTA");
                // isScarpetta = true;
                break;
            case "Sweetbitter":
                Debug.Log("I AM SWEETBITTER");
                // isSweetbitter = true;
                break;
            case "Twins":
                Debug.Log("I AM TWINS");
                // isTwins = true;
                break;
            case "Crowpunk":
                Debug.Log("CrowPunk Hit animation");
                animator.Play("CrowHit");
                break;
            case "Bag o' Snakes":
                Debug.Log("Bag of Snakes Hit animation");
                animator.Play("BagHit");
                break;
            case "Fingas":
                Debug.Log("Fingas Hit animation");
                animator.Play("Fingas_hit");
                break;
            case "Singelotte":
                Debug.Log("Singe Hit animation");
                animator.Play("SingeHit");
                break;
            case "Carnival":
                Debug.Log("Carnival hit animation should run here!");
                // animator.Play("SingeHit");
                break;

            default:
                Debug.Log("Failed to set any bools");
                break;
        }
        if(charName == "Fingas")
        {
            Invoke("playIdle", 1.55f);
            return;
        }

        if (animator != null && !dead)
            Invoke("playIdle", animator.GetCurrentAnimatorStateInfo(0).length);
    }

    public void checkReetsCondition()
    {
        int cards = 0;
        foreach (CardDisplay cd in holster.cardList)
        {
            if(cd.card.name != "placeholder")
            {
                cards++;
            }
        }

        cards += deck.deckList.Count;

        Debug.Log("Total number of Reets cards: " + cards);

        if(cards >= 11)
        {
            character.canBeFlipped = true;
            GameManager.manager.sendMessage("You can now flip your character!");
        }
    }

    public void updateHealthUI(string cardQuality = "")
    {
        float numbers;
        if (healthText != null && hpBar != null)
        {
            healthText.GetComponent<Text>().text = hp.ToString();
            if(name == "Singelotte")
                numbers = (float)hp / 33f;
            else
                numbers = (float)hp / 10f;
            // Debug.Log("Fill amount is: " + numbers);
            hpBar.GetComponent<Image>().fillAmount = numbers;
        }


        if (playerHP != null && playerHPCubes != null)
        {
            playerHP.GetComponent<Text>().text = hp.ToString();
            playerHPCubes.GetComponent<Text>().text = hpCubes.ToString();
            // playerHP.GetComponent<Text>().text = "HP: " + hp.ToString() + " /10";
            // playerHPCubes.GetComponent<Text>().text = "Cubes: " + hpCubes.ToString();
        }

        /*
        switch (cardQuality)
        {
            case "Hot":
                // hitAnimation.SetActive(true);
                hitAnimation.GetComponent<Image>().sprite = hitImages[0];
                StartCoroutine(showHit());
                break;
            case "Wet":
                // hitAnimation.SetActive(true);
                hitAnimation.GetComponent<Image>().sprite = hitImages[1];
                StartCoroutine(showHit());
                break;
            case "Cold":
                // hitAnimation.SetActive(true);
                hitAnimation.GetComponent<Image>().sprite = hitImages[2];
                StartCoroutine(showHit());
                break;
            case "Dry":
                // hitAnimation.SetActive(true);
                hitAnimation.GetComponent<Image>().sprite = hitImages[3];
                StartCoroutine(showHit());
                break;
            default:
                hitAnimation.GetComponent<Image>().sprite = hitImages[4];
                StartCoroutine(showHit());
                break;
        }  
        */
    }

    public IEnumerator showHit()
    {
        hitAnimation.SetActive(true);
        yield return new WaitForSeconds(2);
        hitAnimation.SetActive(false);
    }

    public void updatePipsUI()
    {
        if (playerPips != null)
        {
            // playerPips.GetComponent<Text>().text = pips.ToString() + " Pips";
            playerPips.GetComponent<Text>().text = pips.ToString();
        }
    }

    public void nicklesActionTrue()
    {
        nicklesAction = true;
    }

    public void fadeOut()
    {
        // fix this and make sure you properly fade everything out
        // also this is a fucking mess

        // GetComponent<Image>().CrossFadeAlpha(0, 2f, false);
        GetComponent<Image>().DOFade(0, 1.5f);
        playerHP.GetComponent<Image>().DOFade(0, 1.5f);
        playerHPCubes.GetComponent<Image>().DOFade(0, 1.5f);
        bar.GetComponent<Image>().DOFade(0, 1.5f);
        bar.transform.GetChild(0).GetComponent<Image>().DOFade(0, 1.5f);
        hpBar.GetComponent<Image>().DOFade(0, 1.5f);
        holster.GetComponent<Image>().DOFade(0, 1.5f);
        holster.transform.GetChild(0).GetComponent<Image>().DOFade(0, 1.5f);
        foreach (CardDisplay cd in holster.cardList)
        {
            cd.fadeCard();
        }
        deck.GetComponent<Image>().DOFade(0, 1.5f);
        username.transform.GetChild(0).GetComponent<Image>().DOFade(0, 1.5f);
        username.transform.GetChild(1).GetComponent<Image>().DOFade(0, 1.5f);
        username.transform.GetChild(2).GetComponent<Image>().DOFade(0, 1.5f);
        playerHP.GetComponent<Image>().DOFade(0, 1.5f);
        playerHP.gameObject.SetActive(false);
        playerHPCubes.GetComponent<Image>().DOFade(0, 1.5f);
        playerHPCubes.gameObject.SetActive(false);
        playerHPCubes.transform.GetChild(0).GetComponent<Image>().DOFade(0, 1.5f);
        // playerHPCubes.transform.GetChild(0).gameObject.SetActive(false);
        // playerTopName.gameObject.transform.parent.gameObject.SetActive(false);
        StartCoroutine(turnOff());
        GameManager.manager.expUI.DisplayText(this);

    }

    public IEnumerator turnOff()
    {
        // yield return new WaitForSeconds(1f);
        username.transform.GetChild(2).gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
        playerHP.SetActive(false);
        playerHPCubes.SetActive(false);
        hpBar.SetActive(false);
        bar.SetActive(false);
        bar.transform.GetChild(0).gameObject.SetActive(false);
        holster.gameObject.SetActive(false);
        deck.gameObject.SetActive(false);
        playerHP.gameObject.SetActive(false);
        playerHPCubes.gameObject.SetActive(false);
        playerHPCubes.transform.GetChild(0).gameObject.SetActive(false);
        username.SetActive(false);
        username.transform.GetChild(0).gameObject.SetActive(false);
        username.transform.GetChild(1).gameObject.SetActive(false);
        username.transform.GetChild(2).gameObject.SetActive(false);
        // GameManager.manager.playerTopName.gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void checkGauntletBonus()
    {
        foreach (CardDisplay cd in holster.cardList)
        {
            if (cd.card.cardName == "Gauntlet Ring of the Chatelaine")
            {
                Debug.Log("Gauntlet Ring Bonus!!!");
                if (deck.GetComponent<CharacterSlot>() != null)
                    deck.GetComponent<CharacterSlot>().gauntletBonus = true;
                return;
            }
        }
        Debug.Log("Did not find the gauntlet ring!");
        if (deck.GetComponent<CharacterSlot>() != null)
            deck.GetComponent<CharacterSlot>().gauntletBonus = false;
    }

    public void setDefaultTurn()
    {
        Debug.Log("setDefaultTurn triggered");
        currentPlayerHighlight.SetActive(true);

        reetsCycle = false;
        // check if cube should be taken
        if (cubed)
        {
            hpCubes--;
            if (hpCubes > 0)
            {
                hp = 10;
                // check for Echo Shard here!!!
                foreach (Card card in deck.deckList)
                {
                    if(card.cardName == "Echo Shard")
                    {
                        Debug.Log("HP changed to 7 because of Echo Shard!");
                        hp = 7;
                        break;
                    }
                }

                // safety check lol
                foreach (CardDisplay card in holster.cardList)
                {
                    if (card.card.cardName == "Echo Shard")
                    {
                        Debug.Log("HP changed to 7 because of Echo Shard!");
                        hp = 7;
                        break;
                    }
                }



                if (isSweetbitter && hpCubes == 1)
                {
                    character.canBeFlipped = true;
                    Debug.Log("Make UI message for this");
                }
            }
            else
            {
                if (isSweetbitter)
                {
                    foreach (CardDisplay cd in holster.cardList)
                    {
                        if (cd.card.cardName == "Phylactery")
                        {
                            hp = 1;
                            hpCubes = 1;
                            Debug.Log("Survived because of The Phlactery");
                            return;
                        }
                    }
                }

                // check for Echo Shard here!!!
                for (int i = 0; i < deck.deckList.Count; i++)
                {
                    if (deck.deckList[i].cardName == "Echo Shard")
                    {
                        Debug.Log("HP changed to 7 because of Echo Shard!");
                        hpCubes = 1;
                        hp = 7;
                        cubed = false;
                        updateHealthUI();

                        /*
                        GameObject obj = Instantiate(deck.gameObject,
                        deck.gameObject.transform.position,
                        deck.gameObject.transform.rotation,
                        deck.gameObject.transform);
                        */

                        GameManager.manager.td.addCard(deck.deckList[i]);
                        deck.deckList.RemoveAt(i);
                        deck.updateCardSprite();
                        GameManager.manager.sendMessage("The Echo Shard has been trashed!");
                        return;
                    }
                }

                // safety check lol
                for (int i = 0; i < holster.cardList.Count; i++)
                {
                    if (holster.cardList[i].card.cardName == "Echo Shard")
                    {
                        Debug.Log("HP changed to 7 because of Echo Shard!");
                        hpCubes = 1;
                        hp = 7;
                        cubed = false;
                        updateHealthUI();

                        GameManager.manager.td.addCard(holster.cardList[i]);
                        GameManager.manager.sendMessage("The Echo Shard has been trashed!");
                        return;
                    }
                }

                hp = 0;
                Debug.Log("Somebody is dead!");
                dead = true;
                fadeOut();
                // GameManager.manager.checkForEndGame();
                if (GameManager.manager.Game.tutorial)
                {
                    // do achievement check in here
                    // you probably want to make new UI for this so this is placeholder stuff

                    // hold on partner! don't do this yet
                    // GameManager.manager.pauseUI.SetActive(true);
                    GameManager.manager.dialog.endTutorialDialog();
                }
                else
                {
                    GameManager.manager.checkForEndGame();
                    /*
                    int numDead = 0;
                    int numOfThem = 0;
                    // check if every player is dead except one
                    foreach (CardPlayer cp in GameManager.manager.players)
                    {
                        if (cp.gameObject.activeInHierarchy)
                        {
                            numOfThem++;
                        }

                        if (cp.dead)
                        {
                            numDead++;
                        }

                        if (numOfThem - numDead == 1)
                        {
                            Debug.Log("The game is over, somebody won!");
                            Game.completedGame = true;
                            GameManager.manager.pauseUI.SetActive(true);
                        }
                    }
                    */
                }
            }

            }

        if(deck.GetComponent<CharacterSlot>() != null)
            deck.GetComponent<CharacterSlot>().gauntletBonus = false;
        // SPICY CHECK

        foreach (CardDisplay cd in holster.cardList)
        {
            if (cd.card.spicy && cd.card.name != "placeholder")
            {
                if (charName != "Singelotte")
                    subHealth(2);
            }

            if (cd.card.cardName == "Gauntlet Ring of the Chatelaine")
            {
                Debug.Log("Gauntlet Ring Bonus!!!");
                if (deck.GetComponent<CharacterSlot>() != null)
                    deck.GetComponent<CharacterSlot>().gauntletBonus = true;
            }
        }
        updateHealthUI();

        cubed = false;

        if (isPluot)
        {
            pluotCold = false;
            pluotDry = false;
            pluotHot = false;
            pluotWet = false;
        }

        pips = pipCount;

        foreach (CardDisplay cd in holster.cardList)
        {
            // Ring of the Rings
            // Double all ring effects, your rings cost 4 pips
            if (cd.card.cardName == "RingoftheRings")
            {
                doubleRingBonus = true;
            }
        }

        foreach (CardDisplay cd in holster.cardList)
        {

            // check for gambling ring
            if (cd.card.cardName == "RingofGamblingMopoji" && GameManager.manager.players[GameManager.manager.myPlayerIndex].pipsUsedThisTurn == 0)
            {
                Debug.Log("It's gambling time...");
                GameManager.manager.players[GameManager.manager.myPlayerIndex].pipCount = rng.Next(1, 11);
                if (GameManager.manager.players[GameManager.manager.myPlayerIndex].doubleRingBonus)
                {
                    Debug.Log("Double ring bonus");
                    GameManager.manager.players[GameManager.manager.myPlayerIndex].pipCount *= 2;
                }
                Debug.Log("New pip count: " + GameManager.manager.players[GameManager.manager.myPlayerIndex].pipCount);
                Debug.Log("New pip count: " + pipCount);
                pips = pipCount;
                updatePipsUI();
            }
        }

        foreach (CardDisplay cd in holster.cardList)
        {
            // Tiny Ring of Extra Coin Purse
            // Start your turn with +2 pips
            if (cd.card.cardName == "Tiny Ring of the Extra Coin Purse")
            {
                if (doubleRingBonus)
                {
                    pips += 4;
                }
                else
                {
                    pips += 2;
                }
            }

            if (cd.card.cardName == "Blacksnake Pip Sling")
            {
                pips += 2;
            }
        }

        if (hotCoffee)
        {
            Debug.Log("One damage from the coffee...");
            subHealth(1);
            hotCoffee = false;
        }

        // Dipstick Flicker check
        foreach (CardDisplay cd in holster.cardList)
        {
            if (cd.card.cardName == "Dipstick Flicker")
            {
                Debug.Log("Dipstick Flicker!!!");
                if(cd.aPotion.card.cardName == "placeholder")
                {
                    cd.artifactSlot.transform.parent.gameObject.SetActive(true);
                    cd.artifactSlot.transform.gameObject.SetActive(true);
                    int random = GameManager.manager.rng.Next(0, GameManager.manager.starterPotionCards.Count);
                    cd.aPotion.updateCard(GameManager.manager.starterPotionCards[random]);
                    // cd.aPotion.updateCard(GameManager.manager.starterPotionCard);
                }
            }
        }

        pipsUsedThisTurn = 0;
        potionsThrown = 0;
        artifactsUsed = 0;
        vesselsThrown = 0;
        uniqueArtifactsUsed = 0;
        lastArtifactUsed = "";
        ringBonus = false;
        bottleRocketBonus = false;
        blackRainBonus = false;
        opponentPreventedDamage = false;
        healBonus = false;
        phialBonus = false;
        hotCoffee = false;

        if (isNickles)
        {
            nicklesAction = false;
        }

        if (isSweetbitter)
        {
            bool hasRing = false;
            int loadedItems = 0;
            bool hasPhlactery = false;

            foreach (CardDisplay cd in holster.cardList)
            {
                if (cd.card.cardType == "Ring")
                {
                    hasRing = true;
                }

                if (cd.card.cardType == "Artifact" && cd.aPotion.card.cardName != "placeholder")
                {
                    loadedItems++;
                }

                if (cd.card.cardType == "Vessel" && (cd.vPotion1.card.cardName != "placeholder" && cd.vPotion2.card.cardName != "placeholder"))
                {
                    loadedItems++;
                }

                if (cd.card.cardName == "Phylactery")
                {
                    hasPhlactery = true;
                }
            }

            if (hasRing && hasPhlactery && loadedItems == 2)
            {
                Debug.Log("Mega damage!");
                GameManager.manager.dealDamageToAll(12);
            }
        }

        Debug.Log("Reached end of setDefaultTurn");
        updatePipsUI();
        GameManager.manager.checkMarketPrice();
    }

    public void addThePhylactery()
    {
        subPips(6);
        GameManager.manager.sendSuccessMessage(12);
        deck.putCardOnTop(uniqueCards[3]);
    }

    public void addReetsCard()
    {
        if (reetsCycle)
        {
            GameManager.manager.sendMessage("You already performed your action this turn!");
            return;
        }

        int stuff = 0;
        foreach (CardDisplay card in holster.cardList)
        {
            if (card.card.cardName != "placeholder")
            {
                stuff++;
            }
        }

        if(stuff == 0)
        {
            Debug.Log("No room in your Holster!!!");
            return;
        }


        Debug.Log("Activating Reets cycle ability");
        if (deck.deckList.Count >= 1)
        {
            foreach (CardDisplay card in holster.cardList)
            {
                if (card.card.cardName == "placeholder")
                {
                    Card temp = deck.popCard();
                    card.updateCard(temp);
                    // should reets cycle be infinite or limited to once per turn???
                    // reetsCycle = true;
                    GameManager.manager.sendMessage("Added a card into your holster!");
                    break;
                }
            }

            if (!character.flipped)
            {
                subPips(2);
            }
            else
            {
                subPips(1);
            }
        }
        else
        {
            Debug.Log("Reets error");

            // TODO: make error message that signifies that you cannot do action
            GameManager.manager.sendErrorMessage(14);
        }
    }

    public void addCherryBombBadge()
    {
        foreach (CardDisplay card in holster.cardList)
        {
            if (card.card.cardName == "placeholder")
            {
                card.updateCard(uniqueCards[1]);
                return;
            }
        }
        GameManager.manager.sendErrorMessage(13);
    }

    public void addExtraInventory()
    {
        foreach (CardDisplay card in holster.cardList)
        {
            if (card.card.cardName == "placeholder")
            {
                card.updateCard(uniqueCards[2]);
                GameManager.manager.sendSuccessMessage(19);
                return;
            }
        }
        GameManager.manager.sendErrorMessage(13);
    }

    public void addPipSling()
    {
        foreach (CardDisplay card in holster.cardList)
        {
            if (card.card.cardName == "placeholder")
            {
                card.updateCard(uniqueCards[0]);
                // any time the pip sling is added to your hand, gain +2P
                addPips(2);
                return;
            }
        }
        GameManager.manager.sendErrorMessage(13);
    }

    public void setCurrentPlayer()
    {
        currentPlayerHighlight.SetActive(true);
    }

    public void removeCurrentPlayer()
    {
        currentPlayerHighlight.SetActive(false);
    }

    public int checkRings()
    {
        int rings = 0;
        foreach (CardDisplay cd in holster.cardList)
        {
            if (cd.card.cardType == "Ring")
            {
                rings++;
            }
        }
        return rings;
    }

    public int checkOneRing()
    {
        foreach (CardDisplay cd in holster.cardList)
        {
            if (cd.card.cardType == "Ring")
            {
                return 1;
            }
        }
        return 0;
    }

    /***************************
        CHECK ARTIFACT BONUS
    ***************************/

    public int checkArtifactBonus(int damage, CardDisplay selectedCard)
    {
        // probably depends on what card it is and what bonus it has, might have to implement logic for certain pools of cards this way

        foreach (CardPlayer cd in GameManager.manager.players)
        {
            if (cd.name == GameManager.manager.currentPlayerName && cd.isIsadore)
            {
                damage++;
                break;
            }
        }

        // Treasure Cloak Map
        // put a potion from the trash to the top of your deck
        if (selectedCard.card.cardName == "Treasure Cloak Map")
        {
            // add a check to see if a computer player triggered this and don't display the menu
            // probably just select a random card from the trash
            if (gameObject.GetComponent<ComputerPlayer>() == null)
            {
                GameManager.manager.numTrashed = 1;
                GameManager.manager.trashDeckBonus = true;
                GameManager.manager.trashDeckMenu.SetActive(true);
                GameManager.manager.trashText.text = "Take a potion from the trash and put it on top of your deck!";
                GameManager.manager.td.displayTrash();
                return 0;
            } else
            {
                // logic for computer player
                for(int i = 0; i < GameManager.manager.td.deckList.Count; i++)
                {
                    if(GameManager.manager.td.deckList[i].cardType == "Potion")
                    {
                        Card cd = GameManager.manager.td.popCard(i);
                        deck.putCardOnTop(cd);
                        GameManager.manager.sendSuccessMessage(15);
                    }
                }
            }
            
        }

        // The Skateboard
        if (selectedCard.card.cardName == "Skateboard")
        {
            // if a loaded potion has an additional effect, do a trick instead of damage
            // ask matteo and figure out exactly what that means lol

            // if the loaded potion does not have any card text
            // i'm just gonna list the cards because i'm lazy
            // this might not actually be accurate to how the card should work but i'm taking a shortcut
            if (selectedCard.aPotion.card.cardName != "NorthernOquinox" && selectedCard.aPotion.card.cardName != "PotionThatMakesHatsUglier" &&
                selectedCard.aPotion.card.cardName != "SeriesOfPoisonousWords" && selectedCard.aPotion.card.cardName != "VerySeriousThreat" &&
                selectedCard.aPotion.card.cardName != "BottleOfLeastAmountOfSpiders" && selectedCard.aPotion.card.cardName != "ContainerFilledWithAngryBees" &&
                selectedCard.aPotion.card.cardName != "CupOfNoodles" && selectedCard.aPotion.card.cardName != "PassiveAggressiveSlurry" &&
                selectedCard.aPotion.card.cardName != "ClassicFireball" && selectedCard.aPotion.card.cardName != "ElectronicTonic" &&
                selectedCard.aPotion.card.cardName != "LossOfOnesIntimatePossesions" && selectedCard.aPotion.card.cardName != "ShotOfWillOWisp" &&
                selectedCard.aPotion.card.cardName != "BoldBundleOfLightning" && selectedCard.aPotion.card.cardName != "CupfulOfRealm" &&
                selectedCard.aPotion.card.cardName != "GoopGasAttack" && selectedCard.aPotion.card.cardName != "ScreechingCry" &&
                selectedCard.aPotion.card.cardName != "ATearofBlackRain" && selectedCard.aPotion.card.cardName != "QuartOfLemonade" &&
                selectedCard.aPotion.card.cardName != "VintageAromaticKate" && selectedCard.aPotion.card.cardName != "JarFullOfGlitter" &&
                selectedCard.aPotion.card.cardName != "KissFromTheLipsOfAnAncientLove" && selectedCard.aPotion.card.cardName != "HumblingGlimpse")
            {
                Debug.Log("SKATEBOARD TRICK DONE!!!");
                GameManager.manager.sendSuccessMessage(16);
                tricks++;
                // after 4 tricks, add an essence cube to their collection
                if (tricks == 4)
                {
                    tricks = 0;
                    hpCubes++;
                    updateHealthUI();

                    // TODO: add success message signifying you did a trick
                }
                return damage;
            }
        }

        // Hammer of Engagement
        if (selectedCard.card.cardName == "HammerOfEnagagment")
        {
            /*
             * Hot Bonus: +2 Damage.
               Holster Bonus: +1 Damage if a ring is in your Holster.
             */
            if (selectedCard.aPotion.card.cardQuality == "Hot")
            {
                damage += 2;
            }
            damage += checkOneRing();
        }

        // Paperweight of Bauble Collector
        // very similar to hammer of engagement
        if (selectedCard.card.cardName == "PaperweightOfTheBaubleCollector")
        {
            /*
             * Hot Bonus: +1 Damage.
               Holster Bonus: +1 Damage for each Ring in your Holster.
             */
            if (selectedCard.aPotion.card.cardQuality == "Hot")
            {
                damage++;
            }
            damage += checkRings();
        }

        // Gauntlet Mounted Trebuchet
        // this card text is definitely bad lol sorry i don't make the rules
        if (selectedCard.card.cardName == "GauntletMountedTrebuchet")
        {
            // Dry Bonus: Double potion damage
            // what they meant was "double the loaded potion's damage and add it to the artifact damage"
            if (selectedCard.aPotion.card.cardQuality == "Dry")
            {
                damage += (2 * selectedCard.aPotion.card.effectAmount);
            }
        }

        // Spigot of Endless Coinage
        // Add Pips equal to the loaded potions' buy cost, +1 additional Pip
        if (selectedCard.card.cardName == "SpigotOfEndless")
        {
            // this card doesn't deal damage, but it gives you lots of money
            addPips(selectedCard.aPotion.card.buyPrice + 1);
            return 0;
        }

        // The Pocket Counterfeiter
        // Add Pips equal to the loaded potions' damage
        if (selectedCard.card.cardName == "PocketCounterfeiter")
        {
            addPips(selectedCard.aPotion.card.effectAmount);
            return 0;
        }

        // Tablet Containing All Knowledge
        if (selectedCard.card.cardName == "Tablet Containing All Knowledge")
        {
            // Dry Bonus: +2 Damage
            if (selectedCard.aPotion.card.cardQuality == "Dry" || selectedCard.aPotion.card.cardQuality == "Hot")
            {
                addPips(1);
                return 3;
            }
            else if (selectedCard.aPotion.card.cardQuality == "Wet" || selectedCard.aPotion.card.cardQuality == "Cold")
            {
                addPips(1);
                addHealth(3);
                return 0;
            }
            else
            // if cardQuality == "None"
            {
                addPips(3);
                GameManager.manager.deal1ToAll();
            }
        }

        // The Daggerheels
        if (selectedCard.card.cardName == "Daggerheels")
        {
            // Cold Bonus: +1 Damage
            if (selectedCard.aPotion.card.cardQuality == "Cold")
            {
                damage++;
            }
        }

        // The Rapid Fire Caltrop Hand Cannon
        if (selectedCard.card.cardName == "RapidFireCaltrop")
        {
            // Dry Bonus: +2 Damage
            if (selectedCard.aPotion.card.cardQuality == "Dry")
            {
                damage += 2;
            }
        }

        // Pewter Heart Necklace
        if (selectedCard.card.cardName == "PewterHeartNecklace")
        {
            addHealth(2);
            // Wet Bonus: +2 HP
            if (selectedCard.aPotion.card.cardQuality == "Cold")
            {
                addHealth(2);
            }
            // this card doesn't damage anyone
            return 0;
        }

        // Wooden Dryad's Kiss
        if (selectedCard.card.cardName == "WoodenDryadsKiss")
        {
            addHealth(2);
            // Wet Bonus: +2 HP
            if (selectedCard.aPotion.card.cardQuality == "Wet")
            {
                addHealth(2);
            }
            // this card doesn't damage anyone
            return 0;
        }

        // The Bottle Rocket
        if (selectedCard.card.cardName == "BottleRocket")
        {
            // Wet Bonus: +2 Damage
            // 3 P: Double the damage of this artifact this turn. You may only do this once per turn.
            if (selectedCard.aPotion.card.cardQuality == "Wet")
            {
                damage += 2;
                if (bottleRocketBonus)
                {
                    // double this bitch
                    damage = damage * 2;
                }
            }
        }

        return damage;
    }

    /*************************
        CHECK VESSEL BONUS
    *************************/

    public int checkVesselBonus(int damage, CardDisplay selectedCard)
    {
        // Pluot damage bonus
        if (isPluot)
        {
            if (pluotBonusType == selectedCard.vPotion1.card.cardQuality ||
                pluotBonusType == selectedCard.vPotion2.card.cardQuality)
            {
                damage++;
            }


            if (selectedCard.vPotion1.card.cardQuality == "Hot" ||
                selectedCard.vPotion2.card.cardQuality == "Hot")
            {
                pluotHot = true;
            }

            if (selectedCard.vPotion1.card.cardQuality == "Wet" ||
                selectedCard.vPotion2.card.cardQuality == "Wet")
            {
                pluotWet = true;
            }

            if (selectedCard.vPotion1.card.cardQuality == "Cold" ||
                selectedCard.vPotion2.card.cardQuality == "Cold")
            {
                pluotCold = true;
            }

            if (selectedCard.vPotion1.card.cardQuality == "Dry" ||
                selectedCard.vPotion2.card.cardQuality == "Dry")
            {
                pluotDry = true;
            }

            // PLUOT FLIP LOGIC
            if (pluotHot && pluotWet && pluotCold && pluotDry)
            {
                character.canBeFlipped = true;
                GameManager.manager.sendSuccessMessage(13);
            }
        }

        // Hollowed Out Skull of Higyoude
        if (selectedCard.card.cardName == "HollowedOutSkull")
        {
            // does 6 damage, ignores all potion effects
            return 6;
        }

        // Squeezebox
        if (selectedCard.card.cardName == "Squeezebox")
        {
            healBonus = true;
        }

        // Fragile Glass Ornament
        // Does +1 Damage. Ignores loaded potion effects in this Vessel
        if (selectedCard.card.cardName == "Fragile Glass Ornament")
        {
            // this card ignores all potion effects, so i'm returning damage + 1
            Debug.Log("Fragile Glass Ornament damage bonus");
            return damage + 1;
        }

        // checking the potions for throw bonuses
        damage = checkBonus(damage, selectedCard.vPotion1);
        damage = checkBonus(damage, selectedCard.vPotion2);

        // Drinking Horn of a Sea Unicorn's Tooth
        if (selectedCard.card.cardName == "Drinking Horn of a Sea Unicorn's Tooth")
        {
            if (selectedCard.vPotion1.card.cardQuality == "Cold" ||
                selectedCard.vPotion2.card.cardQuality == "Cold")
            {
                if (GameManager.manager.Game.multiplayer)
                {
                    foreach (GamePlayer gp in GameManager.manager.Game.GamePlayers)
                    {
                        if (gp.playerName == GameManager.manager.currentPlayerName)
                        {
                            Debug.Log("Target RPC, Trash Menu Active");
                            gp.RpcTrashMenuActive();
                        }
                    }
                }
                else
                {
                    // TODO: check for computer player and disable this

                    GameManager.manager.numTrashed += 2;
                    GameManager.manager.trashMarketUI.SetActive(true);
                    GameManager.manager.updateTrashMarketMenu();
                }
            }

            if (selectedCard.vPotion1.card.cardQuality == "Wet" ||
                selectedCard.vPotion2.card.cardQuality == "Wet")
            {
                if (GameManager.manager.Game.multiplayer)
                {
                    foreach (GamePlayer gp in GameManager.manager.Game.GamePlayers)
                    {
                        if (gp.playerName == GameManager.manager.currentPlayerName)
                        {
                            Debug.Log("Target RPC, Trash Menu Active");
                            gp.RpcTrashMenuActive();
                        }
                    }
                }
                else
                {
                    GameManager.manager.numTrashed += 2;
                    GameManager.manager.trashMarketUI.SetActive(true);
                    GameManager.manager.updateTrashMarketMenu();
                }
            }
        }

        // First Place Volcano at the Alchemy Faire
        // Put up to 1 card from the Market onto the top of your deck.
        if (selectedCard.card.cardName == "First Place Volcano in the Alchemy Faire")
        {
            // this should follow the similar logic of the trash a card from the market UI
            Debug.Log("Take Market Bonus");
            if (GameManager.manager.Game.multiplayer)
            {
                foreach (GamePlayer gp in GameManager.manager.Game.GamePlayers)
                {
                    if (gp.playerName == GameManager.manager.currentPlayerName)
                    {
                        Debug.Log("Target RPC, Take Menu Active");
                        gp.RpcTMMenuActive();
                    }
                }
            }
            else
            {
                GameManager.manager.takeMarketMenu.SetActive(true);
                GameManager.manager.updateTakeMarketMenu();
            }
        }

        // Vessel Bonus: Put any potion from the trash to the top of your deck
        if ((selectedCard.vPotion1.card.cardName == "A Squeeze of Wheezyfish" || selectedCard.vPotion2.card.cardName == "A Squeeze of Wheezyfish") ||
            (selectedCard.vPotion1.card.cardName == "A Swig of Regained Burning Appetite" || selectedCard.vPotion2.card.cardName == "A Swig of Regained Burning Appetite") ||
            (selectedCard.vPotion1.card.cardName == "TinctureOfMeltedMarble" || selectedCard.vPotion2.card.cardName == "TinctureOfMeltedMarble") ||
            (selectedCard.vPotion1.card.cardName == "A Freshly Caught and Distilled Sickness" || selectedCard.vPotion2.card.cardName == "A Freshly Caught and Distilled Sickness"))
        {
            if (GameManager.manager.Game.multiplayer)
            {
                foreach (GamePlayer gp in GameManager.manager.Game.GamePlayers)
                {
                    if (gp.playerName == GameManager.manager.currentPlayerName)
                    {
                        Debug.Log("Target RPC, Trash Deck Menu Active");
                        gp.RpcTDMenuActive();
                    }
                }
            }
            else
            {
                GameManager.manager.trashDeckBonus = true;
                GameManager.manager.trashDeckMenu.SetActive(true);
                GameManager.manager.trashText.text = "Take a potion from the trash and put it on top of your deck!";
                GameManager.manager.td.displayTrash();
            }
        }

        // Fragile Glass Ornament
        // Does +1 Damage. Ignores loaded potion effects in this Vessel
        if (selectedCard.card.cardName == "Fragile Glass Ornament")
        {
            // this card ignores all potion effects, so i'm returning damage + 1
            return damage + 1;
        }

        // Vessel Bonus: +2 Damage
        if ((selectedCard.vPotion1.card.cardName == "RefectionOfOnesGnarledEmotionalSelf" || selectedCard.vPotion2.card.cardName == "RefectionOfOnesGnarledEmotionalSelf") ||
            (selectedCard.vPotion1.card.cardName == "SoupMadeOfGunpowder" || selectedCard.vPotion2.card.cardName == "SoupMadeOfGunpowder") ||
            (selectedCard.vPotion1.card.cardName == "PourOfReallyAngryAcid" || selectedCard.vPotion2.card.cardName == "PourOfReallyAngryAcid"))
        {
            damage += 2;
        }

        // Hot + Wet Bonus
        // It's only two cards
        if ((selectedCard.vPotion1.card.cardQuality == "Hot" && selectedCard.vPotion2.card.cardQuality == "Wet") ||
           (selectedCard.vPotion2.card.cardQuality == "Hot" && selectedCard.vPotion1.card.cardQuality == "Wet"))
        {
            if (selectedCard.card.cardName == "Empty Ravioli")
            {
                addHealth(4);
            }

            // Furry Flagon Made from the Hide of Hairy Leeches
            // Hot + Wet Bonus: Put 1 card from the trash to the top of your deck
            if (selectedCard.card.cardName == "Furry Flagon fromthe Hide of Hairy Leeches")
            {
                if (gameObject.GetComponent<ComputerPlayer>() == null)
                {
                    GameManager.manager.trashDeckBonus = true;
                    GameManager.manager.trashDeckMenu.SetActive(true);
                    GameManager.manager.trashText.text = "Take a potion from the trash and put it on top of your deck!";
                    GameManager.manager.td.displayTrash();
                } else
                {
                    // logic for computer player
                    for (int i = 0; i < GameManager.manager.td.deckList.Count; i++)
                    {
                        if (GameManager.manager.td.deckList[i].cardType == "Potion")
                        {
                            Card cd = GameManager.manager.td.popCard(i);
                            deck.putCardOnTop(cd);
                            GameManager.manager.sendSuccessMessage(15);
                        }
                    }
                }
                
            }

            // Voluptuous Gallipot of Double Entendre
            // come back to this
            if (selectedCard.card.cardName == "VoluptuousGallipot")
            {
                Debug.Log("Gal, your pot!");
                // Command check starting
                if (GameManager.manager.Game.multiplayer)
                {
                    foreach (GamePlayer gp in GameManager.manager.Game.GamePlayers)
                    {
                        if (gp.playerName == GameManager.manager.currentPlayerName)
                        {
                            Debug.Log("Target RPC, OpponentHolsterMenu Active");
                            gp.RpcTMMenuActive();
                        }
                    }
                    return damage;
                }


                // add a check for a ComputerPlayer component

                // if they're not a computer player
                if (gameObject.GetComponent<ComputerPlayer>() == null)
                {
                    GameManager.manager.opponentHolsterMenu.SetActive(true);
                    GameManager.manager.displayOpponentHolster(GameManager.manager.tempPlayer);
                }
                else
                {
                    // maybe do something for computer
                    int cardNumber = rng.Next(1, 5);
                    GameManager.manager.selectedCardInt = cardNumber;

                    GameObject obj = Instantiate(GameManager.manager.tempPlayer.holster.cardList[GameManager.manager.selectedCardInt - 1].gameObject,
                            GameManager.manager.tempPlayer.holster.cardList[GameManager.manager.selectedCardInt - 1].gameObject.transform.position,
                            GameManager.manager.tempPlayer.holster.cardList[GameManager.manager.selectedCardInt - 1].gameObject.transform.rotation,
                            GameManager.manager.tempPlayer.holster.cardList[GameManager.manager.selectedCardInt - 1].gameObject.transform);

                    GameManager.manager.StartCoroutine(MoveToTrash(obj));

                    GameManager.manager.td.addCard(GameManager.manager.tempPlayer.holster.cardList[GameManager.manager.selectedCardInt - 1]);

                    GameManager.manager.sendMessage("A card in your holster just got trashed!");
                }

                return damage;
            }
        }

        // Cursed Cucumella of Clumsy Acidic Coffee
        // Hot Bonus: Opponent trashes 1 card. Wet Bonus: Opponent trashes 1 card.
        if (selectedCard.card.cardName == "CursedCucumella")
        {
            if (selectedCard.vPotion1.card.cardQuality == "Hot" || selectedCard.vPotion2.card.cardQuality == "Hot")
            {
                if (GameManager.manager.Game.multiplayer)
                {
                    Debug.Log("Command check starting");
                    foreach (GamePlayer gp in GameManager.manager.Game.GamePlayers)
                    {
                        if (gp.playerName == GameManager.manager.tempPlayer.name)
                        {
                            Debug.Log("Target RPC, Trash Deck Menu Active");
                            gp.RpcTrashOneCard(gp.playerName);
                        }
                    }
                }
                else
                {
                    // add a check for a ComputerPlayer component

                    // if the ememy is not a computer player
                    if (GameManager.manager.tempPlayer.gameObject.GetComponent<ComputerPlayer>() == null)
                    {
                        GameManager.manager.opponentHolsterMenu.SetActive(true);
                        GameManager.manager.displayOpponentHolster();
                    }
                    else
                    {
                        // maybe do something for computer
                        // make method that trashes a random card
                        int cardNumber = rng.Next(1, 5);
                        GameManager.manager.selectedCardInt = cardNumber;

                        GameObject obj = Instantiate(GameManager.manager.tempPlayer.holster.cardList[GameManager.manager.selectedCardInt - 1].gameObject,
                                GameManager.manager.tempPlayer.holster.cardList[GameManager.manager.selectedCardInt - 1].gameObject.transform.position,
                                GameManager.manager.tempPlayer.holster.cardList[GameManager.manager.selectedCardInt - 1].gameObject.transform.rotation,
                                GameManager.manager.tempPlayer.holster.cardList[GameManager.manager.selectedCardInt - 1].gameObject.transform);

                        GameManager.manager.StartCoroutine(MoveToTrash(obj));

                        GameManager.manager.td.addCard(GameManager.manager.tempPlayer.holster.cardList[GameManager.manager.selectedCardInt - 1]);

                        GameManager.manager.sendMessage("A card in your holster just got trashed!");
                    }

                    // GameManager.manager.opponentHolsterMenu.SetActive(true);
                    // GameManager.manager.displayOpponentHolster();
                }
            }

            if (selectedCard.vPotion1.card.cardQuality == "Wet" || selectedCard.vPotion2.card.cardQuality == "Wet")
            {
                if (GameManager.manager.Game.multiplayer)
                {
                    Debug.Log("Command check starting");
                    foreach (GamePlayer gp in GameManager.manager.Game.GamePlayers)
                    {
                        if (gp.playerName == GameManager.manager.tempPlayer.name)
                        {
                            Debug.Log("Target RPC, Trash Deck Menu Active");
                            gp.RpcTrashOneCard(gp.playerName);
                        }
                    }
                }
                else
                {
                    // add a check for a ComputerPlayer component

                    // if they're not a computer player
                    if (GameManager.manager.tempPlayer.gameObject.GetComponent<ComputerPlayer>() == null)
                    {
                        GameManager.manager.opponentHolsterMenu.SetActive(true);
                        GameManager.manager.displayOpponentHolster();
                    }
                    else
                    {
                        // maybe do something for computer
                        int cardNumber = rng.Next(1, 5);
                        GameManager.manager.selectedCardInt = cardNumber;

                        GameObject obj = Instantiate(GameManager.manager.tempPlayer.holster.cardList[GameManager.manager.selectedCardInt - 1].gameObject,
                                GameManager.manager.tempPlayer.holster.cardList[GameManager.manager.selectedCardInt - 1].gameObject.transform.position,
                                GameManager.manager.tempPlayer.holster.cardList[GameManager.manager.selectedCardInt - 1].gameObject.transform.rotation,
                                GameManager.manager.tempPlayer.holster.cardList[GameManager.manager.selectedCardInt - 1].gameObject.transform);

                        GameManager.manager.StartCoroutine(MoveToTrash(obj));

                        GameManager.manager.td.addCard(GameManager.manager.tempPlayer.holster.cardList[GameManager.manager.selectedCardInt - 1]);

                        GameManager.manager.sendMessage("A card in your holster just got trashed!");
                    }
                    // GameManager.manager.opponentHolsterMenu.SetActive(true);
                    // GameManager.manager.displayOpponentHolster();
                }
            }
        }

        // Hot + Dry Bonus
        if ((selectedCard.vPotion1.card.cardQuality == "Hot" && selectedCard.vPotion2.card.cardQuality == "Dry") ||
            (selectedCard.vPotion2.card.cardQuality == "Hot" && selectedCard.vPotion1.card.cardQuality == "Dry"))
        {
            // Filthy Urn Holding the Ashes of 60k
            // Hot + Dry Bonus: +4 Damage
            if (selectedCard.card.cardName == "FilthyUrn")
            {
                damage += 4;

            }

            // The Dual Rhyton of Phobos and Deimos
            // Hot + Dry Bonus: Replace all the market cards. You get +3 Pips
            if (selectedCard.card.cardName == "DualRhyton")
            {
                // TODO: make replaceMarketCards() method in GameManager to replace all the market cards
                GameManager.manager.popAllMarketCards();
                addPips(3);
            }
        }


        // Cold + Wet Bonus
        if ((selectedCard.vPotion1.card.cardQuality == "Cold" && selectedCard.vPotion2.card.cardQuality == "Wet") ||
            (selectedCard.vPotion2.card.cardQuality == "Cold" && selectedCard.vPotion1.card.cardQuality == "Wet"))
        {
            // Philty Phlegmbic Alembic of Philters Polemic
            //  1 opponent trashes all cards in their holster, or all opponents trash 1 card 
            if (selectedCard.card.cardName == "PhiltyPhlegmbicAlembic")
            {
                // first they'll need to pick their bonus
                if(gameObject.GetComponent<ComputerPlayer>() == null)
                    GameManager.manager.trashBonusMenu.SetActive(true);
                else
                {
                    Debug.Log("Add computer logic");
                    // Target the main player on purpose lol
                    foreach (CardDisplay cd in GameManager.manager.playerHolster.cardList)
                    {
                        if (cd.card.cardName != "placeholder")
                        {
                            GameObject obj = Instantiate(cd.gameObject,
                            cd.gameObject.transform.position,
                            cd.gameObject.transform.rotation,
                            cd.gameObject.transform);

                            GameManager.manager.StartCoroutine(MoveToTrash(obj));

                            GameManager.manager.td.addCard(cd);

                            GameManager.manager.sendMessage("A card in your holster just got trashed!");
                        }
                    }
                }

                //GameManager.manager.opponentHolsterMenu.SetActive(true);
                //GameManager.manager.displayOpponentHolster();
            }
        }

        // Cold + Dry Bonus
        if ((selectedCard.vPotion1.card.cardQuality == "Cold" && selectedCard.vPotion2.card.cardQuality == "Dry") ||
            (selectedCard.vPotion2.card.cardQuality == "Cold" && selectedCard.vPotion1.card.cardQuality == "Dry"))
        {
            Debug.Log("Cold and Dry Bonus!");

            if (selectedCard.card.cardName == "Empty Ravioli")
            {
                return damage + 4;
            }

            // The Boxing Flask of the Fist Wizard
            // Deal +3 damage to all other opponents
            if (selectedCard.card.cardName == "Boxing Flask of the Fist Wizard")
            {
                // TODO: make deal3ToAll() method inside GameManager to deal 3 damage to every CardPlayer except yourself
                GameManager.manager.deal3ToAll();
                return 0;
            }

            // The Vinyl Demijohn of Tunes and Libation
            // Put a market card on the top of your deck
            if (selectedCard.card.cardName == "VinylDemijohnofTunesandLibation")
            {
                // TODO: check for computer player and check related method in GameManager

                // if they're not a computer player
                if (GameManager.manager.tempPlayer.gameObject.GetComponent<ComputerPlayer>() == null
                    && gameObject.GetComponent<ComputerPlayer>() == null)
                {
                    GameManager.manager.takeMarketMenu.SetActive(true);
                    GameManager.manager.updateTakeMarketMenu();
                }
                else
                {
                    // maybe do something for computer
                    Debug.Log("Add computer logic");
                    int randomCard = rng.Next(1, 7);
                    GameManager.manager.takeMarket(randomCard);
                }

            }
        }
        // Synergy of Opposing Minds
        if (selectedCard.card.cardName == "Synergy of Opposing Minds")
        {
            Debug.Log("Synergy of Opposing Minds!");
            addHealth(2);
            if ((selectedCard.vPotion1.card.cardQuality == "Cold" && selectedCard.vPotion2.card.cardQuality == "Hot") ||
            (selectedCard.vPotion2.card.cardQuality == "Hot" && selectedCard.vPotion1.card.cardQuality == "Cold"))
            {
                StartCoroutine(LookAtCards());
            }
        }

        // Shining Reliquary of Bleeding Liquid Gold
        if (selectedCard.card.cardName == "ShiningReliquary")
        {
            if (selectedCard.vPotion1.card.cardQuality == "Cold" || selectedCard.vPotion2.card.cardQuality == "Cold")
            {
                addPips(2);
            }
            if (selectedCard.vPotion1.card.cardQuality == "Dry" || selectedCard.vPotion2.card.cardQuality == "Dry")
            {
                addPips(2);
            }
        }

        // A Curious Assortment of Apothecary Pills
        if (selectedCard.card.cardName == "CuriousAssortmentOfPills")
        {
            // this card ignores all potion damage, so I'm returning 0 on purpose
            addHealth(4);
            return 0;
        }

        // Canteen Carved from a Living Meteorite
        if (selectedCard.card.cardName == "CanteenCarvedFromMeteorite")
        {
            if (selectedCard.vPotion1.card.cardQuality == "Cold" || selectedCard.vPotion2.card.cardQuality == "Cold")
            {
                addHealth(3);
            }
            if (selectedCard.vPotion1.card.cardQuality == "Dry" || selectedCard.vPotion2.card.cardQuality == "Dry")
            {
                addHealth(3);
            }
        }

        // Silken Hanky with a Cool Delicate Heartbeat
        if (selectedCard.card.cardName == "SilkenHanky")
        {
            // Hot Bonus: +3 Damage
            if (selectedCard.vPotion1.card.cardQuality == "Hot" || selectedCard.vPotion2.card.cardQuality == "Hot")
            {
                damage += 3;
            }
        }

        // Sandpaper Gunnysack with a Wire Drawstring
        if (selectedCard.card.cardName == "SandpaperGunnysack")
        {
            // Dry Bonus: +3 Damage
            if (selectedCard.vPotion1.card.cardQuality == "Dry" || selectedCard.vPotion2.card.cardQuality == "Dry")
            {
                damage += 3;
            }
        }

        // Cooled Carafe for the Shipwrecked Spirit
        if (selectedCard.card.cardName == "CooledCarafe")
        {
            // Cold Bonus: +3 Damage
            if (selectedCard.vPotion1.card.cardQuality == "Cold" || selectedCard.vPotion2.card.cardQuality == "Cold")
            {
                damage += 3;
            }
        }

        // Drawstring Pouch of a Broken Umbrella
        if (selectedCard.card.cardName == "DrawstringPouch")
        {
            // Wet Bonus: +3 Damage
            if (selectedCard.vPotion1.card.cardQuality == "Wet" || selectedCard.vPotion2.card.cardQuality == "Wet")
            {
                damage += 3;
            }
        }

        return damage;
    }

    public IEnumerator LookAtCards()
    {
        for (int i = 0; i < GameManager.manager.numPlayers; i++)
        {
            foreach (CardDisplay cd in GameManager.manager.players[i].holster.cardList)
            {
                if (cd.card.cardName == "placeholder")
                {
                    GameManager.manager.players[i].subHealth(1);
                    yield return new WaitForSeconds(0.25f);
                }
            }
            // GameManager.manager.players[i].
        }
    }

    public IEnumerator MoveToTrash(GameObject obj)
    {
        obj.transform.SetParent(obj.transform.parent.parent);
        GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.selectedCardInt - 1].artifactSlot.transform.GetChild(0).gameObject.SetActive(false);
        // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].aPotion.gameObject.SetActive(false);
        obj.transform.DOJump(new Vector2(1850f * GameManager.manager.widthRatio, 400f * GameManager.manager.heightRatio), 400f, 1, 1f, false);
        obj.transform.DOScale(0.2f, 1f);
        obj.transform.DORotate(new Vector3(0, 0, 720f), 1f, RotateMode.FastBeyond360);
        yield return new WaitForSeconds(0.1f);
        GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.selectedCardInt - 1].artifactSlot.transform.GetChild(0).gameObject.SetActive(true);
        GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.selectedCardInt - 1].artifactSlot.SetActive(false);
        yield return new WaitForSeconds(0.6f);
        obj.GetComponent<Image>().CrossFadeAlpha(0, 0.2f, false);
        yield return new WaitForSeconds(0.3f);

        Destroy(obj);
        // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).gameObject.SetActive(true);
        // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.SetActive(false);

        // MATTEO : Add trash can thunk sfx here!
        GameManager.manager.td.transform.parent.DOMove(new Vector2
            (GameManager.manager.td.transform.parent.position.x, GameManager.manager.td.transform.parent.position.y - 5), 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    /***********************
        CHECK RING BONUS
    ***********************/

    public int checkRingBonus(int damage, CardDisplay selectedCard)
    {
        // ring bonuses obviously, but only the ones that relate to damage
        // all defensive stuff is going in its own separate method

        foreach (CardDisplay cd in holster.cardList)
        {
            if (cd.card.cardType == "Ring")
            {
                Debug.Log(selectedCard.card.cardType);
                if (cd.card.cardQuality == "Starter" && potionsThrown == 0 && vesselsThrown == 0 && selectedCard.card.cardType != "Artifact")
                {
                    Debug.Log("Starter ring bonus");
                    damage++;
                }
                // Sharpened Ring of Bauble Collector
                if (cd.card.cardName == "Sharpened Ring of the Bauble Collector")
                {
                    if (selectedCard.card.cardType == "Artifact")
                    {
                        // if the card is an artifact that does damage, +1 damage
                        if (selectedCard.card.cardName != "BubbleWand" && selectedCard.card.cardName != "PewterHeartNecklace" &&
                            selectedCard.card.cardName != "Shield of the Mouth of Truth" && selectedCard.card.cardName != "PocketCounterfeiter" &&
                            selectedCard.card.cardName != "SpigotOfEndless" && selectedCard.card.cardName != "Treasure Cloak Map" &&
                            selectedCard.card.cardName != "WoodenDryadsKiss")
                        {
                            if (doubleRingBonus)
                            {
                                damage += 2;
                            }
                            else
                            {
                                damage++;
                            }
                        }
                    }
                }

                // Finger Ring of Additional Pinkie
                if (cd.card.cardName == "FingerRingoftheAdditionalPinkie")
                {
                    // Thrown potions deal +1 damage
                    if (selectedCard.card.cardType == "Potion")
                    {
                        if (doubleRingBonus)
                        {
                            damage += 2;
                        }
                        else
                        {
                            damage++;
                        }
                    }
                }

                // Glass Ring of Things That Contain Things
                if (cd.card.cardName == "GlassRingofThingsThatContainThings")
                {
                    // All of your thrown vessels deal +3 damage
                    if (selectedCard.card.cardType == "Vessel")
                    {
                        if (doubleRingBonus)
                        {
                            damage += 6;
                        }
                        else
                        {
                            damage += 3;
                        }
                    }
                }
            }
        }
        return damage;
    }

    public IEnumerator changeAlpha(GameObject obj)
    {
        yield return new WaitForSeconds(2f);
        obj.SetActive(true);
        if(obj.GetComponent<CanvasGroup>() != null)
            obj.GetComponent<CanvasGroup>().alpha = 0;
    }

    public IEnumerator AddCardToHolster()
    {
        yield return new WaitForSeconds(0.25f);
        if (GameManager.manager.players[GameManager.manager.myPlayerIndex].deck.deckList.Count >= 1)
        {
            Debug.Log("Buffet Bonus!");

            foreach (CardDisplay card in GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList)
            {
                if (card.card.cardName == "placeholder")
                {
                    // Card temp = deck.popCard();
                    // card.updateCard(temp);
                    GameManager.manager.StartCoroutine(GameManager.manager.DeckAnimation(card, GameManager.manager.players[GameManager.manager.myPlayerIndex]));
                    GameManager.manager.sendMessage("Added a card into your holster!");
                    break;
                }
            }
        }
    }

    // this will get messy quickly so actually comment things
    public int checkBonus(int damage, CardDisplay selectedCard)
    {
        // Pluot damage bonus
        if (isPluot)
        {
            if (pluotBonusType == selectedCard.card.cardQuality)
            {
                damage++;
            }


            if (selectedCard.card.cardQuality == "Hot")
            {
                pluotHot = true;
            }

            if (selectedCard.card.cardQuality == "Wet")
            {
                pluotWet = true;
            }

            if (selectedCard.card.cardQuality == "Cold")
            {
                pluotCold = true;
            }

            if (selectedCard.card.cardQuality == "Dry")
            {
                pluotDry = true;
            }

            // PLUOT FLIP LOGIC
            if (pluotHot && pluotWet && pluotCold && pluotDry)
            {
                character.canBeFlipped = true;
                GameManager.manager.sendSuccessMessage(13);
            }
        }

        // snakey motherfucker
        if (selectedCard.card.cardName == "A Swipe of Snake Oil")
        {

            // GameManager.manager.deckMenu.SetActive(true);
            // GameManager.manager.displayDeck();

            Debug.Log("Command check starting");
            if (GameManager.manager.Game.multiplayer)
            {
                foreach (GamePlayer gp in GameManager.manager.Game.GamePlayers)
                {
                    if (gp.playerName == GameManager.manager.currentPlayerName)
                    {
                        Debug.Log("Target RPC, DisplayDeckMenu");
                        gp.RpcDisplayDeckMenu();
                    }
                }

            }
            else
            {
                // not networked logic
                GameManager.manager.deckMenu.SetActive(true);
                GameManager.manager.displayDeck();
            }
            return damage;
        }

        // An Essence of Emotional Labor
        if(selectedCard.card.cardName == "Essence of Emotional Labor")
        {
            // if GameManager.manager.tempPlayer.gameObject.GetComponent<ComputerPlayer>() != null
            if (!Game.multiplayer)
            {
                Debug.Log("Computer logic");

                if (GameManager.manager.tempPlayer.deck.deckList.Count == 0)
                    return damage;

                // Trash a random card from their deck
                // make method that trashes a random card
                int cardNumber = rng.Next(0, GameManager.manager.tempPlayer.deck.deckList.Count);
                // GameManager.manager.selectedCardInt = cardNumber;

                GameObject obj = Instantiate(GameManager.manager.tempPlayer.deck.cardDisplay.gameObject,
                        GameManager.manager.tempPlayer.deck.cardDisplay.gameObject.transform.position,
                        GameManager.manager.tempPlayer.deck.cardDisplay.gameObject.transform.rotation,
                        GameManager.manager.tempPlayer.deck.cardDisplay.gameObject.transform);

                GameManager.manager.StartCoroutine(MoveToTrash(obj));

                GameManager.manager.td.addCard(GameManager.manager.tempPlayer.deck.deckList[cardNumber]);
                GameManager.manager.tempPlayer.deck.deckList.RemoveAt(cardNumber);
                GameManager.manager.tempPlayer.deck.updateCardSprite();
            }
            else
            {
                // add in networked command for opponent
            }
        }

        // A Buffet of Three Wishes
        if (selectedCard.card.cardName == "Buffet of Three Wishes")
        {
            // lose 2 hp
            subHealth(2);

            // gain 2P
            addPips(2);

            // You may put the top card of your deck into your holster.
            // make this into coroutine
            StartCoroutine(AddCardToHolster());
        }

        // A Last Drop of Coffee
        if (selectedCard.card.cardName == "Last Drop of Coffee")
        {
            Debug.Log("Hot coffee!");
            GameManager.manager.tempPlayer.hotCoffee = true;
        }

        // A Phial of Inheritance Powder
        if (selectedCard.card.cardName == "Phial of Inheritance Powder")
        {
            phialBonus = true;
        }

        // An Elephant's Round
        if (selectedCard.card.cardName == "Elephants Round")
        {
            Debug.Log("Adding starter cards from Elephant");
            // Every player creates a starter potion on the top of their deck.
            for (int i = 0; i < GameManager.manager.numPlayers; i++)
            {
                // cp.deck.putCardOnTop(starterPotionCard);
                int random = GameManager.manager.rng.Next(0, GameManager.manager.starterPotionCards.Count);
                GameManager.manager.players[i].deck.putCardOnTop(GameManager.manager.starterPotionCards[random]);
                // GameManager.manager.players[i].deck.putCardOnTop(GameManager.manager.starterPotionCard);
            }
        }

        // A River Dirt Cheese
        if (selectedCard.card.cardName == "River Dirt Cheese")
        {
            // Deals +3 damage to your opponent if it's their last essence cube
            if (GameManager.manager.tempPlayer.hpCubes == 1)
            {
                Debug.Log("River Cheese Bonus!  +3 damage!");
                return damage + 3;
            }    
        }

        if (selectedCard.card.cardName != "NorthernOquinox" && selectedCard.card.cardName != "PotionThatMakesHatsUglier" &&
                selectedCard.card.cardName != "SeriesOfPoisonousWords" && selectedCard.card.cardName != "VerySeriousThreat" &&
                selectedCard.card.cardName != "BottleOfLeastAmountOfSpiders" && selectedCard.card.cardName != "ContainerFilledWithAngryBees" &&
                selectedCard.card.cardName != "CupOfNoodles" && selectedCard.card.cardName != "PassiveAggressiveSlurry" &&
                selectedCard.card.cardName != "ClassicFireball" && selectedCard.card.cardName != "ElectronicTonic" &&
                selectedCard.card.cardName != "LossOfOnesIntimatePossesions" && selectedCard.card.cardName != "ShotOfWillOWisp" &&
                selectedCard.card.cardName != "BoldBundleOfLightning" && selectedCard.card.cardName != "CupfulOfRealm" &&
                selectedCard.card.cardName != "GoopGasAttack" && selectedCard.card.cardName != "ScreechingCry" &&
                selectedCard.card.cardName != "ATearofBlackRain" && selectedCard.card.cardName != "QuartOfLemonade" &&
                selectedCard.card.cardName != "VintageAromaticKate" && selectedCard.card.cardName != "JarFullOfGlitter" &&
                selectedCard.card.cardName != "KissFromTheLipsOfAnAncientLove" && selectedCard.card.cardName != "HumblingGlimpse")
        {
            foreach (CardPlayer cp in GameManager.manager.players)
            {
                if (cp.isIsadore)
                {
                    foreach (CardDisplay cd in cp.holster.cardList)
                    {
                        if (cd.card.cardName == "CherryBomb Badge")
                        {
                            // they should take 1 damage from CherryBomb Badge
                            subHealth(1);
                        }
                    }
                }
            }
        }

        // Opponent trashes 1 card in their Holster
        if (selectedCard.card.cardName == "ParticularlyFrighteningShadeofPurple" ||
        selectedCard.card.cardName == "PhilterOfMalaise" ||
        selectedCard.card.cardName == "MouthfulOfHair" ||
        selectedCard.card.cardName == "PowderOfLaughingFits" ||
        selectedCard.card.cardName == "A Jar of Mummy Finger Butter" ||
        selectedCard.card.cardName == "A Pile of Sweat from Several Humid Days" ||
        selectedCard.card.cardName == "A Loss of Dexterity" ||
        selectedCard.card.cardName == "A Severe Draught That Melts Only Brains")
        {
            /*
            if (GameManager.manager.Game.multiplayer)
            {
                Debug.Log("Command check starting");
                foreach (GamePlayer gp in GameManager.manager.Game.GamePlayers)
                {
                    if (gp.playerName == GameManager.manager.tempPlayer.name)
                    {
                        Debug.Log("Target RPC, Trash Deck Menu Active");
                        gp.RpcTrashOneCard(gp.playerName);
                    }
                }
                return damage;
            }
            */

            // if the attacked player is a computer, do the trashing for them
            if (GameManager.manager.tempPlayer.gameObject.GetComponent<ComputerPlayer>() != null)
            {
                Debug.Log("Computer logic!!!");
                foreach(CardDisplay cd in GameManager.manager.tempPlayer.holster.cardList)
                {
                    if(cd.card.name == "placeholder")
                    {
                        continue;
                    }
                    else
                    {
                        GameObject obj = Instantiate(cd.gameObject,
                        cd.gameObject.transform.position,
                        cd.gameObject.transform.rotation,
                        cd.gameObject.transform);

                        StartCoroutine(MoveToTrash(obj));
                        GameManager.manager.td.addCard(cd);
                        GameManager.manager.sendMessage("Opponent just trashed a card!");
                        return damage;
                    }
                }
            } else
            {
                // if the player is a human
                Debug.Log("No computer player?");
                GameManager.manager.opponentHolsterMenu.SetActive(true);
                GameManager.manager.displayOpponentHolster();
                return damage;
            }

            // IMPORTANT: use this as template for other implementations of ComputerPlayer logic!!!
            // if they're not a computer player
            if (GameManager.manager.tempPlayer.gameObject.GetComponent<ComputerPlayer>() == null)
            {
                Debug.Log("No computer player?");
                GameManager.manager.opponentHolsterMenu.SetActive(true);
                GameManager.manager.displayOpponentHolster();
            }
            else
            {
                // make computer choose random player maybe?
                // I'll figure it out, it's just calling a ComputerPlayer method
                Debug.Log("Computer player detected");

                // TODO: Add method that trashes one random card from ComputerPlayer's holster
            }

        }

        // Choose 1 card in an opponent's Holster and trash it
        if (selectedCard.card.cardName == "A Kind But Ultimately Thoughtless Gesture" ||
            selectedCard.card.cardName == "A Probably Dangerous Brew With A Hole In It" ||
            selectedCard.card.cardName == "PlasticwareContainerOfDadJokes" ||
            selectedCard.card.cardName == "A Totally in NO WAY Suspicious Clear Liquid")
        {
            // Command check starting
            if (GameManager.manager.Game.multiplayer)
            {
                foreach (GamePlayer gp in GameManager.manager.Game.GamePlayers)
                {
                    if (gp.playerName == GameManager.manager.currentPlayerName)
                    {
                        Debug.Log("Target RPC, TrashOpponentMenu Active");
                        gp.RpcTrashOneCard(GameManager.manager.tempPlayer.name);
                    }
                }
                return damage;
            }

            if(gameObject.GetComponent<ComputerPlayer>() != null)
            {
                Debug.Log("Computer logic!!!");

                // maybe do something for computer
                // int cardNumber = rng.Next(1, 5);
                // GameManager.manager.selectedCardInt = cardNumber;

                foreach (CardDisplay cd in GameManager.manager.tempPlayer.holster.cardList)
                {
                    if (cd.card.cardName != "placeholder")
                    {
                        GameObject obj = Instantiate(cd.gameObject,
                        cd.gameObject.transform.position,
                        cd.gameObject.transform.rotation,
                        cd.gameObject.transform);

                        GameManager.manager.StartCoroutine(MoveToTrash(obj));

                        GameManager.manager.td.addCard(GameManager.manager.tempPlayer.holster.cardList[GameManager.manager.selectedCardInt - 1]);

                        GameManager.manager.sendMessage("A card in your holster just got trashed!");

                        break;
                    }
                }

            } else
            {
                GameManager.manager.opponentHolsterMenu.SetActive(true);
                GameManager.manager.displayOpponentHolster();
            }
        }

        // Goldbricker
        // Replace a card in an opponents holster with a Starter card of the same type. Any loaded potions remain loaded
        if (selectedCard.card.cardName == "Goldbricker")
        {
            // make menu in UI with opponent's holster, we're gonna need this for multiple cards anyways

            // if they're not a computer player
            if (GameManager.manager.tempPlayer.gameObject.GetComponent<ComputerPlayer>() == null
                && gameObject.GetComponent<ComputerPlayer>() == null)
            {
                GameManager.manager.replaceStarter = true;
                GameManager.manager.opponentHolsterMenu.SetActive(true);
                GameManager.manager.displayOpponentHolster();
            }
            else
            {
                // maybe do something for computer
            }
        }


        // you may trash up to 2 market cards instead of doing damage
        // i'm gonna do some weird while loop to prevent it from returning damage until the player has selected an option
        if (selectedCard.card.cardName == "A Quizzical Look And a Rummage of Pockets" ||
            selectedCard.card.cardName == "An Example of What Not to Do" ||
            selectedCard.card.cardName == "A Confident Throw Into the Garbage")
        {
            //GameManager.manager.trashOrDamage = true;
            GameManager.manager.numTrashed = 2;
            GameManager.manager.trashorDamageMenu.SetActive(true);
            GameManager.manager.updateTrashMarketMenu();
            return Math.Max(damage - 2, 0);

            // don't do this, this created an infinite loop
            /*
            while (GameManager.manager.trashOrDamage)
            {
                if (!GameManager.manager.trashOrDamage)
                {
                    return 2;
                }

                if (GameManager.manager.trash)
                {
                    return 0;
                }
            }
            */
        }

        // You may create a starter potion on top of your deck
        if (selectedCard.card.cardName == "AMaliciousThought" ||
            selectedCard.card.cardName == "IntenseThirstForKnowledge" ||
            selectedCard.card.cardName == "VioletFireling" ||
            selectedCard.card.cardName == "InnocentLayerCake")
        {
            // Command check starting
            if (GameManager.manager.Game.multiplayer)
            {
                foreach (GamePlayer gp in GameManager.manager.Game.GamePlayers)
                {
                    if (gp.playerName == GameManager.manager.currentPlayerName)
                    {
                        Debug.Log("Target RPC, StarterPotionMenu Active");
                        gp.RpcStarterPotionMenu(gp.playerName);
                    }
                }
                return damage;
            }
            // if they're not a computer player
            // GameManager.manager.tempPlayer.gameObject.GetComponent<ComputerPlayer>() == null
            if (gameObject.GetComponent<ComputerPlayer>() == null
                && GameManager.manager.myPlayerIndex == 0)
            {
                GameManager.manager.starterPotionMenu.SetActive(true);
                // StartCoroutine(changeAlpha(GameManager.manager.starterPotionMenu));
            }
        }

        // you may trash 1 card in the market
        // add all the cards that have this text here
        if (selectedCard.card.cardName == "EssenceOfDifficultManualLabor" ||
            selectedCard.card.cardName == "LiquidPileOfHorrid" ||
            selectedCard.card.cardName == "Chocolatiers Delicate Pride" ||
            selectedCard.card.cardName == "ANaggingFeeling")
        {
            // if they're not a computer player
            if (gameObject.GetComponent<ComputerPlayer>() == null)
            {
                // GameManager method
                Debug.Log("Trash One from the Market Bonus");
                GameManager.manager.numTrashed = 1;
                GameManager.manager.trashMarketUI.SetActive(true);
                // StartCoroutine(changeAlpha(GameManager.manager.trashMarketUI));
                GameManager.manager.updateTrashMarketMenu();
            }
            else
            {
                // do something else lol
                // pick a random number and then trash a market card
                Debug.Log("Implement action for the computer player!");
            }
        }

        // CARDS WITH THROW BONUSES GO HERE!

        // Throw Bonus: Heal +3 HP
        if (selectedCard.card.cardName == "QuartOfLemonade" ||
            selectedCard.card.cardName == "ThimbleOfHoney" ||
            selectedCard.card.cardName == "KissFromTheLipsOfAnAncientLove" ||
            selectedCard.card.cardName == "TallDrinkOfWater")
        {
            if (healBonus)
            {
                addHealth(6);
                healBonus = false;
                return damage;
            }
            addHealth(3);
        }

        // Throw Bonus: +1 Pip
        if (selectedCard.card.cardName == "VintageAromaticKate" ||
            selectedCard.card.cardName == "JarFullOfGlitter" ||
            selectedCard.card.cardName == "BowlOfExtremelyHotSoup" ||
            selectedCard.card.cardName == "HumblingGlimpse")
        {
            addPips(1);
        }

        /*
        // ring damage bonus
        if (ringBonus && potionsThrown == 0)
        {
            damage++;
        }
        */

        // Throw Bonus: If your Holster is empty, place the top 4 cards of the Potion Market Deck into your Holster
        if (selectedCard.card.cardName == "ATearofBlackRain")
        {
            if (healBonus)
            {
                addHealth(4);
                healBonus = false;
            }
            else
            {
                addHealth(2);
            }
            Debug.Log("Tear of Black Rain Bonus");

            foreach (CardDisplay cd in holster.cardList)
            {
                if (cd.card.cardName != "ATearofBlackRain")
                {
                    if (cd.card.cardName != "placeholder")
                    {
                        return damage;
                    }
                }
            }
            //GameManager.manager.put4CardsInHolster();
            blackRainBonus = true;
        }

        return damage;
    }

    /*
    void Awake() {
        GameObject go = new GameObject("HealthController");
        health = go.AddComponent<HealthController>();
    }

    public Player()
    {
        dead = false;
    }

    public Player(int HPCubes)
    {
        hp = 10;
        hpCubes = HPCubes;
        dead = false;
    }
    */

    // TODO: Check to see if opponent thrown card is Artifact.
    // TODO: Prompt targetedPlayer to trash loaded potion in Artifact with defense bonus.
    public int checkDefensiveBonus(int damage, int selectedCardInt)
    {
        // Artifact: BubbleWand = May trash 1 loaded potion to prevent 2 damage.
        // Artifact: ShieldOfMouthOfTruth = May trash 1 loaded potion to prevent 3 damage.
        // Ring: CrustyRingOfCryingRustyTears = Opponent's ARTIFACTS prevent 2 damage.
        // Ring: FoggyRingOfNearsightedOldCrone = All items thrown at you prevent 1 damage.
        // Ring: ThickRingOfFurrowedBrowDolt = During each opponent turn, prevent 2 damage to your HP.

        // TAKE INTO ACCOUNT -> Ring: RingOfRings = Doubles all of your Ring Effects. (Check boolean doubleRingBonus == true)
        int preventedDamage = 0;
        int cardInt = 0;
        foreach (CardDisplay cd in holster.cardList)
        {
            cardInt++;
            if (cd.card.cardType == "Artifact")
            {
                // BubbleWand
                if (cd.card.cardName == "BubbleWand")
                {
                    // if there is a loaded potion
                    if (cd.aPotion.card != cd.placeholder)
                    {
                        // May trash 1 loaded potion to prevent 2 damage.
                        if (GameManager.manager.Game.multiplayer)
                        {
                            foreach (GamePlayer gp in GameManager.manager.Game.GamePlayers)
                            {
                                if (gp.playerName == GameManager.manager.tempPlayer.name)
                                {
                                    Debug.Log("Target RPC, Trash Potion Menu Active");
                                    gp.RpcBubbleWandMenu(gp.playerName, cardInt, damage);
                                }
                            }
                        }   else
                        {
                            Debug.Log("Local command for Bubble Wand");
                            if(gameObject.GetComponent<ComputerPlayer>() == null)
                            {
                                GameManager.manager.bubbleWandMenu.SetActive(true);
                                // GameManager.manager.bubbleWandMenu.GetComponent<CanvasGroup>().alpha = 0;
                            }
                            // StartCoroutine(changeAlpha(GameManager.manager.bubbleWandMenu));
                        }
                    }
                }
                // Shield of the Mouth of Truth
                else if (cd.card.cardName == "Shield of the Mouth of Truth")
                {
                    // if there is a loaded potion
                    if (cd.aPotion.card != cd.placeholder)
                    {
                        // May trash 1 loaded potion to prevent 3 damage.
                        if (GameManager.manager.Game.multiplayer)
                        {
                            foreach (GamePlayer gp in GameManager.manager.Game.GamePlayers)
                            {
                                if (gp.playerName == GameManager.manager.tempPlayer.name)
                                {
                                    Debug.Log("Target RPC, Trash Potion Menu Active");
                                    gp.RpcShieldMenu(gp.playerName, cardInt, damage);
                                }
                            }
                        }
                    }
                }
            }

            else if (cd.card.cardType == "Ring")
            {
                // Crusty Ring of the Crying Rusty Tears
                if (cd.card.cardName == "Crusty Ring of the Crying Rusty Tears")
                {
                    // Opponent's ARTIFACTS prevent 2 damage.
                    // (prevents 4 damage with Ring of Rings in Holster)
                    // checks throwers hand for the artifact, to my knowledge the card is not mutated yet
                    foreach (CardPlayer cd2 in GameManager.manager.players)
                    {
                        if (cd2.name == GameManager.manager.currentPlayerName)
                        {
                            if (cd2.holster.cardList[selectedCardInt - 1].card.cardType == "Artifact")
                            {
                                preventedDamage += doubleRingBonus ? 4 : 2;
                                foreach (CardPlayer cp in GameManager.manager.players)
                                {
                                    if (cp.isIsadore)
                                    {
                                        foreach (CardDisplay cd3 in cp.holster.cardList)
                                        {
                                            if (cd3.card.cardName == "CherryBomb Badge")
                                            {
                                                preventedDamage -= 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                // Foggy Ring of the Nearsighted Old Crone
                else if (cd.card.cardName == "Foggy Ring of the Nearsighted Old Crone")
                {
                    // All items thrown at you prevent 1 damage.
                    // (prevents 2 damage with Ring of Rings in Holster)
                    preventedDamage += doubleRingBonus ? 2 : 1;
                    foreach (CardPlayer cp in GameManager.manager.players)
                    {
                        if (cp.isIsadore)
                        {
                            foreach (CardDisplay cd2 in cp.holster.cardList)
                            {
                                if (cd2.card.cardName == "CherryBomb Badge")
                                {
                                    preventedDamage -= 1;
                                }
                            }
                        }
                    }
                }
                // Thick Ring of the Furrowed Brow Dolt
                else if (cd.card.cardName == "Thick Ring of the Furrowed Brow Dolt")
                {
                    // During each opponent turn, prevent 2 damage to your HP.
                    // (prevents 4 damage with Ring of Rings in Holster)
                    if (!opponentPreventedDamage)
                    {
                        preventedDamage += doubleRingBonus ? 4 : 2;
                        opponentPreventedDamage = true;
                        foreach (CardPlayer cp in GameManager.manager.players)
                        {
                            if (cp.isIsadore)
                            {
                                foreach (CardDisplay cd2 in cp.holster.cardList)
                                {
                                    if (cd2.card.cardName == "CherryBomb Badge")
                                    {
                                        preventedDamage -= 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Calculates new damage amount to be taken by targeted player.
        // If damage taken is negative (below 0), return 0 damage taken.
        int newDamage = damage - preventedDamage;
        return newDamage >= 0 ? newDamage : 0;
    }

    public void addHealth(int health)
    {
        hp += health;

        //Make sure that hp cannot go above 10
        if (hp > 10)
        {
            hp = 10;
        }

        if (healSign != null && healAmount != null)
        {
            healAmount.GetComponent<TMPro.TextMeshProUGUI>().text = health.ToString();
            healSign.SetActive(true);
            healAmount.SetActive(true);
            StartCoroutine(waitThreeSeconds(healSign));
            StartCoroutine(waitThreeSeconds(healAmount));
        }

        updateHealthUI();
    }

    public void subHealth(int damage, string cardQuality = "")
    {
        // If they're dead don't damage them further
        if (dead)
        {
            GameManager.manager.checkForEndGame();
            return;
        }

        /*
         * call a function instead of this
        animator.Play("BoloHit");
        Invoke("playIdle", animator.GetCurrentAnimatorStateInfo(0).length);
        */
        if (animator != null)
            playHit();
        hp -= damage;

        //Make sure that hp doesn't go below 0
        //If hp goes below 0, set it to 10 and subtract a health cube
        if (hp <= 0)
        {
            // check for phial
            if (GameManager.manager.players[GameManager.manager.myPlayerIndex].phialBonus)
            {
                Debug.Log("Phial Bonus!");
                GameManager.manager.players[GameManager.manager.myPlayerIndex].addPips(3);
                GameManager.manager.players[GameManager.manager.myPlayerIndex].phialBonus = false;
            }

            cubed = true;
            hp = 0;
            if (Game.tutorial)
            {
                GameManager.manager.dialog.endTutorialDialog();
            }

            // took out && Game.storyMode out of this, I believe it should be fine
            if(hpCubes == 1)
            {
                // make sure this doesn't fuck with the echo shard
                for (int i = 0; i < deck.deckList.Count; i++)
                {
                    if (deck.deckList[i].cardName == "Echo Shard")
                    {
                        Debug.Log("HP changed to 7 because of Echo Shard!");
                        hpCubes = 1;
                        hp = 7;
                        cubed = false;
                        updateHealthUI();

                        /*
                        GameObject obj = Instantiate(deck.gameObject,
                        deck.gameObject.transform.position,
                        deck.gameObject.transform.rotation,
                        deck.gameObject.transform);
                        */

                        GameManager.manager.td.addCard(deck.deckList[i]);
                        deck.deckList.RemoveAt(i);
                        deck.updateCardSprite();
                        GameManager.manager.sendMessage("The Echo Shard has been trashed!");
                        return;
                    }
                }

                // safety check lol
                for (int i = 0; i < holster.cardList.Count; i++)
                {
                    if (holster.cardList[i].card.cardName == "Echo Shard")
                    {
                        Debug.Log("HP changed to 7 because of Echo Shard!");
                        hpCubes = 1;
                        hp = 7;
                        cubed = false;
                        updateHealthUI();

                        GameManager.manager.td.addCard(holster.cardList[i]);
                        GameManager.manager.sendMessage("The Echo Shard has been trashed!");
                        return;
                    }
                }
                hpCubes = 0;
                updateHealthUI();
                fadeOut();
                dead = true;
                if (GameManager.manager.players[GameManager.manager.myPlayerIndex] == this)
                {
                    GameManager.manager.Invoke("endTurn", 1f);
                }
                GameManager.manager.checkForEndGame();
            }
            // GameManager.manager.checkForEndGame();
        }

        Debug.Log("Subtracted " + damage + "from " + charName);
        Debug.Log(charName + "'s health = " + hp + " HP");
        /*
        if (cubed)
        {
            hpCubes--;
            if (hpCubes == 0)
            {
                // check for Phlactery Sweetbitter situation but otherwise everyone else is dead

                dead = true;
                switch (charName)
                {
                    case "CrowPunk":
                        Debug.Log("CrowPunk Hit animation");
                        animator.Play("CrowHit");
                        break;
                    case "Fingas":
                        Debug.Log("Fingas Hit animation");
                        animator.Play("Fingas_hit");
                        break;
                }
                Debug.Log("Checking endgame situation");
                GameManager.manager.checkForEndGame();
            }
            else
            {
                // animate the cube being awarded to player and refill their health bar

                hp = 10;
            }

            cubed = false;
        }
        */
        StartCoroutine(barAnimation());
        updateHealthUI();

        if (damageSign != null && damageAmount != null)
        {
            damageAmount.GetComponent<TMPro.TextMeshProUGUI>().text = "-" + damage.ToString();
            // damageSign.SetActive(true);
            // damageAmount.SetActive(true);
            // GameObject damageSignCopy = Instantiate(damageSign, damageSign.transform.position, damageSign.transform.rotation, damageSign.transform.parent);
            GameObject damageAmountCopy = Instantiate(damageAmount, damageAmount.transform.position, damageAmount.transform.rotation, damageAmount.transform.parent);
            // damageAmountCopy.transform.SetParent(damageSignCopy.transform);
            // damageSign.SetActive(false);
            // damageAmount.SetActive(false);
            damageAmountCopy.SetActive(true);
            StartCoroutine(healthAnimation(damageAmountCopy));
            // StartCoroutine(healthAnimation(damageAmountCopy));
            /*
            damageSign.SetActive(true);
            damageAmount.SetActive(true);
            StartCoroutine(waitThreeSeconds(damageSign));
            StartCoroutine(waitThreeSeconds(damageAmount));
            */
        }
    }

    public IEnumerator barAnimation()
    {
        bar.transform.DOShakePosition(.2f, 20f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.2f);
        bar.transform.DOShakePosition(.2f, 20f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.2f);
        bar.transform.DOShakePosition(.2f, 20f).SetEase(Ease.Linear);
    }

    public IEnumerator healthAnimation(GameObject obj)
    {
        Vector3 vec = new Vector3(0, 60f * GameManager.manager.heightRatio, 0);
        obj.transform.DOScale(new Vector3(4f, 4f, 4f), 0.6f).SetEase(Ease.Linear);
        obj.transform.DOMove(obj.transform.position + vec, 1f).SetEase(Ease.Linear);
        // yield return new WaitForSeconds(0.25f);
        // obj.transform.DOScale(new Vector3(3f, 3f, 3f), 0.8f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.9f);
        // obj.GetComponent<Image>().CrossFadeAlpha(0, 1f, false);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0.9f);
        yield return new WaitForSeconds(0.03f);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0.8f);
        yield return new WaitForSeconds(0.03f);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0.7f);
        yield return new WaitForSeconds(0.03f);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0.6f);
        yield return new WaitForSeconds(0.03f);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0.5f);
        yield return new WaitForSeconds(0.03f);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0.4f);
        yield return new WaitForSeconds(0.03f);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0.3f);
        yield return new WaitForSeconds(0.03f);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0.2f);
        yield return new WaitForSeconds(0.03f);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0.1f);
        yield return new WaitForSeconds(0.03f);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0);
        Destroy(obj);
    }

    public IEnumerator pipAnimation(GameObject obj)
    {
        Vector3 vec = new Vector3(0, 30f * GameManager.manager.heightRatio, 0);
        // obj.transform.DOScale(new Vector3(4f, 4f, 4f), 0.6f).SetEase(Ease.Linear);
        // obj.transform.DOMove(obj.transform.position + vec, 1f).SetEase(Ease.Linear);
        // yield return new WaitForSeconds(0.25f);
        // obj.transform.DOScale(new Vector3(3f, 3f, 3f), 0.8f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.9f);
        // obj.GetComponent<Image>().CrossFadeAlpha(0, 1f, false);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0.9f);
        yield return new WaitForSeconds(0.03f);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0.8f);
        yield return new WaitForSeconds(0.03f);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0.7f);
        yield return new WaitForSeconds(0.03f);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0.6f);
        yield return new WaitForSeconds(0.03f);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0.5f);
        yield return new WaitForSeconds(0.03f);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0.4f);
        yield return new WaitForSeconds(0.03f);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0.3f);
        yield return new WaitForSeconds(0.03f);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0.2f);
        yield return new WaitForSeconds(0.03f);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0.1f);
        yield return new WaitForSeconds(0.03f);
        obj.GetComponent<TMPro.TextMeshProUGUI>().canvasRenderer.SetAlpha(0);
        Destroy(obj);
    }

    public void addPips(int morePips)
    {
        pips += morePips;
        
        if (pipsSign != null)
        {
            if (morePips > 0)
            {
                pipsSign.SetActive(true);

                pipsSign.GetComponent<TMPro.TextMeshProUGUI>().text = "+ " + morePips.ToString();

                GameObject obj = Instantiate(pipsSign,
                pipsSign.transform.position,
                pipsSign.transform.rotation,
                this.transform);

                // add coroutine

                StartCoroutine(pipAnimation(obj));

                pipsSign.SetActive(false);
            }

            /*
            healAmount.GetComponent<TMPro.TextMeshProUGUI>().text = "+ " + morePips.ToString();
            healSign.SetActive(true);
            healAmount.SetActive(true);
            StartCoroutine(waitThreeSeconds(healSign));
            StartCoroutine(waitThreeSeconds(healAmount));
            */
        }
        GameManager.manager.checkMarketPrice();
        updatePipsUI();
    }

    public void subPips(int lessPips)
    {
        pips -= lessPips;
        pipsUsedThisTurn += lessPips;
        updatePipsUI();
        GameManager.manager.checkMarketPrice();

        // NICKLES FLIP LOGIC
        foreach (CardPlayer cp in GameManager.manager.players)
        {
            if (cp.isNickles && cp.pipsUsedThisTurn >= 10)
            {
                cp.character.canBeFlipped = true;
                GameManager.manager.sendSuccessMessage(13);
            }
        }
        //character.canBeFlipped = true;
        //GameManager.manager.sendSuccessMessage(13);
    }

    //function to call when checking on if the player is dead
    public bool checkDead()
    {
        return dead;
    }

    IEnumerator waitThreeSeconds(GameObject gameObj)
    {
        yield return new WaitForSeconds(3);
        gameObj.SetActive(false);
    }

    /***************************
     CHECK ARTIFACT BONUS
 ***************************/

    public void checkArtifactBonusAnimation(CardDisplay selectedCard)
    {
        // probably depends on what card it is and what bonus it has, might have to implement logic for certain pools of cards this way

        // The Skateboard
        if (selectedCard.card.cardName == "Skateboard")
        {
            // if a loaded potion has an additional effect, do a trick instead of damage
            // ask matteo and figure out exactly what that means lol

            // if the loaded potion does not have any card text
            // i'm just gonna list the cards because i'm lazy
            // this might not actually be accurate to how the card should work but i'm taking a shortcut
            if (selectedCard.aPotion.card.cardName != "NorthernOquinox" && selectedCard.aPotion.card.cardName != "PotionThatMakesHatsUglier" &&
                selectedCard.aPotion.card.cardName != "SeriesOfPoisonousWords" && selectedCard.aPotion.card.cardName != "VerySeriousThreat" &&
                selectedCard.aPotion.card.cardName != "BottleOfLeastAmountOfSpiders" && selectedCard.aPotion.card.cardName != "ContainerFilledWithAngryBees" &&
                selectedCard.aPotion.card.cardName != "CupOfNoodles" && selectedCard.aPotion.card.cardName != "PassiveAggressiveSlurry" &&
                selectedCard.aPotion.card.cardName != "ClassicFireball" && selectedCard.aPotion.card.cardName != "ElectronicTonic" &&
                selectedCard.aPotion.card.cardName != "LossOfOnesIntimatePossesions" && selectedCard.aPotion.card.cardName != "ShotOfWillOWisp" &&
                selectedCard.aPotion.card.cardName != "BoldBundleOfLightning" && selectedCard.aPotion.card.cardName != "CupfulOfRealm" &&
                selectedCard.aPotion.card.cardName != "GoopGasAttack" && selectedCard.aPotion.card.cardName != "ScreechingCry" &&
                selectedCard.aPotion.card.cardName != "ATearofBlackRain" && selectedCard.aPotion.card.cardName != "QuartOfLemonade" &&
                selectedCard.aPotion.card.cardName != "VintageAromaticKate" && selectedCard.aPotion.card.cardName != "JarFullOfGlitter" &&
                selectedCard.aPotion.card.cardName != "KissFromTheLipsOfAnAncientLove" && selectedCard.aPotion.card.cardName != "HumblingGlimpse")
            {
                // do animation
                selectedCard.colorCardHot();
                selectedCard.aPotion.colorCardHot();
            }
        }

        // Hammer of Engagement
        if (selectedCard.card.cardName == "HammerOfEnagagment")
        {
            /*
             * Hot Bonus: +2 Damage.
               Holster Bonus: +1 Damage if a ring is in your Holster.
             */
            if (selectedCard.aPotion.card.cardQuality == "Hot")
            {
                selectedCard.colorCardHot();
                selectedCard.aPotion.colorCardHot();
            }
        }

        // Paperweight of Bauble Collector
        // very similar to hammer of engagement
        if (selectedCard.card.cardName == "PaperweightOfTheBaubleCollector")
        {
            /*
             * Hot Bonus: +1 Damage.
               Holster Bonus: +1 Damage for each Ring in your Holster.
             */
            if (selectedCard.aPotion.card.cardQuality == "Hot")
            {
                selectedCard.colorCardHot();
                selectedCard.aPotion.colorCardHot();
            }
        }

        // Gauntlet Mounted Trebuchet
        // this card text is definitely bad lol sorry i don't make the rules
        if (selectedCard.card.cardName == "GauntletMountedTrebuchet")
        {
            // Dry Bonus: Double potion damage
            // what they meant was "double the loaded potion's damage and add it to the artifact damage"
            if (selectedCard.aPotion.card.cardQuality == "Dry")
            {
                selectedCard.colorCardDry();
                selectedCard.aPotion.colorCardDry();
            }
        }

        // Tablet Containing All Knowledge
        if (selectedCard.card.cardName == "Tablet Containing All Knowledge")
        {
            // Dry Bonus: +2 Damage
            if (selectedCard.aPotion.card.cardQuality == "Dry")
            {
                selectedCard.colorCardDry();
                selectedCard.aPotion.colorCardDry();
            }
            if (selectedCard.aPotion.card.cardQuality == "Hot")
            {
                selectedCard.colorCardHot();
                selectedCard.aPotion.colorCardHot();
            }
            if (selectedCard.aPotion.card.cardQuality == "Wet")
            {
                selectedCard.colorCardWet();
                selectedCard.aPotion.colorCardWet();
            }
            if (selectedCard.aPotion.card.cardQuality == "Cold")
            {
                selectedCard.colorCardCold();
                selectedCard.aPotion.colorCardCold();
            }

        }

        // The Daggerheels
        if (selectedCard.card.cardName == "Daggerheels")
        {
            // Cold Bonus: +1 Damage
            if (selectedCard.aPotion.card.cardQuality == "Cold")
            {
                selectedCard.colorCardCold();
                selectedCard.aPotion.colorCardCold();
            }
        }

        // The Rapid Fire Caltrop Hand Cannon
        if (selectedCard.card.cardName == "RapidFireCaltrop")
        {
            // Dry Bonus: +2 Damage
            if (selectedCard.aPotion.card.cardQuality == "Dry")
            {
                selectedCard.colorCardDry();
                selectedCard.aPotion.colorCardDry();
            }
        }

        // Pewter Heart Necklace
        if (selectedCard.card.cardName == "PewterHeartNecklace")
        {
            // Wet Bonus: +2 HP
            if (selectedCard.aPotion.card.cardQuality == "Cold")
            {
                selectedCard.colorCardCold();
                selectedCard.aPotion.colorCardCold();
            }
            // this card doesn't damage anyone
            return;
        }

        // Wooden Dryad's Kiss
        if (selectedCard.card.cardName == "WoodenDryadsKiss")
        {
            if (selectedCard.aPotion.card.cardQuality == "Wet")
            {
                selectedCard.colorCardWet();
                selectedCard.aPotion.colorCardWet();
            }
            // this card doesn't damage anyone
            return;
        }

        // The Bottle Rocket
        if (selectedCard.card.cardName == "BottleRocket")
        {
            // Wet Bonus: +2 Damage
            // 3 P: Double the damage of this artifact this turn. You may only do this once per turn.
            if (selectedCard.aPotion.card.cardQuality == "Wet")
            {
                selectedCard.colorCardWet();
                selectedCard.aPotion.colorCardWet();
            }
        }

        // return;
    }

    /*************************
    CHECK VESSEL BONUS
*************************/

    public void checkVesselBonusAnimation(CardDisplay selectedCard)
    {

        // Squeezebox
        if (selectedCard.card.cardName == "Squeezebox")
        {
            // maybe add a check for any cards that have healing effects
            if (selectedCard.vPotion1.card.cardName == "QuartOfLemonade" ||
            selectedCard.vPotion1.card.cardName == "ThimbleOfHoney" ||
            selectedCard.vPotion1.card.cardName == "KissFromTheLipsOfAnAncientLove" ||
            selectedCard.vPotion1.card.cardName == "TallDrinkOfWater" ||
            selectedCard.vPotion1.card.cardName == "ATearofBlackRain")
            {
                selectedCard.colorCardCold();
                selectedCard.vPotion1.colorCardCold();
            }

            if (selectedCard.vPotion2.card.cardName == "QuartOfLemonade" ||
            selectedCard.vPotion2.card.cardName == "ThimbleOfHoney" ||
            selectedCard.vPotion2.card.cardName == "KissFromTheLipsOfAnAncientLove" ||
            selectedCard.vPotion2.card.cardName == "TallDrinkOfWater" ||
            selectedCard.vPotion1.card.cardName == "ATearofBlackRain")
            {
                selectedCard.colorCardCold();
                selectedCard.vPotion2.colorCardCold();
            }
        }

        // Drinking Horn of a Sea Unicorn's Tooth
        if (selectedCard.card.cardName == "Drinking Horn of a Sea Unicorn's Tooth")
        {
            if (selectedCard.vPotion1.card.cardQuality == "Cold")
            {
                selectedCard.colorCardCold();
                selectedCard.vPotion1.colorCardCold();
            }

            if (selectedCard.vPotion2.card.cardQuality == "Cold")
            {
                selectedCard.colorCardCold();
                selectedCard.vPotion2.colorCardCold();
            }

            if (selectedCard.vPotion1.card.cardQuality == "Wet")
            {
                selectedCard.colorCardWet();
                selectedCard.vPotion1.colorCardWet();
            }

            if (selectedCard.vPotion2.card.cardQuality == "Wet")
            {
                selectedCard.colorCardWet();
                selectedCard.vPotion2.colorCardWet();
            }
        }

        // Vessel Bonus: +2 Damage
        if (selectedCard.vPotion1.card.cardName == "RefectionOfOnesGnarledEmotionalSelf"  ||
            selectedCard.vPotion1.card.cardName == "SoupMadeOfGunpowder"  ||
            selectedCard.vPotion1.card.cardName == "PourOfReallyAngryAcid")
        {
            // check each loaded potion individually and apply the bonus to the one with the vessel bonus
            selectedCard.colorCardHot();
            selectedCard.vPotion1.colorCardHot();
        }

        if (selectedCard.vPotion2.card.cardName == "RefectionOfOnesGnarledEmotionalSelf" ||
            selectedCard.vPotion2.card.cardName == "SoupMadeOfGunpowder" ||
            selectedCard.vPotion2.card.cardName == "PourOfReallyAngryAcid")
        {
            // check each loaded potion individually and apply the bonus to the one with the vessel bonus
            selectedCard.colorCardHot();
            selectedCard.vPotion2.colorCardHot();
        }

        // Hot + Wet Bonus
        // It's only two cards
        // three cards with DLC lol
        if ((selectedCard.vPotion1.card.cardQuality == "Hot" && selectedCard.vPotion2.card.cardQuality == "Wet") ||
           (selectedCard.vPotion2.card.cardQuality == "Hot" && selectedCard.vPotion1.card.cardQuality == "Wet"))
        {
            if (selectedCard.card.cardName == "Empty Ravioli" ||
                selectedCard.card.cardName == "Furry Flagon fromthe Hide of Hairy Leeches" ||
                selectedCard.card.cardName == "VoluptuousGallipot")
            {
                // addHealth(4);
                if (selectedCard.vPotion1.card.cardQuality == "Hot")
                {
                    selectedCard.colorCardHot();
                    selectedCard.vPotion1.colorCardHot();
                }

                if (selectedCard.vPotion2.card.cardQuality == "Hot")
                {
                    selectedCard.colorCardHot();
                    selectedCard.vPotion2.colorCardHot();
                }

                if (selectedCard.vPotion1.card.cardQuality == "Wet")
                {
                    selectedCard.colorCardWet();
                    selectedCard.vPotion1.colorCardWet();
                }

                if (selectedCard.vPotion2.card.cardQuality == "Wet")
                {
                    selectedCard.colorCardWet();
                    selectedCard.vPotion2.colorCardWet();
                }
            }

            /*
            // Furry Flagon Made from the Hide of Hairy Leeches
            // Hot + Wet Bonus: Put 1 card from the trash to the top of your deck
            if (selectedCard.card.cardName == "Furry Flagon fromthe Hide of Hairy Leeches")
            {
                
            }

            // Voluptuous Gallipot of Double Entendre
            // come back to this
            if (selectedCard.card.cardName == "VoluptuousGallipot")
            {

            }
            */
        }

        // Cursed Cucumella of Clumsy Acidic Coffee
        // Hot Bonus: Opponent trashes 1 card. Wet Bonus: Opponent trashes 1 card.
        if (selectedCard.card.cardName == "CursedCucumella")
        {
            if (selectedCard.vPotion1.card.cardQuality == "Hot")
            {
                selectedCard.colorCardHot();
                selectedCard.vPotion1.colorCardHot();
            }

            if (selectedCard.vPotion2.card.cardQuality == "Hot")
            {
                selectedCard.colorCardHot();
                selectedCard.vPotion2.colorCardHot();
            }

            if (selectedCard.vPotion1.card.cardQuality == "Wet")
            {
                selectedCard.colorCardWet();
                selectedCard.vPotion1.colorCardWet();
            }

            if (selectedCard.vPotion2.card.cardQuality == "Wet")
            {
                selectedCard.colorCardWet();
                selectedCard.vPotion2.colorCardWet();
            }
        }

        // Hot + Dry Bonus
        // Filthy Urn Holding the Ashes of 60k
        // Hot + Dry Bonus: +4 Damage
        if (selectedCard.card.cardName == "FilthyUrn" ||
            selectedCard.card.cardName == "DualRhyton")
        {
            // damage += 4;
            if (selectedCard.vPotion1.card.cardQuality == "Hot" && selectedCard.vPotion2.card.cardQuality == "Dry")
            {
                selectedCard.colorCardHot();
                selectedCard.vPotion1.colorCardHot();
                selectedCard.vPotion2.colorCardDry();
            }

            if (selectedCard.vPotion1.card.cardQuality == "Dry" && selectedCard.vPotion2.card.cardQuality == "Hot")
            {
                selectedCard.colorCardDry();
                selectedCard.vPotion1.colorCardDry();
                selectedCard.vPotion2.colorCardHot();
            }
        }


        // Cold + Wet Bonus
        // Philty Phlegmbic Alembic of Philters Polemic
        //  1 opponent trashes all cards in their holster, or all opponents trash 1 card 
        if (selectedCard.card.cardName == "PhiltyPhlegmbicAlembic")
        {
            if (selectedCard.vPotion1.card.cardQuality == "Cold" && selectedCard.vPotion2.card.cardQuality == "Wet")
            {
                selectedCard.colorCardCold();
                selectedCard.vPotion1.colorCardCold();
                selectedCard.vPotion2.colorCardWet();
            }

            if (selectedCard.vPotion1.card.cardQuality == "Wet" && selectedCard.vPotion2.card.cardQuality == "Cold")
            {
                selectedCard.colorCardWet();
                selectedCard.vPotion1.colorCardWet();
                selectedCard.vPotion2.colorCardCold();
            }
        }
        

        // Cold + Dry Bonus
        if ((selectedCard.vPotion1.card.cardQuality == "Cold" && selectedCard.vPotion2.card.cardQuality == "Dry") ||
            (selectedCard.vPotion2.card.cardQuality == "Cold" && selectedCard.vPotion1.card.cardQuality == "Dry"))
        {
            Debug.Log("Cold and Dry Bonus!");

            if (selectedCard.card.cardName == "Empty Ravioli")
            {
                // return damage + 4;
                if (selectedCard.vPotion1.card.cardQuality == "Cold" && selectedCard.vPotion2.card.cardQuality == "Dry")
                {
                    selectedCard.colorCardCold();
                    selectedCard.vPotion1.colorCardCold();
                    selectedCard.vPotion2.colorCardDry();
                }

                if (selectedCard.vPotion1.card.cardQuality == "Dry" && selectedCard.vPotion2.card.cardQuality == "Cold")
                {
                    selectedCard.colorCardDry();
                    selectedCard.vPotion1.colorCardDry();
                    selectedCard.vPotion2.colorCardCold();
                }
            }

            // The Boxing Flask of the Fist Wizard
            // Deal +3 damage to all other opponents
            if (selectedCard.card.cardName == "Boxing Flask of the Fist Wizard")
            {
                if (selectedCard.vPotion1.card.cardQuality == "Cold" && selectedCard.vPotion2.card.cardQuality == "Dry")
                {
                    selectedCard.colorCardCold();
                    selectedCard.vPotion1.colorCardCold();
                    selectedCard.vPotion2.colorCardDry();
                }

                if (selectedCard.vPotion1.card.cardQuality == "Dry" && selectedCard.vPotion2.card.cardQuality == "Cold")
                {
                    selectedCard.colorCardDry();
                    selectedCard.vPotion1.colorCardDry();
                    selectedCard.vPotion2.colorCardCold();
                }
            }

            // The Vinyl Demijohn of Tunes and Libation
            // Put a market card on the top of your deck
            if (selectedCard.card.cardName == "VinylDemijohnofTunesandLibation")
            {
                if (selectedCard.vPotion1.card.cardQuality == "Cold" && selectedCard.vPotion2.card.cardQuality == "Dry")
                {
                    selectedCard.colorCardCold();
                    selectedCard.vPotion1.colorCardCold();
                    selectedCard.vPotion2.colorCardDry();
                }

                if (selectedCard.vPotion1.card.cardQuality == "Dry" && selectedCard.vPotion2.card.cardQuality == "Cold")
                {
                    selectedCard.colorCardDry();
                    selectedCard.vPotion1.colorCardDry();
                    selectedCard.vPotion2.colorCardCold();
                }
            }
        }
        // Synergy of Opposing Minds
        if (selectedCard.card.cardName == "Synergy of Opposing Minds")
        {
            if (selectedCard.vPotion1.card.cardQuality == "Cold" && selectedCard.vPotion2.card.cardQuality == "Hot")
            {
                selectedCard.colorCardCold();
                selectedCard.vPotion1.colorCardCold();
                selectedCard.vPotion2.colorCardHot();
            }

            if (selectedCard.vPotion1.card.cardQuality == "Hot" && selectedCard.vPotion2.card.cardQuality == "Cold")
            {
                selectedCard.colorCardHot();
                selectedCard.vPotion1.colorCardHot();
                selectedCard.vPotion2.colorCardCold();
            }
        }

        // Shining Reliquary of Bleeding Liquid Gold
        if (selectedCard.card.cardName == "ShiningReliquary")
        {
            if (selectedCard.vPotion1.card.cardQuality == "Hot")
            {
                selectedCard.colorCardHot();
                selectedCard.vPotion1.colorCardHot();
            }

            if (selectedCard.vPotion2.card.cardQuality == "Hot")
            {
                selectedCard.colorCardHot();
                selectedCard.vPotion2.colorCardHot();
            }

            if (selectedCard.vPotion1.card.cardQuality == "Dry")
            {
                selectedCard.colorCardDry();
                selectedCard.vPotion1.colorCardDry();
            }

            if (selectedCard.vPotion2.card.cardQuality == "Dry")
            {
                selectedCard.colorCardDry();
                selectedCard.vPotion2.colorCardDry();
            }
        }

        // Canteen Carved from a Living Meteorite
        if (selectedCard.card.cardName == "CanteenCarvedFromMeteorite")
        {
            if (selectedCard.vPotion1.card.cardQuality == "Cold")
            {
                selectedCard.colorCardCold();
                selectedCard.vPotion1.colorCardCold();
            }

            if (selectedCard.vPotion2.card.cardQuality == "Cold")
            {
                selectedCard.colorCardCold();
                selectedCard.vPotion2.colorCardCold();
            }

            if (selectedCard.vPotion1.card.cardQuality == "Dry")
            {
                selectedCard.colorCardDry();
                selectedCard.vPotion1.colorCardDry();
            }

            if (selectedCard.vPotion2.card.cardQuality == "Dry")
            {
                selectedCard.colorCardDry();
                selectedCard.vPotion2.colorCardDry();
            }
        }

        // Silken Hanky with a Cool Delicate Heartbeat
        if (selectedCard.card.cardName == "SilkenHanky")
        {
            // Hot Bonus: +3 Damage
            if (selectedCard.vPotion1.card.cardQuality == "Hot" || selectedCard.vPotion2.card.cardQuality == "Hot")
            {
                // damage += 3;
                selectedCard.colorCardHot();
                selectedCard.vPotion2.colorCardHot();
            }
        }

        // Sandpaper Gunnysack with a Wire Drawstring
        if (selectedCard.card.cardName == "SandpaperGunnysack")
        {
            // Dry Bonus: +3 Damage
            if (selectedCard.vPotion1.card.cardQuality == "Dry" || selectedCard.vPotion2.card.cardQuality == "Dry")
            {
                // damage += 3;
                selectedCard.colorCardDry();
                selectedCard.vPotion2.colorCardDry();
            }
        }

        // Cooled Carafe for the Shipwrecked Spirit
        if (selectedCard.card.cardName == "CooledCarafe")
        {
            // Cold Bonus: +3 Damage
            if (selectedCard.vPotion1.card.cardQuality == "Cold" || selectedCard.vPotion2.card.cardQuality == "Cold")
            {
                // damage += 3;
                selectedCard.colorCardCold();
                selectedCard.vPotion2.colorCardCold();
            }
        }

        // Drawstring Pouch of a Broken Umbrella
        if (selectedCard.card.cardName == "DrawstringPouch")
        {
            // Wet Bonus: +3 Damage
            if (selectedCard.vPotion1.card.cardQuality == "Wet" || selectedCard.vPotion2.card.cardQuality == "Wet")
            {
                // damage += 3;
                selectedCard.colorCardWet();
                selectedCard.vPotion2.colorCardWet();
            }
        }

        // return damage;
    }

}