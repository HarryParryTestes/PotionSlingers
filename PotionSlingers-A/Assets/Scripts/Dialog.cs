using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dialog : MonoBehaviour, IPointerDownHandler
{
    public TMPro.TextMeshProUGUI dialogBox;
    public TMPro.TextMeshProUGUI directionBox;
    public GameObject nameTag;
    public GameObject directions;
    public int textBoxCounter = 0;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        textBoxCounter++;
        if(textBoxCounter == 1)
        {
            dialogBox.text = "In each game of Potion Slingers, each player starts with the\nsame starter cards!\n\nYou get two potions, a vessel, and an artifact! You also get a\nfancy ring at the top of your deck!\n\n"
                + "Try throwing a starter potion at me! Take your best shot!";
        } else if(textBoxCounter == 2)
        {
            directions.SetActive(true);
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
        else if (textBoxCounter == 4)
        {
            dialogBox.text = "Speaking of artifacts, try loading a potion into an artifact card!\n\n" +
                "Click on the other potion and click LOAD, then click on the\n" +
                "artifact card!";
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
            dialogBox.text = "Now let's try buying a few things! Take a tour of the market\n" +
                "and buy two potions!\n\nThe top row in the market is exclusively for potions,\n" +
                "and the bottom row is for vessels, artifacts, and rings!\n\n" +
                "Buy cards using your hard-earned Pips! You get 6 Pips\nat the start of your turn!";
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
            dialogBox.text = "Now let's end your turn... Click the red END TURN button\n" +
                "in the lower right corner!";
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
            dialogBox.text = "Now let's talk about vessels. Vessels require two loaded " +
                "potions\nto use and can deal huge damage with the right set of cards!\n\n" +
                "Load two potions into the starter vessel and sling\nit over here!";
        }
        else if (textBoxCounter == 17)
        {
            directions.SetActive(true);
            directionBox.text = "Load two potions into the starter\nvessel and throw it!";
            gameObject.SetActive(false);
            nameTag.SetActive(false);
        }
    }             
}
