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
        manager.setOPInt(id);
        Throw();
    }

    public void Throw()
    {
        _throwPotion?.Invoke();
    }
}
