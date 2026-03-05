using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class HealthController : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CardPlayer player;
    public int hp;
    public int essenceCubes;
    public int takenEssenceCubes;    //HP Cubes that have been taken from opponents
    public Text healthText;
    public Text essenceCubesText;
    public bool dead;               //Does the player still have health left?

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

    public void addHealth(int health) {
        hp += health;

        //Make sure that hp cannot go above 10
        if(hp > 10) {
            hp = 10;
        }

        // healthText.text = hp.ToString();
    }

    public void OnDrop(PointerEventData eventData)
    {

        if (Game.tutorial)
            return;

        Debug.Log("Drop happened");
        GameObject heldCard = eventData.pointerDrag;
        DragCard dc = heldCard.GetComponent<DragCard>();
        if (dc.loaded || dc.market)
            return;

        // grabbing the card held by the cursor
        CardDisplay grabbedCard = heldCard.GetComponent<CardDisplay>();
        if(grabbedCard.card.cardType == "Potion")
        {
            player.addHealth(grabbedCard.card.effectAmount);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_Load");

            GameObject obj = Instantiate(grabbedCard.gameObject,
                            grabbedCard.gameObject.transform.position,
                            grabbedCard.gameObject.transform.rotation,
                            grabbedCard.gameObject.transform);

            GameManager.manager.StartCoroutine(GameManager.manager.MoveToTrash(obj));
            GameManager.manager.td.addCard(grabbedCard);
            GameManager.manager.sendMessage("You healed with a potion!");
            grabbedCard.updatePlaceholder(grabbedCard);
        }     
    }
    public void OnPointerExit(PointerEventData eventData)
    {   
        if(gameObject.name == "PlayerHP")
            transform.DOScale(1f, 0.25f).SetId(gameObject.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Game.tutorial)
            return;

        GameObject heldCard = eventData.pointerDrag;
        if(heldCard != null)
        {
            DragCard dc = heldCard.GetComponent<DragCard>();
            if (dc.loaded || dc.market)
                return;
            // grabbing the card held by the cursor
            CardDisplay grabbedCard = heldCard.GetComponent<CardDisplay>();
            if (grabbedCard.card.cardType == "Potion")
                transform.DOScale(1.25f, 0.25f).SetId(gameObject.name);
        }
        
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
