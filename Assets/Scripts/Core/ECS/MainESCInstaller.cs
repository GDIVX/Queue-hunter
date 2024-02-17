using Assets.Scripts.Core.ECS;
using Assets.Scripts.Engine.ECS;
using Zenject;

namespace Core.ECS
{
    public class MainESCInstaller : BaseInstaller<MainESCInstaller>
    {
        protected override void DeclareSignals()
        {
            Container.DeclareSignal<EntityCreatedSignal>();
            Container.DeclareSignal<EntityModifiedSignal>();
        }

        protected override void BindHandlers()
        {
            Container.Bind<IRequestable>().To<RequestHandler>().FromNewComponentOnNewGameObject().AsSingle();
        }

        protected override void BindFactories()
        {
            Container.Bind<IEntityFactory>().To<EntityFactory>().AsSingle();
        }

        protected override void BindSystems()
        {
            Container.Bind<ISystemManager>().To<SystemManager>().AsSingle();
        }

        protected override void BindComponents()
        {
            Container.Bind<IComponentsFactory>().To<ComponentsFactory>().AsSingle();
        }
    }
}