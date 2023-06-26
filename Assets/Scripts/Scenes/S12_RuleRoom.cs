using UnityEngine;

public class S12_RuleRoom : Scenes
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
