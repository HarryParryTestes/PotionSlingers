using System;
using System.Linq;
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

        public static MapPlayerTracker Instance;

        public bool Locked { get; set; }

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
            GameObject transition = GameObject.Find("SceneTransition");
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
                    break;
                case NodeType.Store:
                    break;
                case NodeType.Boss:
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
        }

        private void PlayWarningThatNodeCannotBeAccessed()
        {
            Debug.Log("Selected node cannot be accessed");
        }
    }
}