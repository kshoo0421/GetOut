using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraTicketManager : MonoBehaviour
{
    #region �̱��� ������
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
