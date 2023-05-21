using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketManager : MonoBehaviour
{
    #region �̱��� ������
    private static TicketManager instance;
    private TicketManager() { }
    public static TicketManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<TicketManager>();
        }
    }
    #endregion
}
