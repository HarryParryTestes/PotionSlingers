using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class CardHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        transform.DOScale(0.9f, 0.15f).SetId(gameObject.name);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        transform.DOScale(0.8f, 0.15f).SetId(gameObject.name);
    }
}
