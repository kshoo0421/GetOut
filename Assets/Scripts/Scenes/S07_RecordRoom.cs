using UnityEngine;

public class S07_RecordRoom : Scenes
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