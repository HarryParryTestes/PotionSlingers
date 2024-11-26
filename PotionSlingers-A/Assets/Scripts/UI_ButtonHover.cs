using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ButtonHover : MonoBehaviour, IPointerEnterHandler
{
    Vector3 cachedScale; // Tracks original size of button
    Vector3 originalPos; // Tracks original position of button.
    Vector3 mousePos; // Tracks current mouse cursor position.

    RectTransform rt;

    // canHover is public static because if not static, other button can be hovered
    // over while a button is clicked and attached to cursor.
    public static bool canHover = true; // Determines if button can be hovered over.
   

    // On startup:
    void Start()
    {
        rt = transform.GetComponent<RectTransform>();
        cachedScale = transform.localScale;
        originalPos = gameObject.transform.position;
    }

    
    // On mouse hovering over object: 
    // (aka when mouse cursor enters GameObject's X/Y/Z boundaries)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (canHover)
        {
            
          

            // Button Hover sound effect:
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Menu_Hover");
        }
    }
    
}

