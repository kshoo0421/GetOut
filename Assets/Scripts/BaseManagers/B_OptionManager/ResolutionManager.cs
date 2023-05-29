using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    #region Field
    /* Singleton */
    static ResolutionManager instance;
    #endregion

    #region Singleton
    ResolutionManager() { }

    public static ResolutionManager Instance { get { return instance; } }

    void SetSingleton()
    {
        if (instance == null)
        {
            instance = GetComponent<ResolutionManager>();
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