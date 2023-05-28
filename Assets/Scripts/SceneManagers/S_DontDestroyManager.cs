using UnityEngine;

public class S_DontDestroyManager : MonoBehaviour
{
    private TotalGameManager totalGameManager;
    private B_SceneChangeManager sceneChanger;

    private void Start()
    {
        totalGameManager = TotalGameManager.Instance;
    }

    public void ChangeToScene(int index)
    {
        sceneChanger = totalGameManager.b_SceneChangeManager;
        sceneChanger.ChangetoScene(index);
    }
}