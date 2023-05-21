using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    #region �̱��� ������
    private static GoldManager instance;
    private GoldManager() { }
    public static GoldManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<GoldManager>();
        }
    }
    #endregion
}
