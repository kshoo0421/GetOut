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
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<GoldManager>();
        }
    }
    #endregion
}
