using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public TMPro.TextMeshProUGUI resolutionText;
    public TMPro.TextMeshProUGUI fullscreenText;
    public List<Vector2> resolutions = new List<Vector2>();
    // index of 1920 x 1080 which is the setting that the menu should start with
    public int index = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleFullscreen()
    {
        if(fullscreenText.text == "FULLSCREEN")
        {
            fullscreenText.text = "WINDOWED";
        } else
        {
            fullscreenText.text = "FULLSCREEN";
        }
    }

    public void applyChanges()
    {
        bool full = fullscreenText.text == "FULLSCREEN";
        Debug.Log("Changing resolution to " + (int)resolutions[index].x + "x" + (int)resolutions[index].y);
        Screen.SetResolution((int)resolutions[index].x, (int)resolutions[index].y, full);
    }

    public void upResolution()
    {
        index++;
        if(index > resolutions.Count - 1)
        {
            index = resolutions.Count - 1;
        }

        resolutionText.text = resolutions[index].x.ToString() + "x" + resolutions[index].y.ToString();
    }

    public void downResolution()
    {
        index--;
        if (index < 0)
        {
            index = 0;
        }

        resolutionText.text = resolutions[index].x.ToString() + "x" + resolutions[index].y.ToString();
    }
}
