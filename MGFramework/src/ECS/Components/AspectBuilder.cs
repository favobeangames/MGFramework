using System;

namespace FavobeanGames.MGFramework.ECS;

public class AspectBuilder
{
    private Aspect aspect;

    public AspectBuilder()
    {
        aspect = new Aspect();
    }

    public AspectBuilder All(params Type[] types)
    {
        foreach (var type in types)
        {
            aspect.AllComponents.Add(type);
        }

        return this;
    }

    public AspectBuilder Any(params Type[] types)
    {
        foreach (var type in types)
        {
            aspect.AnyComponents.Add(type);
        }

        return this;
    }

    public AspectBuilder None(params Type[] types)
    {
        foreach (var type in types)
        {
            aspect.NoneComponents.Add(type);
        }

        return this;
    }

    public Aspect Build() => aspect;
}