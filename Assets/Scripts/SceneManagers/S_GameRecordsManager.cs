using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_GameRecordsManager : MonoBehaviour, ISceneManager
{
    private B_SceneChangeManager sceneChanger = new B_SceneChangeManager();

    public void ChangeToScene07()
    {
        sceneChanger.ChangetoScene(7);
    }
}
