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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void healPlayer()
    {
        // load health and essence cubes of player and heal them back to full
        SaveData saveData = SaveSystem.LoadGameData();
        saveData.playerCubes = 3;
        saveData.playerHealth = 10;
        // StartCoroutine(showHealingMessage());
        SaveSystem.SaveGameData(saveData);
    }

    public IEnumerator doIt()
    {
        SaveData saveData = SaveSystem.LoadGameData();
        saveData.transition = true;
        SaveSystem.SaveGameData(saveData);

        // DialogueManager.BarkString("I'm barking this text.", this.transform);


        card1.transform.DOMoveX(-4.5f, 1.5f);
        card2.transform.DOMoveX(4.5f, 1.5f);
        yield return new WaitForSeconds(1.5f);

        if (cutsceneManager.saveData.stage == 5)
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
