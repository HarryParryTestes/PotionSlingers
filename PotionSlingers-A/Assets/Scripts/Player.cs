using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int hp;
    public string name;
    public string charName;
    public int user_id;
    public int hpCubes;
    public int takenHPCubes;    //HP Cubes that have been taken from opponents
    public Deck deck;
    public Holster holster;
    public int pips;
    public bool dead;           //Does the player still have health left?
    public int potionsThrown;
    public CharacterDisplay character;
    public bool ringBonus;
    public int bonusAmount;
    //public HealthController health;
    public GameObject playerHP;
    public GameObject playerHPCubes;
    public GameObject playerPips;
    public GameObject currentPlayerHighlight;

    public Player(int user_id, string name)
    {
        this.user_id = user_id;
        this.name = name;
    }

    public void initHealth() {
        hp = 10;
        hpCubes = 3;
        updateHealthUI();
    }

    public void updateHealthUI() {
        if(playerHP != null && playerHPCubes != null) {
            playerHP.GetComponent<Text>().text = hp.ToString();
            playerHPCubes.GetComponent<Text>().text = hpCubes.ToString();
        }
    }

    public void updatePipsUI() {
        if(playerPips != null) {
            playerPips.GetComponent<Text>().text = pips.ToString() + " Pips";
        }
    }

    public void setDefaultTurn()
    {
        pips = 6;
        potionsThrown = 0;
        ringBonus = false;

        foreach (CardDisplay cd in holster.cardList)
        {
            // if there's a starter ring
            if (cd.card.cardType == "Ring" &&
                cd.card.cardQuality == "Starter")
            {
                ringBonus = true;
            }
        }

        updatePipsUI();
    }

    public void setCurrentPlayer() {
        currentPlayerHighlight.SetActive(true);
    }

    public void removeCurrentPlayer() {
        currentPlayerHighlight.SetActive(false);
    }

    /*
    void Awake() {
        GameObject go = new GameObject("HealthController");
        health = go.AddComponent<HealthController>();
    }

    public Player()
    {
        dead = false;
    }

    public Player(int HPCubes)
    {
        hp = 10;
        hpCubes = HPCubes;
        dead = false;
    }
    */

    // public void addHealth(int health) {
    //     hp += health;

    //     //Make sure that hp cannot go above 10
    //     if(hp > 10) {
    //         hp = 10;
    //     }
    // }

     public void subHealth(int health) {
         hp -= health;

         //Make sure that hp doesn't go below 0
         //If hp goes below 0, set it to 10 and subtract a health cube
        if(hp < 0) {
            if(hpCubes > 0) {
                hp = 10;
                hpCubes--;
            } else {
                dead = true;
            }
        }

        Debug.Log("Subtracted "+health+"from "+charName);
        Debug.Log(charName+"'s health = "+hp+" HP");

        updateHealthUI();
     }

    // public void giveCube(Player player) {
    //     player.getCube();
    //     hpCubes--;
    // }

    // public void getCube() {
    //     takenHPCubes++;
    // }

    public void addPips(int morePips) {
        pips += morePips;
        updatePipsUI();
    }

    public void subPips(int lessPips) {
        pips -= lessPips;
        updatePipsUI();
    }

    //function to call when checking on if the player is dead
    public bool checkDead() {
        return dead;
    }
}
