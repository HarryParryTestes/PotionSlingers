using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;
using Steamworks;
using DG.Tweening;

public class ExpUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI expBox;
    public Vector2 originalPosition;
    public int level;
    public int points;
    public int maxValue;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnUserStatsReceived(UserStatsReceived_t pCallback)
    {
            if (EResult.k_EResultOK == pCallback.m_eResult)
            {
                Debug.Log("Received stats and achievements from Steam\n");

                // load stats
                SteamUserStats.GetStat("exp_points", out points);
                SteamUserStats.GetStat("exp_level", out level);
            }
            else
            {
                Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
            }
    }

    public void DisplayText(CardPlayer cp = null)
    {
        int pointsToAdd = 0;
        SteamUserStats.GetStat("exp_points", out points);
        SteamUserStats.GetStat("exp_level", out level);

        maxValue = handleMaxValue();

        if (cp != null)
        {
            // TODO: make the experience gained somewhat random each time, provide a random range for each enemy

            Debug.Log("Experience character name!!! " + cp.charName);
            switch (cp.charName)
            {
                case "Saltimbocca":
                    pointsToAdd = 200;
                    break;
                case "Bolo":
                    pointsToAdd = 100;
                    break;
                case "Fingas":
                    pointsToAdd = 200;
                    break;
                case "Crowpunk":
                    pointsToAdd = 200;
                    break;
                case "Bag o' Snakes":
                    pointsToAdd = 100;
                    break;
                case "Singelotte":
                    pointsToAdd = 500;
                    break;
                default:
                    pointsToAdd = 100;
                    break;

            }

            Debug.Log("Adding " + pointsToAdd + " points!");
            points += pointsToAdd;
            // add points here
            if (points >= maxValue && level != 0)
                levelUp(level);
        }

        if (level != 0)
        {
            SteamUserStats.SetStat("exp_points", points);
            SteamUserStats.SetStat("exp_level", level);
            Debug.Log("Exp = " + points + ", Level = " + level);
            SteamUserStats.StoreStats();
        }
        

        expBox.text = "+" + pointsToAdd + " Exp! " + points + " / " + maxValue;

        // MATTEO: Add exp gain sound effect

        GameObject obj = Instantiate(this.gameObject, this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform.parent);
        obj.transform.DOMove(new Vector3(originalPosition.x + (-675f * GameManager.manager.widthRatio), originalPosition.y, 0), 1.5f).SetEase(Ease.InOutSine);
        obj.transform.DOMove(new Vector3(originalPosition.x, originalPosition.y, 0), 1.5f).SetEase(Ease.InOutSine).SetDelay(2.5f);
        Destroy(obj, 5);
    }

    public int handleMaxValue()
    {
        switch (level)
        {
            case 0:
                return 100;
                break;
            case 1:
                return 100;
                break;
            case 2:
                return 200;
                break;
            case 3:
                return 300;
                break;
            case 4:
                return 400;
                break;
            case 5:
                return 500;
                break;
            case 6:
                return 600;
                break;
            case 7:
                return 800;
                break;
            case 8:
                return 1000;
                break;
            default:
                return 1000 + (level - 8) * 500;
                break;

        }
    }

    public void levelUp(int level)
    {
        Debug.Log("You leveled up!!!");
        // levelUpText.SetActive(true);
        points -= maxValue;
        level++;
        this.level = level;
        // MATTEO: play level up sound effect

        maxValue = handleMaxValue();
    }

}
