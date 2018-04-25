using UnityEngine;
using Zenject;

public class EventInstaller : MonoInstaller<EventInstaller>
{
    public override void InstallBindings()
    {
        Debug.Log("Install Event");
        Container.Bind<IEventManager>().To<EventManager>().AsSingle();
    }
}