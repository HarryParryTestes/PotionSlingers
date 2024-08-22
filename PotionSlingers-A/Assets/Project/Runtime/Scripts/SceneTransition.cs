using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public GameObject card1;
    public GameObject card2;
    public GameObject loadingScreen;
    public GameObject text;
    public CutsceneManager cutsceneManager;
    public Button saltButton;
    public Button crowButton;
    public Button singeButton;
    public Button demoOverButton;
    public Image background;
    public GameObject treasureMenu;
    public Scrollbar scrollRectHorizontal;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Scene Transition Start is triggering!");
        // set the position of the scrollbar to change depending on the number of stages cleared
        SaveData saveData = SaveSystem.LoadGameData();
        // scrollRectHorizontal.normalizedPosition = new Vector2(0, (saveData.stage * 0.1f));
        switch (saveData.stage)
        {
            case 1:
                scrollRectHorizontal.value = 0;
                break;
            default:
                scrollRectHorizontal.value = saveData.stage * 0.08f;
                // scrollRectHorizontal.normalizedPosition = Vector2.zero;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void healPlayer()
    {
        // load health and essence cubes of player and heal them back to full
        // CHANGING THIS TO ONLY GIVE YOU ONE ESSENCE CUBE BACK
        SaveData saveData = SaveSystem.LoadGameData();
        // saveData.playerCubes++;
        saveData.playerHealth = 10;
        // StartCoroutine(showHealingMessage());
        SaveSystem.SaveGameData(saveData);
    }

    public IEnumerator doIt()
    {
        SaveData saveData = SaveSystem.LoadGameData();
        saveData.transition = true;
        saveData.selectedStage = true;
        SaveSystem.SaveGameData(saveData);

        // DialogueManager.BarkString("I'm barking this text.", this.transform);


        card1.transform.DOMoveX(-4.5f, 1f);
        card2.transform.DOMoveX(4.5f, 1f);
        yield return new WaitForSeconds(1f);

        if (saveData.visitedEnemies.Contains("Singelotte"))
            SceneManager.LoadScene("TitleMenu");
        else
            SceneManager.LoadScene("TownCenter");
        /*
        card1.transform.DOMoveX(-4.5f, 1.5f);
        card2.transform.DOMoveX(4.5f, 1.5f);
        yield return new WaitForSeconds(1.75f);
        loadingScreen.SetActive(true);
        text.SetActive(true);
        card1.transform.DOMoveX(-15f, 1.5f);
        card2.transform.DOMoveX(15f, 1.5f);
        yield return new WaitForSeconds(1.6f);
        
        if (cutsceneManager.saveData.stage == 5)
            SceneManager.LoadScene("TitleMenu");
        else
            SceneManager.LoadScene("TownCenter");
            */
    }

    public void doTransition()
    {
        StartCoroutine(doIt());
    }
}
