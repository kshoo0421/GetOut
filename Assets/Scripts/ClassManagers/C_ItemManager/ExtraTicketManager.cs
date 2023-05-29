using UnityEngine;

public class ExtraTicketManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ »ý¼º¿ë
    private static ExtraTicketManager instance;
    private ExtraTicketManager() { }
    public static ExtraTicketManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<ExtraTicketManager>();
        }
    }
    #endregion
}
