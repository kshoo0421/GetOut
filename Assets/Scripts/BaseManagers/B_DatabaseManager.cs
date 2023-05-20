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
        get
        {
            if (instance == null)
            {
                instance = new B_DatabaseManager();
            }
            return instance;
        }
    }
    #endregion

    DatabaseReference reference;
    void Start()
    {
        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async void SaveData(ResultData resultData)
    {
        string json = JsonUtility.ToJson(resultData);
        await reference.Child("Test").SetValueAsync(json);
    }
}