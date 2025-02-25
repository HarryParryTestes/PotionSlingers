using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IDropHandler
{
    public CardDisplay cd;
    public bool placeholder;

    void Awake()
    {
        if (this.gameObject.GetComponent<CardDisplay>() != null)
            cd = this.gameObject.GetComponent<CardDisplay>();
    }

    public void handleEmptySlotUI(CardDisplay cd, CardDisplay cd2)
    {
        if (cd.aPotion.artifactEmptySlot != null)
        {
            Debug.Log("artifactEmptySlot exists!");
            cd.aPotion.artifactEmptySlot.SetActive(false);
            Destroy(cd.aPotion.artifactEmptySlot);
        }

        if (cd.vesselEmptySlot1 != null)
        {
            Debug.Log("vesselEmptySlot exists!");
            cd.vesselEmptySlot1.SetActive(false);
            Destroy(cd.vesselEmptySlot1);
        }

        if (cd.vesselEmptySlot2 != null)
        {
            Debug.Log("vesselEmptySlot exists!");
            cd.vesselEmptySlot2.SetActive(false);
            Destroy(cd.vesselEmptySlot2);
        }

        if (cd.vesselEmptySlot3 != null)
        {
            Debug.Log("vesselEmptySlot exists!");
            cd.vesselEmptySlot3.SetActive(false);
            Destroy(cd.vesselEmptySlot3);
        }

        if (cd.vesselEmptySlot4 != null)
        {
            Debug.Log("vesselEmptySlot exists!");
            cd.vesselEmptySlot4.SetActive(false);
            Destroy(cd.vesselEmptySlot4);
        }

        if (cd2.card.cardType == "Vessel")
        {
            Debug.Log("Empty vessel slot animation triggering???");
            cd2.vesselSlot1.transform.parent.gameObject.SetActive(true);

            if (cd2.vesselEmptySlot1 != null)
            {
                Destroy(cd2.vesselEmptySlot1);
            }

            if (cd2.vesselEmptySlot2 != null)
            {
                Destroy(cd2.vesselEmptySlot2);
            }

            if (cd2.vesselEmptySlot3 != null)
            {
                Destroy(cd2.vesselEmptySlot3);
            }

            if (cd2.vesselEmptySlot4 != null)
            {
                Destroy(cd2.vesselEmptySlot4);
            }

            cd2.vPotion1.gameObject.SetActive(true);
            cd2.vPotion2.gameObject.SetActive(true);
            cd2.vPotion3.gameObject.SetActive(true);
            cd2.vPotion4.gameObject.SetActive(true);

            // vessel logic
            cd2.vesselEmptySlot1 = Instantiate(cd2.vesselSlot1.transform.GetChild(0).gameObject,
                cd2.vesselSlot1.transform.GetChild(0).position,
                cd2.vesselSlot1.transform.GetChild(0).rotation,
                cd2.vesselSlot1.transform.parent);

            cd2.emptySlot(cd2.vesselEmptySlot1);

            cd2.vesselEmptySlot1.transform.SetAsFirstSibling();

            /*
            vesselEmptySlot2 = Instantiate(GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.loadedCardInt].vesselSlot2.transform.GetChild(0).gameObject,
                GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.loadedCardInt].vesselSlot2.transform.GetChild(0).position,
                GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.loadedCardInt].vesselSlot2.transform.GetChild(0).rotation,
                GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.loadedCardInt].vesselSlot2.transform.parent);
            */

            cd2.vesselEmptySlot2 = Instantiate(cd2.vesselSlot2.transform.GetChild(0).gameObject,
                cd2.vesselSlot2.transform.GetChild(0).position,
                cd2.vesselSlot2.transform.GetChild(0).rotation,
                cd2.vesselSlot2.transform.parent);

            cd2.emptySlot(cd2.vesselEmptySlot2);

            cd2.vesselEmptySlot2.transform.SetAsFirstSibling();

            if (cd2.card.cardEffect == "FourLoad")
            {
                cd2.vesselEmptySlot3 = Instantiate(cd2.vesselSlot3.transform.GetChild(0).gameObject,
                cd2.vesselSlot3.transform.GetChild(0).position,
                cd2.vesselSlot3.transform.GetChild(0).rotation,
                cd2.vesselSlot3.transform.parent);

                cd2.emptySlot(cd2.vesselEmptySlot3);

                cd2.vesselEmptySlot3.transform.SetAsFirstSibling();

                cd2.vesselEmptySlot4 = Instantiate(cd2.vesselSlot4.transform.GetChild(0).gameObject,
                cd2.vesselSlot4.transform.GetChild(0).position,
                cd2.vesselSlot4.transform.GetChild(0).rotation,
                cd2.vesselSlot4.transform.parent);

                cd2.emptySlot(cd2.vesselEmptySlot4);

                cd2.vesselEmptySlot4.transform.SetAsFirstSibling();
            }

            cd2.vPotion1.gameObject.SetActive(false);
            cd2.vPotion2.gameObject.SetActive(false);
            cd2.vPotion3.gameObject.SetActive(false);
            cd2.vPotion4.gameObject.SetActive(false);

        }
    }

    public void PlaceholderDrop(PointerEventData eventData)
    {
        Debug.Log("Drop occurred on CardSlot!");
        GameObject heldCard = eventData.pointerDrag;

        DragCard dc = heldCard.GetComponent<DragCard>();

        if(heldCard.name == "DeckPile")
        {
            Debug.Log("Deckpile!!!");
            return;
        }

        if (dc == null)
            return;

        if (dc.loaded || dc.market)
        {
            Debug.Log("Trying to grab loaded or market card");
            return;
        }
        // grabbing the card held by the cursor
        CardDisplay grabbedCard = heldCard.GetComponent<CardDisplay>();
        Debug.Log(grabbedCard.card.cardName);
        Debug.Log(cd.card.cardName);

        int tempDurability;

        if (grabbedCard.spicy)
        {
            cd.spicy = true;
            cd.flames.SetActive(true);

            grabbedCard.spicy = false;
            grabbedCard.flames.SetActive(false);
        }

        if(cd.card.cardName == "placeholder")
        {
            handleEmptySlotUI(grabbedCard, cd);
            cd.moveCard(grabbedCard.card);
            cd.gameObject.SetActive(true);
            cd.transform.position = Input.mousePosition;
            cd.transform.rotation = grabbedCard.transform.rotation;
            cd.transform.DORotate(cd.gameObject.GetComponent<DragCard>().cardRotation, 0.3f).SetEase(Ease.Linear).SetId(gameObject.name);

            if (grabbedCard.card.cardType == "Artifact")
            {
                tempDurability = grabbedCard.durability;
                cd.durability = tempDurability;
                grabbedCard.durability = 3;
                if (grabbedCard.aPotion != null && grabbedCard.aPotion.card.cardName != "placeholder")
                {
                    cd.aPotion.updateCard(grabbedCard.aPotion.card);
                    cd.artifactSlot.transform.parent.gameObject.SetActive(true);
                    cd.artifactSlot.SetActive(true);
                    cd.aPotion.gameObject.SetActive(true);
                    grabbedCard.aPotion.updateCard(GameManager.manager.td.card);
                    grabbedCard.aPotion.gameObject.SetActive(false);
                }
            }

            if (grabbedCard.card.cardType == "Vessel")
            {
                // cd.updateCard(GameManager.manager.td.card);
                if (grabbedCard.vPotion1 != null && grabbedCard.vPotion1.card.cardName != "placeholder")
                {
                    cd.vPotion1.updateCard(grabbedCard.vPotion1.card);
                    cd.vPotion1.gameObject.SetActive(true);
                    cd.vPotion2.gameObject.SetActive(false);
                    cd.vPotion3.gameObject.SetActive(false);
                    cd.vPotion4.gameObject.SetActive(false);
                    cd.vesselSlot1.transform.parent.gameObject.SetActive(true);
                    grabbedCard.vPotion1.updateCard(GameManager.manager.td.card);
                    grabbedCard.vPotion1.gameObject.SetActive(false);
                    grabbedCard.vPotion2.gameObject.SetActive(false);
                    grabbedCard.vPotion3.gameObject.SetActive(false);
                    grabbedCard.vPotion4.gameObject.SetActive(false);
                }

                if (grabbedCard.vPotion2 != null && grabbedCard.vPotion2.card.cardName != "placeholder")
                {
                    cd.vPotion2.updateCard(grabbedCard.vPotion2.card);
                    cd.vPotion2.gameObject.SetActive(true);
                    cd.vPotion3.gameObject.SetActive(false);
                    cd.vPotion4.gameObject.SetActive(false);
                    cd.vesselSlot1.transform.parent.gameObject.SetActive(true);
                    grabbedCard.vPotion2.updateCard(GameManager.manager.td.card);
                    grabbedCard.vPotion1.gameObject.SetActive(false);
                    grabbedCard.vPotion2.gameObject.SetActive(false);
                    grabbedCard.vPotion3.gameObject.SetActive(false);
                    grabbedCard.vPotion4.gameObject.SetActive(false);
                }

                if (cd.card.cardEffect == "FourLoad")
                {
                    if (grabbedCard.vPotion3 != null && grabbedCard.vPotion3.card.cardName != "placeholder")
                    {
                        cd.vPotion3.updateCard(grabbedCard.vPotion3.card);
                        cd.vPotion3.gameObject.SetActive(true);
                        cd.vPotion4.gameObject.SetActive(false);
                        cd.vesselSlot1.transform.parent.gameObject.SetActive(true);
                        grabbedCard.vPotion3.updateCard(GameManager.manager.td.card);
                        grabbedCard.vPotion1.gameObject.SetActive(false);
                        grabbedCard.vPotion2.gameObject.SetActive(false);
                        grabbedCard.vPotion3.gameObject.SetActive(false);
                        grabbedCard.vPotion4.gameObject.SetActive(false);
                    }

                    if (grabbedCard.vPotion4 != null && grabbedCard.vPotion4.card.cardName != "placeholder")
                    {
                        cd.vPotion4.updateCard(grabbedCard.vPotion4.card);
                        cd.vPotion4.gameObject.SetActive(true);
                        cd.vesselSlot1.transform.parent.gameObject.SetActive(true);
                        grabbedCard.vPotion4.updateCard(GameManager.manager.td.card);
                        grabbedCard.vPotion1.gameObject.SetActive(false);
                        grabbedCard.vPotion2.gameObject.SetActive(false);
                        grabbedCard.vPotion3.gameObject.SetActive(false);
                        grabbedCard.vPotion4.gameObject.SetActive(false);
                    }
                }
            }

            grabbedCard.updatePlaceholder(grabbedCard);
        }
    }

   public void OnDrop(PointerEventData eventData)
    {
        if (placeholder)
        {
            PlaceholderDrop(eventData);
            return;
        }     
        /*
        add in implementation to distinguish between GameObjects
        that have a CardDisplay vs a CharacterDisplay
        */

        GameObject heldCard = eventData.pointerDrag;

        DragCard dc = heldCard.GetComponent<DragCard>();
        // grabbing the card held by the cursor
        CardDisplay grabbedCard = heldCard.GetComponent<CardDisplay>();

        // test this and double check
        if(dc != null)
        {
            if (dc.market)
            {
                Debug.Log("Buy triggered?");
                if (heldCard.GetComponent<TopMarketBuy>() != null)
                {
                    // buy the card
                    Debug.Log("Buy");
                }
                handleBuy(dc.marketCardInt);
                return;
                // heldObject.GetComponent<CardThrow>().throwCard();
            }
        }
        

        if (heldCard.GetComponent<CharacterSlot>() != null)
        {
            Debug.Log("Trying to load deck card!!!");
            // GameManager.manager.setSCInt(grabbedCard);
            GameManager.manager.setLoadedInt(cd);
            GameManager.manager.preLoadPotion(grabbedCard);
            return;
        }

        if (cd != null)
        {
            // call the events needed to get GameManager to trigger throw and load events
            // GameManager.manager.setSCInt(grabbedCard.card.cardName);
            GameManager.manager.setSCInt(grabbedCard);
            // GameManager.manager.setLoadedInt(cd.card.cardName);
            GameManager.manager.setLoadedInt(cd);
            GameManager.manager.preLoadPotion();
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
}
