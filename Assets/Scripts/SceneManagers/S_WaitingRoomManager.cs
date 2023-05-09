using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_WaitingRoomManager : MonoBehaviour, ISceneManager
{
    private B_SceneChangeManager sceneChanger = new B_SceneChangeManager();

    public void ChangeToScene01()
    {
        sceneChanger.ChangetoScene(1);
    }

    public void ChangeToScene03()
    {
        sceneChanger.ChangetoScene(3);
    }

    public void ChangeToScene04()
    {
        sceneChanger.ChangetoScene(4);
    }

}
