using UnityEngine;
using UnityEngine.SceneManagement;

public class B_SceneChangeManager : BehaviorSingleton<B_SceneChangeManager>
{
    #region Change Scene
    public void ChangetoScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    #endregion
}