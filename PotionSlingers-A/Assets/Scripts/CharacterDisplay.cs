using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CharacterDisplay : MonoBehaviour, IPointerDownHandler
{

    Vector3 cachedScale; // Tracks original size of Card (for hovering as it manipulates scale of Card).
    Vector3 originalPos; // Tracks original position of Card.
    Vector3 mousePos; // Tracks current mouse cursor position.
    Quaternion originalRotation;

    RectTransform rt;

    public Character character;
    public Image artworkImage;
    Transform viewCardMenu;
    GameObject viewingCardObject;
    public GameObject menu;
    public bool clicked = false;
    public bool canBeFlipped = false;

    private MyNetworkManager game;
    private MyNetworkManager Game
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

    // Start is called before the first frame update
    void Start()
    {
        rt = transform.GetComponent<RectTransform>();
        cachedScale = transform.localScale;
        originalPos = gameObject.transform.position;
        originalRotation = this.transform.localRotation;
        //menu = GameObject.Find("CardMenu");
        viewCardMenu = this.transform.Find("ViewCardMenu");
        viewingCardObject = GameObject.Find("ViewingCard");
        artworkImage = this.GetComponent<Image>();
        artworkImage.sprite = character.image;
        character.flipped = false;
    }

    public void updateCharacter(Character character)
    {
        artworkImage = this.GetComponent<Image>();
        this.character = character;
        this.artworkImage.sprite = character.image;
    }

    public void onCharacterClick(string character)
    {
        Debug.Log("Send CharReq");
        foreach (Character character2 in MainMenu.menu.characters)
        {
            if (character2.cardName == character)
            {
                Debug.Log(character + " chosen");
                updateCharacter(character2);
            }
        }
    }

    public void menuCheck()
    {
        if (!clicked)
        {
            clicked = true;
            menu.SetActive(true);
        } else
        {
            clicked = false;
            menu.SetActive(false);
        }
    }

    public void flipCard()
    {
        if (Game.tutorial)
        {
            GameManager.manager.StartCoroutine(GameManager.manager.waitThreeSeconds(GameManager.manager.dialog));
        }
        clicked = false;

        if (!canBeFlipped)
        {
            return;
        }
        character.flipped = !character.flipped;
        
        if (character.flipped)
        {
            Debug.Log("You just flipped the card!");
            this.artworkImage.sprite = character.flippedImage;
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Character_Flip");
        } else 
        {
            Debug.Log("You just flipped the card back over!");
            this.artworkImage.sprite = character.image;
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Character_Flip");
        }
    }

    public void ViewCard()
    {
        clicked = false;
        //cardMenu.gameObject.SetActive(false);
        menu.SetActive(false);
        this.transform.SetParent(viewingCardObject.transform);
        this.transform.localRotation = Quaternion.identity;
        this.transform.localScale = new Vector3(2f, 2f, 2f);
        this.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        viewCardMenu.gameObject.SetActive(true);
        this.transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
    }

    public void resetView()
    {
        viewCardMenu.gameObject.SetActive(false);

        //this.transform.SetParent(parentObject.transform);
        this.transform.SetParent(GameManager.manager.md1.transform);
        this.transform.localRotation = Quaternion.identity;
        transform.localScale = cachedScale;
        gameObject.transform.position = originalPos;
        //highlighted.gameObject.SetActive(false);
    }


    //A placeholder for code that will allow the character cards to flip
    //after completing the flip criteria
    //For now, clicking the card allows it to flip
    public void OnPointerDown(PointerEventData pointerEventData) 
    {
        if(SceneManager.GetActiveScene().name == "TitleMenu" && MainMenu.menu.flippable)
        {
            character.flipped = !character.flipped;
            
            if (character.flipped)
            {
                Debug.Log("You just flipped the card!");
                artworkImage.sprite = character.flippedImage;
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Character_Flip");
            } else 
            {
                Debug.Log("You just flipped the card back over!");
                artworkImage.sprite = character.image;
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Character_Flip");
            }
            
            return;
        }


        //character.flipped = !character.flipped;
        /*
        if (character.flipped)
        {
            Debug.Log("You just flipped the card!");
            artworkImage.sprite = character.flippedImage;
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Character_Flip");
        } else 
        {
            Debug.Log("You just flipped the card back over!");
            artworkImage.sprite = character.image;
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Character_Flip");
        }
        */
    }
}
