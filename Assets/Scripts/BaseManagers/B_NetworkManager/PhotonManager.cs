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
        get
        {
            if (instance == null)
            {
                instance = new PhotonManager();
            }
            return instance;
        }
    }
    #endregion

}
