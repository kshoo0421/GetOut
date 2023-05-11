using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_RecordRoomManager : MonoBehaviour
{
    private B_SceneChangeManager sceneChanger = new B_SceneChangeManager();

    public void ChangeToScene01()
    {
        sceneChanger.ChangetoScene(1);
    }

    public void ChangeToScene08()
    {
        sceneChanger.ChangetoScene(8);
    }
}
