using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class MarketUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update

    // public TMPro.TextMeshProUGUI textBox;
    public GameObject textBox;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        textBox.SetActive(true);
        // textBox.gameObject.SetActive(true);
        // GameManager.manager.updateMarketUIText();
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        // textBox.gameObject.SetActive(false);
        textBox.SetActive(false);
    }
}
