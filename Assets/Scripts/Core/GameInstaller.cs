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

        Container.Bind<ISystemManager>().AsSingle();
        Container.Bind<RequestHandler>().AsSingle();
        #endregion
    }
}