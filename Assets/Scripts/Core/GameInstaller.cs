using Assets.Scripts.Core.Assets;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        AssetsSystemInstaller.Install(Container);
    }
}