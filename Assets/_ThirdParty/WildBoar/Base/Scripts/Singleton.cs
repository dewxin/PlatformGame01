using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Singleton
{
    protected static Dictionary<Type,Singleton> type2SingletonDict = new Dictionary<Type, Singleton>();

    protected abstract void Clear();
    protected virtual bool NeedResetWhenPlay => true;

    public static void Reset()
    {
        var resetList = type2SingletonDict.Values.Where(singleton => singleton.NeedResetWhenPlay).ToList();
        foreach(var singleton in resetList)
        {
            singleton.Clear();
            type2SingletonDict.Remove(singleton.GetType());
        }

    }
}

public abstract class Singleton<T> :Singleton
    where T: Singleton<T>, new() 
{

    private static T _instance;

    public static T Instance 
    { 
        get 
        {
            if(_instance == null )
            {
                _instance= new T();
                _instance.Init();
                type2SingletonDict.Add(typeof(T), _instance);
            }
            return _instance; 
        }
    }

    protected override void Clear()
    {
        if(_instance is IDisposable)
        {
            IDisposable disposable = ( IDisposable ) _instance;
            disposable.Dispose();
        }
        _instance= null;
    }

    protected virtual void Init()
    {

    }


}
