using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Animator animator;
    public bool cubed = false;
    public bool ringBonus;
    public bool doubleRingBonus = false;
    public int bonusAmount;
    public int cardsTrashed = 0;
    //public HealthController health;
    public GameObject playerHP;
    public GameObject playerHPCubes;
    public GameObject playerPips;
    public GameObject currentPlayerHighlight;
    public GameObject damageSign;
    public GameObject damageAmount;
    public GameObject healSign;
    public GameObject healAmount;
    public GameObject hpBar;
    public GameObject healthText;
    public GameObject hitAnimation;
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
            foreach(CardPlayer player in GameManager.manager.players)
            {
                if(player.name == GameManager.manager.selectedOpponentName)
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
        switch (character.character.cardName)
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
                break;
            case "Reets":
                Debug.Log("I AM REETS");
                isReets = true;
                break;
            case "Saltimbocca":
                Debug.Log("I AM SALTIMBOCCA");
                isSaltimbocca = true;
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

            default:
                Debug.Log("Failed to set any bools");
                break;
        }
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
        animator.Play("BoloIdle");
    }

    public void updateHealthUI(string cardQuality = "")
    {
        if(healthText != null && hpBar != null)
        {
            healthText.GetComponent<Text>().text = hp.ToString();
            float numbers = (float)hp / 10f;
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

        /* TODO: Add triggers for animations for each CardPlayer 
         * Either do something with the hitImages list I made or make something else
         * I'm taking the below code out for now
         */

        // Doesn't exactly work, fix this later
        animator.Play("BoloHit");
        Invoke("playIdle", animator.GetCurrentAnimatorStateInfo(0).length);

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

    public void setDefaultTurn()
    {
        currentPlayerHighlight.SetActive(true);

        // check if cube should be taken
        if (cubed)
        {
            hpCubes--;
            if (hpCubes > 0)
            {
                hp = 10;

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
                hp = 0;
                Debug.Log("Somebody is dead!");
                dead = true;
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
                }
            }
            updateHealthUI();
        }

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
                } else
                {
                    pips += 2;
                }
            }

            if (cd.card.cardName == "Blacksnake Pip Sling")
            {
                pips += 2;    
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
                if(cd.card.cardType == "Ring")
                {
                    hasRing = true;
                }

                if(cd.card.cardType == "Artifact" && cd.aPotion.card.cardName != "placeholder")
                {
                    loadedItems++;
                }

                if (cd.card.cardType == "Vessel" && (cd.vPotion1.card.cardName != "placeholder" && cd.vPotion2.card.cardName != "placeholder"))
                {
                    loadedItems++;
                }

                if(cd.card.cardName == "Phylactery")
                {
                    hasPhlactery = true;
                }
            }

            if(hasRing && hasPhlactery && loadedItems == 2)
            {
                Debug.Log("Mega damage!");
                GameManager.manager.dealDamageToAll(12);
            }
        }

        updatePipsUI();
    }

    public void addThePhylactery()
    {
        subPips(6);
        GameManager.manager.sendSuccessMessage(12);
        deck.putCardOnTop(uniqueCards[3]);
    }

    public void addReetsCard()
    {
        if (deck.deckList.Count >= 1)
        {
            foreach (CardDisplay card in holster.cardList)
            {
                if (card.card.cardName == "placeholder")
                {
                    Card temp = deck.popCard();
                    card.updateCard(temp);
                    return;
                }
            }

            if (!character.character.flipped)
            {
                subPips(2);
            } else
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
        foreach(CardDisplay card in holster.cardList)
        {
            if(card.card.cardName == "placeholder")
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
        foreach(CardDisplay cd in holster.cardList)
        {
            if(cd.card.cardType == "Ring")
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

        foreach(CardPlayer cd in GameManager.manager.players)
        {
            if(cd.name == GameManager.manager.currentPlayerName && cd.isIsadore)
            {
                damage++;
                break;
            }
        }

        // Treasure Cloak Map
        // put a potion from the trash to the top of your deck
        if(selectedCard.card.cardName == "Treasure Cloak Map")
        {
            // add a check to see if a computer player triggered this and don't display the menu
            // probably just select a random card from the trash

            GameManager.manager.numTrashed = 1;
            GameManager.manager.trashDeckBonus = true;
            GameManager.manager.trashDeckMenu.SetActive(true);
            GameManager.manager.trashText.text = "Take a potion from the trash and put it on top of your deck!";
            GameManager.manager.td.displayTrash();
            return 0;
        }

        // The Skateboard
        if(selectedCard.card.cardName == "Skateboard")
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
                if(tricks == 4)
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
            } else
            // if cardQuality == "None"
            {
                addPips(3);
                GameManager.manager.deal1ToAll();
            }
        }

        // The Daggerheels
        if(selectedCard.card.cardName == "Daggerheels")
        {
            // Cold Bonus: +1 Damage
            if (selectedCard.aPotion.card.cardQuality == "Cold")
            {
                damage++;
            }
        }

        // The Rapid Fire Caltrop Hand Cannon
        if(selectedCard.card.cardName == "RapidFireCaltrop")
        {
            // Dry Bonus: +2 Damage
            if(selectedCard.aPotion.card.cardQuality == "Dry")
            {
                damage += 2;
            }
        }

        // Pewter Heart Necklace
        if(selectedCard.card.cardName == "PewterHeartNecklace")
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
            if(selectedCard.aPotion.card.cardQuality == "Wet")
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
                pluotBonusType == selectedCard.vPotion2.card.cardQuality )
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
                } else
                {
                    // TODO: check for computer player and disable this

                    GameManager.manager.numTrashed += 2;
                    GameManager.manager.trashMarketUI.SetActive(true);
                    GameManager.manager.updateTrashMarketMenu();
                }      
            }

            if (selectedCard.vPotion1.card.cardQuality == "Dry" ||
                selectedCard.vPotion2.card.cardQuality == "Dry")
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
            } else
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
        if((selectedCard.vPotion1.card.cardName == "RefectionOfOnesGnarledEmotionalSelf" || selectedCard.vPotion2.card.cardName == "RefectionOfOnesGnarledEmotionalSelf") ||
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
            // Furry Flagon Made from the Hide of Hairy Leeches
            // Hot + Wet Bonus: Put 1 card from the trash to the top of your deck
            if (selectedCard.card.cardName == "Furry Flagon fromthe Hide of Hairy Leeches")
            {
                GameManager.manager.trashDeckBonus = true;
                GameManager.manager.trashDeckMenu.SetActive(true);
                GameManager.manager.trashText.text = "Take a potion from the trash and put it on top of your deck!";
                GameManager.manager.td.displayTrash();
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
                    GameManager.manager.displayOpponentHolster();
                }
                else
                {
                    // maybe do something for computer
                }

                return damage;
            }
        }

        // Cursed Cucumella of Clumsy Acidic Coffee
        // Hot Bonus: Opponent trashes 1 card. Wet Bonus: Opponent trashes 1 card.
        if (selectedCard.card.cardName == "CursedCucumella")
        {
            if(selectedCard.vPotion1.card.cardQuality == "Hot" || selectedCard.vPotion2.card.cardQuality == "Hot")
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
                } else
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
                GameManager.manager.trashBonusMenu.SetActive(true);
                
                //GameManager.manager.opponentHolsterMenu.SetActive(true);
                //GameManager.manager.displayOpponentHolster();
            }
        }

        // Cold + Dry Bonus
        if ((selectedCard.vPotion1.card.cardQuality == "Cold" && selectedCard.vPotion2.card.cardQuality == "Dry") ||
            (selectedCard.vPotion2.card.cardQuality == "Cold" && selectedCard.vPotion1.card.cardQuality == "Dry"))
        {
            // The Boxing Flask of the Fist Wizard
            // Deal +3 damage to all other opponents
            if(selectedCard.card.cardName == "Boxing Flask of the Fist Wizard")
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
                }
                
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
            if(selectedCard.vPotion1.card.cardQuality == "Cold" || selectedCard.vPotion2.card.cardQuality == "Cold")
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
            if(selectedCard.vPotion1.card.cardQuality == "Wet" || selectedCard.vPotion2.card.cardQuality == "Wet")
            {
                damage += 3;
            }
        }

        return damage;
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
            if(cd.card.cardType == "Ring")
            {
                Debug.Log(selectedCard.card.cardType);
                if(cd.card.cardQuality == "Starter" && potionsThrown == 0 && vesselsThrown == 0 && selectedCard.card.cardType != "Artifact")
                {
                    Debug.Log("Starter ring bonus");
                    damage++;
                }
                // Sharpened Ring of Bauble Collector
                if(cd.card.cardName == "Sharpened Ring of the Bauble Collector")
                {
                    if(selectedCard.card.cardType == "Artifact")
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
                            } else
                            {
                                damage++;
                            }
                        }
                    }
                }

                // Finger Ring of Additional Pinkie
                if(cd.card.cardName == "FingerRingoftheAdditionalPinkie")
                {
                    // Thrown potions deal +1 damage
                    if(selectedCard.card.cardType == "Potion")
                    {
                        if (doubleRingBonus)
                        {
                            damage += 2;
                        } else
                        {
                            damage++;
                        }
                    }
                }

                // Glass Ring of Things That Contain Things
                if(cd.card.cardName == "GlassRingofThingsThatContainThings")
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

    // this will get messy quickly so actually comment things
    public int checkBonus(int damage, CardDisplay selectedCard)
    {
        // Pluot damage bonus
        if (isPluot)
        {
            if(pluotBonusType == selectedCard.card.cardQuality)
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
            if(pluotHot && pluotWet && pluotCold && pluotDry)
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
                
            } else
            {
                // not networked logic
                GameManager.manager.deckMenu.SetActive(true);
                GameManager.manager.displayDeck();
            }
            return damage;
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
                    foreach(CardDisplay cd in cp.holster.cardList)
                    {
                        if(cd.card.cardName == "CherryBomb Badge")
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
            
            // IMPORTANT: use this as template for other implementations of ComputerPlayer logic!!!
            // if they're not a computer player
            if (GameManager.manager.tempPlayer.gameObject.GetComponent<ComputerPlayer>() == null
                && gameObject.GetComponent<ComputerPlayer>() == null)
            {
                Debug.Log("No computer players?");
                GameManager.manager.opponentHolsterMenu.SetActive(true);
                GameManager.manager.displayOpponentHolster();
            } else
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
            GameManager.manager.opponentHolsterMenu.SetActive(true);
            GameManager.manager.displayOpponentHolster();
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
            } else
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
            return 0;
            
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
            if (GameManager.manager.tempPlayer.gameObject.GetComponent<ComputerPlayer>() == null
                && gameObject.GetComponent<ComputerPlayer>() == null)
            {
                GameManager.manager.starterPotionMenu.SetActive(true);
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
                GameManager.manager.updateTrashMarketMenu();
            } else
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

            foreach(CardDisplay cd in holster.cardList)
            {
                if(cd.card.cardName != "ATearofBlackRain")
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
            if(cd.card.cardType == "Artifact") 
            {
                // BubbleWand
                if(cd.card.cardName == "BubbleWand")
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
                        }
                    }
                }
                // Shield of the Mouth of Truth
                else if(cd.card.cardName == "Shield of the Mouth of Truth")
                {
                    // if there is a loaded potion
                    if (cd.aPotion.card != cd.placeholder) {
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
            
            else if(cd.card.cardType == "Ring")
            {
                // Crusty Ring of the Crying Rusty Tears
                if(cd.card.cardName == "Crusty Ring of the Crying Rusty Tears")
                {
                    // Opponent's ARTIFACTS prevent 2 damage.
                    // (prevents 4 damage with Ring of Rings in Holster)
                    // checks throwers hand for the artifact, to my knowledge the card is not mutated yet
                    foreach (CardPlayer cd2 in GameManager.manager.players) {
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
                else if(cd.card.cardName == "Foggy Ring of the Nearsighted Old Crone")
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
                else if(cd.card.cardName == "Thick Ring of the Furrowed Brow Dolt")
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
        if(hp > 10) {
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
            return;
        }

        hp -= damage;

        //Make sure that hp doesn't go below 0
        //If hp goes below 0, set it to 10 and subtract a health cube
        if (hp <= 0)
        {
            cubed = true;
            hp = 0;
        }

        Debug.Log("Subtracted " + damage + "from " + charName);
        Debug.Log(charName + "'s health = " + hp + " HP");
        if (cubed)
        {
            hpCubes--;
            if(hpCubes == 0)
            {
                // check for Phlactery Sweetbitter situation but otherwise everyone else is dead

                dead = true;
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
        updateHealthUI();

        // Flashes damage sign
        /*
        if (damageSign.activeInHierarchy)
        {
            damageAmount.GetComponent<TMPro.TextMeshProUGUI>().text = damage.ToString();
            damageSign.SetActive(true);
            damageAmount.SetActive(true);
            StartCoroutine(waitThreeSeconds(damageSign));
            StartCoroutine(waitThreeSeconds(damageAmount));
        }
        */

        if(damageSign !=  null && damageAmount != null)
        {
            damageAmount.GetComponent<TMPro.TextMeshProUGUI>().text = damage.ToString();
            damageSign.SetActive(true);
            damageAmount.SetActive(true);
            StartCoroutine(waitThreeSeconds(damageSign));
            StartCoroutine(waitThreeSeconds(damageAmount));
        }
    }

    public void addPips(int morePips)
    {
        pips += morePips;
        updatePipsUI();
    }

    public void subPips(int lessPips)
    {
        pips -= lessPips;
        pipsUsedThisTurn += lessPips;
        updatePipsUI();

        // NICKLES FLIP LOGIC
            foreach (CardPlayer cp in GameManager.manager.players)
            {
                if(cp.isNickles && cp.pipsUsedThisTurn >= 10)
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
}