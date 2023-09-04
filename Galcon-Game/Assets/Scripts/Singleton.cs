using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;
    public static T instance => _instance;
    protected virtual void Awake()
    {
        if (_instance != null && this.gameObject != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = (T)this;
        }
    }
}
