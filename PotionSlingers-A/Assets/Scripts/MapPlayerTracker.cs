using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace Map
{
    public class MapPlayerTracker : MonoBehaviour
    {
        public bool lockAfterSelecting = false;
        public float enterNodeDelay = 1f;
        public MapManager mapManager;
        public MapView view;
        public static System.Random rng = new System.Random();
        public static MapPlayerTracker Instance;
        

        public bool Locked { get; set; }

        /*
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
        */

        private void Awake()
        {
            Instance = this;
        }

        public void SelectNode(MapNode mapNode)
        {
            if (Locked) return;

            // Debug.Log("Selected node: " + mapNode.Node.point);

            if (mapManager.CurrentMap.path.Count == 0)
            {
                // player has not selected the node yet, he can select any of the nodes with y = 0
                if (mapNode.Node.point.y == 0)
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
            else
            {
                var currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1];
                var currentNode = mapManager.CurrentMap.GetNode(currentPoint);

                if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(mapNode.Node.point)))
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
        }

        private void SendPlayerToNode(MapNode mapNode)
        {
            Locked = lockAfterSelecting;
            mapManager.CurrentMap.path.Add(mapNode.Node.point);
            mapManager.SaveMap();
            view.SetAttainableNodes();
            view.SetLineColors();
            mapNode.ShowSwirlAnimation();

            DOTween.Sequence().AppendInterval(enterNodeDelay).OnComplete(() => EnterNode(mapNode));
        }

        private static void EnterNode(MapNode mapNode)
        {
            // we have access to blueprint name here as well
            Debug.Log("Entering node: " + mapNode.Node.blueprintName + " of type: " + mapNode.Node.nodeType);
            // load appropriate scene with context based on nodeType:
            // or show appropriate GUI over the map: 
            // if you choose to show GUI in some of these cases, do not forget to set "Locked" in MapPlayerTracker back to false
            SaveData saveData = SaveSystem.LoadGameData();
            if (mapNode.Node.nodeType == NodeType.Carnival)
                saveData.currentEnemyName = "Carnival";
            else
                saveData.currentEnemyName = mapNode.enemyName;
            Debug.Log("Current enemy is " + saveData.currentEnemyName + "!!!");
            SaveSystem.SaveGameData(saveData);
            GameObject transition = GameObject.Find("SceneTransition");

            if (mapNode.Node.nodeType == NodeType.Carnival)
            {
                transition.GetComponent<SceneTransition>().mapCanvasGroup.blocksRaycasts = false;
                transition.GetComponent<SceneTransition>().doTransition();
                return;
            }

            if (mapNode.Node.nodeType == NodeType.Store)
            {
                // transition.GetComponent<SceneTransition>().mapCanvasGroup.blocksRaycasts = false;
                // transition.GetComponent<SceneTransition>().doTransition();
                Debug.Log("Store code here!");
                return;
            }

            if (mapNode.Node.nodeType == NodeType.Treasure)
            {
                transition.GetComponent<SceneTransition>().treasureMenu.SetActive(true);
                transition.GetComponent<SceneTransition>().treasureMenu.GetComponent<TreasureMenu>().chooseCards();
                transition.GetComponent<SceneTransition>().treasureMenu.GetComponent<TreasureMenu>().treasureAnimInit();
                return;
            }

            if (mapNode.Node.nodeType == NodeType.RestSite)
            {
                mapNode.textbox = GameObject.Find("Fully Healed");
                mapNode.textbox2 = Instantiate(mapNode.textbox, mapNode.transform);
                mapNode.textbox2.GetComponent<TMPro.TextMeshProUGUI>().text = "Fully healed!";
                mapNode.StartCoroutine(mapNode.HideText());
                transition.GetComponent<SceneTransition>().healPlayer();
                
                return;
            }

            if (mapNode.Node.nodeType == NodeType.Mystery)
            {
                // transition.GetComponent<SceneTransition>().mapCanvasGroup.blocksRaycasts = false;
                // transition.GetComponent<SceneTransition>().doTransition();
                Debug.Log("Entering cutscene space!!!");

                switch (saveData.currentEnemyName)
                {
                    case "Slinger":

                        if (saveData.slingerName == null)
                        {
                            Debug.Log("Slinger not picked yet!!!");
                            // int num = rng.Next(0, transition.GetComponent<SceneTransition>().slingerPool.Count);
                            int num = transition.GetComponent<SceneTransition>().getSlinger();
                            if (transition.GetComponent<SceneTransition>().slingerPool[num] == "Saltimbocca")
                            {
                                saveData.currentEnemyName = "Saltimbocca";
                                saveData.slingerName = saveData.currentEnemyName;
                                saveData.slingerEncounter = 1;
                                SaveSystem.SaveGameData(saveData);
                                transition.GetComponent<SceneTransition>().hideUI();
                                transition.GetComponent<SceneTransition>().handleSaltCutscene();
                                break;
                            }
                            if (transition.GetComponent<SceneTransition>().slingerPool[num] == "Reets")
                            {
                                Debug.Log("Implement Reets!!!");
                                saveData.currentEnemyName = "Reets";
                                saveData.slingerName = saveData.currentEnemyName;
                                saveData.slingerEncounter = 1;
                                SaveSystem.SaveGameData(saveData);
                                transition.GetComponent<SceneTransition>().hideUI();
                                transition.GetComponent<SceneTransition>().handleReetsCutscene();
                                break;
                            }
                            if (transition.GetComponent<SceneTransition>().slingerPool[num] == "Bolo")
                            {
                                Debug.Log("Implement Bolo!!!");
                                saveData.currentEnemyName = "Bolo";
                                saveData.slingerName = saveData.currentEnemyName;
                                saveData.slingerEncounter = 1;
                                SaveSystem.SaveGameData(saveData);
                                transition.GetComponent<SceneTransition>().hideUI();
                                transition.GetComponent<SceneTransition>().handleBoloCutscene();
                                break;
                            }
                            if (transition.GetComponent<SceneTransition>().slingerPool[num] == "Isadore")
                            {
                                Debug.Log("Implement Isadore!!!");
                                saveData.currentEnemyName = "Isadore";
                                saveData.slingerName = saveData.currentEnemyName;
                                saveData.slingerEncounter = 1;
                                SaveSystem.SaveGameData(saveData);
                                transition.GetComponent<SceneTransition>().hideUI();
                                transition.GetComponent<SceneTransition>().handleIsadoreCutscene();
                                break;
                            }
                        }
                        else
                        {
                            // 2nd encounter with the slinger during the story mode
                            Debug.Log("2nd+ Encounter!!!");
                            switch (saveData.slingerName)
                            {
                                case "Saltimbocca":
                                    if (saveData.slingerEncounter == 1)
                                    {
                                        saveData.currentEnemyName = saveData.slingerName;
                                        saveData.slingerEncounter++;
                                        transition.GetComponent<SceneTransition>().hideUI();
                                        transition.GetComponent<SceneTransition>().handleSaltCutscene();
                                    }
                                    break;
                                case "Reets":
                                    if (saveData.slingerEncounter == 1)
                                    {
                                        saveData.currentEnemyName = saveData.slingerName;
                                        saveData.slingerEncounter++;
                                        transition.GetComponent<SceneTransition>().hideUI();
                                        transition.GetComponent<SceneTransition>().handleReetsCutscene();
                                    }
                                    break;
                                case "Bolo":
                                    if (saveData.slingerEncounter == 1)
                                    {
                                        saveData.currentEnemyName = saveData.slingerName;
                                        saveData.slingerEncounter++;
                                        transition.GetComponent<SceneTransition>().hideUI();
                                        transition.GetComponent<SceneTransition>().handleBoloCutscene();
                                    }
                                    break;
                                case "Isadore":
                                    if (saveData.slingerEncounter == 1)
                                    {
                                        saveData.currentEnemyName = saveData.slingerName;
                                        saveData.slingerEncounter++;
                                        transition.GetComponent<SceneTransition>().hideUI();
                                        transition.GetComponent<SceneTransition>().handleIsadoreCutscene();
                                    }
                                    break;
                            }
                            SaveSystem.SaveGameData(saveData);
                        }
                        break;
                    case "Crowpunk":
                        Debug.Log(saveData.playerCharName);

                        transition.GetComponent<SceneTransition>().handleCrowCutscene();
                        break;
                }

                return;
            }

            switch (saveData.currentEnemyName)
            {
                case "Bag o' Snakes":
                    transition.GetComponent<SceneTransition>().doTransition();
                    break;
                case "Dippit":
                    transition.GetComponent<SceneTransition>().doTransition();
                    break;
                case "Maskid":
                    transition.GetComponent<SceneTransition>().doTransition();
                    break;
                case "Dipkid":
                    transition.GetComponent<SceneTransition>().doTransition();
                    break;
                case "Bag o' Snakes+":
                    transition.GetComponent<SceneTransition>().doTransition();
                    break;
                case "Fingas":
                    transition.GetComponent<SceneTransition>().doTransition();
                    break;
                case "Fingas+":
                    transition.GetComponent<SceneTransition>().doTransition();
                    break;
                case "Crowpunk+":
                    transition.GetComponent<SceneTransition>().doTransition();
                    break;
                case "Singelotte":
                    transition.GetComponent<SceneTransition>().hideUI();
                    transition.GetComponent<SceneTransition>().singeButton.onClick.Invoke();
                    break;
            }
            /*
            switch (mapNode.Node.nodeType)
            {
                case NodeType.MinorEnemy:
                    if(saveData.stage == 1)
                    {
                        transition.GetComponent<SceneTransition>().doTransition();
                        break;
                    }
                       
                    else if (saveData.stage == 2)
                    {
                        transition.GetComponent<SceneTransition>().saltButton.onClick.Invoke();
                        // transition.GetComponent<SceneTransition>().doTransition();
                        break;
                    }
                    else if (saveData.stage == 3)
                    {
                        transition.GetComponent<SceneTransition>().crowButton.onClick.Invoke();
                        // transition.GetComponent<SceneTransition>().doTransition();
                        break;
                    }
                    else if (saveData.stage == 5)
                    {
                        transition.GetComponent<SceneTransition>().demoOverButton.onClick.Invoke();
                        break;
                    }

                    break;
                case NodeType.EliteEnemy:
                    break;
                case NodeType.RestSite:
                    transition.GetComponent<SceneTransition>().healPlayer();
                    break;
                case NodeType.Treasure:
                    transition.GetComponent<SceneTransition>().treasureMenu.SetActive(true);
                    break;
                case NodeType.Store:
                    break;
                case NodeType.Boss:
                    saveData.stage = 4;
                    SaveSystem.SaveGameData(saveData);
                    transition.GetComponent<SceneTransition>().singeButton.onClick.Invoke();
                    if (saveData.stage == 5)
                    {
                        transition.GetComponent<SceneTransition>().demoOverButton.onClick.Invoke();
                        break;
                    }
                    break;
                case NodeType.Mystery:
                    // transition.GetComponent<SceneTransition>().doTransition();
                    if (saveData.stage == 1)
                    {
                        transition.GetComponent<SceneTransition>().doTransition();
                        break;
                    }

                    else if (saveData.stage == 2)
                    {
                        transition.GetComponent<SceneTransition>().saltButton.onClick.Invoke();
                        // transition.GetComponent<SceneTransition>().doTransition();
                        break;
                    }
                    else if (saveData.stage == 3)
                    {
                        transition.GetComponent<SceneTransition>().crowButton.onClick.Invoke();
                        // transition.GetComponent<SceneTransition>().doTransition();
                        break;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            */
        }

        private void PlayWarningThatNodeCannotBeAccessed()
        {
            Debug.Log("Selected node cannot be accessed");
        }
    }
}