using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_FriendsManager : MonoBehaviour
{
    private B_SceneChangeManager sceneChanger = new B_SceneChangeManager();
    public void ChangeToScene(int sceneIndex)
    {
        sceneChanger.ChangetoScene(sceneIndex);
    }
}
