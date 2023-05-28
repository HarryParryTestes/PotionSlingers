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
        if(fullscreenText.text == "Fullscreen")
        {
            fullscreenText.text = "Windowed";
        } else
        {
            fullscreenText.text = "Fullscreen";
        }
    }

    // be careful with this
    public void deletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("DELETING");
    }

    public void loadOptions()
    {
        if (PlayerPrefs.HasKey("fullscreen"))
        {
            Debug.Log("Loading options...");
            int resolutionX = PlayerPrefs.GetInt("resolutionX");
            int resolutionY = PlayerPrefs.GetInt("resolutionY");

            resolutionText.text = resolutionX.ToString() + "x" + resolutionY.ToString();
            fullscreenText.text = PlayerPrefs.GetString("fullscreen");
        }
    }

    public void applyChanges()
    {
        bool full = fullscreenText.text == "Fullscreen";
        Debug.Log("Changing resolution to " + (int)resolutions[index].x + "x" + (int)resolutions[index].y);
        Screen.SetResolution((int)resolutions[index].x, (int)resolutions[index].y, full);

        PlayerPrefs.SetInt("resolutionX", (int)resolutions[index].x);
        PlayerPrefs.SetInt("resolutionY", (int)resolutions[index].y);

        PlayerPrefs.SetString("fullscreen", fullscreenText.text);
        Debug.Log("Saving changes");
        PlayerPrefs.Save();

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
