using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hover_Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 cachedScale;
    Vector3 originalPos;

    RectTransform rt;

    // On startup:
    void Start() {
        rt = transform.GetComponent<RectTransform>();
        cachedScale = transform.localScale;
        originalPos = gameObject.transform.position;    
    }
 
    // On mouse hovering over object:
    public void OnPointerEnter(PointerEventData eventData) {
        float width = rt.sizeDelta.x * rt.localScale.x;
        float height = rt.sizeDelta.y * rt.localScale.y;
        transform.SetSiblingIndex(transform.childCount - 1); // Sets card 
        transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        // transform.position = new Vector3(transform.position.x, height + height/2, transform.position.z);

        // Card Hover sound effect:
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Card_Hover");
    }
 
    // When mouse stops hovering over object:
    public void OnPointerExit(PointerEventData eventData) {
        transform.localScale = cachedScale;
        gameObject.transform.position = originalPos;
    }
}
