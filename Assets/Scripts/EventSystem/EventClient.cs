using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class EventClient : MonoBehaviour {

	[Inject]
	private IEventManager _eventManager;
	protected List<UnityAction> _events;
	public Channel channel;

	// Use this for initialization
	void OnEnable () {
		if (!EventsIsNull()) {
			foreach (UnityAction action in _events) {
				Debug.Log("Add new action... " + action.ToString());
				_eventManager.StartListening(channel, action);
			}
		}
	}

	void OnDisable() {
		if (!EventsIsNull()) {
			foreach (UnityAction action in _events) {
				_eventManager.StopListening(channel, action);
			}
		}
	}

	protected bool EventsIsNull() {
		return (_events == null);
	}

	protected void AddToEvents(UnityAction action) {
		if (EventsIsNull())
			_events = new List<UnityAction>();
		_events.Add(action);
	}
}
