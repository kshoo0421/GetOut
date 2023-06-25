using UnityEngine;
using TMPro;

public class S06_Game : Scenes
{
    #region Field
    int curPlayer;

    [SerializeField] TMP_Text TurnNumText;  // 몇 번째 턴인지
    [SerializeField] TMP_Text TurnStateText;    // Get vs Set
    #endregion

    #region monobehaviour
    void Start()
    {
        InitialSet();
    }
    void Update()
    {
        ForUpdate();
    }
    #endregion

    #region State Set
    void SetStateText(bool b) => TurnStateText.text = b ? "Get" : "Set";


    #endregion

    #region SceneChange
    public void BackToCustom() => ChangeToScene(3);

    public void BackToRandom() => ChangeToScene(4);
    #endregion
}