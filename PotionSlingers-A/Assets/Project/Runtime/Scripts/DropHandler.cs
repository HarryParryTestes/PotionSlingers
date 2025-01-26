using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class DropHandler : MonoBehaviour, IDropHandler
{

    public TreasureMenu treasureMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Drop is happening!!!");
        treasureMenu.addTreasure(eventData.pointerDrag.GetComponent<CardDisplay>());
        eventData.pointerDrag.transform.position = eventData.pointerDrag.GetComponent<CardHover>().originalPosition;
        // blocksraycasts true
        eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        GetComponent<Image>().CrossFadeAlpha(0.5f, 0.3f, true);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        GetComponent<Image>().CrossFadeAlpha(1f, 0.3f, true);
    }
}
