using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_DataManager : MonoBehaviour
{
    #region �̱��� ������
    private static C_DataManager instance;
    private C_DataManager() { }
    public static C_DataManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<C_DataManager>();
        }
    }
    #endregion

}
