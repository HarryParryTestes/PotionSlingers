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
    bool clicked = false;
    Vector3 originalPosition;
    int previousSiblingIndex;
    float speed = .5f;

    Image mask1;
    Image mask2;

    // public Renderer renderer;



    // Start is called before the first frame update
    void Start()
    {
        // mask = GameObject.Find("CardGalleryMenu/Mask");
        originalPosition = transform.position;
        previousSiblingIndex = transform.GetSiblingIndex();
        mask1 = mask.transform.GetChild(1).GetComponent<Image>();
        mask2 = mask.transform.GetChild(2).GetComponent<Image>();
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

            Vector3[] vec = { new Vector3(-66, 50, 0) };

            // TODO: change this to a Vector that is the middle of the screen, I think there's move local or move global? 
            // transform.DOMove(new Vector3(0, 5, 5), 1);
            transform.DOLocalPath(vec, speed, PathType.Linear);
            transform.DOScale(7f, speed);
            mask.SetActive(true);
            // mask1.color = new Color(mask1.color.r, mask1.color.g, mask1.color.b, 0f);
            // mask2.color = new Color(mask2.color.r, mask2.color.g, mask2.color.b, 0f);
            mask1.GetComponent<CanvasRenderer>().SetAlpha(0f);
            mask2.GetComponent<CanvasRenderer>().SetAlpha(0f);

            mask1.CrossFadeAlpha(1, speed, true);
            mask2.CrossFadeAlpha(1, speed, true);
        }
        else
        {
            clicked = false;
            transform.DOMove(originalPosition, speed);
            transform.DOScale(3f, speed);
            mask1.CrossFadeAlpha(0, speed, true);
            mask2.CrossFadeAlpha(0, speed, true);
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
        mask.SetActive(false);
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
