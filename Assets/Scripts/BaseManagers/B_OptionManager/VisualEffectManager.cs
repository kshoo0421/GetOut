using UnityEngine;

public class VisualEffectManager : MonoBehaviour
{
    #region Field
    static VisualEffectManager instance;
    #endregion

    #region Singleton
    VisualEffectManager() { }

    public static VisualEffectManager Instance { get { return instance; } }

    void SetSingleton()
    {
        if (instance == null) instance = GetComponent<VisualEffectManager>();
    }
    #endregion

    #region Monoviour
    void Awake()
    {
        SetSingleton();    
    }
    #endregion
}