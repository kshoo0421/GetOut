using UnityEngine;

public class E_CustumMatchingChatManager : MonoBehaviour
{
    #region Field
    /* Singleton */
    static E_CustumMatchingChatManager instance;
    #endregion

    #region Singleton
    E_CustumMatchingChatManager() { }
    public static E_CustumMatchingChatManager Instance { get { return instance; } }

    void SetSingleton()
    {
        if (instance == null) instance = GetComponent<E_CustumMatchingChatManager>();
    }
    #endregion

    #region Monobehaviour
    void Awake()
    {
        SetSingleton();    
    }
    #endregion
}
