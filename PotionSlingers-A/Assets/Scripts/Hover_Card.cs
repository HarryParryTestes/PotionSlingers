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

    public Animator animator;
    public Transform cardMenu;
    Transform viewCardMenu;
    Transform specialCardMenu;
    Transform highlighted;
    GameObject holster;
    GameObject parentObject;
    GameObject attackMenu;
    GameObject viewingCardObject;
    public bool cardSelected = false;
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
    void Start() {
        //exitMenu = GameObject.Find("EXIT VIEW");
        rt = transform.GetComponent<RectTransform>();
        cachedScale = transform.localScale;
        originalPos = gameObject.transform.position;
        originalRotation = this.transform.localRotation;

        parentObject = this.transform.parent.gameObject;
        holster = this.transform.parent.transform.parent.gameObject;
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
        if (attackMenu != null)
        {
            attackMenu.gameObject.SetActive(false);
        }
        if (highlighted != null)
        {
            highlighted.gameObject.SetActive(false);
        }
        if(specialCardMenu != null)
        {
            specialCardMenu.gameObject.SetActive(false);
        }   
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
            // does taking this line below out change anything big?
            // this.transform.parent.transform.SetSiblingIndex(this.transform.parent.parent.transform.childCount - 1);
            //transform.SetSiblingIndex(transform.childCount - 1); // Sets card 
            transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            // transform.position = new Vector3(transform.position.x, height + height/2, transform.position.z);

            Game.pointCursor();
            // Card Hover sound effect:
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Card_Hover");
            return;
        }

        if (this.gameObject.GetComponent<CardDisplay>().card.cardName != "placeholder")
        {
            // switch cursor
            Game.pointCursor();
            if (canHover) {
                float width = rt.sizeDelta.x * rt.localScale.x;
                float height = rt.sizeDelta.y * rt.localScale.y;

                // does taking this line below out change anything big?
                // this.transform.parent.transform.SetSiblingIndex(this.transform.parent.parent.transform.childCount - 1);
                // transform.SetSiblingIndex(transform.childCount - 1); // Sets card 
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
        
        if (SceneManager.GetActiveScene().name == "TitleMenu")
        {
            Debug.Log("Did this trigger");
            this.transform.localScale = new Vector3(5f, 5f, 5f);
            // this.transform.SetSiblingIndex(this.transform.parent.parent.transform.childCount - 1);
            // this.transform.SetParent(viewingCardObject.transform);
            this.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            // ViewCard();
            return;
        }
        

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
                            ViewCard();
                            // just set these false for now I'm not worrying about the highlight rn
                            if (SceneManager.GetActiveScene().name != "TitleMenu")
                            {
                                highlighted.gameObject.SetActive(false);
                            }
                        } else
                        {
                            canHover = false;
                            clicked = true;
                            transform.localScale = cachedScale;
                            gameObject.transform.position = originalPos;

                            // prevents the eye from popping up when viewing the top card of the deck
                            if(this.gameObject.name != "DeckPile")
                                cardMenu.gameObject.SetActive(true);
                            // you added this in as a funny haha like what would happen type of thing
                            // Debug.Log(originalPos);
                            // THIS IS IMPORTANT
                            ViewCard();
                            // just set these false for now I'm not worrying about the highlight rn
                            if (SceneManager.GetActiveScene().name != "TitleMenu")
                            {
                                if( highlighted != null)
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
            else if(clicked && cardSelected) {

                /*
                 * This is code that triggers when you click to make it go back to original scale
                 * Check the card menus and make sure to modify it correctly
                 */

                clicked = false;
                cardSelected = false;
                canHover = true;
                transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
                cardMenu.gameObject.SetActive(false);
                gameObject.transform.position = originalPos;
                // you added this in as a funny haha like what would happen type of thing
                if (SceneManager.GetActiveScene().name != "TitleMenu")
                {
                    if (highlighted != null)
                    {
                        highlighted.gameObject.SetActive(false);
                    }
                }
                if (specialCardMenu != null)
                {
                    specialCardMenu.gameObject.SetActive(false);
                }
                resetView();
                
                
                //viewCardMenu.gameObject.SetActive(false);
                //exitMenu.SetActive(false);
            }
            // Debug.Log(this.transform.parent.gameObject.name);
        }
    }
 
    // When mouse stops hovering over object:
    // (aka when mouse cursor exits GameObject's X/Y/Z boundaries)
    public void OnPointerExit(PointerEventData eventData) {

        if (this.gameObject.name == "CharacterCard")
        {
            transform.localScale = cachedScale;
            gameObject.transform.position = originalPos;
            Game.obsidianCursor();
            return;
        }
        if (this.gameObject.GetComponent<CardDisplay>().card.cardName != "placeholder")
        {
            // switch cursor back from pointing
            Game.obsidianCursor();
            if (canHover) {
                transform.localScale = cachedScale;
                gameObject.transform.position = originalPos;
            }
        }
    }

    public void ViewCard() {
        viewingCard = true;
        if(cardSelected) {
            // you added this in to change how clicking on a card now brings up the menu
            // cardMenu.gameObject.SetActive(false);
            // let's try this
            this.transform.SetSiblingIndex(this.transform.parent.parent.transform.childCount - 1);
            // Debug.Log(parentObject.name);
            this.transform.SetParent(viewingCardObject.transform);
            // Debug.Log(parentObject.name);
            this.transform.localRotation = Quaternion.identity;
            // Debug.Log(this.transform.parent.gameObject.name);
            // Debug.Log(parentObject.name);
            if (SceneManager.GetActiveScene().name != "TitleMenu")
            {
                // for game scene

                // check for this NRE
                if(highlighted != null)
                {
                    highlighted.gameObject.SetActive(false);
                }
                this.transform.localScale = new Vector3(3f, 3f, 3f);
            } else
            {
                // for title menu
                cardMenu.gameObject.SetActive(false);
                this.transform.localScale = new Vector3(5f, 5f, 5f);
            }
            this.transform.position = new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 0);
            // take this out to prevent the red X from popping up
            // viewCardMenu.gameObject.SetActive(true);
            this.transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
            // Debug.Log(originalPos);
        }
    }

    /*
    public IEnumerator cardAnimation()
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(2);
    }
    */

    public void resetView()
    {
        this.transform.SetParent(parentObject.transform);
        transform.localScale = cachedScale;
        gameObject.transform.position = originalPos;
        // canHover = true;
        viewingCard = false;
        viewCardMenu.gameObject.SetActive(false);
        if (specialCardMenu != null)
        {
            // solves null reference exception
            specialCardMenu.gameObject.SetActive(false);
        }
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
        // canHover = false;
    }

    /*
    public void resetView() {
        canHover = true;
        viewingCard = false;
        viewCardMenu.gameObject.SetActive(false);
        if(cardSelected) {
            transform.localScale = cachedScale;
            gameObject.transform.position = originalPos;
            // the parent object is being overwritten and you need to set it back to the holster
            parentObject = this.transform.parent.gameObject;
            this.transform.SetParent(parentObject.transform);
            this.transform.localRotation = Quaternion.identity;
            transform.localScale = cachedScale;
            gameObject.transform.position = originalPos;
            clicked = false;
            cardSelected = false; 
            // canHover = false;

            if (SceneManager.GetActiveScene().name != "TitleMenu")
            {
                if(highlighted != null)
                {
                    highlighted.gameObject.SetActive(false);
                }
            }
            //highlighted.gameObject.SetActive(false);
        }
        canHover = false;
        viewingCard = false;
        Debug.Log(originalPos);
    }
    */

    public void resetCard() {
        canHover = true;
        if(specialCardMenu != null)
            specialCardMenu.gameObject.SetActive(false);
        transform.localScale = cachedScale;
        gameObject.transform.position = originalPos;
        clicked = false;
        cardSelected = false;
        if(highlighted != null)
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
