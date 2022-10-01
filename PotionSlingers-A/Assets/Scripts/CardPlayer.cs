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

        foreach (CardDisplay cd in holster.cardList)
        {
            // if there's a starter ring
            if (cd.card.cardType == "Ring" &&
                cd.card.cardQuality == "Starter")
            {
                ringBonus = true;
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

    public int checkArtifactBonus(int damage, CardDisplay selectedCard)
    {
        // probably depends on what card it is and what bonus it has, might have to implement logic for certain pools of cards this way

        // The Rapid Fire Caltrop Hand Cannon
        if(selectedCard.card.cardName == "RapidFireCaltrop")
        {
            // Dry Bonus: +2 Damage
            if(selectedCard.aPotion.card.cardQuality == "Dry")
            {
                damage += 2;
            }
        }

        // The Bottle Rocket
        if(selectedCard.card.cardName == "BottleRocket")
        {
            // Wet Bonus: +2 Damage
            // 3 P: Double the damage of this artifact this turn. You may only do this once per turn.
            if(selectedCard.aPotion.card.cardQuality == "Wet")
            {
                damage += 2;
                if (bottleRocketBonus)
                {
                    damage = damage * 2;
                }
            }
        }

        return damage;
    }

    public int checkVesselBonus(int damage, CardDisplay selectedCard)
    {
        // TODO: Vessel bonuses
        return 0;
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
        if(holster.cardList[selectedCard - 1].card.cardName == "AMaliciousThought" ||
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