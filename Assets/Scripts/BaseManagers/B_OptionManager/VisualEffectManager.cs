using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ ±¸Çö
    private static VisualEffectManager instance;
    private VisualEffectManager() { }

    public static VisualEffectManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<VisualEffectManager>();
        }
    }
    #endregion
}
