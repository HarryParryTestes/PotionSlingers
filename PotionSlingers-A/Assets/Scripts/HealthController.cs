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

        // healthText.text = hp.ToString();
    }

    public void subHealth(int health) {
        hp -= health;

        //Make sure that hp doesn't go below 0
        //If hp goes below 0, set it to 10 and subtract a health cube
        if(hp <= 0) {
            if(essenceCubes > 0) {
                hp = 10;
                giveCube();
                // Debug.Log("EssenceCubes.ToString: " + essenceCubes.ToString());
            } else {
                dead = true;
            }
        }

        // essenceCubesText.text = essenceCubes.ToString();
    }

    public void giveCube() {
        essenceCubes--;
    }

    public void getCube() {
        takenEssenceCubes++;
    }

    public void setHP() {

    }

    void Awake() {
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = 10;
        essenceCubes = 2;

        // Debug.Log("hp is: " + hp);
        // Debug.Log("essenceCube is: " + essenceCubes);
        // healthText = GameObject.Find("Health").GetComponent<Text>();
        // essenceCubesText = GameObject.Find("EssenceCubes").GetComponent<Text>();

        // healthText.text = hp.ToString();
        // essenceCubesText.text = essenceCubes.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
