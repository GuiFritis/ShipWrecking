using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallPooling : PoolBase<CannonBallBase>
{
    public static CannonBallPooling Instance;

    protected override void Singleton()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override bool CheckItem(CannonBallBase item)
    {
        return !item.gameObject.activeInHierarchy;
    }
}
