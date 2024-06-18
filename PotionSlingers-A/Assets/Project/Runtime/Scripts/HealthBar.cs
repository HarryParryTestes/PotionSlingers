using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Image image;
    public Image redBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(redBar.fillAmount < image.fillAmount + 0.01f)
        {
            image.fillAmount -= 0.004f;
        }

        if(redBar.fillAmount > image.fillAmount + 0.01f)
        {
            image.fillAmount += 0.004f;
        }
    }
}