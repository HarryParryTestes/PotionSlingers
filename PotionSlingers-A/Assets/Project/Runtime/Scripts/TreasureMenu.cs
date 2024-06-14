using System.Collections;
using System.Collections.Generic;
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

    public void chooseCards()
    {
        foreach(CardDisplay cd in cardDisplays)
        {
            System.Random rng = new System.Random();
            int index = rng.Next(cardPool.Count);
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
