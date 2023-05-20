using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiManager : MonoBehaviour
{
    #region �̱��� ������
    private static EmojiManager instance;
    private EmojiManager() { }
    public static EmojiManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EmojiManager();
            }
            return instance;
        }
    }
    #endregion
}
