using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class DragCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Canvas canvas;

    public bool grabbed = false;
    private Image image;
    private LineRenderer lineRenderer;
    private Vector3 cachedScale;
    private Vector2 originalPosition;
    private Vector3 startPoint;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform parentAfterDrag;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        rectTransform = GetComponent<RectTransform>();
        originalPosition = transform.position;
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
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

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        grabbed = true;
        parentAfterDrag = transform.parent;
        // canvasGroup.alpha = 0.5f;
        image.CrossFadeAlpha(0.5f, 0.3f, true);
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    /*
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        startPoint = Input.mousePosition;
    }
    */

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (this.gameObject.GetComponent<CardDisplay>().card.cardName != "placeholder")
        {
            // switch cursor
            // Game.pointCursor();
            
            float width = rectTransform.sizeDelta.x * rectTransform.localScale.x;
            float height = rectTransform.sizeDelta.y * rectTransform.localScale.y;

            // does taking this line below out change anything big?
            // this.transform.parent.transform.SetSiblingIndex(this.transform.parent.parent.transform.childCount - 1);
            // transform.SetSiblingIndex(transform.childCount - 1); // Sets card 
            transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            // transform.position = new Vector3(transform.position.x, height + height/2, transform.position.z);

            // Card Hover sound effect:
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Card_Hover");

        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // transform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        // rectTransform.position = Input.mousePosition;
        // Vector3 currentPoint = Input.mousePosition;
        // RenderLine(startPoint, currentPoint);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        grabbed = false;
        // canvasGroup.alpha = 1f;
        image.CrossFadeAlpha(1f, 0.3f, true);
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(parentAfterDrag);
        transform.localScale = new Vector3(1f, 1f, 1f);
        // transform.position = originalPosition;
        transform.DOMove(originalPosition, 0.3f);
        // EndLine();
    }

}
