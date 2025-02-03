using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Steamworks;
using Mirror;
using DG.Tweening;

public class PortraitSelector : MonoBehaviour
{

    public Sprite[] characterImages;
    public int charIndex = 0;
    public Image image;
    public TMPro.TextMeshProUGUI name;

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
        
    }

    public void selectCharNameRight()
    {
        

        charIndex += 1;
        if (charIndex > 3)
            charIndex = 0;

        if (charIndex == 1)
        {
            image.gameObject.transform.localScale = new Vector3(6f, 6f, 6f);
            image.gameObject.transform.position = new Vector3(
                image.gameObject.transform.position.x, image.gameObject.transform.position.y - 0.5f, image.gameObject.transform.position.z);
        }
        else if (charIndex == 0)
        {
            image.gameObject.transform.localScale = new Vector3(12f, 6.6f, 6.6f);
            image.gameObject.transform.position = new Vector3(
                image.gameObject.transform.position.x, image.gameObject.transform.position.y + 0.5f, image.gameObject.transform.position.z);
        }           

        Game.storyModeCharName = MainMenu.menu.demoCharacters[charIndex].cardName;
        name.text = Game.storyModeCharName;
        image.sprite = characterImages[charIndex];
    }

    public void selectCharNameLeft()
    {


        charIndex -= 1;
        if (charIndex < 0)
            charIndex = 3;

        if (charIndex == 3)
        {
            image.gameObject.transform.localScale = new Vector3(6f, 6f, 6f);
            image.gameObject.transform.position = new Vector3(
                image.gameObject.transform.position.x, image.gameObject.transform.position.y - 0.5f, image.gameObject.transform.position.z);
        }
        else if (charIndex == 0)
        {
            image.gameObject.transform.localScale = new Vector3(12f, 6.6f, 6.6f);
            image.gameObject.transform.position = new Vector3(
                image.gameObject.transform.position.x, image.gameObject.transform.position.y + 0.5f, image.gameObject.transform.position.z);
        }

        Game.storyModeCharName = MainMenu.menu.demoCharacters[charIndex].cardName;
        image.sprite = characterImages[charIndex];
    }
}
