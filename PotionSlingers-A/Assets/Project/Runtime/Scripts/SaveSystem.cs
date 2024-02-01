using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    // declaring a private instance variable
    public static SaveSystem instance = null;

    public bool savedGame = false;

    // creating a public accessor that will get the instace
    public static SaveSystem Instance
    {
        get
        {
            // test if the instance is null
            // if so, try to get it using FindObjectOfType
            if (instance == null)
                instance = FindObjectOfType<SaveSystem>();

            // if the instance is null again
            // create a new game object
            // attached the Singleton class on it
            // set the instance to the new attached Singleton
            // call don't destroy on load
            if (instance == null)
            {
                GameObject gObj = new GameObject();
                gObj.name = "Singleton";
                instance = gObj.AddComponent<SaveSystem>();
                DontDestroyOnLoad(gObj);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void checkGameData()
    {
        string path = Application.persistentDataPath + "savedata.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            Debug.Log(data.stage);
            Debug.Log(data.playerCharName);
            Debug.Log(data.savedGame);

            GameObject obj = GameObject.Find("Save Data Text");
            obj.GetComponent<TMPro.TextMeshProUGUI>().text = "Current Save Data: Stage " + data.stage;
        }
        else
        {
            GameObject obj = GameObject.Find("Save Data Text");
            obj.GetComponent<TMPro.TextMeshProUGUI>().text = "Current Save Data: None";
        }

        // BinaryFormatter formatter = new BinaryFormatter();
        // FileStream stream = new FileStream(path, FileMode.Open);
    }

    public static void setNewGame()
    {
        string path = Application.persistentDataPath + "savedata.bin";
        if (File.Exists(path))
        {
            Debug.Log("SAVE FILE EXISTS!");

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            // setting new game flag
            data.savedGame = false;

            FileStream stream2 = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream2, data);
            stream2.Close();

        }
    }

    public static void setSaveGame()
    {
        string path = Application.persistentDataPath + "savedata.bin";
        if (File.Exists(path))
        {
            Debug.Log("SAVE FILE EXISTS!");

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            // setting new game flag
            data.savedGame = true;

            FileStream stream2 = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream2, data);
            stream2.Close();

        }
    }

    public static void checkGameDataWithManager()
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

    public static void SaveGameDataWithManager(GameManager manager)
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

    public static void SaveGameData(SaveData saveData)
    {
        Debug.Log("SAVING GAME");
        // manager.players[0] is your character
        // manager.players[2] is the enemy
        // wait does this even matter?

        // saveManager = manager;
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "savedata.bin";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, saveData);
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
