using UnityEngine;

public class B_TimeManager : MonoBehaviour
{
    #region Field
    /* Singleton */
    static B_TimeManager instance;
    #endregion

    #region Singleton
    B_TimeManager() { }
    public static B_TimeManager Instance { get { return instance; } }

    void SetSingleton()
    {
        if (instance == null) instance = GetComponent<B_TimeManager>();
    }
    #endregion

    #region Monobehaviour
    void Awake()
    {
        SetSingleton();
    }
    #endregion
}
