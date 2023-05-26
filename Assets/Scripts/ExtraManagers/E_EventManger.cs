using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_EventManger : MonoBehaviour, IExtraManager
{
    #region �̱��� ������
    private static E_EventManger instance;
    private E_EventManger() { }
    public static E_EventManger Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<E_EventManger>();
        }
    }
    #endregion
}
