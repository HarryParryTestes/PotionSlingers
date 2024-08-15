using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusMenuUIManager : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    private float desiredAlpha;
    private float currentAlpha;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        // set alpha of canvasGroup to 0 in CardPlayer and GameManager script
        // canvasGroup.alpha = 0;
        // image.GetComponent<CanvasRenderer>().SetAlpha(0f);
        // Invoke("changeAlpha", 2f);
        // image.GetComponent<CanvasRenderer>().SetAlpha(0f);
        // image.CrossFadeAlpha(1, 0.5f, true);
    }

    public void setAlphaZero()
    {
        GetComponent<CanvasGroup>().alpha = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<CanvasGroup>().alpha += Time.deltaTime * 1.5f;
    }

    void changeAlpha()
    {
        while(GetComponent<CanvasGroup>().alpha < 1)
        {
            // canvasGroup.alpha += Time.deltaTime / 1000;
        }
        // image.CrossFadeAlpha(1, 0.5f, true);
    }
}
