using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Collections.Specialized;

public class CardMover : MonoBehaviour, IPointerDownHandler
{

    public bool clicked = false;
    Vector3 originalPosition;
    int previousSiblingIndex;


    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        previousSiblingIndex = transform.GetSiblingIndex();
    }

    public void MoveCardToCenter()
    {

    }

    /*
    // On mouse hovering over object: 
    // (aka when mouse cursor enters GameObject's X/Y/Z boundaries)
    public void OnPointerEnter(PointerEventData eventData)
    {

        
    }
    */

    // On Mouse down click: 
    // (any click, a boolean tracks whether the click is to activate or deactivate something)

    // TRY DOTween movement in here
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (!clicked)
        {
            clicked = true;
            // Vector3 originalPosition = transform.position;
            transform.SetSiblingIndex(6);
            // transform.DOMoveX(10, 5);

            // TODO: change this to a Vector that is the middle of the screen, I think there's move local or move global? 
            transform.DOMove(new Vector3(0, 5, 5), 1);
        }
        else
        {
            clicked = false;
            // Vector3 originalPosition = transform.position;
            // transform.DOMoveX(10, 5);
            transform.DOMove(originalPosition, 1);

            // may want a small coroutine to set this after the card has finished moving back
            transform.SetSiblingIndex(previousSiblingIndex);
        }
        
    }

    /*
    // When mouse stops hovering over object:
    // (aka when mouse cursor exits GameObject's X/Y/Z boundaries)
    public void OnPointerExit(PointerEventData eventData)
    {

        if (SceneManager.GetActiveScene().name != "TitleMenu")
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
                // switch cursor back from pointing
                Game.obsidianCursor();
                if (canHover)
                {
                    transform.localScale = cachedScale;
                    gameObject.transform.position = originalPos;
                }
            }
        }

    }
    */
}
