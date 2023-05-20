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
        get
        {
            if (instance == null)
            {
                instance = new GoldManager();
            }
            return instance;
        }
    }
    #endregion
}
