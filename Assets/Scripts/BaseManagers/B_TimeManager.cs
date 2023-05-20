using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_TimeManager : MonoBehaviour, IBaseManager
{
    #region ½Ì±ÛÅæ ±¸Çö
    private static B_TimeManager instance;
    private B_TimeManager() { }

    public static B_TimeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new B_TimeManager();
            }
            return instance;
        }
    }
    #endregion

}
