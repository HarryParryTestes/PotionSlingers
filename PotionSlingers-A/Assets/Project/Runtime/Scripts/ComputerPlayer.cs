using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
            return num + 1 >= GameManager.manager.numPlayers ? 0 : num + 1;
            // chooseRandomPlayer();
        }
        if (charName == GameManager.manager.players[num].character.character.cardName)
        {
            Debug.Log("Changing number");
            return num + 1 == GameManager.manager.numPlayers ? 0 : num + 1;
            // chooseRandomPlayer();
        }

        // if the player is dead, don't attack them
        if (GameManager.manager.players[num].dead)
        {
            Debug.Log("This player is dead! Changing number");
            return num + 1 >= GameManager.manager.numPlayers ? 0 : num + 1;
            // chooseRandomPlayer();
        }
        return num;
    }

    public int chooseMarketCard()
    {
        int numero;
        Card card = GameManager.manager.players[0].deck.placeholder;

        if (artifacts == 0 && vessels == 0)
        {
            numero = rng.Next(1, 7);
        }
        else if ((artifacts + vessels) >= 2)
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

    public IEnumerator MoveToTrash(GameObject obj)
    {
        GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList
            [GameManager.manager.selectedCardInt - 1].artifactSlot.transform.GetChild(0).gameObject.SetActive(false);
        obj.transform.DOJump(new Vector2(1850f, 400f), 400f, 1, 1f, false);
        obj.transform.DOScale(0.2f, 1f);
        obj.transform.DORotate(new Vector3(0, 0, 720f), 1f, RotateMode.FastBeyond360);
        yield return new WaitForSeconds(0.1f);
        GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList
            [GameManager.manager.selectedCardInt - 1].artifactSlot.transform.GetChild(0).gameObject.SetActive(true);
        GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.selectedCardInt - 1].artifactSlot.SetActive(false);
        yield return new WaitForSeconds(0.9f);

        Destroy(obj);
        // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.transform.GetChild(0).gameObject.SetActive(true);
        // players[myPlayerIndex].holster.cardList[selectedCardInt - 1].artifactSlot.SetActive(false);
        GameManager.manager.td.transform.parent.DOMove(new Vector2(GameManager.manager.td.transform.parent.position.x,
            GameManager.manager.td.transform.parent.position.y - 5), 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public void pickRandomHolsterCard()
    {
        int f = 0;
        foreach (CardDisplay cd in GameManager.manager.playerHolster.cardList)
        {
            if (cd.card.name == "placeholder")
                f++;
        }
        if (f == 4)
        {
            GameManager.manager.sendMessage("Spiced up a card in your holster!");
            return;
        }


        int holsterNum = rng.Next(1, 5);
        if (GameManager.manager.playerHolster.cardList[holsterNum - 1].card.name == "placeholder" ||
            GameManager.manager.playerHolster.cardList[holsterNum - 1].card.spicy)
        {
            pickRandomHolsterCard();
            return;
        }
        GameManager.manager.playerHolster.cardList[holsterNum - 1].makeSpicy();
        GameManager.manager.sendMessage("Spiced up a card in your holster!");
    }

    public int pickRandomHolsterCardCrow()
    {
        int f = 0;
        foreach (CardDisplay cd in GameManager.manager.playerHolster.cardList)
        {
            if (cd.card.name == "placeholder")
                f++;
        }
        if (f == 4)
        {
            // GameManager.manager.sendMessage("Spiced up a card in your holster!");
            return -1;
        }

        int holsterNum = rng.Next(1, 5);
        if (GameManager.manager.playerHolster.cardList[holsterNum - 1].card.name == "placeholder")
        {
            pickRandomHolsterCardCrow();
        }
        // GameManager.manager.trashCard(playerHolster.cardList[holsterNum - 1].card);
        // GameManager.manager.td.addCard(playerHolster.cardList[selectedCardInt - 1]);

        // GameManager.manager.sendMessage("Spiced up a card in your holster!");
        return holsterNum;
    }

    public void storyModeTurn()
    {
        Debug.Log("STORY MODE LOGIC");
        int damage;
        int attackType;

        if (this.gameObject.GetComponent<CardPlayer>().name == "Singelotte")
        {
            int cards = 0;
            foreach (CardDisplay cd in GameManager.manager.playerHolster.cardList)
            {
                if (cd.card.name == "placeholder" || cd.card.spicy)
                    cards++;
            }

            if (cards == 4)
            {
                attackType = rng.Next(1, 3);
            }
            else
                attackType = rng.Next(1, 4);
            switch (attackType)
            {
                case 1:
                    GameManager.manager.players[0].subHealth(4);
                    GameManager.manager.sendMessage("Took " + 4 + " damage!");
                    this.gameObject.GetComponent<CardPlayer>().animator.Play("SingeYellowAttack");
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Singe_Swing");
                    // MATTEO: Add Singelotte sfx

                    this.gameObject.GetComponent<CardPlayer>().Invoke("playIdle", 2.2f);
                    GameManager.manager.Invoke("endTurn", 2.5f);
                    break;
                // spicy holster card
                case 2:
                    int cardNum = rng.Next(1, 7);
                    switch (cardNum)
                    {
                        case 1:
                            GameManager.manager.md1.cardDisplay1.makeSpicy();
                            // add DOTween animations for spicy magic here
                            break;
                        case 2:
                            GameManager.manager.md1.cardDisplay2.makeSpicy();
                            break;
                        case 3:
                            GameManager.manager.md1.cardDisplay3.makeSpicy();
                            break;
                        case 4:
                            GameManager.manager.md2.cardDisplay1.makeSpicy();
                            break;
                        case 5:
                            GameManager.manager.md2.cardDisplay2.makeSpicy();
                            break;
                        case 6:
                            GameManager.manager.md2.cardDisplay3.makeSpicy();
                            break;
                        default:
                            break;
                    }
                    GameManager.manager.sendMessage("Spiced up a card in the market!");
                    this.gameObject.GetComponent<CardPlayer>().animator.Play("SingePurpleAttack");
                    // MATTEO: Add Singelotte sfx
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Singe_Spell");
                    this.gameObject.GetComponent<CardPlayer>().Invoke("playIdle", 2.2f);
                    GameManager.manager.Invoke("endTurn", 2.5f);
                    break;
                // spicy market card
                case 3:
                    pickRandomHolsterCard();
                    this.gameObject.GetComponent<CardPlayer>().animator.Play("SingeAttack");
                    // MATTEO: Add Singelotte sfx
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Singe_Horns");
                    this.gameObject.GetComponent<CardPlayer>().Invoke("playIdle", 2.1f);
                    GameManager.manager.Invoke("endTurn", 2.5f);
                    break;

                default:
                    break;
            }
            return;
        }

        if (this.gameObject.GetComponent<CardPlayer>().name == "Crowpunk")
        {
            Debug.Log("Crowpunk attack");
            this.gameObject.GetComponent<CardPlayer>().animator.Play("CrowAttack");
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Crowpunk_attack");
            this.gameObject.GetComponent<CardPlayer>().Invoke("playIdle", this.gameObject.GetComponent<CardPlayer>().animator.GetCurrentAnimatorStateInfo(0).length);
            int cards = 0;
            foreach (CardDisplay cd in GameManager.manager.playerHolster.cardList)
            {
                if (cd.card.name == "placeholder")
                    cards++;
            }

            if (cards == 4)
            {
                damage = rng.Next(1, 4);
            }
            else
                damage = rng.Next(1, 5);
            // damage = rng.Next(1, 5);

            // make the player take damage without using throwing functions
            if (damage == 4)
            {
                int cardNumber = pickRandomHolsterCardCrow();
                if (cardNumber == -1)
                {
                    GameManager.manager.Invoke("endTurn", 2f);
                    return;
                }
                GameManager.manager.selectedCardInt = cardNumber;

                GameObject obj = Instantiate(GameManager.manager.playerHolster.cardList[GameManager.manager.selectedCardInt - 1].gameObject,
                        GameManager.manager.playerHolster.cardList[GameManager.manager.selectedCardInt - 1].gameObject.transform.position,
                        GameManager.manager.playerHolster.cardList[GameManager.manager.selectedCardInt - 1].gameObject.transform.rotation,
                        GameManager.manager.playerHolster.cardList[GameManager.manager.selectedCardInt - 1].gameObject.transform);

                GameManager.manager.StartCoroutine(MoveToTrash(obj));

                GameManager.manager.td.addCard(GameManager.manager.playerHolster.cardList[GameManager.manager.selectedCardInt - 1]);

                // GameManager.manager.playerHolster.cardList[GameManager.manager.selectedCardInt - 1].updateCard
                //     (GameManager.manager.playerHolster.cardList[GameManager.manager.selectedCardInt - 1].placeholder);

                GameManager.manager.sendMessage("CrowPunk just trashed your card!");

                GameManager.manager.Invoke("endTurn", 2f);
                return;
            }

            GameManager.manager.players[0].subHealth(damage);
            GameManager.manager.sendMessage("Took " + damage + " damage!");

            GameManager.manager.Invoke("endTurn", 2f);
            return;
        }
        if (this.gameObject.GetComponent<CardPlayer>().name == "Fingas")
        {
            this.gameObject.GetComponent<CardPlayer>().animator.Play("Fingas_attack");
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Fingas_turn");
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Fingas_snap");
            // this.gameObject.GetComponent<CardPlayer>().Invoke("playIdle", this.gameObject.GetComponent<CardPlayer>().animator.GetCurrentAnimatorStateInfo(0).length);
            this.gameObject.GetComponent<CardPlayer>().Invoke("playIdle", 1.55f);
            // basic enemy that does 1-4 damage per turn
            damage = rng.Next(1, 5);

            // make the player take damage without using throwing functions

            GameManager.manager.players[0].subHealth(damage);
            GameManager.manager.sendMessage("Took " + damage + " damage!");

            GameManager.manager.Invoke("endTurn", 2f);
            return;
        }
        if (this.gameObject.GetComponent<CardPlayer>().name == "Bag o' Snakes")
        {
            this.gameObject.GetComponent<CardPlayer>().animator.Play("BagAttack");
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Snakes_Attack");
            // MATTEO: Add Bag of Snakes sfx
            // FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Crowpunk_attack");
            this.gameObject.GetComponent<CardPlayer>().Invoke("playIdle", this.gameObject.GetComponent<CardPlayer>().animator.GetCurrentAnimatorStateInfo(0).length);
            // basic enemy that does 1-4 damage per turn
            damage = rng.Next(1, 5);

            // make the player take damage without using throwing functions

            GameManager.manager.players[0].subHealth(damage);
            GameManager.manager.sendMessage("Took " + damage + " damage!");

            GameManager.manager.Invoke("endTurn", 2f);
            return;
        }

        // basic enemy that does 1-4 damage per turn
        damage = rng.Next(1, 5);

        // make the player take damage without using throwing functions

        GameManager.manager.players[0].subHealth(damage);
        GameManager.manager.sendMessage("Took " + damage + " damage!");

        GameManager.manager.Invoke("endTurn", 2f);

    }

    public IEnumerator waitASecBro()
    {
        // modify this based on what difficulty the CPU is

        yield return new WaitForSeconds(1.5f);

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

            // Throwing artifacts / vessels
            if (holster.cardList[i].aPotion.card.cardName != "placeholder" ||
                (holster.cardList[i].vPotion1.card.cardName != "placeholder" &&
                holster.cardList[i].vPotion2.card.cardName != "placeholder"))
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
                        GameManager.manager.tempPlayer = GameManager.manager.players[number];
                        GameManager.manager.selectedCardInt = k + 1;
                        GameManager.manager.throwPotion();
                        actionsList.Add("THROW");
                        StartCoroutine(waitASecBro());
                        return;
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

                    // Loading vessels
                    // idk how to prioritize one over the other but fuck it
                    if (AICards[i].cardType == "Vessel" && AICards[k].cardType == "Potion")
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

            // sell the starter ring
            if (turn > 3 && AICards[i].cardQuality == "Starter" && AICards[i].cardType == "Ring")
            {
                GameManager.manager.selectedCardInt = i + 1;
                GameManager.manager.sellCard();
                actionsList.Add("SELL");
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
