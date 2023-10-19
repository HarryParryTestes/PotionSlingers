using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class CardThrow : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] GameEvent _throwPotion;
    public GameManager manager;

    public int id;
    public string username;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        CharacterDisplay opCharacter = this.gameObject.GetComponent<CharacterDisplay>();
        // username = opCharacter.gameObject.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text.ToString();
        username = opCharacter.name;
        Debug.Log("Username: " + username);
         
        Debug.Log("Attacking Char: " + opCharacter.character.cardName);
        manager.setOPName(username);
        // manager.setOPInt(id);
        Throw();
    }

    public void throwCard()
    {
        CharacterDisplay opCharacter = this.gameObject.GetComponent<CharacterDisplay>();
        // username = opCharacter.gameObject.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text.ToString();
        username = opCharacter.name;
        Debug.Log("Username: " + username);

        Debug.Log("Attacking Char: " + opCharacter.character.cardName);
        manager.setOPName(username);
        // manager.setOPInt(id);
        Throw();
    }

    public void Throw()
    {
        _throwPotion?.Invoke();
    }
}
