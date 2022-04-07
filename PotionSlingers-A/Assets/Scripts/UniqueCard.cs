using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unique Card", menuName = "Unique Card")]
public class UniqueCard : ScriptableObject
{
    public string cardName;
    public Sprite cardSprite;
    public string character;
}
