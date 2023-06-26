using UnityEngine;

public class S10_Guide : Scenes
{
    #region monobehaviour
    void Start()
    {
        InitialSet();
    }
    void Update()
    {
        ForUpdate();
    }

    void OnDestroy()
    {
        ForOnDestroy();
    }
    #endregion
}