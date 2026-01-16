using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

namespace Map
{
    public class MapManager : MonoBehaviour
    {
        public MapConfig config;
        public MapConfig stage2Config;
        public MapView view;

        public Map CurrentMap { get; private set; }

        private void Start()
        {
            SaveData data = SaveSystem.LoadGameData();
            if (!data.savedGame)
            {
                Debug.Log("New game started! Making new map...");
                GenerateNewMap();
                return;
            }

            if (data.newWorld)
            {
                Debug.Log("Generating new world!!!");

                if (data.world == 2)
                    GenerateNewMap(stage2Config);

                data.newWorld = false;
                SaveSystem.SaveGameData(data);
                return;
            }

            if (PlayerPrefs.HasKey("Map"))
            {
                var mapJson = PlayerPrefs.GetString("Map");
                var map = JsonConvert.DeserializeObject<Map>(mapJson);
                // using this instead of .Contains()
                if (map.path.Any(p => p.Equals(map.GetBossNode().point)))
                {
                    // payer has already reached the boss, generate a new map
                    GenerateNewMap();
                }
                else
                {
                    CurrentMap = map;
                    // player has not reached the boss yet, load the current map
                    view.ShowMap(map);
                }
            }
            else
            {
                GenerateNewMap();
            }
        }

        public void GenerateNewMap(MapConfig config)
        {
            Debug.Log("Generating world 2 map");
            var map = MapGenerator.GetMap(config);
            CurrentMap = map;
            Debug.Log(map.ToJson());
            view.ShowMap(map);
        }

        public void GenerateNewMap()
        {
            var map = MapGenerator.GetMap(config);
            CurrentMap = map;
            Debug.Log(map.ToJson());
            view.ShowMap(map);
        }

        public void SaveMap()
        {
            if (CurrentMap == null) return;

            var json = JsonConvert.SerializeObject(CurrentMap, Formatting.Indented,
                new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            PlayerPrefs.SetString("Map", json);
            PlayerPrefs.Save();
        }

        private void OnApplicationQuit()
        {
            SaveMap();
        }
    }
}
