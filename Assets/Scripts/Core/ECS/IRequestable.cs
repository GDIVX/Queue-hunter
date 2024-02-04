using System;

namespace Assets.Scripts.Engine.ECS
{
    public interface IRequestable
    {
        RequestHandler.Request Schedule(Action action);
    }
}