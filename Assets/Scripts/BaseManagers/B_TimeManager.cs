using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_TimeManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ ±¸Çö
    private static B_TimeManager instance;
    private B_TimeManager() { }

    public static B_TimeManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<B_TimeManager>();
        }
    }
    #endregion

}
