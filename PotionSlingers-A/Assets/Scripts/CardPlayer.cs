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
    public int pipsUsedThisTurn = 0;
    public bool dead;           //Does the player still have health left?
    public int potionsThrown;
    public int artifactsUsed = 0;
    public CharacterDisplay character;
    public bool ringBonus;
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

    public CardPlayer(int user_id, string name)
    {
        this.user_id = user_id;
        this.name = name;
    }

    void Start()
    {
        
    }

    void Update()
    {
        // yes damage
        if (GameManager.manager.damage)
        {
            foreach(CardPlayer player in GameManager.manager.players)
            {
                if(player.character.character.cardName == GameManager.manager.selectedOpponentCharName)
                {
                    player.subHealth(2);
                    GameManager.manager.damage = false;
                }
            }
        }
        
        // no damage
        if (GameManager.manager.trash)
        {
            GameManager.manager.trashMarketUI.SetActive(true);
            GameManager.manager.trash = false;

        }
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
            }
        }
    }

    public void initHealth()
    {
        hp = 10;
        hpCubes = 3;
        updateHealthUI();
    }

    public void updateHealthUI()
    {
        if (playerHP != null && playerHPCubes != null)
        {
            playerHP.GetComponent<Text>().text = hp.ToString();
            playerHPCubes.GetComponent<Text>().text = hpCubes.ToString();
            // playerHP.GetComponent<Text>().text = "HP: " + hp.ToString() + " /10";
            // playerHPCubes.GetComponent<Text>().text = "Cubes: " + hpCubes.ToString();
        }
    }

    public void updatePipsUI()
    {
        if (playerPips != null)
        {
            playerPips.GetComponent<Text>().text = pips.ToString() + " Pips";
        }
    }

    public void nicklesActionTrue()
    {
        nicklesAction = true;
    }

    public void setDefaultTurn()
    {
        pips = 6;
        pipsUsedThisTurn = 0;
        potionsThrown = 0;
        artifactsUsed = 0;
        ringBonus = false;
        bottleRocketBonus = false;
        blackRainBonus = false;

        if (isNickles)
        {
            nicklesAction = false;
        }

        // putting this logic somewhere else
        /*
        foreach (CardDisplay cd in holster.cardList)
        {
            // if there's a starter ring
            if (cd.card.cardType == "Ring" &&
                cd.card.cardQuality == "Starter")
            {
                ringBonus = true;
            }
        }
        */

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
            Debug.Log("Reets error");
            GameManager.manager.sendErrorMessage(14);
            if (!character.character.flipped)
            {
                addPips(2);
            } else
            {
                addPips(1);
            }
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

    public int checkArtifactBonus(int damage, CardDisplay selectedCard)
    {
        // probably depends on what card it is and what bonus it has, might have to implement logic for certain pools of cards this way

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

    public int checkVesselBonus(int damage, CardDisplay selectedCard)
    {
        // Fragile Glass Ornament
        // Does +1 Damage. Ignores loaded potion effects in this Vessel
        if (selectedCard.card.cardName == "Fragile Glass Ornament")
        {
            // this card ignores all potion effects, so i'm returning damage + 1
            return damage + 1;
        }

        // Hollowed Out Skull of Higyoude
        if(selectedCard.card.cardName == "HollowedOutSkull")
        {
            // does 6 damage, ignores all potion effects
            return 6;
        }

        // Vessel Bonus: +2 Damage
        if((selectedCard.vPotion1.card.cardName == "RefectionOfOnesGnarledEmotionalSelf" || selectedCard.vPotion2.card.cardName == "RefectionOfOnesGnarledEmotionalSelf") ||
            (selectedCard.vPotion1.card.cardName == "SoupMadeOfGunpowder" || selectedCard.vPotion2.card.cardName == "SoupMadeOfGunpowder") ||
            (selectedCard.vPotion1.card.cardName == "PourOfReallyAngryAcid" || selectedCard.vPotion2.card.cardName == "PourOfReallyAngryAcid"))
        {
            damage += 2;
        }

        // Vessel Bonus: Put any potion from the trash to the top of your deck
        if ((selectedCard.vPotion1.card.cardName == "A Freshly Caught and Distilled Sickness" || selectedCard.vPotion2.card.cardName == "A Freshly Caught and Distilled Sickness") ||
            (selectedCard.vPotion1.card.cardName == "TinctureOfMeltedMarble" || selectedCard.vPotion2.card.cardName == "TinctureOfMeltedMarble") ||
            (selectedCard.vPotion1.card.cardName == "A Swig of Regained Burning Appetite" || selectedCard.vPotion2.card.cardName == "A Swig of Regained Burning Appetite") ||
            (selectedCard.vPotion1.card.cardName == "A Squeeze of Wheezyfish" || selectedCard.vPotion2.card.cardName == "A Squeeze of Wheezyfish"))
        {
            // TODO: Implement this

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
                // TODO: make replaceMarketCards() method in GameManager to replace all ther market cards

                addPips(3);
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
                // TODO: make this method in GameManager

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

    // this will get messy quickly so actually comment things
    public int checkBonus(int damage, int selectedCard)
    {
        // Pluot damage bonus
        if (isPluot)
        {
            if(pluotBonusType == holster.cardList[selectedCard - 1].card.cardQuality)
            {
                damage++;
            }

            
            if (holster.cardList[selectedCard - 1].card.cardQuality == "Hot")
            {
                pluotHot = true;
            }

            if (holster.cardList[selectedCard - 1].card.cardQuality == "Wet")
            {
                pluotWet = true;
            }

            if (holster.cardList[selectedCard - 1].card.cardQuality == "Cold")
            {
                pluotCold = true;
            }

            if (holster.cardList[selectedCard - 1].card.cardQuality == "Dry")
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

        // starter ring +1 damage logic
        foreach (CardDisplay cd in holster.cardList)
        {
            // if there's a starter ring
            if ((cd.card.cardType == "Ring" &&
                cd.card.cardQuality == "Starter") && potionsThrown == 0)
            {
                damage++;
            }
        }

        // you may trash up to 2 market cards instead of doing damage
        // i'm gonna do some weird while loop to prevent it from returning damage until the player has selected an option
        if (holster.cardList[selectedCard - 1].card.cardName == "A Quizzical Look And a Rummage of Pockets" ||
            holster.cardList[selectedCard - 1].card.cardName == "An Example of What Not to Do" ||
            holster.cardList[selectedCard - 1].card.cardName == "A Confident Throw Into the Garbage")
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

        // you may trash 1 card in the market
        // add all the cards that have this text here
        if (holster.cardList[selectedCard - 1].card.cardName == "EssenceOfDifficultManualLabor" ||
            holster.cardList[selectedCard - 1].card.cardName == "LiquidPileOfHorrid" ||
            holster.cardList[selectedCard - 1].card.cardName == "Chocolatiers Delicate Pride" ||
            holster.cardList[selectedCard - 1].card.cardName == "ANaggingFeeling")
        {
            // GameManager method
            GameManager.manager.numTrashed = 1;
            GameManager.manager.trashMarketUI.SetActive(true);
            GameManager.manager.updateTrashMarketMenu();
        }

        // CARDS WITH THROW BONUSES GO HERE!

        // Throw Bonus: Heal +3 HP
        if (holster.cardList[selectedCard - 1].card.cardName == "QuartOfLemonade" ||
            holster.cardList[selectedCard - 1].card.cardName == "ThimbleOfHoney" ||
            holster.cardList[selectedCard - 1].card.cardName == "KissFromTheLipsOfAnAncientLove" ||
            holster.cardList[selectedCard - 1].card.cardName == "TallDrinkOfWater")
        {
            addHealth(3);
        }

        // Throw Bonus: +1 Pip
        if (holster.cardList[selectedCard - 1].card.cardName == "VintageAromaticKate" ||
            holster.cardList[selectedCard - 1].card.cardName == "JarFullOfGlitter" ||
            holster.cardList[selectedCard - 1].card.cardName == "BowlOfExtremelyHotSoup" ||
            holster.cardList[selectedCard - 1].card.cardName == "HumblingGlimpse")
        {
            addPips(1);
        }

        // You may create a starter potion on top of your deck
        if (holster.cardList[selectedCard - 1].card.cardName == "AMaliciousThought" ||
            holster.cardList[selectedCard - 1].card.cardName == "IntenseThirstForKnowledge" ||
            holster.cardList[selectedCard - 1].card.cardName == "VioletFireling" ||
            holster.cardList[selectedCard - 1].card.cardName == "InnocentLayerCake")
        {
            GameManager.manager.starterPotionMenu.SetActive(true);
        }

        // ring damage bonus
        if (ringBonus && potionsThrown == 0)
        {
            damage++;
        }

        // Throw Bonus: If your Holster is empty, place the top 4 cards of the Potion Market Deck into your Holster
        if (holster.cardList[selectedCard - 1].card.cardName == "ATearofBlackRain")
        {
            Debug.Log("Tear of Black Rain Bonus");

            for(int i = 0; i < holster.cardList.Count; i++)
            {
                if(i != (selectedCard - 1))
                {
                    if (holster.cardList[i].card.cardName != "placeholder")
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

    public void addHealth(int health) {
        hp += health;

        //Make sure that hp cannot go above 10
        if(hp > 10) {
            hp = 10;
        }
        updateHealthUI();
    }

    public void subHealth(int damage)
    {
        hp -= damage;

        //Make sure that hp doesn't go below 0
        //If hp goes below 0, set it to 10 and subtract a health cube
        if (hp < 0)
        {
            if (hpCubes > 0)
            {
                hp = 10;
                hpCubes--;
            }
            else
            {
                dead = true;
            }
        }

        Debug.Log("Subtracted " + damage + "from " + charName);
        Debug.Log(charName + "'s health = " + hp + " HP");
        updateHealthUI();

        // Flashes damage sign
        damageAmount.GetComponent<TMPro.TextMeshProUGUI>().text = damage.ToString();
        damageSign.SetActive(true);
        damageAmount.SetActive(true);
        StartCoroutine(waitThreeSeconds(damageSign));
        StartCoroutine(waitThreeSeconds(damageAmount));
    }

    // public void giveCube(Player player) {
    //     player.getCube();
    //     hpCubes--;
    // }

    // public void getCube() {
    //     takenHPCubes++;
    // }

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
        if(isNickles && pipsUsedThisTurn >= 10)
        {
            character.canBeFlipped = true;
            GameManager.manager.sendSuccessMessage(13);
        }
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