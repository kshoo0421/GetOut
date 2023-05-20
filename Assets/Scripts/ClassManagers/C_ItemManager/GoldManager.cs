using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ »ý¼º¿ë
    private static GoldManager instance;
    private GoldManager() { }
    public static GoldManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GoldManager();
            }
            return instance;
        }
    }
    #endregion
}
