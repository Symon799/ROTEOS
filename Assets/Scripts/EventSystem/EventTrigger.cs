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
			_eventManager.Trigger(channel);
		}
	}
}
