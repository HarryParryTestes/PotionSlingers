using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Steamworks;
using DG.Tweening;

public class TreasureMenu : MonoBehaviour
{
    public List<Card> cardPool;
    public List<Card> hatPool;
    public List<CardDisplay> cardDisplays;
    public TMPro.TextMeshProUGUI description;

    public GameObject currencyHoverBox;
    public GameObject healthHoverBox;
    public GameObject deck;
    public GameObject holster;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void treasureAnimInit()
    {
        StartCoroutine(treasureAnimation());
    }

    public void FadeIn(GameObject obj)
    {
        obj.SetActive(true);
        if (obj.GetComponent<BonusMenuUIManager>() == null)
            obj.AddComponent<BonusMenuUIManager>();
        if (obj.GetComponent<CanvasGroup>() == null)
            obj.AddComponent<CanvasGroup>();
        obj.GetComponent<BonusMenuUIManager>().setAlphaZero();
    }


    public IEnumerator treasureAnimation()
    {
        FadeIn(this.gameObject);
        FadeIn(this.transform.GetChild(9).GetChild(3).gameObject);
        currencyHoverBox.SetActive(false);
        healthHoverBox.SetActive(false);
        deck.SetActive(false);
        holster.SetActive(false);
        foreach (CardDisplay cd in cardDisplays)
        {
            cd.gameObject.SetActive(false);
        }

        foreach (CardDisplay cd in cardDisplays)
        {
            Vector3 currentPosition = cd.transform.position;
            // cd.GetComponent<CardHover>().originalPosition = currentPosition;
            cd.transform.position = new Vector3(currentPosition.x + 20f, currentPosition.y - 20f, currentPosition.z);
            cd.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            cd.transform.DOJump(currentPosition, 10f, 1, 1f, false);
            cd.transform.DORotate(new Vector3(0, 0, 720f), 1f, RotateMode.FastBeyond360);
            yield return new WaitForSeconds(0.5f);
        }       
    }

    public int GetIntHat()
    {
        System.Random rng = new System.Random();

        var exclude = new HashSet<int>() { };
        for (int i = 0; i < hatPool.Count; i++)
        {
            if (hatPool[i].name == cardDisplays[0].card.name ||
                hatPool[i].name == cardDisplays[1].card.name ||
                hatPool[i].name == cardDisplays[2].card.name)
            {
                exclude.Add(i);
            }
        }

        var range = Enumerable.Range(0, hatPool.Count).Where(i => !exclude.Contains(i));
        int index = rng.Next(0, hatPool.Count - exclude.Count);
        return range.ElementAt(index);
    }

    public int GetInt()
    {

        System.Random rng = new System.Random();

        var exclude = new HashSet<int>() { };
        for (int i = 0; i < cardPool.Count; i++)
        {
            if (cardPool[i].name == cardDisplays[0].card.name ||
                cardPool[i].name == cardDisplays[1].card.name ||
                cardPool[i].name == cardDisplays[2].card.name)
            {
                exclude.Add(i);
            }
        }

        var range = Enumerable.Range(0, cardPool.Count).Where(i => !exclude.Contains(i));
        int index = rng.Next(0, cardPool.Count - exclude.Count);
        return range.ElementAt(index);
    }

    public void chooseHats()
    {
        description.text = "You won a prize! Pick a hat!";

        foreach (CardDisplay cd in cardDisplays)
        {
            System.Random rng = new System.Random();
            int index = GetIntHat();
            cd.updateCard(hatPool[index]);
        }
    }

    public void chooseCards()
    {
        description.text = "You found treasure! Pick a card!";

        foreach (CardDisplay cd in cardDisplays)
        {
            System.Random rng = new System.Random();
            int index = GetInt();
            cd.updateCard(cardPool[index]);
        }
    }

    public void addTreasure(CardDisplay card)
    {
        Debug.Log("Adding card...");
        SaveData data = SaveSystem.LoadGameData();

        if(card.card.cardType == "Fashion")
        {
            SteamUserStats.SetAchievement("FASHIONISTA_1");

            int fashion;

            Debug.Log("Adding fashion!!!");
            SteamUserStats.GetStat("fashion", out fashion);
            fashion++;

            if(fashion >= 5)
                SteamUserStats.SetAchievement("FASHIONISTA_2");

            SteamUserStats.SetStat("fashion", fashion);

            SteamUserStats.StoreStats();

            data.playerFashion.Add(card.card.cardName);
        } else if (card.card.cardType == "Ring")
        {
            Debug.Log("Adding ring!!!");
            data.playerRings.Add(card.card.cardName);
        }
        else
        {
            Debug.Log("Adding card!!!");
            data.treasure.Add(card.card.cardName);
        }
            

        SaveSystem.DebugCards(data);
        data.carnivalWin = false;
        SaveSystem.SaveGameData(data);
        this.gameObject.SetActive(false);
        currencyHoverBox.SetActive(true);
        healthHoverBox.SetActive(true);
        // ADD THIS BACK IN WHEN YOU WANT TO SHOW THE UI AGAIN!
        // deck.SetActive(true);
        // holster.SetActive(true);
    }
}
