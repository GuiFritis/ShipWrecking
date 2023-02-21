using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolBase<T> : MonoBehaviour where T : Behaviour
{
    public int preWarmSize = 2;
    public T PFB_item;

    protected List<T> _pool = new List<T>();

    protected abstract void Singleton();

    protected virtual void Awake()
    {
        Singleton();
        InitPool();
    }

    private void InitPool()
    {
        _pool = new List<T>();

        for (int i = 0; i < preWarmSize; i++)
        {
            CreatePoolItem();
        }
    }

    private void CreatePoolItem()
    {
        T item = Instantiate(PFB_item, gameObject.transform);
        _pool.Add(item);
    }

    public T GetPoolItem()
    {
        T item = null;
        item = _pool.Find(CheckItem);
        if(item == null)
        {
            CreatePoolItem();
            item = _pool[^1];
        }
        return item;
    }

    protected abstract bool CheckItem(T item);
}
