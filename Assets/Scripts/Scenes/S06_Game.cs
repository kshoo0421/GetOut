using UnityEngine;

public class S06_Game : Scenes
{
    #region monobehaviour
    void Start()
    {
        InitialSet();
    }
    #endregion

    #region Back to Waiting Room
    public void BackToCustom()
    {
        ChangeToScene(3);
    }

    public void BackToRandom()
    {
        ChangeToScene(4);
    }
    #endregion
}