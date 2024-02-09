using Assets.Scripts.Core.Assets;
using Assets.Scripts.Core.ECS;
using Assets.Scripts.Engine.ECS;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        #region ECS
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<EntityCreatedSignal>();
        Container.DeclareSignal<EntityModifiedSignal>();

        Container.Bind<IRequestable>().To<RequestHandler>().FromNewComponentOnNewGameObject().AsSingle();

        Container.Bind<ISystemManager>().To<SystemManager>().AsSingle();
        Container.Bind<IEntityFactory>().To<EntityFactory>().AsSingle();
        Container.Bind<IComponentsFactory>().To<ComponentsFactory>().AsSingle();
        #endregion

        #region Assets

        Container.Bind<IAssetsManager>().To<AssetsManager>().AsSingle();
        #endregion

    }
}