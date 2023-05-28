using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_LobbyChatManager : MonoBehaviour
{
    #region �̱��� ������
    private static E_LobbyChatManager instance;
    private E_LobbyChatManager() { }
    public static E_LobbyChatManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<E_LobbyChatManager>();
        }
    }
    #endregion
}
