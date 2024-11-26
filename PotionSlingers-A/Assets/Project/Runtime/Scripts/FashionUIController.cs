using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FashionUIController : MonoBehaviour
{

    public List<FashionUI> fashionList;
    public MyNetworkManager game;
    public MyNetworkManager Game
    {
        get
        {
            if (game != null)
            {
                return game;
            }
            return game = MyNetworkManager.singleton as MyNetworkManager;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Game.storyMode)
        {
            SaveData saveData = SaveSystem.LoadGameData();

            for(int i = 0; i < saveData.playerFashion.Count; i++)
            {
                foreach (Card card in GameManager.manager.database.cardList)
                {
                    if (card.cardName == saveData.playerFashion[i])
                    {
                        Debug.Log("Fashion found!");
                        fashionList[i].updateCard(card);
                        break;
                    }
                }
            }

        }
        else
        {
            gameObject.SetActive(false);
        }       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
