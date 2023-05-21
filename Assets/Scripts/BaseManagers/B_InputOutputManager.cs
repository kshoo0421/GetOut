using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_InputOutputManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ ±¸Çö
    private static B_InputOutputManager instance;
    private B_InputOutputManager() { }

    public static B_InputOutputManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<B_InputOutputManager>();
        }
    }
    #endregion

}
