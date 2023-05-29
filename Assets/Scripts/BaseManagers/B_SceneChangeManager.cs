using UnityEngine;
using UnityEngine.SceneManagement;

public class B_SceneChangeManager : MonoBehaviour
{
    #region Field
    static B_SceneChangeManager instance;
    #endregion

    #region Singleton
    B_SceneChangeManager() { }
    public static B_SceneChangeManager Instance { get { return instance; } }

    void SetSingleton()
    {
        if (instance == null) instance = GetComponent<B_SceneChangeManager>();
    }
    #endregion

    #region Monobehaviour
    void Awake()
    {
        SetSingleton();
    }
    #endregion

    #region Change Scene
    public void ChangetoScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    #endregion
}