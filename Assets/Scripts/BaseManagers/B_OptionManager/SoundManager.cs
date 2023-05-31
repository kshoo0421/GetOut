using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Field
    /* Singleton */
    static SoundManager instance;
    /* current Sound Effect Volume */
    public float curSEVolume { get; set; }
    public float curBGMVolume { get; set; }


    #endregion

    #region Singleton
    SoundManager() { }

    public static SoundManager Instance { get { return instance; } }

    void SetSingleton()
    {
        if (instance == null)
        {
            instance = GetComponent<SoundManager>();
            curSEVolume = 1.0f; // db���� �������� �۾� �ʿ�
            curBGMVolume = 1.0f;
            Debug.Log("SM �ʱ�ȭ �Ϸ�");
            Debug.Log(curBGMVolume + " / " + curBGMVolume);
        }
    }
    #endregion

    #region Monobehaviour
    void Awake()
    {
        SetSingleton();    
    }
    #endregion

    #region Set Sound

    #endregion
}