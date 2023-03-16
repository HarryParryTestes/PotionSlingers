using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
    public static bool clicked = false; // Determines if cards can be clicked.

    public Transform cardMenu;
    public Transform viewCardMenu;
    public Transform specialCardMenu;
    Transform highlighted;
    public Transform scarpettaMenu;
    public Transform onlyViewCardMenu;
    GameObject parentObject;
    GameObject attackMenu;
    GameObject viewingCardObject;
    bool cardSelected = false;
    public static bool viewingCard = false;
    //public GameObject exitMenu;
    public MyNetworkManager game;
    public MyNetworkManager Game
    {
        get
        {
            if (game != null)
            {
                return game;
            }
            return game = MyNetworkManager.singleton as MyNetworkManager;
        }
    }

    // On startup:
    void Start()
    {
        //exitMenu = GameObject.Find("EXIT VIEW");
        rt = transform.GetComponent<RectTransform>();
        cachedScale = transform.localScale;
        originalPos = gameObject.transform.position;
        originalRotation = this.transform.localRotation;

        parentObject = this.transform.parent.gameObject;
        cardMenu = this.transform.Find("CardMenu");
        viewCardMenu = this.transform.Find("ViewCardMenu");
        scarpettaMenu = this.transform.Find("ScarpettaMenu");
        onlyViewCardMenu = this.transform.Find("OnlyViewCardMenu");
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
        if (attackMenu != null)
        {
            attackMenu.gameObject.SetActive(false);
        }
        if (highlighted != null)
        {
            highlighted.gameObject.SetActive(false);
        }
        if (specialCardMenu != null)
        {
            specialCardMenu.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (this.gameObject.name == "CharacterCard")
        {
            return;
        }

        if (this.gameObject.GetComponent<CardDisplay>().card.cardName == "placeholder")
        {
            transform.localScale = cachedScale;
            gameObject.transform.position = originalPos;
        }
    }

    // On mouse hovering over object: 
    // (aka when mouse cursor enters GameObject's X/Y/Z boundaries)
    public void OnPointerEnter(PointerEventData eventData)
    {

        if (this.gameObject.name == "TrashCardDisplay1" ||
                           this.gameObject.name == "TrashCardDisplay2" ||
                           this.gameObject.name == "TrashCardDisplay3")
        {
            Game.pointCursor();
            return;
        }

        if (SceneManager.GetActiveScene().name == "TitleMenu")
        {
            return;
        }

        // for character card
        if (this.gameObject.name == "CharacterCard")
        {
            float width = rt.sizeDelta.x * rt.localScale.x;
            float height = rt.sizeDelta.y * rt.localScale.y;
            // this.transform.parent.transform.SetSiblingIndex(this.transform.parent.parent.transform.childCount - 1);
            //transform.SetSiblingIndex(transform.childCount - 1); // Sets card 
            transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            // transform.position = new Vector3(transform.position.x, height + height/2, transform.position.z);

            Game.pointCursor();
            // Card Hover sound effect:
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Card_Hover");
            return;
        }

        if (this.gameObject.GetComponent<CardDisplay>().card.cardName != "placeholder" ||
            (this.gameObject.name != "TrashCardDisplay1" &&
                            this.gameObject.name != "TrashCardDisplay2" &&
                            this.gameObject.name != "TrashCardDisplay3"))
        {
            Game.pointCursor();
            if (canHover)
            {
                float width = rt.sizeDelta.x * rt.localScale.x;
                float height = rt.sizeDelta.y * rt.localScale.y;
                // this.transform.parent.transform.SetSiblingIndex(this.transform.parent.parent.transform.childCount - 1);
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
        /*
        if (SceneManager.GetActiveScene().name == "TitleMenu")
        {
            Debug.Log("Did this trigger");
            this.transform.localScale = new Vector3(5f, 5f, 5f);
            this.transform.SetSiblingIndex(this.transform.parent.parent.transform.childCount - 1);
            this.transform.SetParent(viewingCardObject.transform);
            // this.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            // ViewCard();
            return;
        }
        */

        if (this.gameObject.GetComponent<CardDisplay>().card.cardName != "placeholder")
        {
            if (!clicked)
            {
                if (!viewingCard)
                {
                    // cardSelected = !cardSelected;
                    cardSelected = true;
                    if (cardSelected)
                    {
                        if (this.gameObject.name == "TrashCardDisplay1" ||
                            this.gameObject.name == "TrashCardDisplay2" ||
                            this.gameObject.name == "TrashCardDisplay3")
                        {
                            if (GameManager.manager.trashDeckBonus)
                            {
                                if (cardMenu.gameObject.activeInHierarchy)
                                {
                                    canHover = true;
                                    cardMenu.gameObject.SetActive(false);
                                }
                                else
                                {
                                    canHover = true;
                                    cardMenu.gameObject.SetActive(true);
                                }

                                
                                if (highlighted != null)
                                {
                                    highlighted.gameObject.SetActive(true);
                                }
                                return;
                            }
                            else
                            {
                                if (GameManager.manager.Game.multiplayer)
                                {
                                    Debug.Log("Reached here");
                                    foreach (CardPlayer cp in GameManager.manager.players)
                                    {
                                        if (cp.isScarpetta && GameManager.manager.currentPlayerName == cp.name)
                                        {
                                            Debug.Log("Scarpetta online");
                                            if (scarpettaMenu != null)
                                            {
                                                scarpettaMenu.gameObject.SetActive(true);
                                                canHover = true;
                                                if (highlighted != null)
                                                {
                                                    highlighted.gameObject.SetActive(true);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (GameManager.manager.players[GameManager.manager.myPlayerIndex].isScarpetta)
                                    {
                                        Debug.Log("Scarpetta offline");
                                        if (scarpettaMenu != null)
                                        {
                                            if (scarpettaMenu.gameObject.activeInHierarchy)
                                            {
                                                scarpettaMenu.gameObject.SetActive(false);
                                                canHover = true;
                                            }
                                            else
                                            {
                                                scarpettaMenu.gameObject.SetActive(true);
                                                canHover = true;
                                            }
                                            if (highlighted != null)
                                            {
                                                highlighted.gameObject.SetActive(true);
                                            }
                                        }
                                        return;
                                    }
                                }
                                // add other menu in the trash displays that only allows them to view the card
                                if (onlyViewCardMenu != null)
                                {
                                    if (onlyViewCardMenu.gameObject.activeInHierarchy)
                                    {
                                        onlyViewCardMenu.gameObject.SetActive(false);
                                        canHover = true;
                                    } else
                                    {
                                        onlyViewCardMenu.gameObject.SetActive(true);
                                        canHover = true;
                                    }
                                    
                                    if (highlighted != null)
                                    {
                                        highlighted.gameObject.SetActive(true);
                                    }
                                }
                            }
                            return;
                        }

                        if (this.gameObject.GetComponent<CardDisplay>().card.cardName == "BottleRocket")
                        {
                            Debug.Log("Bottle Rocket Menu");

                            canHover = false;
                            clicked = true;
                            transform.localScale = cachedScale;
                            gameObject.transform.position = originalPos;
                            // specialCardMenu.gameObject.SetActive(true);
                            ViewCard();
                            if (SceneManager.GetActiveScene().name != "TitleMenu")
                            {
                                highlighted.gameObject.SetActive(false);
                            }
                        }
                        else
                        {
                            canHover = false;
                            clicked = true;
                            transform.localScale = cachedScale;
                            gameObject.transform.position = originalPos;
                            // THIS IS IMPORTANT
                            ViewCard();
                            // cardMenu.gameObject.SetActive(true);
                            if (SceneManager.GetActiveScene().name != "TitleMenu")
                            {
                                if (highlighted != null)
                                {
                                    highlighted.gameObject.SetActive(false);
                                }
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
            else if (clicked && cardSelected)
            {
                // Debug.Log("So that happened!!!");
                resetView();
                return;

                clicked = false;
                cardSelected = false;
                canHover = true;
                transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
                cardMenu.gameObject.SetActive(false);
                if (specialCardMenu != null)
                {
                    specialCardMenu.gameObject.SetActive(false);
                }
                if (SceneManager.GetActiveScene().name != "TitleMenu")
                {
                    if (highlighted != null)
                    {
                        highlighted.gameObject.SetActive(false);
                    }
                }

                //viewCardMenu.gameObject.SetActive(false);
                //exitMenu.SetActive(false);

            }
        }
    }

    // When mouse stops hovering over object:
    // (aka when mouse cursor exits GameObject's X/Y/Z boundaries)
    public void OnPointerExit(PointerEventData eventData)
    {

        if (this.gameObject.name == "CharacterCard")
        {
            transform.localScale = cachedScale;
            gameObject.transform.position = originalPos;
            Game.obsidianCursor();
            return;
        }
        if (this.gameObject.GetComponent<CardDisplay>().card.cardName != "placeholder")
        {
            Game.obsidianCursor();
            if (canHover)
            {
                transform.localScale = cachedScale;
                gameObject.transform.position = originalPos;
            }
        }
    }

    public void ViewCard()
    {
        viewingCard = true;
        if (cardSelected)
        {
            // this sets the market menu active
            cardMenu.gameObject.SetActive(true);
            if (specialCardMenu != null)
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

                // check for this NRE
                if (highlighted != null)
                {
                    highlighted.gameObject.SetActive(false);
                }
                this.transform.localScale = new Vector3(5f, 5f, 5f);
            }
            else
            {
                // for title menu
                this.transform.localScale = new Vector3(5f, 5f, 5f);
            }
            this.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            // taking this out should disbable the red X on the cards, just uncomment it if you want it back
            // viewCardMenu.gameObject.SetActive(true);
            this.transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
        }
    }

    public void resetView()
    {
        this.transform.SetParent(parentObject.transform);
        transform.localScale = cachedScale;
        gameObject.transform.position = originalPos;
        // Debug.Log("So this happened");
        canHover = true;
        viewingCard = false;
        // you changed this
        cardMenu.gameObject.SetActive(false);
        if (cardSelected)
        {
            clicked = false;
            cardSelected = false;
            this.transform.SetParent(parentObject.transform);
            this.transform.localRotation = Quaternion.identity;
            transform.localScale = cachedScale;
            gameObject.transform.position = originalPos;
            canHover = true;
            if (SceneManager.GetActiveScene().name != "TitleMenu")
            {
                if (highlighted != null)
                {
                    highlighted.gameObject.SetActive(false);
                }
            }
            //highlighted.gameObject.SetActive(false);
        }
    }

    public void resetCard()
    {
        canHover = true;
        viewingCard = false;
        if (specialCardMenu != null)
            specialCardMenu.gameObject.SetActive(false);
        transform.localScale = cachedScale;
        gameObject.transform.position = originalPos;
        clicked = false;
        cardSelected = false;
        if (highlighted != null)
        {
            highlighted.gameObject.SetActive(false);
        }
    }

    public void ChooseAttackPlayer()
    {
        viewingCard = true;
        if (cardSelected)
        {
            cardMenu.gameObject.SetActive(false);
            attackMenu.SetActive(true);
        }
    }
}
