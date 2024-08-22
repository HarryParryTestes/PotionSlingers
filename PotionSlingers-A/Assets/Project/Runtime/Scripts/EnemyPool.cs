using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{

    public List<string> level1Enemies;
    public List<string> level2Enemies;
    public List<string> encounteredEnemies = new List<string>();
    public List<string> bosses;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetBossEnemy()
    {
        System.Random rng = new System.Random();

        // change this later lol

        // int index = rng.Next(bosses.Count);
        return "Singelotte";
        // return bosses[index];
    }

    public string GetRandomEnemy()
    {
        System.Random rng = new System.Random();
        SaveData saveData = SaveSystem.LoadGameData();

        Debug.Log("Encountered Enemies:");
        foreach (string thing in encounteredEnemies)
        {
            Debug.Log(thing);
        }
        Debug.Log("Defeated Enemies:");
        foreach (string thing in saveData.visitedEnemies)
        {
            Debug.Log(thing);
        }

        if (saveData.visitedEnemies.Count < 3)
        {
            // LEVEL 1 ENEMIES

            var exclude = new HashSet<int>() { };
            for (int j = 0; j < level1Enemies.Count; j++)
            {
                if (encounteredEnemies.Contains(level1Enemies[j]) || saveData.visitedEnemies.Contains(level1Enemies[j]))
                {
                    // exclude.Add(j);
                    if (level1Enemies.Count > 1)
                        level1Enemies.RemoveAt(j);
                }
            }

            // var range = Enumerable.Range(0, level1Enemies.Count).Where(j => !exclude.Contains(j));
            // Debug.Log("Number of available enemies to select from: " + (level1Enemies.Count - exclude.Count));
            int index = rng.Next(0, level1Enemies.Count);
            encounteredEnemies.Add(level1Enemies[index]);
            return level1Enemies[index];

        } else
        {
            // LEVEL 2 ENEMIES

        var exclude = new HashSet<int>() { };
        for (int j = 0; j < level2Enemies.Count; j++)
        {
            if (encounteredEnemies.Contains(level2Enemies[j]) || saveData.visitedEnemies.Contains(level2Enemies[j]))
            {
                    if (level2Enemies.Count > 1)
                        level2Enemies.RemoveAt(j);
            }
        }

        var range = Enumerable.Range(0, level2Enemies.Count).Where(j => !exclude.Contains(j));
        // Debug.Log("Number of available enemies to select from: " + (level2Enemies.Count - exclude.Count));
        int index = rng.Next(0, level2Enemies.Count);

        encounteredEnemies.Add(level2Enemies[index]);
        return level2Enemies[index];
        // SaveSystem.SaveGameData(saveData);
        }
    }
}
