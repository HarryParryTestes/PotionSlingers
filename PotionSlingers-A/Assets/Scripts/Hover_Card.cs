using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hover_Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    Vector3 cachedScale; // Tracks original size of Card (for hovering as it manipulates scale of Card).
    Vector3 originalPos; // Tracks original position of Card.
    Vector3 mousePos; // Tracks current mouse cursor position.

    RectTransform rt;

    // canHover is public static because if not static, other cards can be hovered
    // over while a card is clicked and attached to cursor.
    public static bool canHover = true; // Determines if cards can be hovered over.
    bool attached = false; // Determines if a card has been clicked and attached to the mouse cursor.

    Transform cardMenu;
    Transform viewCardMenu;
    bool cardSelected = false;
    public static bool viewingCard = false;

    // On startup:
    void Start() {
        rt = transform.GetComponent<RectTransform>();
        cachedScale = transform.localScale;
        originalPos = gameObject.transform.position;
        cardMenu = this.transform.Find("CardMenu");
        viewCardMenu = this.transform.Find("ViewCardMenu");
        if(cardMenu == null) {
            Debug.Log("Error: CardMenu doesn't exist!");
        }
        if(viewCardMenu == null) {
            Debug.Log("Error: ViewCardMenu doesn't exist!");
        }
        cardSelected = false;
        viewingCard = false;
        cardMenu.gameObject.SetActive(false);
        viewCardMenu.gameObject.SetActive(false);
    }

    void Update() {
        // If Card is attached to cursor, the Card follows the cursor's X/Y position until unattached.
        mousePos = Input.mousePosition;
        if(attached == true && canHover == false) {
            this.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }
    }
 
    // On mouse hovering over object: 
    // (aka when mouse cursor enters GameObject's X/Y/Z boundaries)
    public void OnPointerEnter(PointerEventData eventData) {
        if(canHover) {
            float width = rt.sizeDelta.x * rt.localScale.x;
            float height = rt.sizeDelta.y * rt.localScale.y;
            transform.SetSiblingIndex(this.transform.parent.transform.childCount - 1);
            //transform.SetSiblingIndex(transform.childCount - 1); // Sets card 
            transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            // transform.position = new Vector3(transform.position.x, height + height/2, transform.position.z);

            // Card Hover sound effect:
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Card_Hover");
        }
    }

    // On Mouse down click: 
    // (any click, a boolean tracks whether the click is to activate or deactivate something)
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        // If Card is clicked on and attached to cursor, other cards aren't hoverable,
        // else, if no Card is not attached anymore, Cards are hoverable again.

        // attached = !attached;
        // canHover = !canHover;
        // FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Card_Pickup");
        // if(attached) {
        //     mousePos = Input.mousePosition;
        //     this.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        // }
        // if(!attached && canHover) {
        //     transform.localScale = cachedScale;
        //     originalPos = gameObject.transform.position;
        // }
        if(!viewingCard) {
            cardSelected = !cardSelected;
            if(cardSelected) {
                canHover = false;
                transform.localScale = cachedScale;
                gameObject.transform.position = originalPos;
                cardMenu.gameObject.SetActive(true);
            }
            else if (!cardSelected) {
                transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
                canHover = true;
                cardMenu.gameObject.SetActive(false);
            }
        }
    }
 
    // When mouse stops hovering over object:
    // (aka when mouse cursor exits GameObject's X/Y/Z boundaries)
    public void OnPointerExit(PointerEventData eventData) {
        if(canHover) {
            transform.localScale = cachedScale;
            gameObject.transform.position = originalPos;
        }
    }

    public void ViewCard() {
        viewingCard = true;
        if(cardSelected) {
            cardMenu.gameObject.SetActive(false);
            transform.SetSiblingIndex(this.transform.parent.transform.childCount - 1);
            transform.localScale = new Vector3(4f, 4f, 4f);
            this.transform.position = new Vector3(255, 285, 0);
            viewCardMenu.gameObject.SetActive(true);
        }
    }

    public void resetView() {
        viewingCard = false;
        viewCardMenu.gameObject.SetActive(false);
        if(cardSelected) {
            cardSelected = false;
            transform.localScale = cachedScale;
            gameObject.transform.position = originalPos;
            canHover = true;
        }
    }
}
