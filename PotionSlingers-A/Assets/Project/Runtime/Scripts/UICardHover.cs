using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class UICardHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    Vector3 scaled;
    // Start is called before the first frame update
    void Start()
    {
        scaled = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        transform.DOScale(scaled, 0.01f).SetId(gameObject.name);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        transform.DOScale(scaled.x * 1.1f, 0.15f).SetId(gameObject.name);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Card_Hover");
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        transform.DOScale(scaled, 0.15f).SetId(gameObject.name);
    }
}
