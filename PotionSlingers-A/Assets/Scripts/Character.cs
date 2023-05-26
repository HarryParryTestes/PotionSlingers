using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public int id;
    public string cardName;
    public string desc;
    public Sprite image;
    public string flipTask;
    public bool flipped = false;
    public Sprite flippedImage;
    public string flippedDesc;
    public UniqueCard uniqueCard;
    
    /*
    public void OnPointerDown(PointerEventData pointerEventData) {
        flipped = !flipped;
        Debug.Log(cardName + " has been clicked. Flipped is now: " + flipped); 
    }
    */
}
