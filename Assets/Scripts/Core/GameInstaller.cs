using Assets.Scripts.Core.Assets;
using Core.ECS;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        MainESCInstaller.Install(Container);
        AssetsSystemInstaller.Install(Container);
    }
}