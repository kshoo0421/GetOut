using UnityEngine;

public class E_EventManger : MonoBehaviour
{
    #region Field
    /* Singleton */
    static E_EventManger instance;
    #endregion

    #region Singleton
    E_EventManger() { }
    public static E_EventManger Instance { get { return instance; } }

    void SetSingleton()
    {
        if (instance == null) instance = GetComponent<E_EventManger>();
    }
    #endregion

    #region Monobehaviour
    private void Awake()
    {
        SetSingleton();
    }
    #endregion
}
