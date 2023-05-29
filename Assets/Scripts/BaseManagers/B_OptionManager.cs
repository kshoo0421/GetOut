using UnityEngine;

public class B_OptionManager : MonoBehaviour
{
    #region Field
    static B_OptionManager instance;
    #endregion

    #region Singleton
    B_OptionManager() { }
    public static B_OptionManager Instance { get { return instance; } }

    void SetSingleton()
    {
        if (instance == null) instance = GetComponent<B_OptionManager>();
    }
    #endregion

    #region Monobehaviour
    void Awake()
    {
        SetSingleton();
    }
    #endregion
}
