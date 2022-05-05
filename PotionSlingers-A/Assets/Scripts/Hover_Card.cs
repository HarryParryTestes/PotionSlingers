using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hover_Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    Vector3 cachedScale; // Tracks original size of Card (for hovering as it manipulates scale of Card).
    Vector3 originalPos; // Tracks original position of Card.
    Vector3 mousePos; // Tracks current mouse cursor position.
    Quaternion originalRotation;

    RectTransform rt;

    // canHover is public static because if not static, other cards can be hovered
    // over while a card is clicked and attached to cursor.
    public static bool canHover = true; // Determines if cards can be hovered over.
    public static bool clicked = false; // Determines if cards can be clicked.

    Transform cardMenu = null;
    Transform viewCardMenu = null;
    Transform highlighted = null;
    GameObject parentObject = null;
    GameObject attackMenu = null;
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
        cardMenu = this.transform.Find("CardMenu");
        viewCardMenu = this.transform.Find("ViewCardMenu");
        highlighted = parentObject.transform.Find("Highlighted");
        attackMenu = GameObject.Find("ChooseAttackPlayer");
        viewingCardObject = GameObject.Find("ViewingCard");

        cardSelected = false;
        viewingCard = false;
        cardMenu.gameObject.SetActive(false);
        viewCardMenu.gameObject.SetActive(false);
        attackMenu.gameObject.SetActive(false);
        highlighted.gameObject.SetActive(false);
    }

    void Update() {

    }
 
    // On mouse hovering over object: 
    // (aka when mouse cursor enters GameObject's X/Y/Z boundaries)
    public void OnPointerEnter(PointerEventData eventData) {
        if(canHover) {
            float width = rt.sizeDelta.x * rt.localScale.x;
            float height = rt.sizeDelta.y * rt.localScale.y;
            this.transform.parent.transform.SetSiblingIndex(this.transform.parent.parent.transform.childCount - 1);
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
        if (!clicked) {
            if(!viewingCard) {
                // cardSelected = !cardSelected;
                cardSelected = true;
                if(cardSelected) {
                    canHover = false;
                    clicked = true;
                    transform.localScale = cachedScale;
                    gameObject.transform.position = originalPos;
                    cardMenu.gameObject.SetActive(true);
                    highlighted.gameObject.SetActive(true);
                }
                // else if (!cardSelected) {
                //     clicked = false;
                //     canHover = true;
                //     transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
                //     cardMenu.gameObject.SetActive(false);
                //     highlighted.gameObject.SetActive(false);
                // }
            }
        }
        else if(clicked && cardSelected) {
            clicked = false;
            cardSelected = false;
            canHover = true;
            transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            cardMenu.gameObject.SetActive(false);
            highlighted.gameObject.SetActive(false);
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
            this.transform.SetParent(viewingCardObject.transform);
            this.transform.localRotation = Quaternion.identity;
            this.transform.localScale = new Vector3(3f, 3f, 3f);
            this.transform.position = new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 0);
            viewCardMenu.gameObject.SetActive(true);
            this.transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
        }
    }

    public void resetView() {
        viewingCard = false;
        viewCardMenu.gameObject.SetActive(false);
        if(cardSelected) {
            clicked = false;
            cardSelected = false;
            this.transform.SetParent(parentObject.transform);
            this.transform.localRotation = Quaternion.identity;
            transform.localScale = cachedScale;
            gameObject.transform.position = originalPos;
            canHover = true;
            highlighted.gameObject.SetActive(false);
        }
    }

    public void ChooseAttackPlayer() {
        viewingCard = true;
        if(cardSelected) {
            cardMenu.gameObject.SetActive(false);
            attackMenu.SetActive(true);
        }
    }
}
