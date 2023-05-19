using UnityEngine;
using Firebase;
using Firebase.Database;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class B_DatabaseManager : MonoBehaviour
{
    void Start()
    {
        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void writeNewData()
    {
        ResultData resultData = new ResultData();
        string json = JsonUtility.ToJson(resultData);

    }
}