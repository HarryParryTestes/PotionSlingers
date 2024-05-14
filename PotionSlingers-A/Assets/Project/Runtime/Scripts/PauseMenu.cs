using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("TitleMenu");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key was pressed");
            // paused = !paused;
            if (menu.activeInHierarchy == false)
            {
                menu.SetActive(true);
            }
            else
            {
                menu.SetActive(false);
            }

        }
    }
}
