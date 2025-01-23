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

    // salt encounter buttons
    public Button saltButton;
    public Button boloSaltButton;
    public Button isadoreSaltButton;

    public Button crowButton;
    public Button singeButton;
    public Button demoOverButton;
    public Button crowBoloButton;
    public Button crowIsadoreButton;
    public Button crowSaltButton;

    // isadore encounter buttons
    public Button reetsIsadoreButton;
    public Button boloIsadoreButton;
    public Button saltIsadoreButton;

    // reets encounter buttons
    public Button isadoreReetsButton;
    public Button boloReetsButton;
    public Button saltReetsButton;

    // bolo encounter buttons
    public Button reetsBoloButton;
    public Button isadoreBoloButton;
    public Button saltBoloButton;

    public Image background;
    public GameObject treasureMenu;
    public Scrollbar scrollRectHorizontal;
    public TMPro.TextMeshProUGUI currencyCubes;
    public TMPro.TextMeshProUGUI healthCubes;
    public TMPro.TextMeshProUGUI health;
    public List<GameObject> objects = new List<GameObject>();
    public List<string> slingerPool = new List<string>();
    public System.Random rng = new System.Random();

    public Deck playerDeck;
    public Holster playerHolster;

    public MyNetworkManager game;
    public MyNetworkManager Game
    {
        get
        {
            if (game != null)
            {
                return game;
            }
            return game = MyNetworkManager.singleton as MyNetworkManager;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Scene Transition Start is triggering!");
        // set the position of the scrollbar to change depending on the number of stages cleared
        SaveData saveData = SaveSystem.LoadGameData();
        // scrollRectHorizontal.normalizedPosition = new Vector2(0, (saveData.stage * 0.1f));
        currencyCubes.text = saveData.currencyCubes.ToString();
        healthCubes.text = saveData.playerCubes.ToString();
        health.text = saveData.playerHealth.ToString();

        // if you won the carnival game, allow them to pick a hat
        if (saveData.carnivalWin)
        {
            Debug.Log("You won the carnival game! Choose your prize!");
            treasureMenu.SetActive(true);
            treasureMenu.GetComponent<TreasureMenu>().chooseHats();
            StartCoroutine(treasureMenu.GetComponent<TreasureMenu>().treasureAnimation());
        }

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

        currencyCubes.text = saveData.currencyCubes.ToString();
        healthCubes.text = saveData.playerCubes.ToString();
        health.text = saveData.playerHealth.ToString();
    }

    public void hideUI()
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }
    }

    public IEnumerator doIt()
    {
        SaveData saveData = SaveSystem.LoadGameData();
        saveData.transition = true;
        saveData.selectedStage = true;
        SaveSystem.SaveGameData(saveData);

        // DialogueManager.BarkString("I'm barking this text.", this.transform);
        /*
        foreach (GameObject obj in objects) 
        { 
            obj.SetActive(false);
        }
        */


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

    public int getSlinger()
    {
        SaveData saveData = SaveSystem.LoadGameData();
        int num = rng.Next(0, slingerPool.Count);
        if (slingerPool[num] == Game.storyModeCharName ||
            slingerPool[num] == saveData.playerCharName)
        {
            Debug.Log("Same slinger! Redo!");
            getSlinger();
        }
        return num;
    }

    public void handleCrowCutscene()
    {
        Debug.Log(Game.storyModeCharName);

        SaveData saveData = SaveSystem.LoadGameData();
        if (saveData.playerCharName == "")
            saveData.playerCharName = Game.storyModeCharName;

        switch (saveData.playerCharName)
        {
            case "Reets":
                hideUI();
                crowButton.onClick.Invoke();
                break;
            case "Saltimbocca":
                hideUI();
                crowSaltButton.onClick.Invoke();
                break;
            case "Isadore":
                hideUI();
                crowIsadoreButton.onClick.Invoke();
                break;
            case "Bolo":
                hideUI();
                crowBoloButton.onClick.Invoke();
                break;
        }
    }

    public void handleSaltCutscene()
    {
        Debug.Log(Game.storyModeCharName);

        SaveData saveData = SaveSystem.LoadGameData();
        if (saveData.playerCharName == "")
            saveData.playerCharName = Game.storyModeCharName;

        switch (saveData.playerCharName)
        {
            case "Reets":
                hideUI();
                saltButton.onClick.Invoke();
                break;
            case "Isadore":
                hideUI();
                isadoreSaltButton.onClick.Invoke();
                break;
            case "Bolo":
                hideUI();
                boloSaltButton.onClick.Invoke();
                break;
        }
    }

    public void handleReetsCutscene()
    {
        Debug.Log(Game.storyModeCharName);

        SaveData saveData = SaveSystem.LoadGameData();
        if (saveData.playerCharName == "")
            saveData.playerCharName = Game.storyModeCharName;

        switch (saveData.playerCharName)
        {
            case "Saltimbocca":
                hideUI();
                saltReetsButton.onClick.Invoke();
                break;
            case "Isadore":
                hideUI();
                isadoreReetsButton.onClick.Invoke();
                break;
            case "Bolo":
                hideUI();
                boloReetsButton.onClick.Invoke();
                break;
        }
    }

    public void handleBoloCutscene()
    {
        Debug.Log(Game.storyModeCharName);

        SaveData saveData = SaveSystem.LoadGameData();
        if (saveData.playerCharName == "")
            saveData.playerCharName = Game.storyModeCharName;

        switch (saveData.playerCharName)
        {
            case "Reets":
                hideUI();
                reetsBoloButton.onClick.Invoke();
                break;
            case "Isadore":
                hideUI();
                isadoreBoloButton.onClick.Invoke();
                break;
            case "Saltimbocca":
                hideUI();
                saltBoloButton.onClick.Invoke();
                break;
        }
    }

    public void handleIsadoreCutscene()
    {
        Debug.Log(Game.storyModeCharName);

        SaveData saveData = SaveSystem.LoadGameData();
        if (saveData.playerCharName == "")
            saveData.playerCharName = Game.storyModeCharName;

        switch (saveData.playerCharName)
        {
            case "Reets":
                hideUI();
                reetsIsadoreButton.onClick.Invoke();
                break;
            case "Bolo":
                hideUI();
                boloIsadoreButton.onClick.Invoke();
                break;
            case "Saltimbocca":
                hideUI();
                saltIsadoreButton.onClick.Invoke();
                break;
        }
    }

    public void doTransition()
    {
        StartCoroutine(doIt());
    }
}
