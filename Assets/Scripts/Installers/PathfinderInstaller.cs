using UnityEngine;
using Zenject;

public class PathfinderInstaller : MonoInstaller<PathfinderInstaller> {
	public override void InstallBindings()
    {
        Debug.Log("Install Pathfinder");
        Container.Bind<IPathfinder>().To<Pathfinder>().AsSingle();
        Container.BindFactory<Spawner, Spawner.Factory>();
    }
}
