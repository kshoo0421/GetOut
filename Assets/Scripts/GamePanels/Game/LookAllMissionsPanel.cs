using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LookAllMissionsPanel : MonoBehaviour
{
    #region Fields
    [SerializeField] TMP_Text allMissionText;
    MissionSupporter missionSupporter;
    #endregion

    #region Monobehaviour
    private void Awake()
    {
        missionSupporter = MissionSupporter.Instance;
        SetText();
    }
    #endregion

    #region Function
    private void SetText()
    {
        allMissionText.text = $"Low Mission\n" +
            $"1. {missionSupporter.GetMissionInfo(MissionLevel.Low, 0)}({missionSupporter.GetMissionGold(MissionLevel.Low, 0)}G)\n" +
            $"2. {missionSupporter.GetMissionInfo(MissionLevel.Low, 1)}({missionSupporter.GetMissionGold(MissionLevel.Low, 1)}G)\n" +
            $"3. {missionSupporter.GetMissionInfo(MissionLevel.Low, 2)}({missionSupporter.GetMissionGold(MissionLevel.Low, 2)}G)\n" +
            $"4. {missionSupporter.GetMissionInfo(MissionLevel.Low, 3)}({missionSupporter.GetMissionGold(MissionLevel.Low, 3)}G)\n" +
            $"5. {missionSupporter.GetMissionInfo(MissionLevel.Low, 4)}({missionSupporter.GetMissionGold(MissionLevel.Low, 4)}G)\n" +
            $"6. {missionSupporter.GetMissionInfo(MissionLevel.Low, 5)}({missionSupporter.GetMissionGold(MissionLevel.Low, 5)}G)\n" +
            $"7. {missionSupporter.GetMissionInfo(MissionLevel.Low, 6)}({missionSupporter.GetMissionGold(MissionLevel.Low, 6)}G)\n" +
            $"8. {missionSupporter.GetMissionInfo(MissionLevel.Low, 7)}({missionSupporter.GetMissionGold(MissionLevel.Low, 7)}G)\n" +
            $"9. {missionSupporter.GetMissionInfo(MissionLevel.Low, 8)}({missionSupporter.GetMissionGold(MissionLevel.Low, 8)}G)\n" +
            $"10. {missionSupporter.GetMissionInfo(MissionLevel.Low, 9)}({missionSupporter.GetMissionGold(MissionLevel.Low, 9)}G)\n\n" +
            $"Mid Mission\n" +
            $"1. {missionSupporter.GetMissionInfo(MissionLevel.Mid, 0)}({missionSupporter.GetMissionGold(MissionLevel.Mid, 0)}G)\n" +
            $"2. {missionSupporter.GetMissionInfo(MissionLevel.Mid, 1)}({missionSupporter.GetMissionGold(MissionLevel.Mid, 1)}G)\n" +
            $"3. {missionSupporter.GetMissionInfo(MissionLevel.Mid, 2)}({missionSupporter.GetMissionGold(MissionLevel.Mid, 2)}G)\n" +
            $"4. {missionSupporter.GetMissionInfo(MissionLevel.Mid, 3)}({missionSupporter.GetMissionGold(MissionLevel.Mid, 3)}G)\n" +
            $"5. {missionSupporter.GetMissionInfo(MissionLevel.Mid, 4)}({missionSupporter.GetMissionGold(MissionLevel.Mid, 4)}G)\n" +
            $"6. {missionSupporter.GetMissionInfo(MissionLevel.Mid, 5)}({missionSupporter.GetMissionGold(MissionLevel.Mid, 5)}G)\n" +
            $"7. {missionSupporter.GetMissionInfo(MissionLevel.Mid, 6)}({missionSupporter.GetMissionGold(MissionLevel.Mid, 6)}G)\n" +
            $"8. {missionSupporter.GetMissionInfo(MissionLevel.Mid, 7)}({missionSupporter.GetMissionGold(MissionLevel.Mid, 7)}G)\n" +
            $"9. {missionSupporter.GetMissionInfo(MissionLevel.Mid, 8)}({missionSupporter.GetMissionGold(MissionLevel.Mid, 8)}G)\n" +
            $"10. {missionSupporter.GetMissionInfo(MissionLevel.Mid, 9)}({missionSupporter.GetMissionGold(MissionLevel.Mid, 9)}G)\n\n" +
            $"High Mission\n" +
            $"1. {missionSupporter.GetMissionInfo(MissionLevel.High, 0)}({missionSupporter.GetMissionGold(MissionLevel.High, 0)}G)\n" +
            $"2. {missionSupporter.GetMissionInfo(MissionLevel.High, 1)}({missionSupporter.GetMissionGold(MissionLevel.High, 1)}G)\n" +
            $"3. {missionSupporter.GetMissionInfo(MissionLevel.High, 2)}({missionSupporter.GetMissionGold(MissionLevel.High, 2)}G)\n" +
            $"4. {missionSupporter.GetMissionInfo(MissionLevel.High, 3)}({missionSupporter.GetMissionGold(MissionLevel.High, 3)}G)\n" +
            $"5. {missionSupporter.GetMissionInfo(MissionLevel.High, 4)}({missionSupporter.GetMissionGold(MissionLevel.High, 4)}G)\n" +
            $"6. {missionSupporter.GetMissionInfo(MissionLevel.High, 5)}({missionSupporter.GetMissionGold(MissionLevel.High, 5)}G)\n" +
            $"7. {missionSupporter.GetMissionInfo(MissionLevel.High, 6)}({missionSupporter.GetMissionGold(MissionLevel.High, 6)}G)\n" +
            $"8. {missionSupporter.GetMissionInfo(MissionLevel.High, 7)}({missionSupporter.GetMissionGold(MissionLevel.High, 7)}G)\n" +
            $"9. {missionSupporter.GetMissionInfo(MissionLevel.High, 8)}({missionSupporter.GetMissionGold(MissionLevel.High, 8)}G)\n" +
            $"10. {missionSupporter.GetMissionInfo(MissionLevel.High, 9)}({missionSupporter.GetMissionGold(MissionLevel.High, 9)}G)\n\n";
    }
    #endregion
}
