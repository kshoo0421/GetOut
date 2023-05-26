using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_CustumMatchingChatManager : MonoBehaviour, IExtraManager
{
    #region �̱��� ������
    private static E_CustumMatchingChatManager instance;
    private E_CustumMatchingChatManager() { }
    public static E_CustumMatchingChatManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<E_CustumMatchingChatManager>();
        }
    }
    #endregion
}
