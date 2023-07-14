using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Dialog : MonoBehaviour, IPointerDownHandler
{
    public string textInfo;
    public float textSpeed = 0.03f;
    public TMPro.TextMeshProUGUI dialogBox;
    public TMPro.TextMeshProUGUI directionBox;
    public GameObject nameTag;
    public GameObject directions;
    public int textBoxCounter = 0;
    public int textIndex = 0;
    public bool scrolling = false;
    public bool endDialog = false;
    public GameObject arrow;
    public GameObject Bolotalk;
    public GameObject Boloidle;
    


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

    

    void Start()
    {
        /*
        Hello, and welcome to the world of Potion Slingers!

        This tutorial should teach the basics of all the potion-
        slinging action this game has to offer!
        */
        /*
        textBoxCounter = 0;
        textInfo = "Hello, and welcome to the world of Potion Slingers!\n\nThis tutorial should teach the basics of all the potion-\n" +
            "slinging action this game has to offer!";
        ActivateText(dialogBox);
        */
    }

    public void initDialog()
    {
        textBoxCounter = 0;
        textInfo = "Hello, and welcome to the world of Potion Slingers!\n\nThis tutorial should teach the basics of all the potion-\n" +
            "slinging action this game has to offer!";
        ActivateText(dialogBox);
        Boloidle.SetActive(false);
        Bolotalk.SetActive(true);
    }

    public void ActivateText(TMPro.TextMeshProUGUI textBox)
    {
        StartCoroutine(AnimateText(textBox));
    }

    public IEnumerator AnimateText(TMPro.TextMeshProUGUI textBox)
    {
        arrow.SetActive(false);
        scrolling = true;
        for(int i = 0; i < textInfo.Length + 1; i++)
        {
            textBox.text = textInfo.Substring(0, i);
            yield return new WaitForSeconds(textSpeed);
        }
        arrow.SetActive(true);
        Bolotalk.SetActive(false);
        Boloidle.SetActive(true);
        textSpeed = 0.03f;
        scrolling = false;
    }

    public void doThis()
    {
        arrow.SetActive(false);
        if (scrolling)
        {
            textSpeed = 0.001f;
            return;
        }

        if (endDialog)
        {
            /*
            textInfo = "In each game of Potion Slingers, each player starts with the\nsame starter cards!\n\nYou get two potions, a vessel, and an artifact! You also get a\nfancy ring at the top of your deck!\n\n"
                + "Try throwing a starter potion at me! Take your best shot!";
            ActivateText(dialogBox);
            GameManager.manager.pauseUI.SetActive(true);
            */

            // bye bye
            // Debug.Log("Ending tutorial?");
            SceneManager.LoadScene("TitleMenu");
            return;
        }

        textBoxCounter++;
        if (textBoxCounter == 1)
        {
            textInfo = "In each game of Potion Slingers, each player starts with the\nsame starter cards!\n\nYou get two potions, a vessel, and an artifact! You also get a\nfancy ring at the top of your deck!\n\n"
                + "Try throwing a starter potion at me! Take your best shot!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
            //dialogBox.text = "In each game of Potion Slingers, each player starts with the\nsame starter cards!\n\nYou get two potions, a vessel, and an artifact! You also get a\nfancy ring at the top of your deck!\n\n"
            //+ "Try throwing a starter potion at me! Take your best shot!";

        }
        else if (textBoxCounter == 2)
        {
            directions.SetActive(true);
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 4)
        {
            textInfo = "Speaking of artifacts, try loading a potion into an artifact card!\n\n" +
                "Click on the other potion and click LOAD, then click on the\n" +
                "artifact card!";

            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 5)
        {
            directions.SetActive(true);
            directionBox.text = "Click on a potion card and then click\nLOAD! " +
                "Then click on the artifact card!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 7)
        {
            directions.SetActive(true);
            directionBox.text = "Click on an artifact card and click THROW!\nThen click on an opponent!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 9)
        {
            textInfo = "Now let's try buying a few things! Take a tour of the market\n" +
                "and buy two potions!\n\nThe top row in the market is exclusively for potions,\n" +
                "and the bottom row is for vessels, artifacts, and rings!\n\n" +
                "Buy cards using your hard-earned Pips! You get 6 Pips\nat the start of your turn!";
            ActivateText(dialogBox); 
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 10)
        {
            directions.SetActive(true);
            directionBox.text = "Buy two potions from the top row of\nthe market!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 13)
        {
            textInfo = "Now let's end your turn... Click the red END TURN button\n" +
                "in the lower right corner!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 14)
        {
            directions.SetActive(true);
            directionBox.text = "Click the END TURN button!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 16)
        {
            textInfo = "Now let's talk about vessels. Vessels require two loaded " +
                "potions\nto use and can deal huge damage with the right set of cards!\n\n" +
                "Load two potions into the starter vessel and sling\nit over here!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 17)
        {
            directions.SetActive(true);
            directionBox.text = "Load two potions into the starter\nvessel and throw it!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 21)
        {
            textInfo = "Now what do we do when we're faced with junk? We get rid\nof it!\n\n" +
                "Try putting your artifact card in the trash!\n\n" +
                "Click on the artifact card and click TRASH!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 22)
        {
            directions.SetActive(true);
            directionBox.text = "Click on the artifact card\nand click TRASH!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 24)
        {
            directions.SetActive(true);
            directionBox.text = "Buy more potions from the\nmarket! Then click END TURN!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 27)
        {
            textInfo = "Some items sell for more than what you can buy them for!\n\n" +
                "Try selling an item in your holster! Click on a card\n" +
                "you want to sell and click SELL!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 28)
        {
            directions.SetActive(true);
            directionBox.text = "Click on a card in your holster\n" +
                "and click SELL!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 30)
        {
            directions.SetActive(true);
            directionBox.text = "Click on a card in your holster\n" +
                "and click CYCLE!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 32)
        {
            textInfo = "Usually flipping your character requires certain conditions\n" +
                "to be met, but just this once, I'll let you flip for free!\n\n" +
                "Try flipping your character! Click on the character card\n" +
                "and click FLIP!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 33)
        {
            directions.SetActive(true);
            directionBox.text = "Click on the character card\n" +
                "and click FLIP!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 35)
        {
            directions.SetActive(true);
            directionBox.text = "Click on the character card\n" +
                "and click ACTION!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 37)
        {
            textInfo = "The object of the game is to beat your opponents down\n" +
                "to 0 health and take all of their Essence Cubes!\n\n" +
                "The last person standing is declared the winner!";
            ActivateText(dialogBox); 
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 38)
        {
            textInfo = "Once a player's health is reduced to 0, one of their Essence\n" +
                "Cubes will be awarded to the player that deals the killing blow!\n\n" +
                "Try finishing me off and getting me to 0 health!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 39)
        {
            directions.SetActive(true);
            directionBox.text = "Use cards to reduce Bolo's\n" +
                "health down to 0!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
    }

    public void endTutorialDialog()
    {
        endDialog = true;
        Game.completedTutorial = true;
        Game.completedGame = true;
        this.gameObject.SetActive(true);
        textInfo = "Congratulations! You have completed the tutorial!\n" +
            "What do we do after this?";
        ActivateText(dialogBox);
        Boloidle.SetActive(false);
        Bolotalk.SetActive(true);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        arrow.SetActive(false);
        Boloidle.SetActive(false);
        Bolotalk.SetActive(true);
        if (scrolling)
        {
            textSpeed = 0.001f;
            return;
        }
        textBoxCounter++;


        if (endDialog)
        {
            /*
            textInfo = "In each game of Potion Slingers, each player starts with the\nsame starter cards!\n\nYou get two potions, a vessel, and an artifact! You also get a\nfancy ring at the top of your deck!\n\n"
                + "Try throwing a starter potion at me! Take your best shot!";
            ActivateText(dialogBox);
            GameManager.manager.pauseUI.SetActive(true);
            */

            // bye bye
            // Debug.Log("Ending tutorial?");
            SceneManager.LoadScene("TitleMenu");
            return;
        }


        if(textBoxCounter == 1)
        {
            textInfo = "In each game of Potion Slingers, each player starts with the\nsame starter cards!\n\nYou get two potions, a vessel, and an artifact! You also get a\nfancy ring at the top of your deck!\n\n"
                + "Try throwing a starter potion at me! Take your best shot!";
            ActivateText(dialogBox);
            //dialogBox.text = "In each game of Potion Slingers, each player starts with the\nsame starter cards!\n\nYou get two potions, a vessel, and an artifact! You also get a\nfancy ring at the top of your deck!\n\n"
                //+ "Try throwing a starter potion at me! Take your best shot!";
                
        }
        else if(textBoxCounter == 2)
        {
            directions.SetActive(true);
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 4)
        {
            textInfo = "Speaking of artifacts, try loading a potion into an artifact card!\n\n" +
                "Click on the other potion and click LOAD, then click on the\n" +
                "artifact card!";

            ActivateText(dialogBox);
        }
        else if (textBoxCounter == 5)
        {
            directions.SetActive(true);
            directionBox.text = "Click on a potion card and then click\nLOAD! " +
                "Then click on the artifact card!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 7)
        {
            directions.SetActive(true);
            directionBox.text = "Click on an artifact card and click THROW!\nThen click on an opponent!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 9)
        {
            textInfo = "Now let's try buying a few things! Take a tour of the market\n" +
                "and buy two potions!\n\nThe top row in the market is exclusively for potions,\n" +
                "and the bottom row is for vessels, artifacts, and rings!\n\n" +
                "Buy cards using your hard-earned Pips! You get 6 Pips\nat the start of your turn!";
            ActivateText(dialogBox);
        }
        else if (textBoxCounter == 10)
        {
            directions.SetActive(true);
            directionBox.text = "Buy two potions from the top row of\nthe market!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 13)
        {
            textInfo = "Now let's end your turn... Click the red END TURN button\n" +
                "in the lower right corner!";
            ActivateText(dialogBox);
        }
        else if (textBoxCounter == 14)
        {
            directions.SetActive(true);
            directionBox.text = "Click the END TURN button!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 16)
        {
            textInfo = "Now let's talk about vessels. Vessels require two loaded " +
                "potions\nto use and can deal huge damage with the right set of cards!\n\n" +
                "Load two potions into the starter vessel and sling\nit over here!";
            ActivateText(dialogBox);
        }
        else if (textBoxCounter == 17)
        {
            directions.SetActive(true);
            directionBox.text = "Load two potions into the starter\nvessel and throw it!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 21)
        {
            textInfo = "Now what do we do when we're faced with junk? We get rid\nof it!\n\n" +
                "Try putting your artifact card in the trash!\n\n" +
                "Click on the artifact card and click TRASH!";
            ActivateText(dialogBox);
        }
        else if (textBoxCounter == 22)
        {
            directions.SetActive(true);
            directionBox.text = "Click on the artifact card\nand click TRASH!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 24)
        {
            directions.SetActive(true);
            directionBox.text = "Buy more potions from the\nmarket! Then click END TURN!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 27)
        {
            textInfo = "Some items sell for more than what you can buy them for!\n\n" +
                "Try selling an item in your holster! Click on a card\n" +
                "you want to sell and click SELL!";
            ActivateText(dialogBox);
        }
        else if (textBoxCounter == 28)
        {
            directions.SetActive(true);
            directionBox.text = "Click on a card in your holster\n" +
                "and click SELL!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 30)
        {
            directions.SetActive(true);
            directionBox.text = "Click on a card in your holster\n" +
                "and click CYCLE!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 32)
        {
            textInfo = "Usually flipping your character requires certain conditions\n" +
                "to be met, but just this once, I'll let you flip for free!\n\n" +
                "Try flipping your character! Click on the character card\n" +
                "and click FLIP!";
            ActivateText(dialogBox);
        }
        else if (textBoxCounter == 33)
        {
            directions.SetActive(true);
            directionBox.text = "Click on the character card\n" +
                "and click FLIP!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 35)
        {
            directions.SetActive(true);
            directionBox.text = "Click on the character card\n" +
                "and click ACTION!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 37)
        {
            textInfo = "The object of the game is to beat your opponents down\n" +
                "to 0 health and take all of their Essence Cubes!\n\n" +
                "The last person standing is declared the winner!";
            ActivateText(dialogBox);
        }
        else if (textBoxCounter == 38)
        {
            textInfo = "Once a player's health is reduced to 0, one of their Essence\n" +
                "Cubes will be awarded to the player that deals the killing blow!\n\n" +
                "Try finishing me off and getting me to 0 health!";
            ActivateText(dialogBox);
        }
        else if (textBoxCounter == 39)
        {
            directions.SetActive(true);
            directionBox.text = "Use cards to reduce Bolo's\n" +
                "health down to 0!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
    }             
}