using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
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
        resolutions.Clear();
        Resolution[] resolutionsList = Screen.resolutions;

        // Print the resolutions
        foreach (var res in resolutionsList)
        {
            Debug.Log(res.width + "x" + res.height);
            resolutions.Add(new Vector2(res.width, res.height));
        }
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
        index = 3;
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

        // if (SceneManager.GetActiveScene().name == "TownCenter")
            // GameManager.manager.moveUI();
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
