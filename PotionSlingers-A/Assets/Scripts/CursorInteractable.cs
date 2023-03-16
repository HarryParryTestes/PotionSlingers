using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorInteractable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private MyNetworkManager game;
    private MyNetworkManager Game
    {
        get
        {
            if (game != null)
            {
                return game;
            }
            return game = MyNetworkManager.singleton as MyNetworkManager;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Game.pointCursor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Game.obsidianCursor();
    }
}
