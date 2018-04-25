using UnityEngine;
using Zenject;

public class WebRequesterInstaller : MonoInstaller<WebRequesterInstaller>
{
    public override void InstallBindings()
    {
        Debug.Log("Install Requester");
        Container.Bind<IWebRequester>().To<WebRequester>().AsSingle();
    }
}