using UnityEngine;

public class E_LobbyChatManager : MonoBehaviour
{
    #region Field
    static E_LobbyChatManager instance;
    #endregion

    #region Singleton
    E_LobbyChatManager() { }
    public static E_LobbyChatManager Instance { get { return instance; } }

    void SetSingleton()
    {
        if (instance == null) instance = GetComponent<E_LobbyChatManager>();
    }
    #endregion

    #region Monobehaviour
    void Awake()
    {
        SetSingleton();    
    }
    #endregion
}
