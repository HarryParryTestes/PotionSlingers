using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class DragCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Canvas canvas;

    public bool grabbed = false;
    public bool clicked = false;
    public bool loaded;
    public bool market;
    public int marketCardInt;
    private Image image;
    private LineRenderer lineRenderer;
    private Vector3 cachedScale;
    public Vector2 originalPosition;
    private Vector3 startPoint;
    public Vector3 cardRotation; 
    public RectTransform rectTransform;
    public CanvasGroup canvasGroup;
    public Transform parentAfterDrag;
    private Image artifactCard;
    private Image vesselCard1;
    private Image vesselCard2;
    int parentSiblingIndex;

    void Update()
    {
        /*
        if(transform.position.y < 0 && !market)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        */
        /*
        if (GameManager.manager.heightRatio == 2 && GameManager.manager.widthRatio == 2)
        {
            // Debug.Log("Different screen resolution");
            if (gameObject.name == "Card1Display")
            {
                originalPosition = new Vector3(1000f, 0, 0);
            }
        }
        */
        if (Screen.width == 2560)
        {
            switch (gameObject.name)
            {
                case "Card1Display":
                    originalPosition = new Vector3(670f, 0, 0);
                    break;
                case "Card2Display":
                    originalPosition = new Vector3(1075f, 80f, 0);
                    break;
                case "Card3Display":
                    originalPosition = new Vector3(1485f, 80f, 0);
                    break;
                case "Card4Display":
                    originalPosition = new Vector3(1890f, -5f, 0);
                    break;
                case "CardDisplay (Potion1)":
                    originalPosition = new Vector3(1005f, 1025f, 0);
                    break;
                case "CardDisplay (Potion2)":
                    originalPosition = new Vector3(1286f, 1025f, 0);
                    break;
                case "CardDisplay (Potion3)":
                    originalPosition = new Vector3(1570f, 1025f, 0);
                    break;
                case "CardDisplay (Special1)":
                    originalPosition = new Vector3(1005f, 621f, 0);
                    break;
                case "CardDisplay (Special2)":
                    originalPosition = new Vector3(1286f, 621f, 0);
                    break;
                case "CardDisplay (Special3)":
                    originalPosition = new Vector3(1570f, 621f, 0);
                    break;
                default:
                    break;
            }
        } else if (Screen.width == 3840)
        {
            switch (gameObject.name)
            {
                case "Card1Display":
                    originalPosition = new Vector3(1010f, -10f, 0);
                    break;
                case "Card2Display":
                    originalPosition = new Vector3(1610f, 120f, 0);
                    break;
                case "Card3Display":
                    originalPosition = new Vector3(2225f, 120f, 0);
                    break;
                case "Card4Display":
                    originalPosition = new Vector3(2830f, -10f, 0);
                    break;
                case "CardDisplay (Potion1)":
                    originalPosition = new Vector3(1510f, 1538f, 0);
                    break;
                case "CardDisplay (Potion2)":
                    originalPosition = new Vector3(1926f, 1538f, 0);
                    break;
                case "CardDisplay (Potion3)":
                    originalPosition = new Vector3(2355f, 1538f, 0);
                    break;
                case "CardDisplay (Special1)":
                    originalPosition = new Vector3(1510f, 928f, 0);
                    break;
                case "CardDisplay (Special2)":
                    originalPosition = new Vector3(1926f, 928f, 0);
                    break;
                case "CardDisplay (Special3)":
                    originalPosition = new Vector3(2355f, 928f, 0);
                    break;
                default:
                    break;
            }
        }
        else if (Screen.width == 1366)
        {
            switch (gameObject.name)
            {
                case "Card1Display":
                    originalPosition = new Vector3(358f, 0, 0);
                    break;
                case "Card2Display":
                    originalPosition = new Vector3(573f, 45f, 0);
                    break;
                case "Card3Display":
                    originalPosition = new Vector3(793f, 45f, 0);
                    break;
                case "Card4Display":
                    originalPosition = new Vector3(1008f, -3f, 0);
                    break;
                case "CardDisplay (Potion1)":
                    originalPosition = new Vector3(536f, 548f, 0);
                    break;
                case "CardDisplay (Potion2)":
                    originalPosition = new Vector3(689f, 548f, 0);
                    break;
                case "CardDisplay (Potion3)":
                    originalPosition = new Vector3(838f, 548f, 0);
                    break;
                case "CardDisplay (Special1)":
                    originalPosition = new Vector3(536f, 330f, 0);
                    break;
                case "CardDisplay (Special2)":
                    originalPosition = new Vector3(689f, 330f, 0);
                    break;
                case "CardDisplay (Special3)":
                    originalPosition = new Vector3(838f, 330f, 0);
                    break;
                default:
                    break;
            }
        }

    }

    private void Awake()
    {
        parentAfterDrag = transform.parent;
        parentSiblingIndex = transform.parent.GetSiblingIndex();
        lineRenderer = GetComponent<LineRenderer>();
        rectTransform = GetComponent<RectTransform>();
        if (!market)
        {
            originalPosition = transform.position;
            // originalPosition = new Vector2(transform.position.x * GameManager.manager.widthRatio, transform.position.y * GameManager.manager.heightRatio);

        }
        else
        {
            // TODO: fix this so that the position changes depending on whether or not the market is selected or not
            originalPosition = transform.position + new Vector3(0, 647.5f, 0);
        }
        
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
        cardRotation = rectTransform.rotation.eulerAngles;
        Debug.Log(cardRotation);
        if (!market && !loaded)
        {
            artifactCard = this.transform.parent.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
            vesselCard1 = this.transform.parent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
            vesselCard2 = this.transform.parent.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>();
        }
        // Debug.Log(artifactCard.gameObject.name);
    }

    // TODO: make a function so that the all cards move back to their proper positions  
    // if either the market deck is closed or if another card is clicked on

    public void marketBack()
    {
        if (clicked && market && !GameManager.manager.marketSelected)
        {
            DOTween.Pause(gameObject.name);
            clicked = false;
            transform.SetParent(parentAfterDrag);
            transform.DOScale(1f, 0.3f).SetId(gameObject.name);
            transform.DORotate(cardRotation, 0.3f).SetEase(Ease.Linear).SetId(gameObject.name);
            Vector2 thing = new Vector2(0, 333f * GameManager.manager.heightRatio);
            transform.DOMove(originalPosition - thing, 0.3f).SetId(gameObject.name);
            return;
            // StartCoroutine(SibIndex());
        }

        if (clicked)
        {
            clicked = false;
            transform.DOScale(1f, 0.3f).SetId(gameObject.name);
            transform.DORotate(cardRotation, 0.3f).SetEase(Ease.Linear).SetId(gameObject.name);
            transform.DOMove(originalPosition, 0.3f).SetId(gameObject.name);
            StartCoroutine(SibIndex());
        }
        
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        // experimenting
        // gameObject.GetComponent<CardDisplay>().colorCard();

        if (market && !GameManager.manager.marketSelected)
        {
            DOTween.Pause(gameObject.name);
            return;
        } 

        if (!clicked && this.gameObject.GetComponent<CardDisplay>().card.cardName != "placeholder")
        {
            DOTween.Pause(gameObject.name);
            GameManager.manager.moveMarketCards();

            canvasGroup.interactable = false;
            Invoke("makeInteractable", 0.6f);
            clicked = true;
            transform.DORotate(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.Linear).SetId(gameObject.name);
            //Output to console the clicked GameObject's name and the following message. You can replace this with your own actions for when clicking the GameObject.
            // Debug.Log(name + " Game Object Clicked!");
            // parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            Vector3 pos = new Vector3(960 * GameManager.manager.widthRatio, 550 * GameManager.manager.heightRatio, 0);
            if (loaded)
            {
                transform.DOMove(pos, 0.5f).SetId(gameObject.name);
                transform.DOScale(4.4f, 0.5f).SetId(gameObject.name);
                return;
            }

            if (!market)
            {
                transform.DOScale(2.15f, 0.5f).SetId(gameObject.name);
            } else if(GameManager.manager.marketSelected)
            {
                transform.DOScale(4f, 0.5f).SetId(gameObject.name);
                transform.DOMove(pos, 0.5f).SetId(gameObject.name);
                return;
            }
            
            transform.DOMove(pos, 0.5f).SetId(gameObject.name);

        } else
        {
            clicked = false;
            DOTween.Pause(gameObject.name);

            if (market && !GameManager.manager.marketSelected)
            {
                transform.DOScale(1f, 0.3f).SetId(gameObject.name);
                transform.DORotate(cardRotation, 0.3f).SetEase(Ease.Linear).SetId(gameObject.name);
                // transform.DORotate(cardRotation, 0.3f).SetEase(Ease.Linear);
                Vector2 thing = new Vector2(0, 647.5f * GameManager.manager.heightRatio);
                transform.DOMove(originalPosition - thing, 0.3f).SetId(gameObject.name);
                // transform.DOMove(originalPosition, 0.3f).SetId(gameObject.name);
                StartCoroutine(SibIndex());
                return;
            }
            // transform.SetParent(parentAfterDrag);
            // transform.parent.SetSiblingIndex(parentSiblingIndex);
            /*
            if (!market)
            {
                transform.DOScale(1f, 0.3f);
            }
            else
            {
                transform.DOScale(1.2f, 0.3f);
            }
            */
            transform.DOScale(1f, 0.3f).SetId(gameObject.name);
            transform.DORotate(cardRotation, 0.3f).SetEase(Ease.Linear).SetId(gameObject.name);
            // transform.DORotate(cardRotation, 0.3f).SetEase(Ease.Linear);
            transform.DOMove(originalPosition, 0.3f).SetId(gameObject.name);
            // transform.DOMove(new Vector2(originalPosition.x * GameManager.manager.widthRatio, originalPosition.y * GameManager.manager.heightRatio), 0.3f).SetId(gameObject.name);
            // rectTransform.DOAnchorPos(originalPosition, 0.3f, false);
            StartCoroutine(SibIndex());
            // transform.SetParent(parentAfterDrag);
        }
        
    }

    public void makeInteractable()
    {
        canvasGroup.interactable = true;
    }

    public IEnumerator SibIndex()
    {
        yield return new WaitForSeconds(0.1f);
        transform.SetParent(parentAfterDrag);
        Debug.Log("Sibling index changed");

        if(!market)
            transform.DOScale(1f, 0.1f).SetId(gameObject.name);
        // transform.localScale = new Vector3(1f, 1f, 1f);
        // transform.parent.SetSiblingIndex(parentSiblingIndex);
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        if (loaded)
            return;

        if (this.gameObject.GetComponent<CardDisplay>().card.cardName == "placeholder")
            return;

        if (market && !GameManager.manager.marketSelected)
            return;
        DOTween.Pause(gameObject.name);

        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Card_Pickup");

        grabbed = true;
        clicked = false;
        transform.position = Input.mousePosition;
        transform.DORotate(new Vector3(0f, 0f, 0f), 0.2f).SetEase(Ease.Linear).SetId(gameObject.name);
        transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        // parentAfterDrag = transform.parent;
        // canvasGroup.alpha = 0.5f;
        image.CrossFadeAlpha(0.5f, 0.3f, true);
        if(canvasGroup != null)
            canvasGroup.blocksRaycasts = false;
        GameManager.manager.canvasGroup.blocksRaycasts = false;
        if (transform.parent != transform.root)
        {
            transform.SetParent(transform.root);
        }
        transform.SetAsLastSibling();

        if (!market && !loaded)
        {
            if (artifactCard.gameObject.activeInHierarchy)
            {
                artifactCard.CrossFadeAlpha(0.3f, 0.3f, true);
            }

            if (vesselCard1.gameObject.activeInHierarchy)
            {
                vesselCard1.CrossFadeAlpha(0.3f, 0.3f, true);
            }

            if (vesselCard2.gameObject.activeInHierarchy)
            {
                vesselCard2.CrossFadeAlpha(0.3f, 0.3f, true);
            }
        }
    }

    /*
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        startPoint = Input.mousePosition;
    }
    */

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (this.gameObject.GetComponent<CardDisplay>().card.cardName != "placeholder" && !clicked)
        {
            if (market && !GameManager.manager.marketSelected)
                return;
            // switch cursor
            // Game.pointCursor();
            if (loaded)
            {
                transform.DOMoveY(originalPosition.y + (50f * GameManager.manager.heightRatio), 0.25f);
                transform.DOScale(1.25f, 0.25f).SetId(gameObject.name);
                return;
            }

            // originally at 150, experimenting with 200
            if (!market)
            {
                // transform.position = new Vector3(originalPosition.x, originalPosition.y + (190f * GameManager.manager.heightRatio), 0);
                transform.DOMove(new Vector3(originalPosition.x, originalPosition.y + (190f * GameManager.manager.heightRatio), 0), 0.25f);
            }

            // transform.position += new Vector3(0, 150, 0);
            // transform.DOMove(new Vector3(originalPosition.x, originalPosition.y + (190f * GameManager.manager.heightRatio), 0), 0.5f);


            float width = rectTransform.sizeDelta.x * rectTransform.localScale.x;
            float height = rectTransform.sizeDelta.y * rectTransform.localScale.y;

            // does taking this line below out change anything big?
            // this.transform.parent.transform.SetSiblingIndex(this.transform.parent.parent.transform.childCount - 1);
            // transform.SetSiblingIndex(transform.childCount - 1); // Sets card 
            // transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            if(!market)
                DOTween.Pause(gameObject.name);
            transform.localScale = new Vector3(1f, 1f, 1f);
            if(!market)
                transform.DOScale(1.75f, 0.25f).SetId(gameObject.name);
            else
                transform.DOScale(1.25f, 0.25f).SetId(gameObject.name);

            // transform.position = new Vector3(transform.position.x, height + height/2, transform.position.z);

            if (!market)
            {
                transform.SetParent(transform.root);
                // transform.SetAsLastSibling();
            }
            transform.SetAsLastSibling();

            // Card Hover sound effect:
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Card_Hover");
            /*
            Outline outline = gameObject.AddComponent<Outline>();

            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 5f;
            */
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (!clicked && !grabbed && this.gameObject.GetComponent<CardDisplay>().card.cardName != "placeholder")
        {
            
            if (!market)
            {
                // transform.position -= new Vector3(0, 150, 0);
                transform.DOMove(originalPosition, 0.3f).SetId(gameObject.name);
            }
            // transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            transform.SetParent(parentAfterDrag);
            transform.localScale = new Vector3(1f, 1f, 1f);
            // DOTween.Pause(gameObject.name);
            transform.DOScale(1f, 0.5f).SetId(gameObject.name);
            if (!market)
            {
                transform.DOMove(originalPosition, 0.3f).SetId(gameObject.name);
                transform.DORotate(cardRotation, 0.3f).SetEase(Ease.Linear).SetId(gameObject.name);
            }
                
            /*
            if (transform.position.y < 0)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
            */

        }   
        // StartCoroutine(SibIndex());
        // transform.parent.SetSiblingIndex(parentSiblingIndex);
        /*
        if(!clicked && grabbed)
            transform.DOScale(1f, 0.3f);
        */
    }

    public void findCard(string cardName)
    {

        for (int i = 0; i < 4; i++)
        {
            if (GameManager.manager.players[GameManager.manager.myPlayerIndex].holster.cardList[i].card.cardName == cardName)
            {
                // Debug.Log(cardName);
                GameManager.manager.selectedCardInt = i + 1;
                break;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        GameManager.manager.moveMarketCards();

        if (market && !GameManager.manager.marketSelected)
            return;
        // find card int here?
        /*
        for (int i = 0; i < 4; i++)
        {
            if (GameManager.manager.players[myPlayerIndex].holster.cardList[i].card.cardName == cardName)
            {
                GameManager.manager.loadedCardInt = i;
                break;
            }
        }
        */
        if (this.gameObject.GetComponent<CardDisplay>().card.cardName == "placeholder")
            return;

        if (!market)
            findCard(this.gameObject.GetComponent<CardDisplay>().card.cardName);

        // transform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void beforeDisappear()
    {
        DOTween.Pause(gameObject.name);
        grabbed = false;
        clicked = false;
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = true;
        // GameManager.manager.canvasGroup.blocksRaycasts = true;
        transform.SetParent(parentAfterDrag);
        transform.localScale = new Vector3(1f, 1f, 1f);
        //transform.DOScale(1f, 0.3f);
        // transform.position = originalPosition;
        transform.DORotate(cardRotation, 0.3f).SetEase(Ease.Linear).SetId(gameObject.name);
        transform.DOMove(originalPosition, 0.3f).SetId(gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DOTween.Pause(gameObject.name);
        grabbed = false;
        clicked = false;
        // canvasGroup.alpha = 1f;
        image.CrossFadeAlpha(1f, 0.3f, true);
        if (!market && !loaded)
        {
            if (artifactCard.gameObject.activeInHierarchy)
            {
                artifactCard.CrossFadeAlpha(1f, 0.3f, true);
            }

            if (vesselCard1.gameObject.activeInHierarchy)
            {
                vesselCard1.CrossFadeAlpha(1f, 0.3f, true);
            }

            if (vesselCard2.gameObject.activeInHierarchy)
            {
                vesselCard2.CrossFadeAlpha(1f, 0.3f, true);
            }
        }
        
        canvasGroup.blocksRaycasts = true;
        // GameManager.manager.canvasGroup.blocksRaycasts = true;
        transform.SetParent(parentAfterDrag);
        transform.localScale = new Vector3(1f, 1f, 1f);
        //transform.DOScale(1f, 0.3f);
        // transform.position = originalPosition;
        transform.DORotate(cardRotation, 0.3f).SetEase(Ease.Linear).SetId(gameObject.name);
        transform.DOMove(originalPosition, 0.3f).SetId(gameObject.name);
        // EndLine();
    }

}
