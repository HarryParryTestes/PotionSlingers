using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnButtonManager : MonoBehaviour
{
    public Image image;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Game.storyMode)
        {
            if (GameManager.manager.myPlayerIndex != 0)
            {
                // this.gameObject.SetActive(false);
                image.CrossFadeAlpha(0.25f, 0.5f, true);
                this.GetComponent<CanvasGroup>().interactable = false;
            } else
            {
                // this.gameObject.SetActive(true);
                image.CrossFadeAlpha(1, 0.5f, true);
                this.GetComponent<CanvasGroup>().interactable = true;
            }
            
        } 
        */

        if (GameManager.manager.myPlayerIndex != 0)
        {
            // this.gameObject.SetActive(false);
            image.CrossFadeAlpha(0.25f, 0.5f, true);
            this.GetComponent<CanvasGroup>().interactable = false;
        }
        else
        {
            // this.gameObject.SetActive(true);
            image.CrossFadeAlpha(1, 0.5f, true);
            this.GetComponent<CanvasGroup>().interactable = true;
        }
    }
}
