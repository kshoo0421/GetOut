using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public static bool IsFirebaseReady { get; set; }
    static bool IsSignInOnProgress { get; set; }
    /* database */
    // public static ResultData resultData;
    public static GameData gameData;
    public static UserData userData;
    #endregion
    
    #region Monobehavior
    void Awake()
    {
        FirebaseApp.Create();
        InitializeFM();
    }

    void OnDestroy() => SignOut();
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
                reference = firebaseDatabase.GetReference("/"); // Default
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
                    PhotonManager.Instance.SetUserID(User.Email, "Test1");  // need to change
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
            bool signedIn = (User != firebaseAuth.CurrentUser) && (firebaseAuth.CurrentUser != null);
            if (!signedIn && User != null) return null;
            User = firebaseAuth.CurrentUser;
        }
        return User;
    }
    #endregion

    #region Database - GameData 
    public void InitializeGameData()
    {
        gameData = new GameData();
        TurnData[] turnData = new TurnData[6];
        gameData.players = new PlayersD[4];
        for(int i = 0; i<4; i++) gameData.players[i].turnData = turnData;

    }

    public Task SetGameIndex()
    {
        return reference.Child("GamePlayData").Child("gameIndex").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot != null && snapshot.Exists)
                {
                    // resultData.gameIndex = (long)snapshot.Value;
                    gameData.gameIndex = (long)snapshot.Value;
                    // UpdateGameIndex(resultData.gameIndex + 1);
                    UpdateGameIndex(gameData.gameIndex + 1);
                }
            }
            else if (task.IsFaulted) Debug.LogError($"Failed to set Gameindex: {task.Exception}");
        });
    }

    void UpdateGameIndex(long newData)
    {
        // ������Ʈ�� ������ ����
        var updateData = new Dictionary<string, object> { { "gameIndex", newData }};

        // ������ ������Ʈ
        reference.Child("GamePlayData").UpdateChildrenAsync(updateData).ContinueWith(task =>
        {
            if (task.IsFaulted) Debug.LogError($"Failed to retrieve data: {task.Exception}");
        });
    }

    public async void SaveGameData()
    {
        await SetGameIndex();
        // string json = JsonUtility.ToJson(resultData);
        string json = JsonUtility.ToJson(gameData);
        //reference.Child("GamePlayData").Child("GPD").Child(resultData.gameIndex.ToString()).Push();
        //await reference.Child("GamePlayData").Child("GPD").Child(resultData.gameIndex.ToString()).SetRawJsonValueAsync(json).ContinueWith(task =>
        //{
        //    if (task.IsFaulted) Debug.LogError("Failed to save resultdata: " + task.Exception);
        //});

        reference.Child("GamePlayData").Child("GPD").Child(gameData.gameIndex.ToString()).Push();
        await reference.Child("GamePlayData").Child("GPD").Child(gameData.gameIndex.ToString()).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted) Debug.LogError("Failed to save resultdata: " + task.Exception);
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
        if (await isDataExist(uid)) // get data from database
        {
            await reference.Child("UserData").Child(uid).GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    string temp = task.Result.GetRawJsonValue();
                    userData = JsonUtility.FromJson<UserData>(temp);
                    PrefsBundle.Instance.SetPrefsData(userData.prefsdata);
                }
                SaveUserData();
            });
        }
        else // initialize user data and save
        {
            InitializeUserData();   
            SaveUserData();
        }
    }

    void InitializeUserData()
    {
        userData = new UserData();
        userData.id = User.UserId;
        userData.email = User.Email;
        userData.nickName = "nickName1";    // need change
        userData.isFirst = 1;
        InitItemData();
    }

    public async void SaveUserData()
    {
        if (userData.id != "0")
        {
            PrefsBundle.Instance.GetPrefsData();
            userData.prefsdata = PrefsBundle.prefsData;

            /*if (!(await isDataExist(User.UserId))) */reference.Child("UserData").Child(User.UserId).Push();

            string json = JsonUtility.ToJson(userData);
            await reference.Child("UserData").Child(User.UserId).SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsFaulted) Debug.LogError("Failed to save userdata: " + task.Exception);
            });
        }
    }
    #endregion

    #region ItemData
    public void InitItemData()
    {
        userData.itemData = new ItemData();
        userData.itemData.extraTicket = 10;
        userData.itemData.ticket = 5;
        userData.itemData.ticketTime = DateTime.Now.ToString();
        userData.itemData.gold = 5000;
    }

    public async void SaveItemData()
    {
        if (userData.id != "0")
        {
            string json = JsonUtility.ToJson(userData.itemData);
            await reference.Child("UserData").Child(User.UserId).Child("itemData").SetRawJsonValueAsync(json);
        }
        else return;
    }
    #endregion

    #region ticket
    public static string restMinute = "00", restSecond = "00";

    public bool isFullTicket() => (userData.itemData.ticket == 5) ? true : false;
    public bool CanUseTicket() => (userData.itemData.ticket > 0) ? true : false;   
    public void UseTicket()
    {
        userData.itemData.ticket -= 1;
        if(userData.itemData.ticket == 4)
        {
            DateTime currentTime = DateTime.Now;
            userData.itemData.ticketTime = currentTime.ToString();
        }
        SaveItemData();
    }

    public void AutoFillTicket()   // cooltime : 1 hour
    {
        DateTime currentTime = DateTime.Now;
        DateTime tmpDT = Convert.ToDateTime(userData.itemData.ticketTime);
        TimeSpan diff = currentTime - tmpDT;
        if(diff.Hours > 0)  // 1�ð� �Ѱ� ���̳��ٸ�
        {
            int tmp = diff.Hours;
            tmpDT.AddHours(tmp);
            userData.itemData.ticket += tmp;
            if(userData.itemData.ticket >= 5)
            {
                userData.itemData.ticket = 5;
                userData.itemData.ticketTime = DateTime.Now.ToString();
            }
            else
            {
                userData.itemData.ticketTime = tmpDT.ToString();
                Debug.Log(tmpDT.ToString());
            }
            SaveItemData(); 
        }
        CheckRestTime(tmpDT.AddHours(1) - currentTime);
    }

    void CheckRestTime(TimeSpan diff)
    {
        if (isFullTicket())
        {
            Debug.Log("isFullTicket");
            restMinute = "00";
            restSecond = "00";
        }
        else
        {
            restMinute = diff.Minutes.ToString("00");
            restSecond = diff.Seconds.ToString("00");
        }
    }
    #endregion
}