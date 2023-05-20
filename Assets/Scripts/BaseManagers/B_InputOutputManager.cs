using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_InputOutputManager : MonoBehaviour
{
    #region �̱��� ����
    private static B_InputOutputManager instance;
    private B_InputOutputManager() { }

    public static B_InputOutputManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new B_InputOutputManager();
            }
            return instance;
        }
    }
    #endregion

}
