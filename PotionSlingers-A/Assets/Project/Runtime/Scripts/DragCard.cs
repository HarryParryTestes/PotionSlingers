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
    public bool market;
    public int marketCardInt;
    private Image image;
    private LineRenderer lineRenderer;
    private Vector3 cachedScale;
    private Vector2 originalPosition;
    private Vector3 startPoint;
    private Vector3 cardRotation; 
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform parentAfterDrag;
    private Image artifactCard;
    private Image vesselCard1;
    private Image vesselCard2;
    int parentSiblingIndex;

    private void Awake()
    {
        parentAfterDrag = transform.parent;
        parentSiblingIndex = transform.parent.GetSiblingIndex();
        lineRenderer = GetComponent<LineRenderer>();
        rectTransform = GetComponent<RectTransform>();
        if (!market)
        {
            originalPosition = transform.position;
        } else
        {
            originalPosition = transform.position + new Vector3(0, 647.5f, 0);
        }
        
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
        cardRotation = rectTransform.rotation.eulerAngles;
        Debug.Log(cardRotation);
        if (!market)
        {
            artifactCard = this.transform.parent.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
            vesselCard1 = this.transform.parent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
            vesselCard2 = this.transform.parent.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>();
        }
        // Debug.Log(artifactCard.gameObject.name);
    }

    /*
    public void RenderLine(Vector3 startPoint, Vector3 endPoint)
    {
        Debug.Log("Rendering line?");

        lineRenderer.positionCount = 2;
        Vector3[] points = new Vector3[2];
        points[0] = startPoint;
        points[1] = endPoint;

        lineRenderer.SetPositions(points);
    }

    public void EndLine()
    {
        lineRenderer.positionCount = 0;
    }
    */

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!clicked)
        {
            clicked = true;
            transform.DORotate(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.Linear);
            //Output to console the clicked GameObject's name and the following message. You can replace this with your own actions for when clicking the GameObject.
            // Debug.Log(name + " Game Object Clicked!");
            // parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            Vector3 pos = new Vector3(960, 550, 0);
            if (!market)
            {
                transform.DOScale(2.15f, 0.5f);
            } else
            {
                transform.DOScale(4f, 0.5f);
            }
            
            transform.DOMove(pos, 0.5f);

        } else
        {
            clicked = false;
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
            transform.DOScale(1f, 0.3f);
            transform.DORotate(cardRotation, 0.3f).SetEase(Ease.Linear);
            transform.DOMove(originalPosition, 0.3f);
            StartCoroutine(SibIndex());
            // transform.SetParent(parentAfterDrag);
        }
        
    }

    public IEnumerator SibIndex()
    {
        yield return new WaitForSeconds(0.5f);
        transform.SetParent(parentAfterDrag);
        // transform.localScale = new Vector3(1f, 1f, 1f);
        // transform.parent.SetSiblingIndex(parentSiblingIndex);
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        grabbed = true;
        clicked = false;
        transform.DORotate(new Vector3(0f, 0f, 0f), 0.2f).SetEase(Ease.Linear);
        transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        // parentAfterDrag = transform.parent;
        // canvasGroup.alpha = 0.5f;
        image.CrossFadeAlpha(0.5f, 0.3f, true);
        canvasGroup.blocksRaycasts = false;
        GameManager.manager.canvasGroup.blocksRaycasts = false;
        if (transform.parent != transform.root)
        {
            transform.SetParent(transform.root);
        }
        transform.SetAsLastSibling();

        if (!market)
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
            // switch cursor
            // Game.pointCursor();
            if (!market)
                transform.position += new Vector3(0, 150, 0);
            
            float width = rectTransform.sizeDelta.x * rectTransform.localScale.x;
            float height = rectTransform.sizeDelta.y * rectTransform.localScale.y;

            // does taking this line below out change anything big?
            // this.transform.parent.transform.SetSiblingIndex(this.transform.parent.parent.transform.childCount - 1);
            // transform.SetSiblingIndex(transform.childCount - 1); // Sets card 
            transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            // transform.position = new Vector3(transform.position.x, height + height/2, transform.position.z);

            if (!market)
            {
                transform.SetParent(transform.root);
                // transform.SetAsLastSibling();
            }
            transform.SetAsLastSibling();

            // Card Hover sound effect:
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Card_Hover");

        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (!clicked && !grabbed && this.gameObject.GetComponent<CardDisplay>().card.cardName != "placeholder")
        {
            
            if (!market)
            {
                transform.position -= new Vector3(0, 150, 0);
                
            }
            // transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            transform.SetParent(parentAfterDrag);
            transform.localScale = new Vector3(1f, 1f, 1f);

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

        if (!market)
            findCard(this.gameObject.GetComponent<CardDisplay>().card.cardName);

        // transform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        // rectTransform.position = Input.mousePosition;
        // Vector3 currentPoint = Input.mousePosition;
        // RenderLine(startPoint, currentPoint);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        grabbed = false;
        clicked = false;
        // canvasGroup.alpha = 1f;
        image.CrossFadeAlpha(1f, 0.3f, true);
        if (!market)
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
        transform.DORotate(cardRotation, 0.3f).SetEase(Ease.Linear);
        transform.DOMove(originalPosition, 0.3f);
        // EndLine();
    }

}
