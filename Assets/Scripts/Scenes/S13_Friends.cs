using UnityEngine;

public class S13_Friends : Scenes
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