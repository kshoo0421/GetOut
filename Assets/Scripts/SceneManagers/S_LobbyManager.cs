using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_LobbyManager : MonoBehaviour, ISceneManager
{
    private B_SceneChangeManager sceneChanger = new B_SceneChangeManager();

    public void ChangeToScene00()
    {
        sceneChanger.ChangetoScene(0);
    }

    public void ChangeToScene02()
    {
        sceneChanger.ChangetoScene(2);
    }

    public void ChangeToScene07()
    {
        sceneChanger.ChangetoScene(7);
    }

    public void ChangeToScene09()
    {
        sceneChanger.ChangetoScene(9);
    }

    public void ChangeToScene10()
    {
        sceneChanger.ChangetoScene(10);
    }

    public void ChangeToScene12()
    {
        sceneChanger.ChangetoScene(12);
    }

    public void ChangeToScene13()
    {
        sceneChanger.ChangetoScene(13);
    }

}
