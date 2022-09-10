using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CharacterDisplay : MonoBehaviour, IPointerDownHandler
{
    public Character character;
    public Image artworkImage;

    // Start is called before the first frame update
    void Start()
    {
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

    //A placeholder for code that will allow the character cards to flip
    //after completing the flip criteria
    //For now, clicking the card allows it to flip
    public void OnPointerDown(PointerEventData pointerEventData) 
    {
        if(SceneManager.GetActiveScene().name == "TitleMenu")
        {
            return;
        }

        character.flipped = !character.flipped;
        
        if(character.flipped)
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
    }
}
