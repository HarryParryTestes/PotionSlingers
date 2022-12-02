using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPlayer : CardPlayer
{

    public List<Card> AICards = new List<Card>(4);
    public int actions = 0;
    public int potions = 0;
    public int artifacts = 0;
    public int vessels = 0;
    public int rings = 0;
    private int j = 0;
    public int turn = 0;
    public bool easy = false;
    public bool medium = false;
    public bool hard = false;
    public System.Random rng = new System.Random();

    public List<string> actionsList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int chooseRandomPlayer()
    {
        int num = rng.Next(0, 4);
        Debug.Log("Number: " + num);
        if (num == j)
        {
            Debug.Log("Changing number");
            chooseRandomPlayer();
        }
        return num;
    }

    public int chooseMarketCard()
    {
        int numero;
        Card card = GameManager.manager.players[0].deck.placeholder;

        if (artifacts == 0 && vessels == 0)
        {
            numero = rng.Next(4, 7);
        }
        else if (potions >= 2)
        {
            // exclusively buy potions as ammunition
            numero = rng.Next(1, 4);
        } else
        {
            numero = rng.Next(1, 7);
        }

        switch (numero)
        {
            case 1:
                card = GameManager.manager.md1.cardDisplay1.card;
                break;
            case 2:
                card = GameManager.manager.md1.cardDisplay2.card;
                break;
            case 3:
                card = GameManager.manager.md1.cardDisplay3.card;
                break;
            case 4:
                card = GameManager.manager.md2.cardDisplay1.card;
                break;
            case 5:
                card = GameManager.manager.md2.cardDisplay2.card;
                break;
            case 6:
                card = GameManager.manager.md2.cardDisplay3.card;
                break;
            default:
                break;
        }

        switch (card.cardType)
        {
            case "Artifact":
                if (artifacts > 1)
                {
                    chooseMarketCard();
                }
                break;
            case "Vessel":
                if (vessels > 1)
                {
                    chooseMarketCard();
                }
                break;
            case "Ring":
                if (rings > 2)
                {
                    chooseMarketCard();
                }
                break;
            default:
                break;
        }

        return numero;
    }

    public IEnumerator waitASecBro()
    {
        // modify this based on what difficulty the CPU is
        yield return new WaitForSeconds(3);
        if (easy)
        {
            AITurn();
        }
        AITurn();
    }

    /*
    public void mcts()
    {
        if (actionsList.Count == 0)
        {
            actionsList.Add("BUY");
            actionsList.Add("SELL");
            actionsList.Add("THROW");
            actionsList.Add("LOAD");
            actionsList.Add("CYCLE");
            actionsList.Add("TRASH");
        }
    }
    */

    public void AITurn()
    {
        if (actionsList.Count == 0)
        {
            actionsList.Add("BUY");
            actionsList.Add("SELL");
            actionsList.Add("THROW");
            actionsList.Add("LOAD");
            actionsList.Add("CYCLE");
            actionsList.Add("TRASH");
        }

        artifacts = 0;
        potions = 0;
        vessels = 0;
        rings = 0;
        if(actions == 0)
        {
            turn++;
        }

        actions++;
        if(actions > 5)
        {
            actions = 0;
            GameManager.manager.endTurn();
            return;
        }

        AICards.Clear();

        if (holster == null)
        {
            Debug.Log("Grabbing the holster");
            holster = gameObject.GetComponent<CardPlayer>().holster;
            name = gameObject.GetComponent<CardPlayer>().name;
            pips = gameObject.GetComponent<CardPlayer>().pips;
        }

        
        foreach(CardDisplay cd in holster.cardList)
        {
            AICards.Add(cd.card);
            switch (cd.card.cardType)
            {
                case "Potion":
                    potions++;
                    break;
                case "Artifact":
                    artifacts++;
                    break;
                case "Vessel":
                    vessels++;
                    break;
                case "Ring":
                    rings++;
                    break;
            }
        }

        for(int i = 0; i < AICards.Count; i++)
        {
            Debug.Log(AICards[i].cardName);

            // Throwing artifacts
            if (holster.cardList[i].aPotion.card.cardName != "placeholder")
            {
                // j is index of current player in players array
                for (j = 0; j < GameManager.manager.numPlayers; j++)
                {
                    if (GameManager.manager.players[j].name == name)
                    {
                        Debug.Log("J: " + j);
                        break;
                    }
                }

                int number = chooseRandomPlayer();
                GameManager.manager.selectedOpponentCharName = GameManager.manager.players[number].character.character.cardName;
                GameManager.manager.selectedCardInt = i + 1;
                GameManager.manager.throwPotion();
                StartCoroutine(waitASecBro());
                return;
            }

            for (int k = 0; k < AICards.Count; k++)
            {
                if(AICards[i] != AICards[k])
                {
                    // Throwing potions with starter ring
                    // if the potion actually does damage and doesn't heal you
                    if ((AICards[i].cardType == "Ring" && AICards[i].cardQuality == "Starter") &&
                        AICards[k].cardType == "Potion" && AICards[k].effectAmount > 0)
                    {
                        int number = chooseRandomPlayer();
                        GameManager.manager.selectedOpponentCharName = GameManager.manager.players[number].character.character.cardName;
                        GameManager.manager.selectedCardInt = k + 1;
                        GameManager.manager.throwPotion();
                    }

                    // Loading artifacts
                    if (AICards[i].cardType == "Artifact" && AICards[k].cardType == "Potion")
                    {
                        GameManager.manager.loadedCardInt = i;
                        GameManager.manager.selectedCardInt = k + 1;
                        GameManager.manager.loadPotion();
                        StartCoroutine(waitASecBro());
                        return;
                    }
                }
            }

            // Trashing cards
            // Trash the starter cards on turn 2?
            // make sure to not throw away the starter ring just yet
            if (turn > 1 && potions == 0 && AICards[i].cardQuality == "Starter" && AICards[i].cardType != "Ring")
            {
                GameManager.manager.selectedCardInt = i + 1;
                GameManager.manager.trashCard();
                StartCoroutine(waitASecBro());
                return;
            }

            if (turn > 2 && AICards[i].cardQuality == "Starter" && AICards[i].cardType != "Ring")
            {
                GameManager.manager.selectedCardInt = i + 1;
                GameManager.manager.trashCard();
                StartCoroutine(waitASecBro());
                return;
            }
        }

        // Post-attack logic goes here
        // Buying
        int marketNum = chooseMarketCard();
        
        switch (marketNum)
        {
            case 1:
                GameManager.manager.md1.cardInt = 1;
                GameManager.manager.topMarketBuy();
                break;
            case 2:
                GameManager.manager.md1.cardInt = 2;
                GameManager.manager.topMarketBuy();
                break;
            case 3:
                GameManager.manager.md1.cardInt = 3;
                GameManager.manager.topMarketBuy();
                break;
            case 4:
                GameManager.manager.md2.cardInt = 1;
                GameManager.manager.bottomMarketBuy();
                break;
            case 5:
                GameManager.manager.md2.cardInt = 2;
                GameManager.manager.bottomMarketBuy();
                break;
            case 6:
                GameManager.manager.md2.cardInt = 3;
                GameManager.manager.bottomMarketBuy();
                break;
            default:
                break;
        }

        // put coroutine here
        StartCoroutine(waitASecBro());
        //AITurn();
    }
}
