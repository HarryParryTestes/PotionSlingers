using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class CardThrow : MonoBehaviour
{
    [SerializeField] GameEvent _throwPotion;
    public GameManager manager;

    public int id;
    public string username;

    /*
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        CardPlayer cp = this.gameObject.GetComponent<CardPlayer>();
        // username = opCharacter.gameObject.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text.ToString();
        username = cp.name;
        Debug.Log("Username: " + username);
         
        // Debug.Log("Attacking Char: " + opCharacter.character.cardName);
        manager.setOPName(username);
        // manager.setOPInt(id);
        Throw();
    }
    */

    public void throwCard()
    {
        CardPlayer cp = this.gameObject.GetComponent<CardPlayer>();
        // username = opCharacter.gameObject.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text.ToString();
        username = cp.name;
        Debug.Log("Username: " + username);

        // Debug.Log("Attacking Char: " + opCharacter.character.cardName);
        // manager.setOPName(username);
        manager.tempPlayer = cp;
        // manager.setOPInt(id);
        Throw();
    }

    public void Throw()
    {
        _throwPotion?.Invoke();
    }
}
