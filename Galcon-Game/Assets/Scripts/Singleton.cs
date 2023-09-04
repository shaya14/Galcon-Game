using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    // CR: (style)
    //     private static T _instance;
    //     public static T instance => _instance;
    private static T instance;
    public static T Instance { get { return instance; } }
    protected virtual void Awake()
    {
        if (instance != null && this.gameObject != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this;
        }
    }
}
