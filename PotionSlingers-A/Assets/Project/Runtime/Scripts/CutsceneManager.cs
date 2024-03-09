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
    public List<GameObject> stage1Objects = new List<GameObject>();
    public List<GameObject> stage2Objects = new List<GameObject>();
    public List<GameObject> stage3Objects = new List<GameObject>();
    SaveData saveData;

    // Start is called before the first frame update
    void Start()
    {
        saveData = SaveSystem.LoadGameData();
        /*
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
        */

        handlePath();
    }

    public void moveMap()
    {
        transform.DOMoveX(4000, 1);
    }

    public IEnumerator displayPath()
    {
        saveData = SaveSystem.LoadGameData();

        switch (saveData.stage)
        {
            case 1:
                Debug.Log("Stage 1!!!");
                stage2Button.SetActive(false);
                stage3Button.SetActive(false);

                foreach(GameObject obj in stage1Objects)
                {
                    yield return new WaitForSeconds(0.3f);
                    // obj.SetActive(true);
                    obj.GetComponent<CanvasGroup>().alpha = 1;
                }

                stage1Button.SetActive(true);
                break;

            case 2:
                Debug.Log("Stage 2!!!");
                
                stage1Button.SetActive(false);
                stage3Button.SetActive(false);

                foreach (GameObject obj in stage1Objects)
                {
                    // yield return new WaitForSeconds(0.3f);
                    obj.GetComponent<CanvasGroup>().alpha = 1;
                }

                foreach (GameObject obj in stage2Objects)
                {
                    yield return new WaitForSeconds(0.3f);
                    obj.GetComponent<CanvasGroup>().alpha = 1;
                }
                stage2Button.SetActive(true);
                break;

            case 3:
                Debug.Log("Stage 3!!!");
                
                stage2Button.SetActive(false);
                stage1Button.SetActive(false);
                foreach (GameObject obj in stage1Objects)
                {
                    // yield return new WaitForSeconds(0.3f);
                    obj.GetComponent<CanvasGroup>().alpha = 1;
                }

                foreach (GameObject obj in stage2Objects)
                {
                    // yield return new WaitForSeconds(0.3f);
                    obj.GetComponent<CanvasGroup>().alpha = 1;
                }
                foreach (GameObject obj in stage3Objects)
                {
                    yield return new WaitForSeconds(0.3f);
                    obj.GetComponent<CanvasGroup>().alpha = 1;
                }
                stage3Button.SetActive(true);
                break;

            default:
                break;
        }
    }

    public void handlePath()
    {
        saveData = SaveSystem.LoadGameData();

        StartCoroutine(displayPath());
        /*
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
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
