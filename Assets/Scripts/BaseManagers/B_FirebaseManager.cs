using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class B_FirebaseManager : BehaviorSingleton<B_FirebaseManager>
{   
    #region Field
    static FirebaseUser User;

    /* Base Set */
    static FirebaseApp firebaseApp;
    static FirebaseAuth firebaseAuth;
    static FirebaseDatabase firebaseDatabase;
    static DatabaseReference reference;

    [SerializeField] static bool IsFirebaseReady { get; set; }
    [SerializeField] static bool IsSignInOnProgress { get; set; }
    /* database */
    long gameIndex = 0;

    #endregion
    
    #region Monobehavior
    void Awake()
    {
        FirebaseApp.Create();
        InitializeFM();
    }

    void OnDestroy()
    {
        SignOut();
    }
    #endregion
   
    #region Initialize  
    void InitializeFM()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(continuation: task =>
        {
            var result = task.Result;
            if (result != DependencyStatus.Available)
            {
                Debug.LogError(message: result.ToString());
                IsFirebaseReady = false;
            }
            else
            {
                IsFirebaseReady = true;
                firebaseApp = FirebaseApp.DefaultInstance;
                firebaseAuth = FirebaseAuth.DefaultInstance;
                firebaseDatabase = FirebaseDatabase.DefaultInstance;
                reference = firebaseDatabase.GetReference("/");
                reference.GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;
                    }
                });
            }
            Debug.Log("Firebase Set �Ϸ�");
        }
        );
    }
    #endregion

    #region SignUp
    public bool checkEmailOverlap() // �̸��� �ߺ����� üũ, ���� �ʿ�
    {
        return true;
    }

    public bool checkPasswordOverlap()  // ��й�ȣ ���� �������� üũ, ���� �ʿ�
    {
        return true;
    }

    public void SignUp(string emailText, string passwordText)    // ȸ������
    {
        firebaseAuth.CreateUserWithEmailAndPasswordAsync(emailText, passwordText).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("ȸ������ ���");
                return;
            }
            if (task.IsFaulted)
            {
                // ȸ������ ���� ���� => �̸����� ������ / ��й�ȣ�� �ʹ� ���� / �̹� ���Ե� �̸��� ���...
                Debug.LogError("ȸ������ ����");  
                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.LogError("ȸ������ �Ϸ�");
        });
    }
    #endregion

    #region SignIn
    public bool checkSignIn()
    {
        if (!IsFirebaseReady || IsSignInOnProgress || User != null)
        {
            return false;
        }
        return true;
    }

    public async Task SignIn(string emailText, string passwordText)
    {
        IsSignInOnProgress = true;

        await firebaseAuth.SignInWithEmailAndPasswordAsync(emailText, passwordText).ContinueWithOnMainThread(
            (task) =>
            {
                Debug.Log(message: $"Sign in status : {task.Status}");

                IsSignInOnProgress = false;

                if (task.IsFaulted)
                {
                    Debug.LogError(task.Exception);
                }
                else if (task.IsCanceled)
                {
                    Debug.LogError(message: "Sign-in canceled");
                }
                else
                {
                    User = task.Result.User;
                }
            });
    }
    #endregion

    #region SignOut
    public void SignOut()
    { 
        firebaseAuth.SignOut();
        User = null;
    }
    #endregion

    #region Get User Information
    public FirebaseUser GetCurUser()
    {
        if (firebaseAuth != null && firebaseAuth.CurrentUser != null)
        {   
            if (User != FirebaseAuth.DefaultInstance.CurrentUser)
                User = FirebaseAuth.DefaultInstance.CurrentUser;
            return User;
        }
        return null;
}
    #endregion

    #region Database - GameData
    public long GetGameIndex() { return gameIndex; }
    
    public void SetGameIndex()
    {
        reference.Child("GamePlayData").Child("gameIndex").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot != null && snapshot.Exists)
                {
                    gameIndex = (long)snapshot.Value;
                    UpdateGameIndex(gameIndex + 1);
                }
                else
                {
                    Debug.Log("No data found at the specified path.");
                }
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Failed to retrieve data: " + task.Exception);
            }
        });
    }

    void UpdateGameIndex(long newData)
    {
        // ������Ʈ�� ������ ����
        var updateData = new Dictionary<string, object>
        {
            { "gameIndex", newData }
        };

        // ������ ������Ʈ
        reference.Child("GamePlayData").UpdateChildrenAsync(updateData).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Long data updated successfully.");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Failed to update data: " + task.Exception);
            }
        });
    }

    public async void SaveResultData(ResultData resultData, long gIndex)
    {
        string json = JsonUtility.ToJson(resultData);
        reference.Child("GamePlayData").Child("GPD").Child("GPD : " + gIndex).Push();
        await reference.Child("GamePlayData").Child("GPD").Child("GPD : " + gIndex).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Data saved successfully!");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Failed to save data: " + task.Exception);
            }
        }
        );
    }
    #endregion

    #region Database - UserData

    #endregion

}