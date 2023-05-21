using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_OptionManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ ±¸Çö
    private static B_OptionManager instance;
    private B_OptionManager() { }

    public static B_OptionManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<B_OptionManager>();
        }
    }
    #endregion

}
