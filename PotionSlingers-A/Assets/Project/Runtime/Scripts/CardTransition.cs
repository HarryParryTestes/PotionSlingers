using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardTransition : MonoBehaviour
{

    public GameObject card1;
    public GameObject card2;

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
        SaveData saveData = SaveSystem.LoadGameData();

        if (Game.storyMode)
        {
            if(!saveData.transition)
                this.gameObject.SetActive(false);
        } else
        {
            this.gameObject.SetActive(false);
        }
        StartCoroutine(transition());
    }

    public IEnumerator transition()
    {
        yield return new WaitForSeconds(0.5f);
        card1.transform.DOMoveX(-1000 * GameManager.manager.widthRatio, 1.5f);
        card2.transform.DOMoveX(3000 * GameManager.manager.widthRatio, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
