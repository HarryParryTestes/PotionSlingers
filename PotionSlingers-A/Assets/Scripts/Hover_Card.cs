using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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

    Transform cardMenu;
    Transform viewCardMenu;
    Transform specialCardMenu;
    Transform highlighted;
    GameObject parentObject;
    GameObject attackMenu;
    GameObject viewingCardObject;
    bool cardSelected = false;
    public static bool viewingCard = false;
    //public GameObject exitMenu;

    // On startup:
    void Start() {
        //exitMenu = GameObject.Find("EXIT VIEW");
        rt = transform.GetComponent<RectTransform>();
        cachedScale = transform.localScale;
        originalPos = gameObject.transform.position;
        originalRotation = this.transform.localRotation;

        parentObject = this.transform.parent.gameObject;
        cardMenu = this.transform.Find("CardMenu");
        viewCardMenu = this.transform.Find("ViewCardMenu");
        specialCardMenu = this.transform.Find("SpecialCardMenu");
        highlighted = parentObject.transform.Find("Highlighted");
        attackMenu = GameObject.Find("ChooseAttackPlayer");
        viewingCardObject = GameObject.Find("ViewingCard");

        if (SceneManager.GetActiveScene().name == "TitleMenu")
        {
            viewingCardObject = this.gameObject;
        }

        cardSelected = false;
        viewingCard = false;
        cardMenu.gameObject.SetActive(false);
        viewCardMenu.gameObject.SetActive(false);
        attackMenu.gameObject.SetActive(false);
        highlighted.gameObject.SetActive(false);
        specialCardMenu.gameObject.SetActive(false);
    }

    void Update() {
        if(this.gameObject.name == "CharacterCard")
        {
            return;
        }

        if(this.gameObject.GetComponent<CardDisplay>().card.cardName == "placeholder")
        {
            transform.localScale = cachedScale;
            gameObject.transform.position = originalPos;
        }
    }
 
    // On mouse hovering over object: 
    // (aka when mouse cursor enters GameObject's X/Y/Z boundaries)
    public void OnPointerEnter(PointerEventData eventData) {

        if (SceneManager.GetActiveScene().name == "TitleMenu")
        {
            return;
        }

        // for character card
        if (this.gameObject.name == "CharacterCard")
        {
            float width = rt.sizeDelta.x * rt.localScale.x;
            float height = rt.sizeDelta.y * rt.localScale.y;
            this.transform.parent.transform.SetSiblingIndex(this.transform.parent.parent.transform.childCount - 1);
            //transform.SetSiblingIndex(transform.childCount - 1); // Sets card 
            transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            // transform.position = new Vector3(transform.position.x, height + height/2, transform.position.z);

            // Card Hover sound effect:
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Card_Hover");
            return;
        }

        if (this.gameObject.GetComponent<CardDisplay>().card.cardName != "placeholder")
        {
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
    }

    // On Mouse down click: 
    // (any click, a boolean tracks whether the click is to activate or deactivate something)
    public void OnPointerDown(PointerEventData pointerEventData)
    {

        if (this.gameObject.GetComponent<CardDisplay>().card.cardName != "placeholder")
        {
            if (!clicked) {
                if(!viewingCard) {
                    // cardSelected = !cardSelected;
                    cardSelected = true;
                    if(cardSelected) {
                        if (this.gameObject.GetComponent<CardDisplay>().card.cardName == "BottleRocket")
                        {
                            Debug.Log("Bottle Rocket Menu");
                            canHover = false;
                            clicked = true;
                            transform.localScale = cachedScale;
                            gameObject.transform.position = originalPos;
                            specialCardMenu.gameObject.SetActive(true);
                            if (SceneManager.GetActiveScene().name != "TitleMenu")
                            {
                                highlighted.gameObject.SetActive(true);
                            }
                        } else
                        {
                            canHover = false;
                            clicked = true;
                            transform.localScale = cachedScale;
                            gameObject.transform.position = originalPos;
                            cardMenu.gameObject.SetActive(true);
                            if (SceneManager.GetActiveScene().name != "TitleMenu")
                            {
                                highlighted.gameObject.SetActive(true);
                            }
                        } 
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
                specialCardMenu.gameObject.SetActive(false);
                if (SceneManager.GetActiveScene().name != "TitleMenu")
                {
                    highlighted.gameObject.SetActive(false);
                }
                //viewCardMenu.gameObject.SetActive(false);
                //exitMenu.SetActive(false);
            }
        }
    }
 
    // When mouse stops hovering over object:
    // (aka when mouse cursor exits GameObject's X/Y/Z boundaries)
    public void OnPointerExit(PointerEventData eventData) {

        if (this.gameObject.name == "CharacterCard")
        {
            transform.localScale = cachedScale;
            gameObject.transform.position = originalPos;
            return;
        }
        if (this.gameObject.GetComponent<CardDisplay>().card.cardName != "placeholder")
        {
            if(canHover) {
                transform.localScale = cachedScale;
                gameObject.transform.position = originalPos;
            }
        }
    }

    public void ViewCard() {
        viewingCard = true;
        if(cardSelected) {
            cardMenu.gameObject.SetActive(false);
            if(specialCardMenu != null)
            {
                // solves null reference exception
                specialCardMenu.gameObject.SetActive(false);
            }
            // let's try this
            this.transform.SetSiblingIndex(this.transform.parent.parent.transform.childCount - 1);
            this.transform.SetParent(viewingCardObject.transform);
            this.transform.localRotation = Quaternion.identity;
            if (SceneManager.GetActiveScene().name != "TitleMenu")
            {
                // for game scene
                highlighted.gameObject.SetActive(false);
                this.transform.localScale = new Vector3(3f, 3f, 3f);
            } else
            {
                // for title menu
                this.transform.localScale = new Vector3(5f, 5f, 5f);
            }
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
            if (SceneManager.GetActiveScene().name != "TitleMenu")
            {
                highlighted.gameObject.SetActive(false);
            }
            //highlighted.gameObject.SetActive(false);
        }
    }

    public void resetCard() {
        canHover = true;
        specialCardMenu.gameObject.SetActive(false);
        transform.localScale = cachedScale;
        gameObject.transform.position = originalPos;
        clicked = false;
        cardSelected = false;
        highlighted.gameObject.SetActive(false);
    }

    public void ChooseAttackPlayer() {
        viewingCard = true;
        if(cardSelected) {
            cardMenu.gameObject.SetActive(false);
            attackMenu.SetActive(true);
        }
    }
}
