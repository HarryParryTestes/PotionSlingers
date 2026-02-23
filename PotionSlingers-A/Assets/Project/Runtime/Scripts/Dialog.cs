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
        GameManager.manager.tutorialArrow.SetActive(false);
        GameManager.manager.tutorialArrow2.SetActive(false);

        textBoxCounter = 0;
        textInfo = "Hey it's me, tutorial girl! Welcome to the world of Potion Slingers!\n\nThis tutorial should teach the basics of all the potion-slinging " +
            "action this game has to offer!";
        ActivateText(dialogBox);
        Boloidle.SetActive(false);
        Bolotalk.SetActive(true);
    }

    public void initEndDemoDialog()
    {
        textBoxCounter = 45;
        textInfo = "Good Job, You won! This marks the end of the demo currently.\n\nWishlist us on Steam, join the Discord, and follow " +
            "us on social\nmedia for development updates and an upcoming Beta Test!\n\nThank you so much for playing!!!";
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
            textInfo = "Let's not waste too much time doing this... even though it's MY favorite thing.";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);

        }
        else if (textBoxCounter == 2)
        {
            textInfo = "We need to get you to learn how to play as quickly as you can. I promise you'll get the hang of it!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);       
        }
        else if (textBoxCounter == 3)
        {
            textInfo = "Let's start with the bread and butter, throwing potions! There’s one right here in your Holster!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 4)
        {
            textInfo = "Now drag it across the screen towards the enemy over there to throw it! ";
                // "artifact card!";

            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 5)
        {
            directions.SetActive(true);
            gameObject.SetActive(false);
            nameTag.SetActive(false);
            // SetActive the arrow here
            // GameManager.manager.tutorialArrow.SetActive(true);
            // GameManager.manager.tutorialArrow2.SetActive(true);
            GameManager.manager.potionDragAnimation();
            /*
            directions.SetActive(true);
            directionBox.text = "Drag a potion card onto the artifact card to load it!";
                // "Then click on the artifact card!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
            GameManager.manager.tutorialArrow.GetComponent<ArrowMover>().checkArrow();
            GameManager.manager.tutorialArrow.SetActive(true);
            */
        }
        else if (textBoxCounter == 7)
        {
            textInfo = "Since your Holster is empty, you’ll need to buy more potions now. Let’s open the market!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 8)
        {
            textInfo = "Every turn you can buy items from the market with Pips, and you get 6P every turn!\n\nThey go away though, so you gotta use em or lose em!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 9)
        {
            textInfo = "Try buying a couple potions now!";
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
            textInfo = "You don’t get them immediately, but at the start of your NEXT turn, the cards will go into the empty slots of your Holster until they are full!\n\nMake sure to make room for them!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 14)
        {
            textInfo = "Ok, next let’s buy a VESSEL from the bottom row of the market, and I’ll explain once it’s in your Holster!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 15)
        {
            /*
            textInfo = "Now let's talk about vessels. Vessels require two loaded " +
                "potions to use and can deal huge damage with the right set of cards!\n\n" +
                "Load two potions into the starter vessel and sling it over here!";
            */
            /*
            textInfo = "Ok, next let’s buy a VESSEL from the bottom row of the market, and I’ll explain once it’s in your Holster!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
            */
            directions.SetActive(true);
            directionBox.text = "Buy a vessel from the bottom row of the market!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 16)
        {
            directions.SetActive(true);
            directionBox.text = "Buy a vessel from the bottom row of the market!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 17)
        {
            directions.SetActive(true);
            directionBox.text = "Click the PASS button to end your turn!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 18)
        {
            GameManager.manager.tutorialHealth.SetActive(true);
            GameManager.manager.FadeIn(GameManager.manager.tutorialHealth);
            textInfo = "This is where your HEALTH is!\n\nEnemies will want to hit you so be careful not to lose it all!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 19)
        {
            GameManager.manager.tutorialHealth.SetActive(false);
            GameManager.manager.tutorialEssence.SetActive(true);
            GameManager.manager.FadeIn(GameManager.manager.tutorialEssence);
            textInfo = "These are your Essence Cubes! If your health is empty you’ll use one of these automatically to refill it. If you lose all your Essence Cubes, who knows what’ll happen?";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 20)
        {
            GameManager.manager.tutorialEssence.SetActive(false);
            gameObject.SetActive(false);
            GameManager.manager.StartCoroutine(GameManager.manager.HolsterFill(GameManager.manager.cardPlayer));
            /*
            textInfo = "Ok, your turn again! At the start of your turn your Holster will reload with the cards on top of your deck!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
            */
        }
        else if (textBoxCounter == 21)
        {
            textInfo = "Ok, your turn again! At the start of your turn your Holster will reload with the cards on top of your deck!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 22)
        {
            textInfo = "Now let's try using that vessel you bought. We can LOAD two potions into it and combine them together! Try dragging the potion cards on top of the vessel card!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 23)
        {
            directions.SetActive(true);
            directionBox.text = "Load both potions into the vessel by dragging the cards on top of it!";
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
        else if (textBoxCounter == 26)
        {
            directions.SetActive(true);
            directionBox.text = "Drag the vessel onto the enemy to throw it!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 27)
        {
            textInfo = "Not only does it do big damage, but the loaded potions get cycled back into the bottom of your deck!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 28)
        {
            textInfo = "Ok, your Holster is empty again, what does that mean?";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 29)
        {
            textInfo = "BUYING! Remember to visit the market every turn and use your Pips so you can always be on the attack!\n\nThis time try buying a potion and an artifact!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 30)
        {
            directions.SetActive(true);
            directionBox.text = "Buy a potion and an artifact from the market!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 32)
        {
            textInfo = "Usually flipping your character requires certain conditions" +
                " to be met, but just this once, I'll let you flip for free!\n\n" +
                "Try flipping your character! Click on the character card" +
                " and click FLIP!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 33)
        {
            directions.SetActive(true);
            directionBox.text = "Click the PASS button to end your turn!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 34)
        {
            textInfo = "Ok now that your ARTIFACT is available, you can fire it! Load ONE POTION in and fire away!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 35)
        {
            // try this
            GameManager.manager.playerHolster.card3.updateCard(GameManager.manager.playerDeck.placeholder);
            GameManager.manager.playerHolster.card4.updateCard(GameManager.manager.playerDeck.placeholder);
            directions.SetActive(true);
            directionBox.text = "Drag a potion card onto the artifact card to load it!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
            GameManager.manager.artifactDragAnimation();
        }
        else if (textBoxCounter == 37)
        {
            directions.SetActive(true);
            directionBox.text = "Drag the artifact card onto the enemy to use it!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 39)
        {
            GameManager.manager.daggerheels.SetActive(true);
            GameManager.manager.FadeIn(GameManager.manager.daggerheels);
            textInfo = "One more thing, see the bonus on this particular artifact? That means if you load a particular QUALITY of potion into it, you’ll trigger a bonus!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 40)
        {
            GameManager.manager.playerHolster.card3.updateCard(GameManager.manager.database.cardList[74]);
            textInfo = "This time, try loading the COLD quality potion into the artifact!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 41)
        {
            GameManager.manager.daggerheels.SetActive(false);
            directions.SetActive(true);
            directionBox.text = "Drag the cold potion onto the artifact to load it!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
            GameManager.manager.artifactDragAnimation();
        }
        else if (textBoxCounter == 43)
        {
            directions.SetActive(true);
            directionBox.text = "Drag the artifact card onto the enemy to use it!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 45)
        {
            textInfo = "Now say you need to make space in your Holster, or don’t have enough money for a shiny new artifact in the market. You can SELL cards in your Holster by dragging them to the SELL ICON on the right!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 46)
        {
            textInfo = "Try dragging your artifact card onto the sell icon to SELL it!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 47)
        {
            directions.SetActive(true);
            directionBox.text = "Drag your artifact card onto the sell icon to sell it!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 49)
        {
            GameManager.manager.playerHolster.card3.updateCard(GameManager.manager.database.cardList[12]);
            textInfo = "Try dragging a card to the trash!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 50)
        {
            directions.SetActive(true);
            directionBox.text = "Drag a card onto the trash can to trash it!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 52)
        {
            GameManager.manager.playerHolster.card3.updateCard(GameManager.manager.database.cardList[55]);
            textInfo = "Try cycling a card to your deck!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 53)
        {
            directions.SetActive(true);
            directionBox.text = "Drag a card onto your deck to cycle it!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 55)
        {
            GameManager.manager.playerDeck.putCardOnBottom(GameManager.manager.database.cardList[38]);
            GameManager.manager.playerDeck.putCardOnBottom(GameManager.manager.database.cardList[50]);
            GameManager.manager.playerDeck.putCardOnBottom(GameManager.manager.database.cardList[77]);
            textInfo = "C'mon, try it out! Click on your character portrait to use it!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 56)
        {
            directions.SetActive(true);
            directionBox.text = "Click on your character portrait to unleash your ultimate attack!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 58)
        {
            textInfo = "Last but not least, your BACKPACK is up here! It will store special gear you can pick up that will give you permanent buffs throughout your run!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 59)
        {
            textInfo = "A seasoned slinger should always optimize their builds depending on the situations they find themselves in!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 60)
        {
            textInfo = "I think that's it! You are on your own now!\n\nThey grow up so fast! Come visit me some time on the main menu, yeah?";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 61)
        {
            SceneManager.LoadScene("TitleMenu");
        }
        else if (textBoxCounter == 62)
        {
            SceneManager.LoadScene("TitleMenu");
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

        doThis();
        return;
        textBoxCounter++;


        

        if (textBoxCounter == 1)
        {
            textInfo = "Let's not waste too much time doing this... even though it's MY favorite thing.";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);

        }
        else if (textBoxCounter == 2)
        {
            textInfo = "We need to get you to learn how to play as quickly as you can. I promise you'll get the hang of it!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 3)
        {
            textInfo = "Let's start with the bread and butter, throwing potions! There’s one right here in your Holster!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 4)
        {
            textInfo = "Now drag it across the screen towards the enemy over there to throw it! ";
            // "artifact card!";

            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 5)
        {
            directions.SetActive(true);
            gameObject.SetActive(false);
            nameTag.SetActive(false);
            // SetActive the arrow here
            // GameManager.manager.tutorialArrow.SetActive(true);
            // GameManager.manager.tutorialArrow2.SetActive(true);
            GameManager.manager.potionDragAnimation();
            /*
            directions.SetActive(true);
            directionBox.text = "Drag a potion card onto the artifact card to load it!";
                // "Then click on the artifact card!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
            GameManager.manager.tutorialArrow.GetComponent<ArrowMover>().checkArrow();
            GameManager.manager.tutorialArrow.SetActive(true);
            */
        }
        else if (textBoxCounter == 7)
        {
            textInfo = "Since your Holster is empty, you’ll need to buy more potions now. Let’s open the market!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 8)
        {
            textInfo = "Every turn you can buy items from the market with Pips, and you get 6P every turn!\n\nThey go away though, so you gotta use em or lose em!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 9)
        {
            textInfo = "Try buying a couple potions now!";
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
            textInfo = "Good! See how they went to the top of your deck?";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 14)
        {
            textInfo = "You don’t get them immediately, but at the start of your NEXT turn, the cards will go into the empty slots of your Holster until they are full!\n\nMake sure to make room for them!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 15)
        {
            /*
            textInfo = "Now let's talk about vessels. Vessels require two loaded " +
                "potions to use and can deal huge damage with the right set of cards!\n\n" +
                "Load two potions into the starter vessel and sling it over here!";
            */
            textInfo = "Ok, next let’s buy a VESSEL from the bottom row of the market, and I’ll explain once it’s in your Holster!";
            ActivateText(dialogBox);
            Boloidle.SetActive(false);
            Bolotalk.SetActive(true);
        }
        else if (textBoxCounter == 17)
        {
            directions.SetActive(true);
            directionBox.text = "Buy a vessel from the bottom row of the market!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }

        /*
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
                "to the\ncoin icon to sell it!";
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
                " to be met, but just this once, I'll let you flip for free!\n\n" +
                "Try flipping your character! Click on the character card\n" +
                " and click FLIP!";
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
        else if (textBoxCounter == 46)
        {
            SceneManager.LoadScene("TitleMenu");
        }
        else if (textBoxCounter == 47)
        {
            SceneManager.LoadScene("TitleMenu");
        }
        */
    }             
}
