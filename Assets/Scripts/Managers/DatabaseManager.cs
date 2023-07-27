using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Google;

public class DatabaseManager : BehaviorSingleton<DatabaseManager>
{
    #region Field
    public static FirebaseUser User;

    /* Authentication */

    /* Base Set */
    private static FirebaseApp firebaseApp;
    public static FirebaseAuth auth; 
    private static FirebaseDatabase firebaseDatabase;
    private static DatabaseReference reference;

    public static bool IsFirebaseReady { get; set; }

    /* Photon */
    public static bool enteredRoom = false;

    /* database */
    public static GameData gameData;
    public static UserData userData;
    public static DateTime gameTime;
    public static string leftTime = "00";
    public static long curGold = 0;
    public static bool bannerAd = false;

    /* Data For Game */
    public static GamePlayer MyPlayer;
    public static GamePhase gamePhase;
    public static RandomOrCustom randomOrCustom;
    public static bool isGet;
    public static int goldAmount;
    public static int curTurn;
    public static int suggestCount, getCount;
    #endregion

    #region Monobehavior
    private void Awake()
    {
        FirebaseApp.Create();
        InitializeDM();
    }


    private void OnDestroy() => SignOut();
    #endregion

    #region Initialize  
    private void InitializeDM()
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
                auth = FirebaseAuth.DefaultInstance;
                firebaseDatabase = FirebaseDatabase.DefaultInstance;
                reference = firebaseDatabase.GetReference("/"); // Default
            }
        }
        );
    }

    public void InitGameData()
    {
        gameData = new GameData(0);
    }
    #endregion

    #region Google Login
    public async Task GoogleSignInMethod()
    {
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            WebClientId = "767643581180-cpaa2vtm8cjvi32pss5i315nkmk9tah6.apps.googleusercontent.com"
        };

        Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();

        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();
        await signIn.ContinueWith(async task =>
        {
            if (task.IsCanceled)
            {
                signInCompleted.SetCanceled();
            }
            else if (task.IsFaulted)
            {
                signInCompleted.SetException(task.Exception);
            }
            else
            {

                Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
                await auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
                {
                    if (authTask.IsCanceled)
                    {
                        signInCompleted.SetCanceled();
                    }
                    else if (authTask.IsFaulted)
                    {
                        signInCompleted.SetException(authTask.Exception);
                    }
                    else
                    {
                        signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);
                    }
                });
                // await SetUserData();
            }
        });
    }
    #endregion

    #region Siggin Common Functions
    public void SetPhotonNickName()
    {
        int index = User.Email.IndexOf("@");
        PhotonManager.NickNameString = User.Email.Substring(0, index);
        PhotonManager.Instance.SetUserID(User.UserId);
    }
    #endregion

    #region SignOut
    public void SignOut()
    {
        auth.SignOut();
        User = null;
        userData.id = "0";
        PhotonManager.Instance.SignOutID();
        Debug.Log("로그아웃 완료");
    }
    #endregion

    #region Get User Information
    public FirebaseUser GetCurUser()
    {
        if (auth.CurrentUser != User)
        {
            bool signedIn = (User != auth.CurrentUser) && (auth.CurrentUser != null);
            if (!signedIn && User != null) return null;
            User = auth.CurrentUser;
        }
        return User;
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
                    gameData.gameIndex = (long)snapshot.Value;
                    UpdateGameIndex(gameData.gameIndex + 1);
                }
            }
            else if (task.IsFaulted) Debug.LogError($"Failed to set Gameindex: {task.Exception}");
        });
    }

    private void UpdateGameIndex(long newData)
    {
        // 업데이트할 데이터 생성
        var updateData = new Dictionary<string, object> { { "gameIndex", newData } };

        // 데이터 업데이트
        reference.Child("GamePlayData").UpdateChildrenAsync(updateData).ContinueWith(task =>
        {
            if (task.IsFaulted) Debug.LogError($"Failed to retrieve data: {task.Exception}");
        });
    }

    public async void FirstSaveGameData()
    {
        await SetGameIndex();
        string json = JsonUtility.ToJson(gameData);

        reference.Child("GamePlayData").Child("GPD").Child(gameData.gameIndex.ToString()).Push();
        await reference.Child("GamePlayData").Child("GPD").Child(gameData.gameIndex.ToString()).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted) Debug.LogError("Failed to save resultdata: " + task.Exception);
        });
    }

    public async void SaveGameData()
    {
        string json = JsonUtility.ToJson(gameData);

        reference.Child("GamePlayData").Child("GPD").Child(gameData.gameIndex.ToString()).Push();
        await reference.Child("GamePlayData").Child("GPD").Child(gameData.gameIndex.ToString()).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted) Debug.LogError("Failed to save resultdata: " + task.Exception);
        });
    }

    public async void SavePlayerMissionData(int playerNum, PlayerMissionData pmd)
    {
        string json = JsonUtility.ToJson(pmd);

        await reference.Child("GamePlayData").Child("GPD").Child(gameData.gameIndex.ToString())
            .Child("playerMissionData").Child(playerNum.ToString()).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted) Debug.LogError("Failed to save resultdata: " + task.Exception);
        });
    }

    public async void SaveTurnData(int turnNum)
    {
        string json = JsonUtility.ToJson(gameData.turnData[turnNum]);

        await reference.Child("GamePlayData").Child("GPD").Child(gameData.gameIndex.ToString())
            .Child("turnData").Child(turnNum.ToString()).SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsFaulted) Debug.LogError("Failed to save resultdata: " + task.Exception);
            });
    }

    public async void UpdateGameData()
    {
        await reference.Child("GamePlayData").Child("GPD").Child(gameData.gameIndex.ToString()).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                string temp = task.Result.GetRawJsonValue();
                gameData = JsonUtility.FromJson<GameData>(temp);
                PrefsBundle.Instance.SetPrefsData(userData.prefsdata);
            }
        });
    }
    #endregion

    #region Database - UserData
    private async Task<bool> isDataExist(string uid)
    {
        DataSnapshot snapshot = await reference.Child("UserData").Child(uid).GetValueAsync();
        return snapshot.Exists;
    }

    public async Task SetUserData()
    {
        string uid = User.UserId;
        if (await isDataExist(uid)) // get data from database
        {
            await reference.Child("UserData").Child(uid).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    string temp = task.Result.GetRawJsonValue();
                    userData = JsonUtility.FromJson<UserData>(temp);
                    PrefsBundle.Instance.SetPrefsData(userData.prefsdata);
                }
                // SaveUserData();
            });
        }
        else // initialize user data and save
        {
            InitializeUserData();
            SaveUserData();
        }
    }

    private void InitializeUserData()
    {
        userData = new UserData();
        userData.id = User.UserId;
        userData.email = User.Email;
        int index = User.Email.IndexOf("@");
        userData.nickName = User.Email.Substring(0, index);
        userData.isFirst = 1;
        InitItemData();
    }

    private async void SaveUserData()
    {
        if (userData.id != "0")
        {
            PrefsBundle.Instance.GetPrefsData();
            userData.prefsdata = PrefsBundle.prefsData;

            /*if (!(await isDataExist(User.UserId))) */
            reference.Child("UserData").Child(User.UserId).Push();

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
        string json = JsonUtility.ToJson(userData.itemData);
        await reference.Child("UserData").Child(User.UserId).Child("itemData").SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted) Debug.LogError("Failed to save itemdata: " + task.Exception);
        }); ;
    }
    #endregion

    #region ticket
    public static string restMinute = "00", restSecond = "00";

    public bool isFullTicket() => (userData.itemData.ticket == 5);
    public bool CanUseTicket() => (userData.itemData.ticket > 0);
    public void UseTicket()
    {
        userData.itemData.ticket -= 1;
        if (userData.itemData.ticket == 4)
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
        if (diff.Hours > 0)  // 1시간 넘게 차이난다면
        {
            int tmp = diff.Hours;
            tmpDT.AddHours(tmp);
            userData.itemData.ticket += tmp;
            if (userData.itemData.ticket >= 5)
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

    private void CheckRestTime(TimeSpan diff)
    {
        if (isFullTicket())
        {
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

    public void UpdateCurGold()
    {
        curGold = 0;
        for (int i = 0; i <= curTurn; i++)
        {
            if (gameData.turnData[i].success[MyPlayer.playerNum])
            {
                curGold += gameData.turnData[i].gold[MyPlayer.playerNum];
            } 
        }
    }  
}