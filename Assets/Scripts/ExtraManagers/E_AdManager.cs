using UnityEngine;

public class E_AdManager : MonoBehaviour
{
    #region Field
    /* Singleton */
    static E_AdManager instance;
    #endregion

    #region Singleton
    E_AdManager() { }
    public static E_AdManager Instance { get { return instance; } }

    void SetSingleton()
    {
        if (instance == null) instance = GetComponent<E_AdManager>();
    }
    #endregion

    #region Monobehaviour
    void Awake()
    {
        SetSingleton();
    }
    #endregion
}
