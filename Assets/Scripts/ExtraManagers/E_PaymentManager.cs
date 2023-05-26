using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_PaymentManager : MonoBehaviour, IExtraManager
{
    #region ½Ì±ÛÅæ »ý¼º¿ë
    private static E_PaymentManager instance;
    private E_PaymentManager() { }
    public static E_PaymentManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<E_PaymentManager>();
        }
    }
    #endregion
}