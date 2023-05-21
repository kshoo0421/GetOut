using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class B_SceneChangeManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ ±¸Çö
    private static B_SceneChangeManager instance;
    private B_SceneChangeManager() { }

    public static B_SceneChangeManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<B_SceneChangeManager>();
        }
    }
    #endregion

    public void ChangetoScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
