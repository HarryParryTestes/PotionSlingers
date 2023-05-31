using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void checkGameData()
    {
        string path = Application.persistentDataPath + "savedata.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameManager manager = formatter.Deserialize(stream) as GameManager;
            stream.Close();

            GameObject obj = GameObject.Find("Save Data Text");
            obj.GetComponent<TMPro.TextMeshProUGUI>().text = "Current Save Data: Stage 1";
        }
        else
        {
            GameObject obj = GameObject.Find("Save Data Text");
            obj.GetComponent<TMPro.TextMeshProUGUI>().text = "Current Save Data: None";
        }

        // BinaryFormatter formatter = new BinaryFormatter();
        // FileStream stream = new FileStream(path, FileMode.Open);
    }

    public static void SaveGameData(GameManager manager)
    {
        Debug.Log("SAVING GAME");
        // manager.players[0] is your character
        // manager.players[2] is the enemy
        // wait does this even matter?

        // saveManager = manager;
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "savedata.bin";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(manager);
        // playerName = manager.players[0].name;

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("GAME SAVED");
    }

    public static SaveData LoadGameData()
    {
        string path = Application.persistentDataPath + "savedata.bin";
        if (File.Exists(path))
        {
            Debug.Log("SAVE FILE EXISTS!");

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;

        }
        else
        {
            Debug.Log("NO SAVE FILE!");
            return null;
        }
    }


}
