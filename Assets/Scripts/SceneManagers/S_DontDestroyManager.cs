using UnityEngine;

public class S_DontDestroyManager : MonoBehaviour
{
    #region Field
    /* Managers */
    TotalGameManager totalGameManager;
    B_SceneChangeManager sceneChanger;
    #endregion

    #region monobehaviour
    //void Awake()
    //{
    //    SetManagers();
    //}
    #endregion

    #region Set Managers
    void SetManagers()
    {
        totalGameManager = TotalGameManager.Instance;
        sceneChanger = totalGameManager.b_SceneChangeManager;
    }
    #endregion

    #region Change Scene
    public void ChangeToScene(int sceneIndex)
    {
        SetManagers();
        sceneChanger.ChangetoScene(sceneIndex);
    }
    #endregion
}