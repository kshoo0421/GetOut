using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_MissionManager : MonoBehaviour, IClassManager
{
    #region ½Ì±ÛÅæ »ý¼º¿ë
    private static C_MissionManager instance;
    private C_MissionManager() { }
    public static C_MissionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new C_MissionManager();
            }
            return instance;
        }
    }
    #endregion
}
