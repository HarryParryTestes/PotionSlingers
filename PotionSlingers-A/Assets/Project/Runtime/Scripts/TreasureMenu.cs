using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addTreasure(CardDisplay card)
    {
        Debug.Log("Adding card...");
        SaveData data = SaveSystem.LoadGameData();
        data.playerDeck.Add(card.card.name);
        SaveSystem.SaveGameData(data);
    }
}
