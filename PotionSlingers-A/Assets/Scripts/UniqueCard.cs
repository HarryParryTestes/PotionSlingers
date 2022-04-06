using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "New Unique Card", menuName = "Unique Card")]
public class UniqueCard : ScriptableObject
{
    // Start is called before the first frame update
    public string cardName;
    public Sprite cardSprite;
    public string desc;
    public string character;
}
