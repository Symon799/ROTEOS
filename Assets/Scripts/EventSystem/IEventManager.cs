using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Channel {
	Alpha,
	Beta
}

public interface IEventManager {

    void Trigger(Channel channel);

    void StartListening(Channel channel, UnityAction unityAction);

    void StopListening(Channel channel, UnityAction unityAction);

    void Init();

}
