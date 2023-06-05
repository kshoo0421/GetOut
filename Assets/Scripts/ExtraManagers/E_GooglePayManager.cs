using UnityEngine;

public class E_GooglePayManager : MonoBehaviour
{
    #region Field
    /* Singleton*/
    static E_GooglePayManager _instance;
    #endregion

    #region Singleton
    public static E_GooglePayManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new E_GooglePayManager();
            }
            return _instance;
        }
    }
    #endregion
}
