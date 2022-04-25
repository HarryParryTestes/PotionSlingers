using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardThrow : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] GameEvent _throwPotion;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        Throw();
    }

    public void Throw()
    {
        _throwPotion?.Invoke();
    }
}
