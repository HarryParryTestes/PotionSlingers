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
        textInfo = "Hello, and welcome to the world of Potion Slingers!\n\nThis tutorial should teach the basics of all the potion-slinging\n" +
            "action this game has to offer!";
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
            textInfo = "In each game of Potion Slingers, each player starts with the same starter cards!\n\nYou get two potions, a vessel, and an artifact! You also get a fancy ring at the top of your deck!\n\n"
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
            // SetActive the arrow here
            GameManager.manager.tutorialArrow.SetActive(true);
            GameManager.manager.tutorialArrow2.SetActive(true);
        }
        else if (textBoxCounter == 4)
        {
            textInfo = "Speaking of artifacts, try loading a potion into an artifact card!\n\n" +
                "Try dragging a potion card onto that artifact card!";
                // "artifact card!";

            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 5)
        {
            directions.SetActive(true);
            directionBox.text = "Drag a potion card onto the artifact card to load it!";
                // "Then click on the artifact card!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
            GameManager.manager.tutorialArrow.GetComponent<ArrowMover>().checkArrow();
            GameManager.manager.tutorialArrow.SetActive(true);
            
        }
        else if (textBoxCounter == 7)
        {
            directions.SetActive(true);
            directionBox.text = "Drag the artifact card onto your opponent to use it on them!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 9)
        {
            textInfo = "Let's try buying a few things! Click on the market " +
                "button to open up the market and buy two potions!\n\nThe top row in the market is exclusively for potions, " +
                "and the bottom row is for vessels, artifacts, and rings!\n\n" +
                "Buy cards using your hard-earned Pips! You get 6 Pips at the start of your turn!";
            ActivateText(dialogBox); 
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 10)
        {
            directions.SetActive(true);
            directionBox.text = "Buy two potions from the top row of the market!\n\nDrag a card from the market onto your deck to buy them!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
            GameManager.manager.tutorialArrow.GetComponent<ArrowMover>().checkArrow();
            GameManager.manager.tutorialArrow.SetActive(true);
        }
        else if (textBoxCounter == 13)
        {
            textInfo = "Now let's end your turn... Click that PASS button " +
                "in the lower right corner!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 14)
        {
            directions.SetActive(true);
            directionBox.text = "Click the PASS button!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
            GameManager.manager.tutorialArrow.GetComponent<ArrowMover>().checkArrow();
            GameManager.manager.tutorialArrow.SetActive(true);
        }
        else if (textBoxCounter == 16)
        {
            textInfo = "Now let's talk about vessels. Vessels require two loaded " +
                "potions to use and can deal huge damage with the right set of cards!\n\n" +
                "Load two potions into the starter vessel and sling it over here!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 17)
        {
            directions.SetActive(true);
            directionBox.text = "Load two potions into the starter vessel and throw it!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 21)
        {
            textInfo = "Now what do we do when we're faced with junk? We get rid of it!\n\n" +
                "Try dragging your artifact card into the trash!\n\n" +
                "It's just over there on the right!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 22)
        {
            directions.SetActive(true);
            directionBox.text = "Drag the artifact card into the trash can on the right!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 24)
        {
            directions.SetActive(true);
            directionBox.text = "Buy more potions from the market! Then click PASS!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 27)
        {
            textInfo = "Some items sell for more than what you can buy them for!\n\n" +
                "Try selling an item in your holster! Drag a card " +
                "you want to sell onto the coin icon to sell it!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 28)
        {
            directions.SetActive(true);
            directionBox.text = "Drag a card from your holster " +
                "to the coin icon to sell it!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 30)
        {
            directions.SetActive(true);
            directionBox.text = "Drag a card from your holster " +
                "to your deck to cycle it!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 32)
        {
            textInfo = "Usually flipping your character requires certain conditions" +
                "to be met, but just this once, I'll let you flip for free!\n\n" +
                "Try flipping your character! Click on the character card" +
                "and click FLIP!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 33)
        {
            directions.SetActive(true);
            directionBox.text = "Click on the character card and click FLIP!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 35)
        {
            directions.SetActive(true);
            directionBox.text = "Click on the character card and click ACTION!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 37)
        {
            textInfo = "The object of the game is to beat your opponents down" +
                "to 0 health and take all of their Essence Cubes!\n\n" +
                "The last person standing is declared the winner!";
            ActivateText(dialogBox); 
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 38)
        {
            textInfo = "Once a player's health is reduced to 0, one of their Essence " +
                "Cubes will be awarded to the player that deals the killing blow!\n\n" +
                "Try finishing me off and getting me to 0 health!";
            ActivateText(dialogBox);
            // setting Bolo back to 10 health
            GameManager.manager.tempPlayer.hp = 10;
            GameManager.manager.tempPlayer.updateHealthUI();
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 39)
        {
            directions.SetActive(true);
            directionBox.text = "Use cards to reduce Bolo's health down to 0!";
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
        textInfo = "Congratulations! You have completed the tutorial!\n\n" +
            "Try playing a round of Potion Slingers with some of the CPU characters!";
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
                "Drag a potion card onto the artifact card to load it!";
                //"artifact card!";

            ActivateText(dialogBox);
        }
        else if (textBoxCounter == 5)
        {
            directions.SetActive(true);
            directionBox.text = "Drag a potion card onto the artifact card to load it!";
                //"Then click on the artifact card!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
            GameManager.manager.tutorialArrow.GetComponent<ArrowMover>().checkArrow();
            GameManager.manager.tutorialArrow.SetActive(true);
        }
        else if (textBoxCounter == 7)
        {
            directions.SetActive(true);
            directionBox.text = "Drag the artifact card onto your opponent to use it on them!";
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
            directionBox.text = "Buy two potions from the top row of the market!\n\nDrag a card from the market onto your deck to buy them!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
            GameManager.manager.tutorialArrow.GetComponent<ArrowMover>().checkArrow();
            GameManager.manager.tutorialArrow.SetActive(true);
        }
        else if (textBoxCounter == 13)
        {
            textInfo = "Now let's end your turn... Click the PASS button in the lower\n" +
                "right corner!";
            ActivateText(dialogBox);
        }
        else if (textBoxCounter == 14)
        {
            directions.SetActive(true);
            directionBox.text = "Click the PASS button!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
            GameManager.manager.tutorialArrow.GetComponent<ArrowMover>().checkArrow();
            GameManager.manager.tutorialArrow.SetActive(true);
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
                "Try dragging your artifact card into the trash!\n\n" +
                "It's just over there on the right!";
            ActivateText(dialogBox);
        }
        else if (textBoxCounter == 22)
        {
            directions.SetActive(true);
            directionBox.text = "Drag the artifact card over\nto the trash can!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 24)
        {
            directions.SetActive(true);
            directionBox.text = "Buy more potions from the\nmarket! Then click PASS!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 27)
        {
            textInfo = "Some items sell for more than what you can buy them for!\n\n" +
                "Try selling an item in your holster! Drag a card you want to sell onto the coin icon on the right to sell it!";
            ActivateText(dialogBox);
        }
        else if (textBoxCounter == 28)
        {
            directions.SetActive(true);
            directionBox.text = "Drag a card from your holster " +
                "to the\nmarket button to sell it!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 30)
        {
            directions.SetActive(true);
            directionBox.text = "Drag a card from your holster\n" +
                "to your deck to cycle it!";
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
            directionBox.text = "Click on the character card and click\n" +
                "FLIP!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 35)
        {
            directions.SetActive(true);
            directionBox.text = "Click on the character card and click\n" +
                "ACTION!";
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
            textInfo = "Once a player's health is reduced to 0, one of their Essence " +
                "Cubes will be awarded to the player that deals the killing blow!\n\n" +
                "Try finishing me off and getting me to 0 health!";
            ActivateText(dialogBox);
        }
        else if (textBoxCounter == 39)
        {
            directions.SetActive(true);
            directionBox.text = "Use cards to reduce Bolo's " +
                "health down to 0!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
    }             
}
