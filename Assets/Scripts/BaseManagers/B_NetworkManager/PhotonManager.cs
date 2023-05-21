using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviour
{
    #region �̱��� ����
    private static PhotonManager instance;
    private PhotonManager() { }

    public static PhotonManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<PhotonManager>();
        }
    }
    #endregion

}
