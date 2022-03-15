using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hover_MarketCards : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 cachedScale;
    Vector3 originalPos;

    RectTransform rt;
    
    // Start is called before the first frame update
    void Start()
    {
        rt = transform.GetComponent<RectTransform>();
        cachedScale = transform.localScale;
        originalPos = gameObject.transform.position;
    }

    // On mouse hovering over object:
    public void OnPointerEnter(PointerEventData eventData) {
        float width = rt.sizeDelta.x * rt.localScale.x;
        float height = rt.sizeDelta.y * rt.localScale.y;
        // transform.parent
        transform.SetSiblingIndex(transform.childCount - 1); // Sets card 
        transform.localScale = new Vector3(3f, 3f, 3f);
        // transform.position = new Vector3(transform.position.x, height + height/2, transform.position.z);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Card_Hover");
    }
 
    // When mouse stops hovering over object:
    public void OnPointerExit(PointerEventData eventData) {
        transform.localScale = cachedScale;
        gameObject.transform.position = originalPos;
    }
}
