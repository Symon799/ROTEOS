using UnityEngine;
using Zenject;

public class EventInstaller : MonoInstaller<EventInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IEventManager>().To<EventManager>().AsSingle();
    }
}