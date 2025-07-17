using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public HoverBox hoverBox;
    public CardPlayer cp;
    bool unfill = false;

    public MyNetworkManager game;
    public MyNetworkManager Game
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

    void FixedUpdate()
    {
        if (unfill)
            cp.barImage.fillAmount -= 0.025f;

        if (cp.barImage.fillAmount == 0)
            unfill = false;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (gameObject.name == "Character_Profile")
            return;

        if (!Game.multiplayer && GameManager.manager.myPlayerIndex != 0)
        {
            Debug.Log("Not your turn!");
            return;
        }

        if (cp.barImage.fillAmount == 1)
        {
            Debug.Log("Bar full!");
            // cp.abilityBar.GetComponent<Image>().fillAmount = 0;
            unfill = true;
            cp.doSuper();
            cp.ability = 0;
        }
        else
        {
            Debug.Log("Not full");
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        // hoverbox code in here!
        if (hoverBox != null)
        {
            hoverBox.gameObject.SetActive(true);
            hoverBox.GetComponent<HoverBox>().UpdateAbility(cp);
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        // hoverbox code in here!
        if (hoverBox != null)
        {
            hoverBox.gameObject.SetActive(false);
        }
    }
}
