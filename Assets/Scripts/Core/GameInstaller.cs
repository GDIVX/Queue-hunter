using Assets.Scripts.Core.ECS;
using Assets.Scripts.Engine.ECS;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        #region ECS
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<EntityCreatedSignal>();
        Container.DeclareSignal<EntityModifiedSignal>();

        Container.Bind<GameSystem>();
        Container.Bind<ISystemManager>().To<SystemManager>().AsSingle();
        Container.Bind<IRequestable>().To<RequestHandler>().AsSingle();
        Container.Bind<IEntityGenerator>().To<EntityGenerator>().AsSingle();
        #endregion
    }
}