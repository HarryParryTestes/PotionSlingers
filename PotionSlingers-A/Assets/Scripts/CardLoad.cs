using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardLoad : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] GameEvent _loadPotion;
    public GameManager manager;
    public CardDisplay cd;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        manager.setLoadedInt(cd.card.cardName);
        Throw();
    }

    public void Throw()
    {
        _loadPotion?.Invoke();
    }
}