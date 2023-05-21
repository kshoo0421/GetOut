using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region �̱��� ����
    private static SoundManager instance;
    private SoundManager() { }

    public static SoundManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<SoundManager>();
        }
    }
    #endregion


}
