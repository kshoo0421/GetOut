using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_StartGameManager : MonoBehaviour
{
    private B_SceneChangeManager sceneChanger = new B_SceneChangeManager();

    public void ChangeToScene03()
    {
        sceneChanger.ChangetoScene(3);
    }

    public void ChangeToScene04()
    {
        sceneChanger.ChangetoScene(4);
    }

    public void ChangeToScene06()
    {
        sceneChanger.ChangetoScene(6);
    }
}
