using UnityEngine;
using Zenject;

public class LocationFinderInstaller : MonoInstaller<LocationFinderInstaller>
{
    public override void InstallBindings()
    {
		Debug.Log("Install Location Finder");
        Container.Bind<ILocationFinder>().To<LocationFinder>().AsSingle();
    }
}