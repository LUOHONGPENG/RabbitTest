using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Singleton<T> where T : class
{
    private static T instance;
    private static readonly object instanceLock = new object();

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (instanceLock)
                {
                    instance = (T)Activator.CreateInstance(typeof(T), true);
                }
            }
            return instance;
        }
    }
}
