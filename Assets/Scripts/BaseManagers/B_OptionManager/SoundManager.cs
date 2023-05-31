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
            curSEVolume = 1.0f; // db에서 가져오는 작업 필요
            curBGMVolume = 1.0f;
            Debug.Log("SM 초기화 완료");
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