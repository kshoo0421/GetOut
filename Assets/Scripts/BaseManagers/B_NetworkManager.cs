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
        get
        {
            if (instance == null)
            {
                instance = new B_NetworkManager();
            }
            return instance;
        }
    }
    #endregion

}
