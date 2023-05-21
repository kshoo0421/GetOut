using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ItemManager : MonoBehaviour, IClassManager
{
    #region ½Ì±ÛÅæ »ý¼º¿ë
    private static C_ItemManager instance;
    private C_ItemManager() { }
    public static C_ItemManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<C_ItemManager>();
        }
    }
    #endregion
}
