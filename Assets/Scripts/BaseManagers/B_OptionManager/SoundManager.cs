using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Field
    /* Singleton */
    static SoundManager instance;
    #endregion

    #region Singleton
    SoundManager() { }

    public static SoundManager Instance { get { return instance; } }

    void SetSingleton()
    {
        if (instance == null)
        {
            instance = GetComponent<SoundManager>();
        }
    }
    #endregion

    #region Monobehaviour
    void Awake()
    {
        SetSingleton();    
    }
    #endregion
}