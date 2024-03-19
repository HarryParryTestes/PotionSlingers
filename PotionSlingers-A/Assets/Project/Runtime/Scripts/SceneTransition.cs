using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public GameObject card1;
    public GameObject card2;
    public GameObject loadingScreen;
    public GameObject text;
    public CutsceneManager cutsceneManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator doIt()
    {
        // SaveData saveData = SaveSystem.LoadGameData();

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
    }

    public void doTransition()
    {
        StartCoroutine(doIt());
    }
}
