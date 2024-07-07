using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // CardDisplay cd;
    public CardPlayer cp;
    public CharacterDisplay cd;
    public Vector2 originalPosition;
    public bool grabbed = false;
    public bool clicked = false;
    public bool loaded;
    public bool market;
    public bool gauntletBonus = false;
    public RectTransform rectTransform;
    private Image image;
    private Vector3 cachedScale;
    private Vector3 startPoint;
    public Vector3 cardRotation;
    public CanvasGroup canvasGroup;
    public Transform parentAfterDrag;
    private Image artifactCard;
    private Image vesselCard1;
    private Image vesselCard2;
    int parentSiblingIndex;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentAfterDrag = transform.parent;
        parentSiblingIndex = transform.parent.GetSiblingIndex();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
        cardRotation = rectTransform.rotation.eulerAngles;

        if (this.gameObject.GetComponent<CharacterDisplay>() != null)
        {
            cp = this.gameObject.GetComponent<CardPlayer>();
            cd = cp.character;
            Debug.Log(cp.gameObject.name);
        }

        if(this.gameObject.name == "DeckPile")
        {
            originalPosition = new Vector2(transform.position.x, transform.position.y);
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

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        if (this.gameObject.name == "DeckPile")
            cp.checkGauntletBonus();

        if (this.gameObject.name != "DeckPile")
            return;

        if (!gauntletBonus)
            return;

        if (this.gameObject.GetComponent<CardDisplay>().card.cardName == "placeholder")
            return;

        if (market && !GameManager.manager.marketSelected)
            return;
        DOTween.Pause(gameObject.name);

        grabbed = true;
        clicked = false;
        transform.position = Input.mousePosition;
        transform.DORotate(new Vector3(0f, 0f, 0f), 0.2f).SetEase(Ease.Linear).SetId(gameObject.name);
        transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        // parentAfterDrag = transform.parent;
        // canvasGroup.alpha = 0.5f;
        image.CrossFadeAlpha(0.5f, 0.3f, true);
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = false;
        GameManager.manager.canvasGroup.blocksRaycasts = false;
        if (transform.parent != transform.root)
        {
            transform.SetParent(transform.root);
        }
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!gauntletBonus)
            return;

        if (this.gameObject.name != "DeckPile")
            return;
        GameManager.manager.moveMarketCards();
        
        if (this.gameObject.GetComponent<CardDisplay>().card.cardName == "placeholder")
            return;

        // findCard(this.gameObject.GetComponent<CardDisplay>().card.cardName);

        // transform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        if (this.gameObject.name == "DeckPile")
        {
            rectTransform.anchoredPosition += eventData.delta / GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;
        }
        
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!gauntletBonus)
            return;

        if (this.gameObject.name != "DeckPile")
            return;
        DOTween.Pause(gameObject.name);
        grabbed = false;
        clicked = false;
        // canvasGroup.alpha = 1f;
        image.CrossFadeAlpha(1f, 0.3f, true);

        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        // GameManager.manager.canvasGroup.blocksRaycasts = true;
        transform.SetParent(parentAfterDrag);
        transform.SetSiblingIndex(1);
        transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        //transform.DOScale(1f, 0.3f);
        // transform.position = originalPosition;
        transform.DORotate(cardRotation, 0.3f).SetEase(Ease.Linear).SetId(gameObject.name);
        transform.DOMove(originalPosition, 0.3f).SetId(gameObject.name);
        // EndLine();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (GameManager.manager.snakeBonus)
        {
            Debug.Log("Snake bonus???");
            GameManager.manager.tempPlayer = cp;
            GameManager.manager.nicklesAttackMenu.SetActive(false);
            GameManager.manager.opponentHolsterMenu.SetActive(true);
            GameManager.manager.displayOpponentHolster(cp);
            GameManager.manager.holster = false;
            return;
        }

        if(GameManager.manager.nicklesDamage > 0 && GameManager.manager.nicklesAttackMenu.activeInHierarchy)
        {
            Debug.Log("Nickles damage action damaged for: " + GameManager.manager.nicklesDamage);
            GameManager.manager.players[GameManager.manager.myPlayerIndex].subPips(GameManager.manager.nicklesDamage);
            cp.subHealth(GameManager.manager.nicklesDamage);
            GameManager.manager.nicklesDamage = 0;
            GameManager.manager.nicklesAttackMenu.SetActive(false);
            GameManager.manager.sendSuccessMessage(21);
            return;
        }

        if (GameManager.manager.nicklesAttackMenu.activeInHierarchy)
        {
            // GameManager.manager.tempPlayer = cp;
            foreach (CardDisplay cd in cp.holster.cardList)
            {
                if (cd.card.cardName != "placeholder")
                {
                    GameObject obj = Instantiate(cd.gameObject,
                    cd.gameObject.transform.position,
                    cd.gameObject.transform.rotation,
                    cd.gameObject.transform);

                    StartCoroutine(GameManager.manager.MoveToTrash(obj, cd));

                    GameManager.manager.td.addCard(cd);
                }
            }
            GameManager.manager.nicklesAttackMenu.SetActive(false);
            GameManager.manager.sendMessage("Trashed an opponent's holster!");
            return;
        }

    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if(cp != null && this.gameObject.name != "DeckPile")
        {
            if(pointerEventData.pointerDrag != null)
            {
                return;
            }
            // hoverbox code in here!
            cp.hoverBox.SetActive(true);
            cp.hoverBox.GetComponent<HoverBox>().UpdateText(cp);
        }

        if (this.gameObject.name == "DeckPile")
        {
            // transform.position += new Vector3(0, 100, 0);
            transform.DOMove(new Vector3(originalPosition.x, originalPosition.y + (95f * GameManager.manager.heightRatio), 0), 0.25f);
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Card_Hover");
            // Invoke("checkDeck", 0.25f);
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {

        if (cp != null && this.gameObject.name != "DeckPile")
        {
            // hoverbox code in here!
            cp.hoverBox.SetActive(false);
        }
        if (this.gameObject.name == "DeckPile" && !grabbed)
        {
            // transform.position -= new Vector3(0, 100, 0);
            // Debug.Log("Pointer exited! See if this affects dragging");
            transform.DOMove(originalPosition, 0.25f);
            // Invoke("checkDeck", 0.25f);
        }   
    }

    public void OnDrop(PointerEventData eventData)
    {
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
        if (dc.market && this.gameObject.name == "DeckPile")
        {
            Debug.Log("Buy triggered?");
            if (heldCard.GetComponent<TopMarketBuy>() != null)
            {
                // buy the card
                Debug.Log("Buy");
            }
            handleBuy(dc.marketCardInt);
            // heldObject.GetComponent<CardThrow>().throwCard();
        } else if(this.gameObject.name == "DeckPile")
        {
            Debug.Log("Dropped card triggered?");
            GameManager.manager.setSCInt(grabbedCard.card.cardName);
            GameManager.manager.cycleCard();
        }

        // attacking player
        if (cd != null && !dc.market)
        {
            Debug.Log("Throw triggered?");
            cd.gameObject.GetComponent<CardThrow>().throwCard();
        }
    }
}
