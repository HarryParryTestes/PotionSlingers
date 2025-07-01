using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public HoverBox hoverBox;
    public CardPlayer cp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (cp.abilityBar.GetComponent<Image>().fillAmount == 1)
        {
            Debug.Log("Bar full!");
            cp.abilityBar.GetComponent<Image>().fillAmount = 0;
            cp.doAbility();
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
