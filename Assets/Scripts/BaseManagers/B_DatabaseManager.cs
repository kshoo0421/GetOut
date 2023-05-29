using UnityEngine;

public class B_DatabaseManager : MonoBehaviour
{
    #region Field
    static B_DatabaseManager instance;
    #endregion

    #region Singleton
    B_DatabaseManager() { }
    public static B_DatabaseManager Instance { get { return instance; } }

    void SetSingleton()
    {
        if (instance == null) instance = GetComponent<B_DatabaseManager>();
    }
    #endregion

    #region Monobehaviour
    private void Awake()
    {
        SetSingleton();
    }
    #endregion
}