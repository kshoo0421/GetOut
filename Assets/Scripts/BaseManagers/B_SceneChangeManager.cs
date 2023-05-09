using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class B_SceneChangeManager : MonoBehaviour, IBaseManager
{
    public void ChangetoScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        Debug.Log("æ¿ " + sceneIndex + "¿∏∑Œ ¿Ãµø");
    }
}
