using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ ±¸Çö
    private static SoundManager instance;
    private SoundManager() { }

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SoundManager();
            }
            return instance;
        }
    }
    #endregion


}
