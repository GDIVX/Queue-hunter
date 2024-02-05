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

        Container.Bind<SystemManager>().AsSingle();
        Container.Bind<RequestHandler>().AsSingle();
        Container.Bind<EntityGenerator>().AsSingle();
        #endregion
    }
}