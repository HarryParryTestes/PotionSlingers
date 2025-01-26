using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class CardHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Canvas canvas;
    public Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        // originalPosition = transform.position;
        originalPosition = new Vector3(transform.position.x - 20f, transform.position.y + 20f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        transform.DOScale(0.9f, 0.15f).SetId(gameObject.name);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Card_Hover");
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        transform.DOScale(0.7f, 0.15f).SetId(gameObject.name);
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        
        DOTween.Pause(gameObject.name);

        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Card_Pickup");

        
        // transform.position = Input.mousePosition;
        transform.DORotate(new Vector3(0f, 0f, 0f), 0.2f).SetEase(Ease.Linear).SetId(gameObject.name);
        transform.DOScale(0.7f, 0.15f).SetId(gameObject.name);
        GetComponent<Image>().CrossFadeAlpha(0.5f, 0.3f, true);

        transform.SetAsLastSibling();
        // transform.SetSiblingIndex(5);
        if (GetComponent<CanvasGroup>() != null)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }      
    }

    public void OnDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().anchoredPosition += eventData.delta / canvas.scaleFactor;
        // transform.position += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DOTween.Pause(gameObject.name);
        GetComponent<Image>().CrossFadeAlpha(1f, 0.3f, true);
        transform.DOMove(originalPosition, 0.3f).SetId(gameObject.name);
        if (GetComponent<CanvasGroup>() != null)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}
