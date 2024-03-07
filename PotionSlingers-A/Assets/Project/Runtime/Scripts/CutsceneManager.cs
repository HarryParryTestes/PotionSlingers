using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
// using PixelCrushers;

public class CutsceneManager : MonoBehaviour
{
    public GameObject stage1Button;
    public GameObject stage2Button;
    public GameObject stage3Button;
    SaveData saveData;

    // Start is called before the first frame update
    void Start()
    {
        saveData = SaveSystem.LoadGameData();

        switch (saveData.stage)
        {
            case 1:
                Debug.Log("Stage 1!!!");
                stage1Button.SetActive(true);
                stage2Button.SetActive(false);
                stage3Button.SetActive(false);
                break;

            case 2:
                Debug.Log("Stage 2!!!");
                stage2Button.SetActive(true);
                stage1Button.SetActive(false);
                stage3Button.SetActive(false);
                break;

            case 3:
                Debug.Log("Stage 3!!!");
                stage3Button.SetActive(true);
                stage2Button.SetActive(false);
                stage1Button.SetActive(false);
                break;

            default:
                break;
        }
    }

    public void moveMap()
    {
        transform.DOMoveX(4000, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
