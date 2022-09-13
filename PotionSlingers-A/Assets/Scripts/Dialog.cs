using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dialog : MonoBehaviour, IPointerDownHandler
{
    public TMPro.TextMeshProUGUI dialogBox;
    public TMPro.TextMeshProUGUI directionBox;
    public GameObject nameTag;
    public GameObject directions;
    public int textBoxCounter = 0;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        textBoxCounter++;
        if(textBoxCounter == 1)
        {
            directions.SetActive(true);
            gameObject.SetActive(false);
            nameTag.SetActive(false);
            //dialogBox.text = "This is a test to see if this works!\nCan I get new lines too?";
        }
    }

}
