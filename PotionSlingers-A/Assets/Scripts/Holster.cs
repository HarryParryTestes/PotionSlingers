using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Holster : MonoBehaviour, IPointerDownHandler
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

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        Debug.Log(name + "Game Object Click in Progress");
        deck.putCardOnTop(card4.card);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
