using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance 
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject(typeof(T).ToString());
                _instance = go.AddComponent<T>();
            }
            return _instance;
        }
    }    
}
