using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.AddressableAssets;

public class B_DatabaseManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ ±¸Çö
    private static B_DatabaseManager instance;
    private B_DatabaseManager() { }

    public static B_DatabaseManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<B_DatabaseManager>();
        }
    }
    #endregion
}