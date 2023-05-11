using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_InitialManager : MonoBehaviour
{
    private B_SceneChangeManager sceneChanger = new B_SceneChangeManager();

    public void ChangeToScene1()
    {
        sceneChanger.ChangetoScene(1);
    }
}
