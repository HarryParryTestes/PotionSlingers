using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class ResetDemoButton : MonoBehaviour
{
   public GameObject reset;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(reset);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("TitleMenu");
        }
    }
}
