using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopMarketBuy : MonoBehaviour
{
    [SerializeField] GameEvent Market1Buy;
    public GameManager manager;
    // Start is called before the first frame update
    void OnMouseDown()
    {
        Throw();
    }

    // Update is called once per frame
    void Throw()
    {
        Market1Buy?.Invoke();
    }
}
