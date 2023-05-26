using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ShopManager : MonoBehaviour
{
    #region monobehaviour
    TotalGameManager totalGameManager;
    GoogleAdMobManager googleAdMobManager;

    private void Awake()
    {
        totalGameManager = TotalGameManager.Instance;
        googleAdMobManager = totalGameManager.googleAdMobManager;
    }
    #endregion

    #region ¾À º¯°æ
    private B_SceneChangeManager sceneChanger = B_SceneChangeManager.Instance;

    public void ChangeToScene(int sceneIndex)
    {
        sceneChanger.ChangetoScene(sceneIndex);
    }
    #endregion

    #region ±¤°í ¼³Á¤
    public void ShowRewardedAd()
    {
        googleAdMobManager.LoadRewardedAd();
        googleAdMobManager.ShowRewardedAd();
        googleAdMobManager.DestroyFunc();
    }
    #endregion
}
