using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameEventListener : MonoBehaviour
{
    [SerializeField] GameEvent gameEvent;
    [SerializeField] UnityEvent unityEvent;

    void Awake() => gameEvent.Register(this);

    void OnDestroy() => gameEvent.Deregister(this);

    public void RaiseEvent() => unityEvent.Invoke();
   
}
