using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardThrow : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] GameEvent _throwPotion;
    public GameManager manager;

    public int id;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        CharacterDisplay opCharacter = this.gameObject.GetComponent<CharacterDisplay>();
        Debug.Log("Attacking Char: " + opCharacter.character.cardName);
        manager.setOPName(opCharacter.character.cardName);
        // manager.setOPInt(id);
        Throw();
    }

    public void Throw()
    {
        _throwPotion?.Invoke();
    }
}
