using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComputerPlayer : CardPlayer
{

    public List<Card> AICards = new List<Card>(4);
    public int actions = 0;
    public int potions = 0;
    public int artifacts = 0;
    public int vessels = 0;
    public int rings = 0;
    private int j = 3;
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
        Debug.Log("Number of players: " + GameManager.manager.numPlayers);

        int num = rng.Next(0, GameManager.manager.numPlayers);
        Debug.Log("Number: " + num);

        if (j == num)
        {
            Debug.Log("Why are you hitting yourself???");
            chooseRandomPlayer();
        }
        if (charName == GameManager.manager.players[num].character.character.cardName)
        {
            Debug.Log("Changing number");
            chooseRandomPlayer();
        }

        // if the player is dead, don't attack them
        if (GameManager.manager.players[num].dead)
        {
            Debug.Log("This player is dead! Changing number");
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
        }
        else
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

    public void storyModeTurn()
    {
        Debug.Log("STORY MODE LOGIC");
        
        if (this.gameObject.GetComponent<CardPlayer>().name == "CrowPunk")
        {
            this.gameObject.GetComponent<CardPlayer>().animator.Play("CrowAttack");
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Crowpunk_attack");
            this.gameObject.GetComponent<CardPlayer>().Invoke("playIdle", this.gameObject.GetComponent<CardPlayer>().animator.GetCurrentAnimatorStateInfo(0).length);
        }
        if (this.gameObject.GetComponent<CardPlayer>().name == "Fingas")
        {
            this.gameObject.GetComponent<CardPlayer>().animator.Play("Fingas_attack");
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Fingas_turn");
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Fingas_snap");
            // this.gameObject.GetComponent<CardPlayer>().Invoke("playIdle", this.gameObject.GetComponent<CardPlayer>().animator.GetCurrentAnimatorStateInfo(0).length);
            this.gameObject.GetComponent<CardPlayer>().Invoke("playIdle", 1.55f);
        }
        if (this.gameObject.GetComponent<CardPlayer>().name == "Bag o' Snakes")
        {
            this.gameObject.GetComponent<CardPlayer>().animator.Play("BagAttack");
            // MATTEO: Add Bag of Snakes sfx
            // FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Crowpunk_attack");
            this.gameObject.GetComponent<CardPlayer>().Invoke("playIdle", this.gameObject.GetComponent<CardPlayer>().animator.GetCurrentAnimatorStateInfo(0).length);
        }

        // basic enemy that does 1-4 damage per turn
        int damage = rng.Next(1, 5);

        // make the player take damage without using throwing functions

        GameManager.manager.players[0].subHealth(damage);
        GameManager.manager.sendMessage("Took " + damage + " damage!");

        GameManager.manager.Invoke("endTurn", 3f);

    }

    public IEnumerator waitASecBro()
    {
        // modify this based on what difficulty the CPU is
        yield return new WaitForSeconds(2);

        // story mode logic
        if (Game.storyMode && this.gameObject.GetComponent<CardPlayer>().name != "Saltimbocca")
        {
            // insert appropriate logic for whatever enemies we have in here
            storyModeTurn();
            yield break;
        }

        // regular CPU logic
        if (easy)
        {
            AITurn();
        }
        AITurn();
    }

    public void AITurn()
    {
        if (this.gameObject.GetComponent<CardPlayer>().dead)
        {
            Debug.Log("The computer player is dead");
            GameManager.manager.endTurn();
            return;
        }

        /*
        if (actionsList.Count == 0)
        {
            actionsList.Add("BUY");
            actionsList.Add("SELL");
            actionsList.Add("THROW");
            actionsList.Add("LOAD");
            actionsList.Add("CYCLE");
            actionsList.Add("TRASH");
        }
        */

        artifacts = 0;
        potions = 0;
        vessels = 0;
        rings = 0;
        if (actions == 0)
        {
            turn++;
        }

        actions++;
        if (actions > 5)
        {
            actions = 0;

            Debug.Log("Actions done by the computer this turn:");

            foreach (string act in actionsList)
            {
                Debug.Log(act);
            }

            // Debug.Log(actionsList);
            actionsList.Clear();
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


        foreach (CardDisplay cd in holster.cardList)
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

        for (int i = 0; i < AICards.Count; i++)
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

                // if two players, just hardcode throwing at player 1
                if (GameManager.manager.numPlayers == 2)
                {
                    // GameManager.manager.selectedOpponentName = GameManager.manager.players[0].name;
                    GameManager.manager.tempPlayer = GameManager.manager.players[0];
                    GameManager.manager.selectedCardInt = i + 1;
                    GameManager.manager.throwPotion();
                    actionsList.Add("THROW");
                    StartCoroutine(waitASecBro());
                    return;
                }

                int number = chooseRandomPlayer();
                // GameManager.manager.selectedOpponentName = GameManager.manager.players[number].name;
                GameManager.manager.tempPlayer = GameManager.manager.players[number];
                Debug.Log("Opponent name is: " + GameManager.manager.players[number].name);
                GameManager.manager.selectedCardInt = i + 1;
                GameManager.manager.throwPotion();
                actionsList.Add("THROW");
                StartCoroutine(waitASecBro());
                return;
            }

            for (int k = 0; k < AICards.Count; k++)
            {
                if (AICards[i] != AICards[k])
                {
                    // Throwing potions with starter ring
                    // if the potion actually does damage and doesn't heal you
                    if ((AICards[i].cardType == "Ring" && AICards[i].cardQuality == "Starter") &&
                        AICards[k].cardType == "Potion" && AICards[k].effectAmount > 0)
                    {
                        int number = chooseRandomPlayer();
                        Debug.Log("Number is: " + number);
                        GameManager.manager.selectedOpponentName = GameManager.manager.players[number].name;
                        GameManager.manager.selectedCardInt = k + 1;
                        GameManager.manager.throwPotion();
                        actionsList.Add("THROW");
                    }

                    // Loading artifacts
                    if (AICards[i].cardType == "Artifact" && AICards[k].cardType == "Potion")
                    {
                        GameManager.manager.loadedCardInt = i;
                        GameManager.manager.selectedCardInt = k + 1;
                        GameManager.manager.loadPotion();
                        actionsList.Add("LOAD");
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
                actionsList.Add("TRASH");
                StartCoroutine(waitASecBro());
                return;
            }

            if (turn > 2 && AICards[i].cardQuality == "Starter" && AICards[i].cardType != "Ring")
            {
                GameManager.manager.selectedCardInt = i + 1;
                GameManager.manager.trashCard();
                actionsList.Add("TRASH");
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
                actionsList.Add("BUY");
                break;
            case 2:
                GameManager.manager.md1.cardInt = 2;
                GameManager.manager.topMarketBuy();
                actionsList.Add("BUY");
                break;
            case 3:
                GameManager.manager.md1.cardInt = 3;
                GameManager.manager.topMarketBuy();
                actionsList.Add("BUY");
                break;
            case 4:
                GameManager.manager.md2.cardInt = 1;
                GameManager.manager.bottomMarketBuy();
                actionsList.Add("BUY");
                break;
            case 5:
                GameManager.manager.md2.cardInt = 2;
                GameManager.manager.bottomMarketBuy();
                actionsList.Add("BUY");
                break;
            case 6:
                GameManager.manager.md2.cardInt = 3;
                GameManager.manager.bottomMarketBuy();
                actionsList.Add("BUY");
                break;
            default:
                break;
        }

        // put coroutine here
        StartCoroutine(waitASecBro());
        //AITurn();
    }
}
