using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardMover : MonoBehaviour, IPointerDownHandler
{

    public GameObject mask;
    // public CanvasGroup canvasGroup;
    public bool clicked = false;
    Vector3 originalPosition;
    int previousSiblingIndex;
    float speed = .75f;

    // public Renderer renderer;



    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        previousSiblingIndex = transform.GetSiblingIndex();
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

    public void Unmask()
    {
        UnityEngine.Debug.Log("Mask clicked, not the card!");
        transform.parent.GetChild(transform.parent.childCount - 1).GetComponent<CardMover>().checkClick();
        if (transform.parent.GetChild(transform.parent.childCount - 1).GetComponent<CardMover>().clicked == false)
        {
            transform.parent.GetChild(transform.parent.childCount - 1).GetComponent<CardMover>().StartCoroutine(SibIndex());
        }
    }

    public void checkClick()
    {
        if (!clicked)
        {
            clicked = true;
            // StopCoroutine(SibIndex());
            // Vector3 originalPosition = transform.position;
            transform.SetSiblingIndex(transform.parent.childCount - 1);
            // transform.DOMoveX(10, 5);

            Vector3[] vec = { new Vector3(-66, 0, 0) };

            // TODO: change this to a Vector that is the middle of the screen, I think there's move local or move global? 
            // transform.DOMove(new Vector3(0, 5, 5), 1);
            transform.DOLocalPath(vec, speed, PathType.Linear);
            transform.DOScale(5f, speed);
            mask.SetActive(true);
            // mask.GetComponent<Image>().material.DOFade(1f, speed);
            // canvasGroup.DOFade(1f, speed);
            // StartCoroutine(MaskFade());
        }
        else
        {
            clicked = false;
            // Vector3 originalPosition = transform.position;
            // transform.DOMoveX(10, 5);
            transform.DOMove(originalPosition, speed);
            transform.DOScale(3f, speed);
            // mask.GetComponent<Image>().material.DOFade(0.5f, speed);
            // canvasGroup.DOFade(0f, speed);
            mask.SetActive(false);
            // StartCoroutine(MaskFade());

            // may want a small coroutine to set this after the card has finished moving back
            // there's a callback you can use ONLY WITH OTHER TWEEN METHODS, otherwise use a coroutine
        }
    }

    /*
    public IEnumerator MaskFade()
    {
        if (!clicked)
        {
            yield return new WaitForSeconds(speed);
            transform.parent.GetChild(1).SetSiblingIndex(transform.parent.childCount - 2);
        } else
        {
            yield return new WaitForSeconds(speed);
            transform.parent.GetChild(transform.parent.childCount - 2).SetSiblingIndex(1);
        }
    }
    */
    

    public IEnumerator SibIndex()
    {
        yield return new WaitForSeconds(speed);
        transform.SetSiblingIndex(previousSiblingIndex);
        UnityEngine.Debug.Log("Sibling index changed");
    }

    // TRY DOTween movement in here
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        checkClick();
        if (!clicked)
        {
            StartCoroutine(SibIndex());
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
