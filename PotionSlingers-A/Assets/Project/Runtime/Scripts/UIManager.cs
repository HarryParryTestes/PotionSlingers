using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(Screen.width == 1280)
        {
            if(this.gameObject.name == "Scroll View" || this.gameObject.name == "Card1" ||
                this.gameObject.name == "Card2" || this.gameObject.name == "LeftCard" ||
                this.gameObject.name == "RightCard")
            {
                this.transform.localScale = new Vector3(this.transform.localScale.x * 1.12f, this.transform.localScale.y * 1.12f, 0);
                return;
            }

            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
