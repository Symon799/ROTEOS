using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EventManagerPrefab : MonoBehaviour {

	[Inject]
	private IEventManager _eventManager;
}
