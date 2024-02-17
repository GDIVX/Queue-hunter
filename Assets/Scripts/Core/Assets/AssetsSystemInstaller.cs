namespace Assets.Scripts.Core.Assets
{
    public class AssetsSystemInstaller : BaseInstaller<AssetsSystemInstaller>
    {
        protected override void BindManagers()
        {
            Container.Bind<IAssetsManager>().To<AssetsManager>().AsSingle();
        }
    }
}