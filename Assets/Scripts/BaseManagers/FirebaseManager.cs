using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class FirebaseManager : BehaviorSingleton<FirebaseManager>
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
    public static long gameIndex = 0;
    public static UserData userData;

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
            Debug.Log("Firebase Set 완료");
        }
        );
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
            task =>
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
                    PhotonManager.Instance.SetUserID(User.Email, "Test1");
                }
            });
        await SetUserData();
    }
    #endregion

    #region SignOut
    public void SignOut()
    { 
        firebaseAuth.SignOut();
        User = null;
        userData.id = "0";
        PhotonManager.Instance.SignOutID();   
    }
    #endregion

    #region Get User Information
    public FirebaseUser GetCurUser()
    {
        if (firebaseAuth.CurrentUser != User)
        {
            bool signedIn = User != firebaseAuth.CurrentUser && firebaseAuth.CurrentUser != null;
            if (!signedIn && User != null)
            {
                Debug.Log("Signed out " + User.UserId);
                return null;
            }
            User = firebaseAuth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + User.UserId);
            }
        }
        return User;
    }

    public string GetCurUserString()
    {
        string result = "null";
        if (User != null)
        {
            result = GetCurUser().ToString();
        }
        return result;
    }
    #endregion

    #region Database - GameData 
    public Task SetGameIndex()
    {
        return reference.Child("GamePlayData").Child("gameIndex").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot != null && snapshot.Exists)
                {
                    gameIndex = (long)snapshot.Value;
                    UpdateGameIndex(gameIndex + 1);
                    Debug.Log($"gameIndex : {gameIndex}");
                }
                else
                {
                    Debug.Log("No data found at the specified path.");
                }
            }
            else if (task.IsFaulted)
            {
                Debug.LogError($"Failed to retrieve data: {task.Exception}");
            }
        });
    }

    void UpdateGameIndex(long newData)
    {
        Debug.Log($"new Data : {newData}");
        // 업데이트할 데이터 생성
        Dictionary<string, object> updateData = new Dictionary<string, object>
        {
            { "gameIndex", newData }
        };

        // 데이터 업데이트
        reference.Child("GamePlayData").UpdateChildrenAsync(updateData).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Long data updated successfully.");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError($"Failed to retrieve data: {task.Exception}");
            }
        });
    }

    public async void SaveResultData(ResultData resultData)
    {
        await SetGameIndex();
        resultData.gameIndex = gameIndex;

        string json = JsonUtility.ToJson(resultData);
        reference.Child("GamePlayData").Child("GPD").Child("GPD : " + gameIndex).Push();
        await reference.Child("GamePlayData").Child("GPD").Child("GPD : " + gameIndex).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Data saved successfully!");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Failed to save data: " + task.Exception);
            }
        });
    }
    #endregion

    #region Database - UserData
    async Task<bool> isDataExist(string uid)
    {
        DataSnapshot snapshot = await reference.Child("UserData").Child(uid).GetValueAsync();
        return snapshot.Exists;
    }

    async Task SetUserData()
    {
        string uid = User.UserId;
        Debug.Log($"is Data Exist : {await isDataExist(uid)}");
        if (await isDataExist(uid))
        {
            await reference.Child("UserData").Child(uid).GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    string temp = task.Result.GetRawJsonValue();
                    userData = JsonUtility.FromJson<UserData>(temp);
                }
                SaveUserData();
            });
        }
        else
        {
            InitializeUserData();
            SaveUserData();
        }
        Debug.Log("SetUserData");
    }

    void InitializeUserData()
    {
        userData = new UserData();
        userData.id = User.UserId;
        userData.email = User.Email;
        userData.nickName = "nickName1";    // need change
        userData.itemData = new ItemData();
        userData.itemData.gold = 1000;
        userData.itemData.ticket = 5;
        userData.itemData.extra_ticket = 0;
        userData.itemData.ticket_time = "0";    // need Change
        Debug.Log("InitializeUserData");
    }

    public async void SaveUserData()
    {
        if (userData.id != "0")
        {
            // Debug.Log($"is Data Exist2 : {await isDataExist(User.UserId)}");
            if (!(await isDataExist(User.UserId)))
            {
                reference.Child("UserData").Child(User.UserId).Push();
                Debug.Log("Push 완료");
            }

            string json = JsonUtility.ToJson(userData);
            await reference.Child("UserData").Child(User.UserId).SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("UserData saved successfully!");
                }
                else if (task.IsFaulted)
                {
                    Debug.LogError("Failed to save data: " + task.Exception);
                }
            });
        }
        else return;
    }
    #endregion
}