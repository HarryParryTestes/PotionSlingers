using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public int hp;
    public int essenceCubes;
    public int takenEssenceCubes;    //HP Cubes that have been taken from opponents
    public Text healthText;
    public Text essenceCubesText;
    public bool dead;               //Does the player still have health left?

    public void addHealth(int health) {
        hp += health;

        //Make sure that hp cannot go above 10
        if(hp > 10) {
            hp = 10;
        }
    }

    public void subHealth(int health) {
        hp -= health;

        //Make sure that hp doesn't go below 0
        //If hp goes below 0, set it to 10 and subtract a health cube
        if(hp < 0) {
            if(essenceCubes > 0) {
                hp = 10;
                essenceCubes--;
            } else {
                dead = true;
            }
        }
    }

    public void giveCube(Player player) {
        player.getCube();
        essenceCubes--;
    }

    public void getCube() {
        takenEssenceCubes++;
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = 10;
        essenceCubes = 2;
        healthText.text = "" + hp;
        essenceCubesText.text = "" + essenceCubes;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
