using UnityEngine;
using Zenject;

public class MeteoManagerInstaller : MonoInstaller<MeteoManagerInstaller>
{
    public override void InstallBindings()
    {
		Debug.Log("Install Meteo Manager");
        Container.Bind<IMeteoManager>().To<MeteoManager>().AsSingle();
    }
}