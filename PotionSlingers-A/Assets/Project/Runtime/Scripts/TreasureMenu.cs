using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreasureMenu : MonoBehaviour
{
    public List<Card> cardPool;
    public List<Card> hatPool;
    public List<CardDisplay> cardDisplays;
    public TMPro.TextMeshProUGUI description;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        /*
        System.Random rng = new System.Random();
        int i = rng.Next(cardPool.Count);
        if (cardPool[i].name == cardDisplays[0].card.name)
        {
            Debug.Log("Duplicate card!");
            return GetInt();
        }
            
        if (cardPool[i].name == cardDisplays[1].card.name)
        {
            Debug.Log("Duplicate card!");
            return GetInt();
        }
        if (cardPool[i].name == cardDisplays[2].card.name)
        {
            Debug.Log("Duplicate card!");
            return GetInt();
        }
        return i;
        */

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
            Debug.Log("Adding fashion!!!");
            data.playerFashion.Add(card.card.name);
        } else
            data.playerDeck.Add(card.card.name);
        data.carnivalWin = false;
        SaveSystem.SaveGameData(data);
    }
}
