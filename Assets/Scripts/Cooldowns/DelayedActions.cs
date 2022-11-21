using System;
using UnityEngine;
using UnityEngine.Events;
using CooldownManagement;

public class DelayedActions : MonoBehaviour
{
    [Serializable]
    public class Action
    {
        [SerializeField] internal float delay;
        [SerializeField] internal UnityEvent events;
    }

    [SerializeField] private Action[] actions;

    private void Start() {
        foreach (var action in actions) {
            Cooldown.Wait(action.delay)
              .OnComplete(() => {
                  action.events.Invoke();
              });
        }
    }
}