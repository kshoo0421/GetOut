using UnityEngine;

public class EmojiManager : MonoBehaviour
{
    #region �̱��� ������
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
