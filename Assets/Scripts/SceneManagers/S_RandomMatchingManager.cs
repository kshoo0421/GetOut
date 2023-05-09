using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_RandomMatchingManager : MonoBehaviour, ISceneManager
{
    private B_SceneChangeManager sceneChanger = new B_SceneChangeManager();

    public void ChangeToScene02()
    {
        sceneChanger.ChangetoScene(2);
    }

    public void ChangeToScene05()
    {
        sceneChanger.ChangetoScene(5);
    }
}
