using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    GameObject obj = new GameObject(typeof(T) + "");
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }
   
}
