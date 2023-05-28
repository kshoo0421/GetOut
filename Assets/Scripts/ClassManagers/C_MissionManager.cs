using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_MissionManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ »ý¼º¿ë
    private static C_MissionManager instance;
    private C_MissionManager() { }
    public static C_MissionManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<C_MissionManager>();
        }
    }
    #endregion
}
