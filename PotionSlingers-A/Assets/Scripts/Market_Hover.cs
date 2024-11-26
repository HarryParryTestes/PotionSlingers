using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Market_Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    Vector3 cachedScale; // Tracks original size of Card (for hovering as it manipulates scale of Card).
    Vector3 originalPos; // Tracks original position of Card.
    Vector3 mousePos; // Tracks current mouse cursor position.
    Quaternion originalRotation;

    RectTransform rt;

    // canHover is public static because if not static, other cards can be hovered
    // over while a card is clicked and attached to cursor.
    public static bool canHover = true; // Determines if cards can be hovered over.
    // bool attached = false; // Determines if a card has been clicked and attached to the mouse cursor.

    public Transform cardMenu;
    public Transform onlyViewCardMenu;
    Transform viewCardMenu = null;
    Transform highlighted = null;
    GameObject parentObject = null;
    GameObject viewingCardObject = null;
    bool cardSelected = false;
    public static bool viewingCard = false;

    // On startup:
    void Start() {
        rt = transform.GetComponent<RectTransform>();
        cachedScale = transform.localScale;
        originalPos = gameObject.transform.position;
        originalRotation = this.transform.localRotation;

        parentObject = this.transform.parent.gameObject;
        //cardMenu = this.transform.Find("CardMenu");
        viewCardMenu = this.transform.Find("ViewCardMenu");
        viewingCardObject = GameObject.Find("ViewingCard");

        if(parentObject == null) {
            Debug.Log("Error: parentObject doesn't exist!");
        }
        if(viewingCardObject == null) {
            Debug.Log("Error: ViewingCard doesn't exist!");
        }

        cardSelected = false;
        viewingCard = false;
        cardMenu.gameObject.SetActive(false);
        viewCardMenu.gameObject.SetActive(false);
        // highlighted.gameObject.SetActive(false);
    }

    void Update() {

    }
 
    // On mouse hovering over object: 
    // (aka when mouse cursor enters GameObject's X/Y/Z boundaries)
    public void OnPointerEnter(PointerEventData eventData) {
        // never had to deal with this until i used these with the trash deck lol
        if (this.gameObject.GetComponent<CardDisplay>().card.cardName == "placeholder")
        {
            return;
        }
            if (canHover) {
            float width = rt.sizeDelta.x * rt.localScale.x;
            float height = rt.sizeDelta.y * rt.localScale.y;
            transform.SetSiblingIndex(this.transform.parent.transform.childCount - 1);
            //transform.SetSiblingIndex(transform.childCount - 1); // Sets card 
            if (this.gameObject.name == "TrashCardDisplay1" ||
                this.gameObject.name == "TrashCardDisplay2" ||
                this.gameObject.name == "TrashCardDisplay3")
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Card_Hover");
                return;
            }
            transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);

            // Card Hover sound effect:
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Card_Hover");
        }
    }

    // On Mouse down click: 
    // (any click, a boolean tracks whether the click is to activate or deactivate something)
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (this.gameObject.GetComponent<CardDisplay>().card.cardName == "placeholder")
        {
            return;
        }
            if (!viewingCard) {
            cardSelected = !cardSelected;
            if(cardSelected) {
                if (this.gameObject.name == "TrashCardDisplay1" ||
                this.gameObject.name == "TrashCardDisplay2" ||
                this.gameObject.name == "TrashCardDisplay3")
                {
                    if (GameManager.manager.trashDeckBonus)
                    {
                        canHover = true;
                        cardMenu.gameObject.SetActive(true);
                        if (highlighted != null)
                        {
                            highlighted.gameObject.SetActive(true);
                        }
                        return;
                    } else
                    {
                        // add other menu in the trash displays that only allows them to view the card
                        if (onlyViewCardMenu != null)
                        {
                            onlyViewCardMenu.gameObject.SetActive(true);
                            canHover = true;
                            if (highlighted != null)
                            {
                                highlighted.gameObject.SetActive(true);
                            }
                        }
                    }
                    return;
                }
                canHover = false;
                transform.localScale = cachedScale;
                gameObject.transform.position = originalPos;
                cardMenu.gameObject.SetActive(true);
                if(highlighted != null)
                {
                    highlighted.gameObject.SetActive(true);
                }  
            }
            else if (!cardSelected) {
                if (this.gameObject.name == "TrashCardDisplay1" ||
                this.gameObject.name == "TrashCardDisplay2" ||
                this.gameObject.name == "TrashCardDisplay3")
                {
                    canHover = true;
                    cardMenu.gameObject.SetActive(false);
                    if (highlighted != null)
                    {
                        highlighted.gameObject.SetActive(false);
                    }
                    return;
                }
                transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
                canHover = true;
                cardMenu.gameObject.SetActive(false);
                if (onlyViewCardMenu != null)
                {
                    onlyViewCardMenu.gameObject.SetActive(false);
                }
                    if (highlighted != null)
                {
                    highlighted.gameObject.SetActive(false);
                }
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
            if(onlyViewCardMenu != null)
            {
                onlyViewCardMenu.gameObject.SetActive(false);
            }
            this.transform.SetParent(viewingCardObject.transform);
            this.transform.localRotation = Quaternion.identity;
            this.transform.localScale = new Vector3(5f, 5f, 5f);
            this.transform.position = new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 0);
            viewCardMenu.gameObject.SetActive(true);
            this.transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
        }
    }

    public void resetCard() {
        viewingCard = false;
        viewCardMenu.gameObject.SetActive(false);
        cardMenu.gameObject.SetActive(false);
        if (cardSelected) {
            cardSelected = false;
            this.transform.SetParent(parentObject.transform);
            this.transform.localRotation = Quaternion.identity;
            transform.localScale = cachedScale;
            gameObject.transform.position = originalPos;
            canHover = true;
        }
    }
}
