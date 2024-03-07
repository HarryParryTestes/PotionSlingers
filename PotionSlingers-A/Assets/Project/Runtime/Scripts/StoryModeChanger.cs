using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryModeChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeFromStoryModeScene()
    {
        // add animation in here
        // instantiate the card backs as a prefab and have it do the animation
        SceneManager.LoadScene("TownCenter");
        //networkManager.ServerChangeScene("TownCenter");
    }
}
