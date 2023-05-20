using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_CustomMatchingManager : MonoBehaviour
{
    private B_SceneChangeManager sceneChanger = B_SceneChangeManager.Instance;

    public void ChangeToScene(int sceneIndex)
    {
        sceneChanger.ChangetoScene(sceneIndex);
    }
}
