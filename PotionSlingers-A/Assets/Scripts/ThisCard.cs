using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThisCard : MonoBehaviour
{
    public Card card;
    public int id;
    public string cardName; // Ex: PS_AS_BasicBattleBaster
    public string cardType; // Potion, Artifact, Vessel, Ring
    public string cardEffect; // Attack, Heal, etc.
    public int effectAmount; // +2 Damage, +3 Heal, etc.
    public string cardQuality; // Cold, Dry, Wet, Hot, Starter, None
    public Sprite cardSprite;

    private string folderLocation;
    private Image myImg;
    
    // Start is called before the first frame update
    void Start()
    {
       
        card = CardDatabase.cardList[0];
        id = card.id;
        cardName = card.cardName;
        cardType = card.cardType;
        cardEffect = card.cardEffect;
        effectAmount = card.effectAmount;
        cardQuality = card.cardQuality;

        if(cardQuality == "STARTER") {
            folderLocation = "Cards/Starter Cards/";
        }
        else {
            switch(cardType) {
                case "ARTIFACT": folderLocation = "Cards/Artifacts/"; break;
                case "POTION": folderLocation = "Cards/Potions/"; break;
                case "RING": folderLocation = "Cards/Rings/"; break;
                case "VESSEL": folderLocation = "Cards/Vessels/"; break;
                default: folderLocation = "Cards/Starter Cards/"; break;
            }
        }

        myImg = this.GetComponent<Image>();
        cardSprite = Resources.Load<Sprite>("" + folderLocation + cardName);
        myImg.sprite = cardSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
