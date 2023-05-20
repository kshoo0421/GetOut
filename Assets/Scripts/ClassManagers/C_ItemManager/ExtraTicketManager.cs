using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraTicketManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ »ý¼º¿ë
    private static ExtraTicketManager instance;
    private ExtraTicketManager() { }
    public static ExtraTicketManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ExtraTicketManager();
            }
            return instance;
        }
    }
    #endregion
}
