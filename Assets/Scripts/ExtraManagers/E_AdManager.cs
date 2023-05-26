using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_AdManager : MonoBehaviour, IExtraManager
{
    #region ½Ì±ÛÅæ »ý¼º¿ë
    private static E_AdManager instance;
    private E_AdManager() { }
    public static E_AdManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<E_AdManager>();
        }
    }
    #endregion
}
