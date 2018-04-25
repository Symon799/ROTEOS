using UnityEngine;
using Zenject;

public class MeteoInstaller : MonoInstaller<MeteoInstaller>
{
    public override void InstallBindings()
    {
		Debug.Log("Install Meteo");
        Container.Bind<IMeteoStatus>().To<MeteoStatus>().AsSingle();
    }
}