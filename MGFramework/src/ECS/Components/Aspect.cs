using System;
using System.Collections.Generic;
using MonoGame.Extended.Collections;

namespace FavobeanGames.MGFramework.ECS;

public class Aspect
{
    private ComponentService componentService;

    public Bag<Type> AllComponents;
    public Bag<Type> AnyComponents;
    public Bag<Type> NoneComponents;
    public Aspect()
    {
        AllComponents = new Bag<Type>();
        AnyComponents = new Bag<Type>();
        NoneComponents = new Bag<Type>();
    }

    public static AspectBuilder All(params Type[] types)
    {
        return new AspectBuilder().All(types);
    }

    public static AspectBuilder Any(params Type[] types)
    {
        return new AspectBuilder().Any(types);
    }

    public static AspectBuilder None(params Type[] types)
    {
        return new AspectBuilder().None(types);
    }

    public void Initialize(ComponentService componentService)
    {
        this.componentService = componentService;
    }

    public bool CheckValidEntity(int entityId)
    {
        if (AllComponents.Count > 0 && !componentService.EntityContainsAllComponents(entityId, AllComponents))
            return false;

        if (AnyComponents.Count > 0 && !componentService.EntityContainsAnyComponents(entityId, AnyComponents))
            return false;

        if (NoneComponents.Count > 0 && !componentService.EntityContainsNoComponents(entityId, NoneComponents))
            return false;

        return true;
    }
}
