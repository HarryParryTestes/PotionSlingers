using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Steamworks;

public class SceneTransition : MonoBehaviour
{
    public GameObject UIbackground;
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

    public GameObject boloShop;

    public List<CardDisplay> shopCardDisplays;
    public List<Card> cardPool;
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
    public DeckMenuScroll deckMenuScroll;

    public CanvasGroup mapCanvasGroup;

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

        // deckMenuScroll.displayHolsterInStoryModeMenu();
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

    public void buyBolo(CardDisplay card)
    {
        Debug.Log("Adding card...");
        SaveData data = SaveSystem.LoadGameData();

        if (data.currencyCubes < 3)
        {
            Debug.Log("Gray out buy boxes to show that you can't select it");
            return;
        }
        else
        {
            data.currencyCubes -= 3;
            currencyCubes.text = data.currencyCubes.ToString();
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_BuySell");
        }
            


        if (card.card.cardType == "Fashion")
        {
            SteamUserStats.SetAchievement("FASHIONISTA_1");

            int fashion;

            Debug.Log("Adding fashion!!!");
            SteamUserStats.GetStat("fashion", out fashion);
            fashion++;

            if (fashion >= 5)
                SteamUserStats.SetAchievement("FASHIONISTA_2");

            SteamUserStats.SetStat("fashion", fashion);

            SteamUserStats.StoreStats();

            data.playerFashion.Add(card.card.cardName);
        }
        else if (card.card.cardType == "Ring")
            data.playerRings.Add(card.card.cardName);
        else
        {
            Debug.Log("Adding card!!!");
            data.treasure.Add(card.card.cardName);
        }

        System.Random rng = new System.Random();
        int index = GetInt();
        card.updateCard(cardPool[index]);

        SaveSystem.DebugCards(data);

        SaveSystem.SaveGameData(data);
        // this.gameObject.SetActive(false);
        // ADD THIS BACK IN WHEN YOU WANT TO SHOW THE UI AGAIN!
        // deck.SetActive(true);
        // holster.SetActive(true);
    }

    public int GetInt()
    {

        System.Random rng = new System.Random();

        var exclude = new HashSet<int>() { };
        for (int i = 0; i < cardPool.Count; i++)
        {
            if (cardPool[i].name == shopCardDisplays[0].card.name ||
                cardPool[i].name == shopCardDisplays[1].card.name ||
                cardPool[i].name == shopCardDisplays[2].card.name)
            {
                exclude.Add(i);
            }
        }

        var range = Enumerable.Range(0, cardPool.Count).Where(i => !exclude.Contains(i));
        int index = rng.Next(0, cardPool.Count - exclude.Count);
        return range.ElementAt(index);
    }

    public void chooseCards()
    {
        foreach (CardDisplay cd in shopCardDisplays)
        {
            System.Random rng = new System.Random();
            int index = GetInt();
            cd.updateCard(cardPool[index]);
        }
    }

    public void shopAnimInit()
    {
        // hide backpack, holster, and deck buttons
        objects[2].SetActive(false);
        objects[3].SetActive(false);
        objects[4].SetActive(false);
        StartCoroutine(shopAnimation());
    }

    public IEnumerator shopAnimation()
    {
        // FadeIn(this.gameObject);
        // FadeIn(this.transform.GetChild(9).GetChild(3).gameObject);
        // currencyHoverBox.SetActive(false);
        // healthHoverBox.SetActive(false);
        // deck.SetActive(false);
        // holster.SetActive(false);
        foreach (CardDisplay cd in shopCardDisplays)
        {
            cd.gameObject.SetActive(false);
        }

        foreach (CardDisplay cd in shopCardDisplays)
        {
            Vector3 currentPosition = cd.transform.position;
            // cd.GetComponent<CardHover>().originalPosition = currentPosition;
            cd.transform.position = new Vector3(currentPosition.x + 20f, currentPosition.y - 20f, currentPosition.z);
            cd.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            cd.transform.DOJump(currentPosition, 10f, 1, 1f, false);
            cd.transform.DORotate(new Vector3(0, 0, 720f), 1f, RotateMode.FastBeyond360);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator doIt()
    {
        SaveData saveData = SaveSystem.LoadGameData();
        saveData.transition = true;
        saveData.selectedStage = true;
        SaveSystem.SaveGameData(saveData);


        card1.transform.DOMoveX(-4.5f, .75f);
        card2.transform.DOMoveX(4.5f, .75f);
        yield return new WaitForSeconds(.75f);

        /*
        if (saveData.visitedEnemies.Contains("Singelotte"))
            SceneManager.LoadScene("TitleMenu");
        else
        */

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
            default:
                Debug.Log("Default case!!!");
                hideUI();
                boloSaltButton.onClick.Invoke();
                saveData.currentEnemyName = "Bolo";
                saveData.slingerName = saveData.currentEnemyName;
                saveData.slingerEncounter = 1;
                SaveSystem.SaveGameData(saveData);
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
            default:
                Debug.Log("Default case!!!");
                hideUI();
                boloReetsButton.onClick.Invoke();
                saveData.currentEnemyName = "Bolo";
                saveData.slingerName = saveData.currentEnemyName;
                saveData.slingerEncounter = 1;
                SaveSystem.SaveGameData(saveData);
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
            default:
                Debug.Log("Default case!!!");
                hideUI();
                saltBoloButton.onClick.Invoke();
                saveData.currentEnemyName = "Saltimbocca";
                saveData.slingerName = saveData.currentEnemyName;
                saveData.slingerEncounter = 1;
                SaveSystem.SaveGameData(saveData);
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
            default:
                Debug.Log("Default case!!!");
                hideUI();
                saltIsadoreButton.onClick.Invoke();
                saveData.currentEnemyName = "Saltimbocca";
                saveData.slingerName = saveData.currentEnemyName;
                saveData.slingerEncounter = 1;
                SaveSystem.SaveGameData(saveData);
                break;
        }
    }

    public void doTransition()
    {
        mapCanvasGroup.blocksRaycasts = false;
        StartCoroutine(doIt());
    }
}
