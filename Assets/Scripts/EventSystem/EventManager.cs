using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : IEventManager  {

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
        if (this._eventDictionary == null) { Init(); }
        if (this._eventDictionary.TryGetValue(channel, out newEvent))
        {
            newEvent.AddListener(unityAction);
        } 
        else
        {
            newEvent = new UnityEvent();
            newEvent.AddListener(unityAction);
            _eventDictionary.Add(channel, newEvent);
        }
    }

    public void StopListening(Channel channel, UnityAction unityAction)
    {
        UnityEvent newEvent = null;
        if (_eventDictionary.TryGetValue(channel, out newEvent))
        {
            newEvent.RemoveListener(unityAction);
        }
    }

    public void Init() 
    {
        Debug.Log("Init Event Manager");
		if (_eventDictionary == null) {
            Debug.Log("Was Null");
			_eventDictionary = new Dictionary<Channel, UnityEvent>();
		}
	}

}
