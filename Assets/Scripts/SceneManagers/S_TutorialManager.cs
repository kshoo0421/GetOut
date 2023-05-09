using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_TutorialManager : MonoBehaviour, ISceneManager
{
    private B_SceneChangeManager sceneChanger = new B_SceneChangeManager();

    public void ChangeToScene10()
    {
        sceneChanger.ChangetoScene(10);
    }
}
