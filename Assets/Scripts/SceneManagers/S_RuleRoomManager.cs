using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_RuleRoomManager : MonoBehaviour, ISceneManager
{
    private B_SceneChangeManager sceneChanger = new B_SceneChangeManager();

    public void ChangeToScene01()
    {
        sceneChanger.ChangetoScene(1);
    }
}
