using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class EventTrigger : MonoBehaviour {
	[Inject]
	private IEventManager _eventManager;
	public Channel channel;
	
	// Use this for initialization
	public void Trigger() {
		if (_eventManager != null) {
			Debug.Log("GOOD");
			_eventManager.Trigger(channel);
		}
		else
			Debug.Log("BAD");
	}
}
