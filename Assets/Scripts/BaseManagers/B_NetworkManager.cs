using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_NetworkManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ ±¸Çö
    private static B_NetworkManager instance;
    private B_NetworkManager() { }

    public static B_NetworkManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<B_NetworkManager>();
        }
    }
    #endregion

}
