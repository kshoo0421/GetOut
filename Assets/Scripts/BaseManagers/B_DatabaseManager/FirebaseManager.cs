using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    #region Field
    static FirebaseUser User;
    /* Singleton */
    static FirebaseManager instance;

    /* Base Set */
    static FirebaseApp firebaseApp;
    static FirebaseAuth firebaseAuth;
    static FirebaseDatabase firebaseDatabase;
    static DatabaseReference reference;

    [SerializeField] static bool IsFirebaseReady { get; set; }
    [SerializeField] static bool IsSignInOnProgress { get; set; }
    /* database */
    #endregion

    #region Singleton
    FirebaseManager() { }
    public static FirebaseManager Instance { get { return instance; } }

    void SetSingleton()
    {
        if (instance == null)
        {
            instance = GetComponent<FirebaseManager>();
            FirebaseApp.Create();
            InitializeFM();
        }
    }

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
        }
        );
    }
    #endregion

    #region Monobehavior
    void Awake()
    {
        SetSingleton();
    }

    #endregion

    #region SignUp
    public bool checkEmailOverlap() // 이메일 중복여부 체크, 구현 필요
    {
        return true;
    }

    public bool checkPasswordOverlap()  // 비밀번호 조건 충족여부 체크, 구현 필요
    {
        return true;
    }

    public void SignUp(string emailText, string passwordText)    // 회원가입
    {
        firebaseAuth.CreateUserWithEmailAndPasswordAsync(emailText, passwordText).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("회원가입 취소");
                return;
            }
            if (task.IsFaulted)
            {
                // 회원가입 실패 이유 => 이메일이 비정상 / 비밀번호가 너무 간단 / 이미 가입된 이메일 등등...
                Debug.LogError("회원가입 실패");
                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.LogError("회원가입 완료");
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
    public void SignOut() => firebaseAuth.SignOut();
    #endregion

    #region Get User Information
    public FirebaseUser GetCurUser()
    {
        if (User != FirebaseAuth.DefaultInstance.CurrentUser)
            User = FirebaseAuth.DefaultInstance.CurrentUser;
        return User;
    }
    #endregion

    #region Database
    public long GetGameIndex()
    {
        long data = -1;
        reference.Child("WriteTest").Child("gameIndex").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot != null && snapshot.Exists)
                {
                    data = (long)snapshot.Value;
                    UpdateGameIndex(data + 1);
                    Debug.Log("람다 안 : " + data);
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
        Debug.Log("람다 밖 : " + data);
        return data;
    }

    void UpdateGameIndex(long newData)
    {
        // 업데이트할 데이터 생성
        var updateData = new Dictionary<string, object>
        {
            { "gameIndex", newData }
        };

        // 데이터 업데이트
        reference.Child("WriteTest").UpdateChildrenAsync(updateData).ContinueWith(task =>
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

    public async void SaveResultData(ResultData resultData, long gameKey)
    {
        string json = JsonUtility.ToJson(resultData);
        reference.Child("WriteTest").Child("GamePlayData" + gameKey).Push();
        await reference.Child("WriteTest").Child("GamePlayData" + gameKey).SetRawJsonValueAsync(json).ContinueWith(task =>
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
}