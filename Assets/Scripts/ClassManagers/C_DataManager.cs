using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_DataManager : MonoBehaviour, IClassManager
{
    #region ½Ì±ÛÅæ »ý¼º¿ë
    private static C_DataManager instance;
    private C_DataManager() { }
    public static C_DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new C_DataManager();
            }
            return instance;
        }
    }
    #endregion

}
