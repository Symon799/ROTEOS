using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : IEventManager
{

    private Dictionary<Channel, UnityEvent> _eventDictionary;

    public void Trigger(Channel channel)
    {
        UnityEvent newEvent = null;
        if (_eventDictionary.TryGetValue(channel, out newEvent))
        {
            Debug.Log("Invoke...");
            newEvent.Invoke();
        }
    }

    public void StartListening(Channel channel, UnityAction unityAction)
    {
        UnityEvent newEvent = null;
        if (dictionaryInstance().TryGetValue(channel, out newEvent))
        {
            newEvent.AddListener(unityAction);
        }
        else
        {
            newEvent = new UnityEvent();
            newEvent.AddListener(unityAction);
            dictionaryInstance().Add(channel, newEvent);
        }
    }

    public void StopListening(Channel channel, UnityAction unityAction)
    {
        UnityEvent newEvent = null;
        if (dictionaryInstance().TryGetValue(channel, out newEvent))
        {
            newEvent.RemoveListener(unityAction);
        }
    }

    private Dictionary<Channel, UnityEvent> dictionaryInstance()
    {
        if (_eventDictionary == null)
            _eventDictionary = new Dictionary<Channel, UnityEvent>();
        return _eventDictionary;
    }
}
