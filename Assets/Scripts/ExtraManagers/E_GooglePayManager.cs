using UnityEngine;

public class E_GooglePayManager : MonoBehaviour
{
    #region Field
    static E_GooglePayManager instance;
    #endregion

    #region Singleton
    E_GooglePayManager() { }
    public static E_GooglePayManager Instance { get { return instance; } }

    void SetSingleton()
    {
        if (instance == null) instance = GetComponent<E_GooglePayManager>();
    }
    #endregion

    #region Monobehaviour
    void Awake()
    {
        SetSingleton();
    }
    #endregion
}
