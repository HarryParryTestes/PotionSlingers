using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class Holster : MonoBehaviour, IPointerDownHandler, IDropHandler
{
    public List<CardDisplay> cardList;
    public Deck deck;
    public CardDisplay card1;
    public CardDisplay card2;
    public CardDisplay card3;
    public CardDisplay card4;
    // Start is called before the first frame update
    void Start()
    {
        cardList.Add(card1);
        cardList.Add(card2);
        cardList.Add(card3);
        cardList.Add(card4);
    }

    public void OnDrop(PointerEventData eventData)
    {
        // copy CharacterSlot code lol
        /*
       add in implementation to distinguish between GameObjects
       that have a CardDisplay vs a CharacterDisplay
       */
        Debug.Log("Drop happened on character");

        GameObject heldCard = eventData.pointerDrag;
        DragCard dc = heldCard.GetComponent<DragCard>();
        // grabbing the card held by the cursor
        CardDisplay grabbedCard = heldCard.GetComponent<CardDisplay>();

        // test this and double check
        // dc.market && this.gameObject.name == "DeckPile"
        if (dc.market)
        {
            Debug.Log("Buy triggered?");
            if (heldCard.GetComponent<TopMarketBuy>() != null)
            {
                // buy the card
                Debug.Log("Buy");
            }
            handleBuy(dc.marketCardInt);
            // heldObject.GetComponent<CardThrow>().throwCard();
        }
    }

    public void handleBuy(int cardInt)
    {
        if (cardInt == 1 || cardInt == 2 || cardInt == 3)
        {
            GameManager.manager.md1.cardInt = cardInt;
            GameManager.manager.topMarketBuy();
        }

        if (cardInt == 4 || cardInt == 5 || cardInt == 6)
        {
            GameManager.manager.md2.cardInt = cardInt - 3;
            GameManager.manager.bottomMarketBuy();
        }
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        Debug.Log(name + "Game Object Click in Progress");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
