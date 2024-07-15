using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;

[System.Serializable]
public class CharacterDisplay : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
    public GameObject parentObject;
    public GameObject menu;
    public bool viewed = false;
    public bool clicked;
    public bool canBeFlipped = false;
    public bool uniqueCardUsed = false;

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
        parentObject = this.transform.parent.gameObject;
        rt = transform.GetComponent<RectTransform>();
        cachedScale = transform.localScale;
        originalPos = gameObject.transform.position;
        originalRotation = this.transform.localRotation;
        //menu = GameObject.Find("CardMenu");
        viewCardMenu = this.transform.Find("ViewCardMenu");
        viewingCardObject = GameObject.Find("ViewingCard");
        artworkImage = this.GetComponent<Image>();
        // reworking this to start using animated sprites
        // artworkImage.sprite = character.image;
        character.flipped = false;
    }

    public void onView()
    {
        DOTween.Pause(gameObject.name);
        viewed = true;
        menu.SetActive(false);
        Vector3 pos = new Vector3(960 * GameManager.manager.widthRatio, 660 * GameManager.manager.heightRatio, 0);
        transform.DOMove(pos, 0.5f).SetId(gameObject.name);
        transform.DOScale(1.75f, 0.5f).SetId(gameObject.name);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        // menu.SetActive(false);
        // transform.DOMoveY(50f, 0.6f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Game.pointCursor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Game.obsidianCursor();
    }

    public void updateCharacter(Character character)
    {
        Debug.Log("Updating character image");
        artworkImage = this.GetComponent<Image>();
        this.character = character;
        this.artworkImage.sprite = character.image;
    }

    public void onCharacterClick(string character)
    {
        if (character == "Carnival")
        {
            Debug.Log("Add carnival sprite here!");
            return;
        }

        Debug.Log("Send CharReq");
        foreach (Character character2 in MainMenu.menu.characters)
        {
            if (character2.cardName == character)
            {
                Debug.Log(character + " chosen");
                Game.storyModeCharName = character;
                updateCharacter(character2);
            }
        }
    }

    public void menuCheck()
    {
        if (viewed)
        {
            DOTween.Pause(gameObject.name);
            viewed = false;
            menu.SetActive(false);
            transform.DOMove(originalPos, 0.5f).SetId(gameObject.name);
            transform.DOScale(1.25f, 0.5f).SetId(gameObject.name);
        }

        if (clicked)
        {
            clicked = false;
            menu.SetActive(true);
            if (Game.tutorial)
            {
                transform.DOMoveY(400f * GameManager.manager.heightRatio, 0.6f);
            } else
            {
                transform.DOMoveY(515f * GameManager.manager.heightRatio, 0.6f);
            }
        } else
        {
            clicked = true;
            menu.SetActive(false);
            transform.DOMoveY(50f, 0.6f);
        }
    }

    public void flipCard()
    {
        if (Game.tutorial)
        {
            GameManager.manager.StartCoroutine(GameManager.manager.waitThreeSeconds(GameManager.manager.dialog));
        }
        clicked = true;
        menu.SetActive(false);

        if (!canBeFlipped)
        {
            GameManager.manager.sendMessage("You can't flip your character just yet!");
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

        this.transform.SetParent(parentObject.transform);
        //this.transform.SetParent(GameManager.manager.md1.transform);
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
