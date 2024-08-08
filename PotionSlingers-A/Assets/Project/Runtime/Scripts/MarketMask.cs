using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MarketMask : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnPointerClick(PointerEventData pointerEventData)
    {
        // Debug.Log("This got clicked on");

        if(GameManager.manager.marketSelected)
            GameManager.manager.moveMarket();

        GameManager.manager.moveMarketCards();
    }
}
