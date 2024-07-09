using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Steamworks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryModeLobbyUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI nameBox;
    public TMPro.TextMeshProUGUI infoBox;
    public string[] infoQuotes;

    private MyNetworkManager game;
    private MyNetworkManager Game
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
        infoQuotes[0] = "Bolo is an adept salesman. He will always be able to upsell his items, and can even trade useless baubles for powerful treasure!" +
            "\n\nFront:\nYou gain +1P when you sell a non-potion card.\n\nBack:\nAll cards in your Holster sell for +1P.";

        // Isadore
        infoQuotes[1] = "Isadore\t\tPlaystyle: Aggressive\nDifficulty: * *\tProficiency: Artifacts\n";

        //Nickles
        infoQuotes[2] = "Nickles\t\tPlaystyle: Flexible\nDifficulty: * *\tProficiency: Pips\nNickles can use " +
            "leftover pips as an extra\nresource. Use his unique item to rustle\nup a couple extra pips to sling or even\n   to spend!";

        // Pluot
        infoQuotes[3] = "Pluot\t\t\tPlaystyle: Aggressive\nDifficulty: *\t\tProficiency: Potions\nPluot's strategy is simple; collect potions\n" +
            "and throw them for a lot of damage! Her\nitem allows you to be even more efficient!";

        // Reets
        infoQuotes[4] = "Reets\nPlaystyle: Technical\nDifficulty: * * *\nProficiency: Inventory\n\n\nReets always has the right tools on hand!\n" +
            "He can cycle through his items in his\nHolster and Deck much faster than the\nrest.";

        // Saltimbocca
        infoQuotes[5] = "Saltimbocca\nPlaystyle: Strategic\nDifficulty: * *\nProficiency: Thrifting\n\n\n" +
            "With intimidation and an eye for quality,\nBocca wants the best items and wants 'em\ncheap. Preferably also sharp, heavy, and\naerodynamic.";

        // Scarpetta
        infoQuotes[6] = "Scarpetta\nPlaystyle:  ? ? ?\nDifficulty: ? ? ?\nProficiency: Trash\n\n\nScarpetta loves trash. While others may\n" +
            "scoff at as useless junk, he revels in an\never-increasing pile of treasures. Refuse,\nreuse, recycle!";

        // Sweetbitter
        infoQuotes[7] = "Sweetbitter\t\tPlaystyle: Technical\nDifficulty: * * * *  Proficiency: Resilience\nSweetbitter has a strange object that\n" +
            "prevents her from dying. She also has a\nnefarious plan. She just has to collect a\n  few things...";

        // Twins
        infoQuotes[8] = "Twins\nPlaystyle: Flexible\nDifficulty: *\nProficiency: Self Healing\n\n\n" +
            "The Twins focus on keeping their HP\nhigh, at the expense of everyone else! Flip\nthem over to have their vessels heal too!";
    }

    // Update is called once per frame
    void Update()
    {
        switch (Game.storyModeCharName)
        {
            case "Bolo":
                infoBox.text = "Bolo is an adept salesman. He will always be able to upsell his items, and can even trade useless baubles for powerful treasure!" +
            "\n\nFront:\nYou gain +1P when you sell a non-potion card.\n\nBack:\nAll cards in your Holster sell for +1P.";
                break;
            case "Isadore":
                infoBox.text = "Isadore is a master of artifacts, and is extremely versatile with them. Flip her to equip her badge, and punish her foes even further!" +
            "\n\nFront:\nArtifacts in your Holster deal +1 damage.\n\nBack:\nOnce per turn you may create a starter potion loaded into an artifact in your Holster." +
            "\nPay 3P: Put The Cherrybomb Badge into your Holster.";
                break;
            case "Nickles":
                infoBox.text = "Nickles can use leftover pips as an extra resource. Use his unique item to rustle up a couple extra pips to sling or even to spend!" +
            "\n\nFront:\nYou may spend up to 4P per turn to deal that much damage to a player.\n\nBack:\nYou may spend any amount of P to damage a player by that amount spent.\n" +
            "Pay 3P: Put The Blacksnake Pip Sling into your Holster.";
                break;
            case "Pluot":
                infoBox.text = "Pluot's strategy is simple; collect potions and throw them for a lot of damage! Her item allows you to be even more efficient!" +
            "\n\nFront:\nAt the start of your turn, choose Hot, Cold, Wet, or Dry. Potions of that Quality deal +1 damage.\n\nBack:\nYou may put The Extra Inventory" +
            " into your Holster. If you do, flip Pluot.";
                break;
            case "Reets":
                infoBox.text = "Reets always has the right tools on hand! He can cycle through his items in his Holster and Deck much faster than the rest." +
            "\n\nFront:\nPay 2P to put the top card of your Deck into your Holster.\n\nBack:\nDrop items for free. Pay 1P to put the top card of your Deck into your Holster.";
                break;
            case "Saltimbocca":
                infoBox.text = "With intimidation and an eye for quality, Bocca wants the best items and wants 'em cheap! Preferably also sharp, heavy, and aerodynamic..." +
            "\n\nFront:\nMarket cards cost you -1P. Cards cannot cost less than 1P.\n\nBack:\nAll cards in your Holster can be thrown. Damage is equal to their buy cost.";
                break;
            case "Scarpetta":
                infoBox.text = "Scarpetta loves trash. While others may scoff at as useless junk, he revels in a never-increasing pile of treasures. Refuse, reuse, recycle!" +
            "\n\nFront:\nYou may only buy non-potion cards if they are in the trash. Pay 2P: Trash any card in the Market.\n\nBack:\n" +
            "You may only buy cards from the Trash.";
                break;
            case "Sweetbitter":
                infoBox.text = "Sweetbitter has a strange object that prevents her from dying. She also has a nefarious plan. She just has to collect a few things..." +
            "\n\nFront:\nPay 6P: Put The Phylactery on the top of your Deck.\n\nBack:\nAll cards in your Holster sell for +1P.";
                break;
            case "Twins":
                infoBox.text = "The Twins focus on keeping their HP high, at the expense of everyone else! Flip them over to have their vessels heal too!" +
            "\n\nFront:\nPotions you throw heal you +1 HP.\n\nBack:\nPotions you throw heal you +2 HP. Vessels you throw heal +4 HP.";
                break;
        }
    }
}
