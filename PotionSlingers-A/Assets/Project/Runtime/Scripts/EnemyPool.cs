using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{

    public List<string> level1Enemies;
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

        int index = rng.Next(bosses.Count);
        return bosses[index];

        /*
        // Get random index
        int index = rng.Next(level1Enemies.Count);
        Debug.Log(index);
        SaveData saveData = SaveSystem.LoadGameData();

        if (saveData.visitedEnemies.Count < 1 && encounteredEnemies.Count < 1)
        {
            Debug.Log("First one?");
            encounteredEnemies.Add(level1Enemies[index]);
            return level1Enemies[index];
        }

        if (encounteredEnemies.Count >= 3)
        {
            Debug.Log("over the limit");
            return "dummy";
        }

        if (saveData.visitedEnemies.Contains(level1Enemies[index]) || encounteredEnemies.Contains(level1Enemies[index]))
        {
            Debug.Log("Picking new enemy");
            return GetRandomEnemy();
        }
        // encounteredEnemies.Add(level1Enemies[index]);
        encounteredEnemies.Add(level1Enemies[index]);
        return level1Enemies[index];
        // SaveSystem.SaveGameData(saveData);
        */
    }

    public string GetRandomEnemy(int num)
    {
        Debug.Log("Number " + num + "!!!");
        if(num == 10)
        {
            Debug.Log("Returning dummy");
            return "dummy";
        }
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

        // Get random index
        int index = rng.Next(level1Enemies.Count);
        Debug.Log(index);
        // SaveData saveData = SaveSystem.LoadGameData();

        if (saveData.visitedEnemies.Count < 1 && encounteredEnemies.Count < 1)
        {
            Debug.Log("First one?");
            encounteredEnemies.Add(level1Enemies[index]);
            return level1Enemies[index];
        }

        if (encounteredEnemies.Count >= 3)
        {
            Debug.Log("over the limit");
            Debug.Log("Returning dummy");
            return "dummy";
        }

        if (saveData.visitedEnemies.Contains(level1Enemies[index]) || encounteredEnemies.Contains(level1Enemies[index]))
        {
            Debug.Log("Picking new enemy");
            return GetRandomEnemy(num + 1);
        }
        // encounteredEnemies.Add(level1Enemies[index]);
        encounteredEnemies.Add(level1Enemies[index]);
        return level1Enemies[index];
        // SaveSystem.SaveGameData(saveData);
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
        /*
        // try out exclude number random
        var exclude = new HashSet<int>();
        for(int i = 0 i < .Count)
        var range = Enumerable.Range(1, 100).Where(i => !exclude.Contains(i));

        var rand = new System.Random();
        int index = rand.Next(0, 100 - exclude.Count);
        return range.ElementAt(index);
        */

        // Get random index
        int index = rng.Next(level1Enemies.Count);
        Debug.Log(index);
        // SaveData saveData = SaveSystem.LoadGameData();

        if (saveData.visitedEnemies.Count < 1 && encounteredEnemies.Count < 1)
        {
            Debug.Log("First one?");
            encounteredEnemies.Add(level1Enemies[index]);
            return level1Enemies[index];
        }
        
        if (encounteredEnemies.Count >= 3)
        {
            Debug.Log("over the limit");
            Debug.Log("Returning dummy");
            return "dummy";
        }

        if (saveData.visitedEnemies.Contains(level1Enemies[index]) || encounteredEnemies.Contains(level1Enemies[index]))
        {
            Debug.Log("Picking new enemy");
            return GetRandomEnemy(1);
        }
        // encounteredEnemies.Add(level1Enemies[index]);
        encounteredEnemies.Add(level1Enemies[index]);
        return level1Enemies[index];
        // SaveSystem.SaveGameData(saveData);
    }
}
