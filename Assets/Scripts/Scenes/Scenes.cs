using Photon.Pun;
using UnityEngine;

public class Scenes : MonoBehaviour
{
    #region Managers
    protected FirebaseManager firebaseManager;
    protected GoogleAdMobManager googleAdMobManager;
    protected ItemManager itemManager;
    protected OptionManager optionManager;
    protected PhotonManager photonManager;
    #endregion

    #region Change Scene
    public void ChangeToScene(int sceneIndex) => UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    #endregion

    #region Set Managers
    protected void SetManagers()
    {
        firebaseManager = FirebaseManager.Instance;
        googleAdMobManager = GoogleAdMobManager.Instance;
        itemManager = ItemManager.Instance;
        optionManager = OptionManager.Instance;
        photonManager = PhotonManager.Instance;
    }
    #endregion
}
