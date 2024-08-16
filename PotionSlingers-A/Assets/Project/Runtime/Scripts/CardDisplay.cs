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
    public int durability = 3;

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

    public void makeSpicy()
    {
        card.spicy = true;
        if (flames != null)
            flames.SetActive(true);
    }

    public IEnumerator updateCrucibleCards(GameObject obj)
    {
        yield return new WaitForSeconds(.10f);
        foreach (Card cd in crucibleCards)
        {
            // cd.updateCard(GameManager.manager.md2.popCard());
            Debug.Log("Animation happening");

            // GameManager.manager.playerHolster.cardList[0].gameObject
            GameObject obj2 = Instantiate(GameManager.manager.md1.cardDisplay2.transform.parent.gameObject,
                        GameManager.manager.md2.cardDisplay4.transform.parent.gameObject.transform.position,
                        GameManager.manager.md2.cardDisplay4.transform.parent.gameObject.transform.rotation,
                        GameManager.manager.md2.cardDisplay4.transform.parent.gameObject.transform);
            // obj2.SetActive(true);
            obj2.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
            GameManager.manager.FadeIn(obj2);
            obj2.transform.GetChild(0).gameObject.GetComponent<CardDisplay>().updateCard(cd);
            obj2.transform.GetChild(0).gameObject.GetComponent<CardDisplay>().artworkImage.color = GameManager.manager.whiteColor;

            // obj2.transform.localScale = new Vector3(0.2f, 0.2f, -0.2f);
            obj2.transform.DOJump(new Vector2(obj.transform.parent.position.x, obj.transform.parent.position.y), 400f, 1, 1f, false);
            obj2.transform.DORotate(new Vector3(0, 0, 720f), 1f, RotateMode.FastBeyond360);
            yield return new WaitForSeconds(1f);
            obj.transform.DOMove(new Vector2(obj.transform.parent.position.x, obj.transform.parent.position.y - 5), 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
            obj.GetComponent<CardDisplay>().updateCard(cd);
            Destroy(obj2);
        }
        crucibleCards[1] = GameManager.manager.crucibleCard;
    }

    public void updateCard(Card card)
    {
        artworkImage = this.GetComponent<Image>();
        this.card = card;
        artworkImage.sprite = card.cardSprite;

        if (card.spicy)
        {
            if(flames != null)
                flames.SetActive(true);
        }

        else
        {
            if (flames != null)
                flames.SetActive(false);
        }
        // trying something
        // might take this out for now.
        if(gameObject.GetComponent<DragCard>() != null)
        {
            if (card.cardName == "Crucible" && gameObject.GetComponent<DragCard>().market)
            {
                Debug.Log("CRUCIBLE CRUCIBLE CRUCIBLE CRUCIBLE CRUCIBLE");
                // Add two cards to display on top of market card display
                crucibleCards.Add(GameManager.manager.md2.popCard());
                crucibleCards.Add(GameManager.manager.md2.popCard());
                // we'll see how this ends up working
                // StartCoroutine(updateCrucibleCards(gameObject));
                StartCoroutine(updateCrucibleCards(gameObject));

            }
        }      

        // durability check?
        if (card.cardType == "Artifact")
        {
            durability = 3;
        }
        
        if(card.cardName == "placeholder" && this.gameObject.name != "DeckPile")
        {
            if(GetComponent<DragCard>() != null && !GetComponent<DragCard>().market && !GetComponent<DragCard>().loaded)
            {
                GetComponent<DragCard>().beforeDisappear();
            }
                
            this.gameObject.SetActive(false);
            
        }
        else
            this.gameObject.SetActive(true);
        if(SceneManager.GetActiveScene().name == "TownCenter")
            GameManager.manager.updateMarketDeckDisplay();
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
            cd.durability = 3;
        }
        if (cd.spicy)
        {
            spicy = true;
            flames.SetActive(true);
        }

        else
        {
            spicy = false;
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
