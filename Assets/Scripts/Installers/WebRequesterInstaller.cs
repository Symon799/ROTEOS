using UnityEngine;
using Zenject;

public class WebRequesterInstaller : MonoInstaller<WebRequesterInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IWebRequester>().To<WebRequester>().AsSingle();
    }
}