using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ ±¸Çö
    private static ResolutionManager instance;
    private ResolutionManager() { }

    public static ResolutionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ResolutionManager();
            }
            return instance;
        }
    }
    #endregion
}