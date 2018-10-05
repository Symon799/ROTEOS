using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SailsRequesterInstaller : MonoInstaller<SailsRequesterInstaller> {

	public override void InstallBindings()
    {
        Debug.Log("Install Sails Requester");
        Container.Bind<ISailsRequester>().To<SailsRequester>().AsSingle();
        Container.BindFactory<Spawner, Spawner.Factory>();
    }
}
