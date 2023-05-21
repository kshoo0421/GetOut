using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ »ý¼º¿ë
    private static EmojiManager instance;
    private EmojiManager() { }
    public static EmojiManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<EmojiManager>();
        }
    }
    #endregion
}
