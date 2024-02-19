using Zenject;

public abstract class BaseInstaller<T> : Installer<T> where T : Installer<T>
{
    public override void InstallBindings()
    {
        DeclareSignals();
        BindHandlers();
        BindFactories();
        BindManagers();
        BindCommand();
        BindComponents();
        BindEntities();
        BindSystems();
    }

    protected virtual void DeclareSignals(){}
    protected virtual void BindManagers(){}
    protected virtual void BindEntities(){}
    protected virtual void BindSystems(){}
    protected virtual void BindComponents(){}
    protected virtual void BindCommand(){}
    protected virtual void BindHandlers(){}
    protected virtual void BindFactories(){}
}
