using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBox : MonoBehaviour
{
    public CardPlayer player;
    Vector3 originalPos;

    public TMPro.TextMeshProUGUI textBox;
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;

        if (player.charName == "Singelotte")
        {
            Debug.Log("Moving HoverBox for Singelotte");
            transform.position = new Vector3(-184f * GameManager.manager.widthRatio, 0, 0) + new Vector3(transform.position.x, transform.position.y, transform.position.z);    
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText(CardPlayer cp)
    {
        if (cp.charName == "Crowpunk"  || cp.charName == "Crowpunk+")
        {
            textBox.text = "Attacks:\nDeal 1-4 damage\nSteal a random card from your holster";
        }
        if (cp.charName == "Bag o' Snakes" || cp.charName == "Bag o' Snakes+" ||
            cp.charName == "Fingas" || cp.charName == "Fingas+")
        {
            textBox.text = "Attacks:\nDeal 1-4 damage";
        }
        // update this with the characters
        if (cp.charName == "Singelotte")
        {   
            textBox.text = "Attacks: \nDeal 4 damage\nSpice up a card in your holster\nSpice up a card in the market";
        }
        if (cp.charName == "Bolo")
        {
            if (cp.character.character.flipped)
            {
                textBox.text = "All cards in your Holster sell for +1P.";
                return;
            }
            // interface with the hoverbox and change the text accordingly
            textBox.text = "You gain +1P when you sell a non-potion card."; 
        }
        if (cp.charName == "Reets")
        {
            if (cp.character.character.flipped)
            {
                textBox.text = "Cycle items for free. Pay 2P to put the top card of your deck into your Holster.";
                return;
            }
            // interface with the hoverbox and change the text accordingly
            textBox.text = "Pay 2P to put the top card of your deck into your Holster.";
        }
        if (cp.charName == "Isadore")
        {
            if (cp.character.character.flipped)
            {
                textBox.text = "Once per turn you may create a starter potion on top of your deck.\n" +
                    "Pay 3P: Put The Cherrybomb Badge into your Holster.";
                return;
            }
            // interface with the hoverbox and change the text accordingly
            textBox.text = "Artifacts in your Holster deal +1 damage.";
        }
        if (cp.charName == "Twins")
        {
            if (cp.character.character.flipped)
            {
                textBox.text = "Potions you throw heal you +1 HP.\nVessels you throw heal you +2 HP.";
                return;
            }
            // interface with the hoverbox and change the text accordingly
            textBox.text = "Potions you throw heal you +1 HP.";
        }
        if (cp.charName == "Scarpetta")
        {
            if (cp.character.character.flipped)
            {
                textBox.text = "You can only buy cards from the trash.";
                return;
            }
            // interface with the hoverbox and change the text accordingly
            textBox.text = "You may only buy non-potion cards if they are in the trash.\n" +
                "2P: Trash any card in the Market.";
        }
        if (cp.charName == "Sweetbitter")
        {
            if (cp.character.character.flipped)
            {
                textBox.text = "At the start of your turn, if your Holster contains 1 ring, 2 loaded items, and The Phylactery, " +
                    "deal 12 damage to each player.";
                return;
            }
            // interface with the hoverbox and change the text accordingly
            textBox.text = "Pay 6P: Put The Phylactery on the top of your Deck.";
        }
        if (cp.charName == "Nickles")
        {
            if (cp.character.character.flipped)
            {
                textBox.text = "You may spend up to 6P per turn to deal that much damage to a player.\n" +
                    "Pay 3P: Put The Blacksnake Pip SLing into your Holster.";
                return;
            }
            // interface with the hoverbox and change the text accordingly
            textBox.text = "You may spend up to 4P per turn to deal that much damage to a player.";
        }
        if (cp.charName == "Saltimbocca")
        {
            if (cp.character.character.flipped)
            {
                textBox.text = "Once per turn you may create a starter potion on top of your deck.\n" +
                    "Pay 3P: Put The Cherrybomb Badge into your Holster.";
                return;
            }
            // interface with the hoverbox and change the text accordingly
            textBox.text = "Market cards cost you -1P. Cards cannot cost less than 1P.";
        }
        if (cp.charName == "Pluot")
        {
            if (cp.character.character.flipped)
            {
                textBox.text = "You may put The Extra Inventory into your Holster. If you do, flip Pluot.";
                return;
            }
            // interface with the hoverbox and change the text accordingly
            textBox.text = "At the start of your turn, choose Hot, Cold, Wet, or Dry. Potions of that Quality deal +1 damage.";
        }
    }
}
