using UnityEngine;

public class PhotonManager : MonoBehaviour
{
    #region Field
    static PhotonManager instance;

    #endregion

    #region Singleton
    PhotonManager() { }

    public static PhotonManager Instance { get { return instance; } }

    void SetSingleton()
    {
        if (instance == null) instance = GetComponent<PhotonManager>();
    }
    #endregion

    #region Monobehaviour
    void Awake()
    {
        SetSingleton();    
    }
    #endregion

}
