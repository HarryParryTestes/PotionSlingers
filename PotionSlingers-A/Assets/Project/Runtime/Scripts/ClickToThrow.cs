using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClickToThrow : MonoBehaviour
{
    [SerializeField] GameEvent ThrowPotion;
    // Start is called before the first frame update
    void OnMouseDown()
    {
        Throw();
    }

    // Update is called once per frame
    void Throw()
    {
        ThrowPotion?.Invoke();
    }
}
