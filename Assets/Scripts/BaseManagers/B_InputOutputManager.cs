using UnityEngine;

public class B_InputOutputManager : MonoBehaviour
{
    #region Field
    static B_InputOutputManager instance;
    #endregion

    #region Singleton
    B_InputOutputManager() { }
    public static B_InputOutputManager Instance { get { return instance; } }

    void SetSingleton()
    {
        if (instance == null) instance = GetComponent<B_InputOutputManager>();
    }
    #endregion

    #region Monobehaviour
    void Awake()
    {
        SetSingleton();
    }
    #endregion

}
