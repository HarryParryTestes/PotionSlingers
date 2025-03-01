using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CardDisplay : MonoBehaviour
{
    public Card card;
    public UniqueCard uniqueCard;
    public Image artworkImage;
    public Card placeholder;

    // Artifact loaded potion slot:
    public GameObject artifactSlot;
    public CardDisplay aPotion;
    
    // Vessel loaded potion slots:
    public GameObject vesselSlot1;
    public GameObject vesselSlot2;
    public GameObject vesselSlot3;
    public GameObject vesselSlot4;
    public CardDisplay vPotion1;
    public CardDisplay vPotion2;
    public CardDisplay vPotion3;
    public CardDisplay vPotion4;
    public List<Card> crucibleCards;

    public GameObject flames;
    public bool spicy;
    public int durability = 5;

    public GameObject artifactEmptySlot;
    public GameObject vesselEmptySlot1;
    public GameObject vesselEmptySlot2;
    public GameObject vesselEmptySlot3;
    public GameObject vesselEmptySlot4;

    // Start is called before the first frame update
    void Start()
    {
        /*
        if(spicyPrefab != null)
        {
            GameObject obj = Instantiate(spicyPrefab, new Vector3(0, -25, 0), Quaternion.identity);
            Debug.Log("OBJ!");
        }
        */

        artworkImage = this.GetComponent<Image>();
        if (uniqueCard != null)
        {
            artworkImage.sprite = uniqueCard.cardSprite;
            return;
        }
        //artworkImage = this.GetComponent<Image>();
        if (card != null)
        {
            artworkImage.sprite = card.cardSprite;
            return;
        }
        
    }

    // public void OnPointerDown(PointerEventData pointerEventData)
    // {
    //     //Output the name of the GameObject that is being clicked
    //     Debug.Log("Card is clicked!");
    //     attached = !attached;
    //     if(!attached) {
    //         this.transform.position = Input.mousePosition;
    //         Hover_Card.canHover = true;
    //     }
    // }

    public string checkStatus()
    {
        if (spicy)
            return "spicy";
        else
            return "none";
    }

    public void makeSpicy()
    {
        spicy = true;
        if (flames != null)
            flames.SetActive(true);
    }

    public IEnumerator updateCrucibleCards(GameObject obj, string cardName)
    {
        // GameObject obj2;
        obj.GetComponent<DragCard>().dontMoveThis = true;

        yield return new WaitForSeconds(.10f);
        foreach (Card cd in crucibleCards)
        {
            // cd.updateCard(GameManager.manager.md2.popCard());
            Debug.Log("Animation happening");           

            // GameManager.manager.playerHolster.cardList[0].gameObject
            if (cardName == "Crucible")
            {
                GameObject obj2 = Instantiate(GameManager.manager.md1.cardDisplay2.transform.parent.gameObject,
                        GameManager.manager.md2.cardDisplay4.transform.parent.gameObject.transform.position,
                        GameManager.manager.md2.cardDisplay4.transform.parent.gameObject.transform.rotation,
                        GameManager.manager.md2.cardDisplay4.transform.parent.gameObject.transform);

                // obj2.SetActive(true);
                obj2.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
                GameManager.manager.FadeIn(obj2);
                // obj2.transform.GetChild(0).gameObject.GetComponent<CardDisplay>().updateCard(cd);
                obj2.transform.GetChild(0).gameObject.GetComponent<CardDisplay>().card = cd;
                obj2.transform.GetChild(0).gameObject.GetComponent<CardDisplay>().artworkImage.sprite = cd.cardSprite;
                obj2.transform.GetChild(0).gameObject.GetComponent<CardDisplay>().artworkImage.color = GameManager.manager.whiteColor;

                // obj2.transform.localScale = new Vector3(0.2f, 0.2f, -0.2f);
                obj2.transform.DOJump(new Vector2(obj.transform.parent.position.x, obj.transform.parent.position.y), 400f, 1, 1f, false);
                obj2.transform.DORotate(new Vector3(0, 0, 720f), 1f, RotateMode.FastBeyond360);
                yield return new WaitForSeconds(1f);
                // Might remove this to stop some UI bugs from happening, you should maybe reimplement this at some point
                // obj.transform.DOMove(new Vector2(obj.transform.parent.position.x, obj.transform.parent.position.y - 5), 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
                obj.GetComponent<CardDisplay>().updateCard(cd);
                Destroy(obj2);
            }

            if (cardName == "Spoonful of Ambrosia")
            {
                GameObject obj2 = Instantiate(GameManager.manager.md2.cardDisplay2.transform.parent.gameObject,
                        GameManager.manager.md1.cardDisplay4.transform.parent.gameObject.transform.position,
                        GameManager.manager.md1.cardDisplay4.transform.parent.gameObject.transform.rotation,
                        GameManager.manager.md1.cardDisplay4.transform.parent.gameObject.transform);

                // obj2.SetActive(true);
                obj2.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
                GameManager.manager.FadeIn(obj2);
                // obj2.transform.GetChild(0).gameObject.GetComponent<CardDisplay>().updateCard(cd);
                obj2.transform.GetChild(0).gameObject.GetComponent<CardDisplay>().card = cd;
                obj2.transform.GetChild(0).gameObject.GetComponent<CardDisplay>().artworkImage.sprite = cd.cardSprite;
                obj2.transform.GetChild(0).gameObject.GetComponent<CardDisplay>().artworkImage.color = GameManager.manager.whiteColor;

                // obj2.transform.localScale = new Vector3(0.2f, 0.2f, -0.2f);
                obj2.transform.DOJump(new Vector2(obj.transform.parent.position.x, obj.transform.parent.position.y), 400f, 1, 1f, false);
                obj2.transform.DORotate(new Vector3(0, 0, 720f), 1f, RotateMode.FastBeyond360);
                yield return new WaitForSeconds(1f);
                // Might remove this to stop some UI bugs from happening, you should maybe reimplement this at some point
                // obj.transform.DOMove(new Vector2(obj.transform.parent.position.x, obj.transform.parent.position.y - 5), 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
                obj.GetComponent<CardDisplay>().updateCard(cd);
                Destroy(obj2);
            }            
        }
        if(cardName == "Crucible")
            crucibleCards[1] = GameManager.manager.crucibleCard;
        if (cardName == "Spoonful of Ambrosia")
            crucibleCards[1] = GameManager.manager.ambrosiaCard;

        yield return new WaitForSeconds(0.1f);
        obj.GetComponent<DragCard>().dontMoveThis = false;
        /*
        // check to make sure the crucible or ambrosia was not placed on top
        if(obj.GetComponent<CardDisplay>().card.cardName == "Crucible" ||
            obj.GetComponent<CardDisplay>().card.cardName == "Spoonful of Ambrosia")
        {
            Debug.Log("Card not properly shuffled back!");
            // assume crucible card is in front and that the other cards need to be shifted up 1
            // Card temp;
            // temp = obj.GetComponent<CardDisplay>().card;
            // obj.GetComponent<CardDisplay>().card = crucibleCards[0];
            obj.GetComponent<CardDisplay>().updateCard(crucibleCards[0]);
            crucibleCards[0] = crucibleCards[1];
            if (cardName == "Crucible")
                crucibleCards[1] = GameManager.manager.crucibleCard;
            if (cardName == "Spoonful of Ambrosia")
                crucibleCards[1] = GameManager.manager.ambrosiaCard;
        }
        */
    }

    public void updatePlaceholder(CardDisplay cd)
    {
        cd.updateCard(GameManager.manager.td.card);

        if (cd.card.cardType == "Artifact")
        {
            if (cd.aPotion != null && cd.aPotion.card.cardName != "placeholder")
            {
                cd.aPotion.updateCard(GameManager.manager.td.card);
                cd.aPotion.gameObject.SetActive(false);
            }
        }

        if (cd.card.cardType == "Vessel")
        {
            cd.updateCard(GameManager.manager.td.card);
            if (cd.vPotion1 != null && cd.vPotion1.card.cardName != "placeholder")
            {
                cd.vPotion1.updateCard(GameManager.manager.td.card);
                cd.vPotion1.gameObject.SetActive(false);
                cd.vPotion2.gameObject.SetActive(false);
            }

            if (cd.vPotion2 != null && cd.vPotion2.card.cardName != "placeholder")
            {
                cd.vPotion2.updateCard(GameManager.manager.td.card);
                cd.vPotion1.gameObject.SetActive(false);
                cd.vPotion2.gameObject.SetActive(false);
            }

            if (cd.card.cardEffect == "FourLoad")
            {
                if (cd.vPotion3 != null && cd.vPotion3.card.cardName != "placeholder")
                {
                    cd.vPotion3.updateCard(GameManager.manager.td.card);
                    cd.vPotion1.gameObject.SetActive(false);
                    cd.vPotion2.gameObject.SetActive(false);
                    cd.vPotion3.gameObject.SetActive(false);
                    cd.vPotion4.gameObject.SetActive(false);
                }

                if (cd.vPotion4 != null && cd.vPotion4.card.cardName != "placeholder")
                {
                    cd.vPotion4.updateCard(GameManager.manager.td.card);
                    cd.vPotion1.gameObject.SetActive(false);
                    cd.vPotion2.gameObject.SetActive(false);
                    cd.vPotion3.gameObject.SetActive(false);
                    cd.vPotion4.gameObject.SetActive(false);
                }
            }
        }

        if (artifactEmptySlot != null)
        {
            Destroy(artifactEmptySlot);
        }

        if (vesselEmptySlot1 != null)
        {
            Destroy(vesselEmptySlot1);
        }

        if (vesselEmptySlot2 != null)
        {
            Destroy(vesselEmptySlot2);
        }

        if (vesselEmptySlot3 != null)
        {
            Destroy(vesselEmptySlot3);
        }

        if (vesselEmptySlot4 != null)
        {
            Destroy(vesselEmptySlot4);
        }
    }

    public void emptySlot(GameObject obj)
    {
        obj.GetComponent<CardDisplay>().card = GameManager.manager.emptySlotCard;
        obj.GetComponent<Image>().sprite = GameManager.manager.emptySlotCard.cardSprite;
        obj.GetComponent<Image>().DOKill();
        if(obj.GetComponent<CanvasGroup>() == null)
        {
            obj.AddComponent<CanvasGroup>();
        }
        // obj.GetComponent<CanvasGroup>().alpha = 0f;
        // obj.GetComponent<CanvasGroup>().DOFade(0, 3f).SetDelay(1, true).SetLoops(-1, LoopType.Yoyo);

        obj.GetComponent<CanvasGroup>().alpha = 0f;
        Sequence mySequence = DOTween.Sequence();
        mySequence.PrependInterval(1).Append(obj.GetComponent<CanvasGroup>().DOFade(1, 4f))
          .PrependInterval(3)
          .Append(obj.GetComponent<CanvasGroup>().DOFade(0, 3f))
          .PrependInterval(1).SetLoops(-1, LoopType.Restart);

        mySequence.Play();

    }

    public void moveCard(Card card, string status = null)
    {
        artworkImage = this.GetComponent<Image>();
        this.card = card;
        artworkImage.sprite = card.cardSprite;

        if (spicy)
        {
            if (flames != null)
                flames.SetActive(true);
        }

        else
        {
            if (flames != null)
                flames.SetActive(false);
        }

        if (status != null)
        {
            if (status == "spicy")
            {
                spicy = true;
                if (flames != null)
                    flames.SetActive(true);
            }

            if (status == "none")
            {
                spicy = false;
                if (flames != null)
                    flames.SetActive(false);
            }
        }
    }

    public void updateCard(Card card, string status = null)
    {
        if (artifactEmptySlot != null)
        {
            Destroy(artifactEmptySlot);
        }

        if (vesselEmptySlot1 != null)
        {
            Destroy(vesselEmptySlot1);
        }

        if (vesselEmptySlot2 != null)
        {
            Destroy(vesselEmptySlot2);
        }

        if (vesselEmptySlot3 != null)
        {
            Destroy(vesselEmptySlot3);
        }

        if (vesselEmptySlot4 != null)
        {
            Destroy(vesselEmptySlot4);
        }

        artworkImage = this.GetComponent<Image>();
        this.card = card;
        artworkImage.sprite = card.cardSprite;

        if (spicy)
        {
            if(flames != null)
                flames.SetActive(true);
        }

        else
        {
            if (flames != null)
                flames.SetActive(false);
        }

        if(status != null)
        {
            if(status == "spicy")
            {
                spicy = true;
                if (flames != null)
                    flames.SetActive(true);
            }

            if (status == "none")
            {
                spicy = false;
                if (flames != null)
                    flames.SetActive(false);
            }
        }
        // trying something
        // might take this out for now.
        if(gameObject.GetComponent<DragCard>() != null)
        {
            if ((card.cardName == "Crucible" || card.cardName == "Spoonful of Ambrosia") && gameObject.GetComponent<DragCard>().market)
            {
                Debug.Log("CRUCIBLE CRUCIBLE CRUCIBLE CRUCIBLE CRUCIBLE");

                if (gameObject.name == "CardDisplay (Special4)" ||
                    gameObject.name == "CardDisplay (Potion4)")
                {
                    Debug.Log("Crucible stuff occurred in fourth card");
                } else
                {

                    crucibleCards.Clear();
                    // Add two cards to display on top of market card display
                    if (card.cardName == "Crucible")
                    {
                        crucibleCards.Add(GameManager.manager.md2.popCard());
                        crucibleCards.Add(GameManager.manager.md2.popCard());
                    }

                    if (card.cardName == "Spoonful of Ambrosia")
                    {
                        crucibleCards.Add(GameManager.manager.md1.popCard());
                        crucibleCards.Add(GameManager.manager.md1.popCard());
                    }
                    // we'll see how this ends up working
                    // StartCoroutine(updateCrucibleCards(gameObject));
                    StartCoroutine(updateCrucibleCards(gameObject, card.cardName));
                }
            }
        }      

        // durability check?
        if (card.cardType == "Artifact")
        {
            durability = 5;
        }

        if (GetComponent<DragCard>() != null && !GetComponent<DragCard>().market && this.gameObject.name != "DeckPile")
        {
            /*
            if (this.card.cardType == "Artifact")
            {
                Debug.Log("Empty slot animation triggering???");

                this.artifactSlot.gameObject.SetActive(true);
                this.aPotion.gameObject.SetActive(true);

                if (artifactEmptySlot != null)
                {
                    Destroy(artifactEmptySlot);
                }
                // artifact logic
                artifactEmptySlot = Instantiate(this.artifactSlot.transform.GetChild(0).gameObject,
                    this.artifactSlot.transform.GetChild(0).position,
                    this.artifactSlot.transform.GetChild(0).rotation,
                    this.artifactSlot.transform.parent);

                // obj.GetComponent<CardDisplay>().updateCard(GameManager.manager.emptySlotCard);
                emptySlot(artifactEmptySlot);

                this.aPotion.gameObject.SetActive(false);
            }
            */

            if (this.card.cardType == "Vessel")
            {
                Debug.Log("Empty vessel slot animation triggering???");
                this.vesselSlot1.transform.parent.gameObject.SetActive(true);

                if (vesselEmptySlot1 != null)
                {
                    Destroy(vesselEmptySlot1);
                }

                if (vesselEmptySlot2 != null)
                {
                    Destroy(vesselEmptySlot2);
                }

                if (vesselEmptySlot3 != null)
                {
                    Destroy(vesselEmptySlot3);
                }

                if (vesselEmptySlot4 != null)
                {
                    Destroy(vesselEmptySlot4);
                }

                this.vPotion1.gameObject.SetActive(true);
                this.vPotion2.gameObject.SetActive(true);
                this.vPotion3.gameObject.SetActive(true);
                this.vPotion4.gameObject.SetActive(true);

                // vessel logic
                vesselEmptySlot1 = Instantiate(this.vesselSlot1.transform.GetChild(0).gameObject,
                    this.vesselSlot1.transform.GetChild(0).position,
                    this.vesselSlot1.transform.GetChild(0).rotation,
                    this.vesselSlot1.transform.parent);

                emptySlot(vesselEmptySlot1);

                vesselEmptySlot1.transform.SetAsFirstSibling();

                /*
                vesselEmptySlot2 = Instantiate(GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.loadedCardInt].vesselSlot2.transform.GetChild(0).gameObject,
                    GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.loadedCardInt].vesselSlot2.transform.GetChild(0).position,
                    GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.loadedCardInt].vesselSlot2.transform.GetChild(0).rotation,
                    GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.loadedCardInt].vesselSlot2.transform.parent);
                */

                vesselEmptySlot2 = Instantiate(this.vesselSlot2.transform.GetChild(0).gameObject,
                    this.vesselSlot2.transform.GetChild(0).position,
                    this.vesselSlot2.transform.GetChild(0).rotation,
                    this.vesselSlot2.transform.parent);

                emptySlot(vesselEmptySlot2);

                vesselEmptySlot2.transform.SetAsFirstSibling();

                if (this.card.cardEffect == "FourLoad")
                {
                    vesselEmptySlot3 = Instantiate(this.vesselSlot3.transform.GetChild(0).gameObject,
                    this.vesselSlot3.transform.GetChild(0).position,
                    this.vesselSlot3.transform.GetChild(0).rotation,
                    this.vesselSlot3.transform.parent);

                    emptySlot(vesselEmptySlot3);

                    vesselEmptySlot3.transform.SetAsFirstSibling();

                    vesselEmptySlot4 = Instantiate(this.vesselSlot4.transform.GetChild(0).gameObject,
                    this.vesselSlot4.transform.GetChild(0).position,
                    this.vesselSlot4.transform.GetChild(0).rotation,
                    this.vesselSlot4.transform.parent);

                    emptySlot(vesselEmptySlot4);

                    vesselEmptySlot4.transform.SetAsFirstSibling();
                }

                this.vPotion1.gameObject.SetActive(false);
                this.vPotion2.gameObject.SetActive(false);
                this.vPotion3.gameObject.SetActive(false);
                this.vPotion4.gameObject.SetActive(false);

            }
        }
        
        if(card.cardName == "placeholder" && this.gameObject.name != "DeckPile")
        {
            if(GetComponent<DragCard>() != null && !GetComponent<DragCard>().market && !GetComponent<DragCard>().loaded)
            {
                GetComponent<DragCard>().beforeDisappear();
            }

            spicy = false;
                
            this.gameObject.SetActive(false);

            if (GetComponent<DragCard>() != null && GetComponent<DragCard>().loaded)
            {

                

                // add empty slot animation here
                if (GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.loadedCardInt].card.cardType == "Artifact" &&
                    GameManager.manager.myPlayerIndex == 0)
                {
                    Debug.Log("Empty slot animation triggering???");

                    GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.loadedCardInt].artifactSlot.gameObject.SetActive(true);
                    GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.loadedCardInt].aPotion.gameObject.SetActive(true);

                    if(artifactEmptySlot != null)
                    {
                        Destroy(artifactEmptySlot);
                    }
                    // artifact logic
                    artifactEmptySlot = Instantiate(GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.loadedCardInt].artifactSlot.transform.GetChild(0).gameObject,
                        GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.loadedCardInt].artifactSlot.transform.GetChild(0).position,
                        GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.loadedCardInt].artifactSlot.transform.GetChild(0).rotation,
                        GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.loadedCardInt].artifactSlot.transform.parent);

                    // obj.GetComponent<CardDisplay>().updateCard(GameManager.manager.emptySlotCard);
                    emptySlot(artifactEmptySlot);

                    GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[GameManager.manager.loadedCardInt].aPotion.gameObject.SetActive(false);
                }
            }

        }
        else
        {
            if (!this.gameObject.activeInHierarchy)
            {
                if (GetComponent<DragCard>() != null &&
                !GetComponent<DragCard>().market)
                {
                    Debug.Log("Fixing resolution stuff hopefully");
                    Invoke("marketBack", 0.05f);
                    /*
                    this.gameObject.SetActive(true);
                    GetComponent<DragCard>().marketBack();
                    this.transform.position = new Vector3(GetComponent<DragCard>().originalPosition.x, GetComponent<DragCard>().originalPosition.y, 0);
                    this.transform.position = new Vector3(GetComponent<DragCard>().originalPosition.x, GetComponent<DragCard>().originalPosition.y, 0);
                    this.transform.position = new Vector3(GetComponent<DragCard>().originalPosition.x, GetComponent<DragCard>().originalPosition.y, 0);
                    
                    Debug.Log("Fixing resolution stuff hopefully");
                    this.gameObject.SetActive(true);
                    this.transform.position = new Vector3(GetComponent<DragCard>().originalPosition.x, GetComponent<DragCard>().originalPosition.y, 0);
                    transform.DOMove(GetComponent<DragCard>().originalPosition, 0.1f).SetId(gameObject.name);
                    */
                }
            }
            this.gameObject.SetActive(true);
        }
            
        if(SceneManager.GetActiveScene().name == "TownCenter")
            GameManager.manager.updateMarketDeckDisplay();
    }

    public void marketBack()
    {
        // yield return new WaitForSeconds(.10f);
        this.transform.position = new Vector3(GetComponent<DragCard>().originalPosition.x, GetComponent<DragCard>().originalPosition.y, 0);
        this.transform.position = new Vector3(GetComponent<DragCard>().originalPosition.x, GetComponent<DragCard>().originalPosition.y, 0);
        this.transform.position = new Vector3(GetComponent<DragCard>().originalPosition.x, GetComponent<DragCard>().originalPosition.y, 0);
    }

    public void fadeMarketCard()
    {
        artworkImage.DOKill();
        Color newColor = artworkImage.color;
        newColor.a = 1;
        artworkImage.color = newColor;
        //artworkImage.color.a = 1f;         
        artworkImage.DOFade(0.5f, 1f).SetLoops(-1, LoopType.Yoyo);
        // artworkImage.DOColor(GameManager.manager.fadedDryColor, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    public void fadeCard()
    {
        // image.color = MapView.Instance.lockedColor;
        artworkImage.DOKill();
        artworkImage.DOFade(0, 1.5f);
        // null check
        if(aPotion != null)
            aPotion.fadeCard();
        if(vPotion1 != null) 
            vPotion1.fadeCard();
        if (vPotion2 != null)
            vPotion2.fadeCard();
    }

    public void colorCard()
    {
        // image.color = MapView.Instance.lockedColor;
        artworkImage.DOKill();        
        artworkImage.DOColor(GameManager.manager.hotBonusColor, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void colorCardHot()
    {
        // image.color = MapView.Instance.lockedColor;
        artworkImage.DOKill();
        artworkImage.DOColor(GameManager.manager.hotBonusColor, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void colorCardCold()
    {
        // MapView.Instance.lockedColor;
        artworkImage.DOKill();
        artworkImage.DOColor(GameManager.manager.coldBonusColor, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void colorCardWet()
    {
        // image.color = MapView.Instance.lockedColor;
        artworkImage.DOKill();
        artworkImage.DOColor(GameManager.manager.wetBonusColor, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void colorCardDry()
    {
        // image.color = MapView.Instance.lockedColor;
        artworkImage.DOKill();
        artworkImage.DOColor(GameManager.manager.dryBonusColor, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void grayCard()
    {
        artworkImage.DOKill();
        artworkImage.DOColor(GameManager.manager.grayedColor, 0.5f);
    }

    public void whiteCard()
    {
        artworkImage.DOKill();
        artworkImage.DOColor(GameManager.manager.whiteColor, 0.5f);
    }

    public void updateCard(CardDisplay cd)
    {
        artworkImage = this.GetComponent<Image>();
        this.card = cd.card;
        artworkImage.sprite = card.cardSprite;
        // durability check?
        if (card.cardType == "Artifact")
        {
            cd.durability = 5;
        }
        if (cd.spicy)
        {
            spicy = true;
            if(flames != null)
                flames.SetActive(true);
        }

        else
        {
            spicy = false;
            if (flames != null)
                flames.SetActive(false);
        }
    }

    public void updateCard(UniqueCard card)
    {
        artworkImage = this.GetComponent<Image>();
        this.uniqueCard = card;
        artworkImage.sprite = uniqueCard.cardSprite;
    }

    public void updateUniqueCard(UniqueCard card)
    {
        artworkImage = this.GetComponent<Image>();
        this.uniqueCard = card;
        artworkImage.sprite = card.cardSprite;
    }

    // called when a card in deck/holster is clicked
    public void clicked()
    {
        Debug.Log(card.cardName + " has been clicked.");
    }
}
