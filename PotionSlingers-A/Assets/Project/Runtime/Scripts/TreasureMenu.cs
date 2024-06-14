using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreasureMenu : MonoBehaviour
{
    public List<Card> cardPool;
    public List<CardDisplay> cardDisplays;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetInt()
    {
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
    }

    public void chooseCards()
    {
        
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
        data.playerDeck.Add(card.card.name);
        SaveSystem.SaveGameData(data);
    }
}
