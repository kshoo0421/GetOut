using TMPro;
using UnityEngine;

public class MissionCheckPanel : MonoBehaviour
{
    #region Fields
    private PlayerMissionData pmd;  // player mission data
    private MissionSupporter ms; // mission supporter

    [SerializeField] private TMP_Text[] MissionGold;
    [SerializeField] private TMP_Text[] MissionInformation;
    [SerializeField] private GameObject AllMissionsPanel;
    #endregion

    #region Monobehaviours
    private void OnEnable()
    {
        pmd = DatabaseManager.gameData.playerMissionData[DatabaseManager.MyPlayer.playerNum];
        ms = MissionSupporter.Instance;
        SetMissionCheckData();
    }
    #endregion

    #region Mission Check Panel
    private void ToggleAllMissionsPanel(bool b) => AllMissionsPanel.SetActive(b);

    public void LookAllMissionsBtn() => ToggleAllMissionsPanel(true);

    public void CloseAllMissionsBtn() => ToggleAllMissionsPanel(false);

    private void SetMissionCheckData()
    {
        MissionGold[0].text = ms.GetMissionGold(MissionLevel.Low, pmd.low.missionNum) + "G"; 
        MissionInformation[0].text = ms.GetMissionInfo(MissionLevel.Low, pmd.low.missionNum);

        MissionGold[1].text = ms.GetMissionGold(MissionLevel.Mid, pmd.mid.missionNum) + "G";
        MissionInformation[1].text = ms.GetMissionInfo(MissionLevel.Mid, pmd.mid.missionNum);

        MissionGold[2].text = ms.GetMissionGold(MissionLevel.High, pmd.high.missionNum) + "G";
        MissionInformation[2].text = ms.GetMissionInfo(MissionLevel.High, pmd.high.missionNum);
    }
    #endregion
}
