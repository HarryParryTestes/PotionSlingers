using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

public class EditModeTests
{
    public static GameManager manager;
    public static MyNetworkManager Game;
    public int num = 3;

    [MenuItem("AssetDatabase/LoadAssetExample")]
    [Test]
    public void ExampleGameTest()
    {
        var gd = new GameObject();
        var gf = gd.AddComponent<MyNetworkManager>();
        Game = gf;
        var go = new GameObject();
        var gm = go.AddComponent<GameManager>();
        manager = gm;
        var gameObject = new GameObject();
        var cardPlayer1 = gameObject.AddComponent<CardPlayer>();
        var gameObject2 = new GameObject();
        var cardPlayer2 = gameObject2.AddComponent<CardPlayer>();
        var gameObj = new GameObject();
        var md1 = gameObj.AddComponent<MarketDeck>();
        var obj = new GameObject();
        var cd1 = obj.AddComponent<CardDisplay>();
        cd1.card = AssetDatabase.LoadAssetAtPath<Card>("Assets / Project / Runtime / ScriptableCards / Potions / AConfidentThrowIntoTheGarbage.asset");
        // Debug.Log(cd1.card.name);
        md1.cardDisplay1 = cd1;
        md1.deckList = new List<Card>();
        manager.md1 = md1;
        var gameOb = new GameObject();
        var md2 = gameOb.AddComponent<MarketDeck>();
        md2.cardDisplay1 = cd1;
        md2.deckList = new List<Card>();
        manager.md2 = md2;

        // yield return new WaitForSeconds(1f);
        Assert.AreEqual(cardPlayer1.pipCount, 6);
        // Assert.AreEqual(num, 3);
    }
}
    

