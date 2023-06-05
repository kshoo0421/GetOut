using UnityEngine;

public class E_EventManager : MonoBehaviour
{
    #region Field
    /* Singleton*/
    static E_EventManager _instance;
    #endregion

    #region Singleton
    public static E_EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new E_EventManager();
            }
            return _instance;
        }
    }
    #endregion
}