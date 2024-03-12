using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
// using PixelCrushers;

public class CutsceneManager : MonoBehaviour
{
    public GameObject stage1Button;
    public GameObject stage2Button;
    public GameObject stage3Button;
    public GameObject stage4Button;
    public GameObject stage5Button;
    public GameObject healingMessage;
    public List<GameObject> stage1Objects = new List<GameObject>();
    public List<GameObject> stage2Objects = new List<GameObject>();
    public List<GameObject> stage3Objects = new List<GameObject>();
    public List<GameObject> stage4Objects = new List<GameObject>();
    public List<GameObject> stage5Objects = new List<GameObject>();
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

    public void healPlayer()
    {
        // load health and essence cubes of player and heal them back to full
        saveData = SaveSystem.LoadGameData();
        saveData.playerCubes = 3;
        saveData.playerHealth = 10;
        StartCoroutine(showHealingMessage());
        SaveSystem.SaveGameData(saveData);
    }

    public IEnumerator showHealingMessage()
    {
        // MATTEO: Add sfx here
        healingMessage.SetActive(true);
        yield return new WaitForSeconds(2f);
        healingMessage.SetActive(false);

    }

        public void goBackToTitle()
    {
        // TODO: add some animation here
        SceneManager.LoadScene("TitleMenu");
    }

    public void moveMap()
    {
        transform.DOMoveX(35, 1);
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
                stage4Button.SetActive(false);
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
                foreach (GameObject obj in stage4Objects)
                {
                    yield return new WaitForSeconds(0.3f);
                    obj.GetComponent<CanvasGroup>().alpha = 1;
                }
                stage3Button.SetActive(true);
                break;

            case 4:
                Debug.Log("Stage 4!!!");
                
                stage2Button.SetActive(false);
                stage1Button.SetActive(false);
                stage3Button.SetActive(false);
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
                foreach (GameObject obj in stage4Objects)
                {
                    // yield return new WaitForSeconds(0.3f);
                    obj.GetComponent<CanvasGroup>().alpha = 1;
                }
                foreach (GameObject obj in stage3Objects)
                {
                    yield return new WaitForSeconds(0.3f);
                    obj.GetComponent<CanvasGroup>().alpha = 1;
                }
                stage4Button.SetActive(true);
                stage5Button.SetActive(true);
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
