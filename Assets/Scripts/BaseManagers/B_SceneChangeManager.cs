using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class B_SceneChangeManager : MonoBehaviour, IBaseManager
{
    #region ½Ì±ÛÅæ ±¸Çö
    private static B_SceneChangeManager instance;
    private B_SceneChangeManager() { }

    public static B_SceneChangeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new B_SceneChangeManager();
            }
            return instance;
        }
    }
    #endregion

    public void ChangetoScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        Debug.Log("¾À " + sceneIndex + "À¸·Î ÀÌµ¿");
    }
}
