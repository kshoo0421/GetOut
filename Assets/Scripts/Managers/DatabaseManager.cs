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
    private static FirebaseUser User;

    /* Authentication */
    public static string SignInMessage;
    public static string SignUpMessage;

    /* Base Set */
    private static FirebaseApp firebaseApp;
    private static FirebaseAuth auth; 
    private static FirebaseDatabase firebaseDatabase;
    private static DatabaseReference reference;

    public static bool IsFirebaseReady { get; set; }
    private static bool IsSignInOnProgress { get; set; }

    /* Photon */
    public static bool enteredRoom = false;

    /* database */
    public static GameData gameData;
    public static UserData userData;
    public static DateTime gameTime;
    public static string leftTime = "00";
    public static long curGold = 0;

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
        SignUpMessage = "";
        SignInMessage = "";
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

    #region Email Sign Up
    public bool checkEmailOverlap() // �̸��� �ߺ����� üũ, ���� �ʿ�
    {
        //FirebaseUser userRecord;
        
        //UserRecord userRecord;
        //await firebaseAuth.GetUserByEmailAsync(emailText).;
        
        return true;
    }

    public bool checkPasswordOverlap()  // ��й�ȣ ���� �������� üũ, ���� �ʿ�
    {
        return true;
    }

    public void SignUp(string emailText, string passwordText)    // ȸ������
    {
        auth.CreateUserWithEmailAndPasswordAsync(emailText, passwordText).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                SignUpMessage = task.Exception.Message;
                return;
            }
            if (task.IsFaulted)
            {
                SignUpMessage = task.Exception.Message;
                return;
            }
            FirebaseUser newUser = task.Result.User;
            SignUpMessage = "Sign Up Success";
            int index = emailText.IndexOf("@");
            UpdateUserProfile(emailText.Substring(0, index));
        });
    }

    private void UpdateUserProfile(string UserName)
    {
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            UserProfile profile = new UserProfile
            {
                DisplayName = UserName
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error : " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");
            });
        }
    }
    #endregion

    #region Email Sign In
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

        await auth.SignInWithEmailAndPasswordAsync(emailText, passwordText).ContinueWithOnMainThread(
            task =>
            {
                Debug.Log(message: $"Sign in status : {task.Status}");

                IsSignInOnProgress = false;

                if (task.IsCanceled)
                {
                    SignInMessage = task.Exception.Message;
                }
                else if (task.IsFaulted)
                {
                    Exception exception = task.Exception;
                    FirebaseException firebaseEx = exception as FirebaseException;
                    if(firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        SignInMessage = "Error : " + GetErrorMessage(errorCode);
                    }
                }
                else
                {
                    SignInMessage = "Log In Success";
                    User = task.Result.User;
                    SetPhotonNickName();
                }
            });
        await SetUserData();
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
    private void SetPhotonNickName()
    {
        int index = User.Email.IndexOf("@");
        PhotonManager.NickNameString = User.Email.Substring(0, index);
        PhotonManager.Instance.SetUserID(User.UserId);
        Debug.Log($"NickName : {PhotonManager.NickNameString}");
    }
    #endregion

    #region SignOut
    public void SignOut()
    {
        auth.SignOut();
        User = null;
        userData.id = "0";
        PhotonManager.Instance.SignOutID();
        Debug.Log("�α׾ƿ� �Ϸ�");
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

    #region Get Error Message
    private static string GetErrorMessage(AuthError errorCode)
    {
        string message = "";
        switch(errorCode)
        {
            case AuthError.AccountExistsWithDifferentCredentials:
                message = "Account Not Exist";
                break;
            case AuthError.MissingPassword:
                message = "Missing Password";
                break;
            case AuthError.WrongPassword:
                message = "Wrong Password";
                break;
            case AuthError.EmailAlreadyInUse:
                message = "Your Email Already in Use";
                break;
            case AuthError.InvalidEmail:
                message = "Your Email Invalid";
                break;
            case AuthError.MissingEmail:
                message = "Your Email Missing";
                break;
            default:
                message = "Invalid Error";
                break;
        }
        return message;
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
        // ������Ʈ�� ������ ����
        var updateData = new Dictionary<string, object> { { "gameIndex", newData } };

        // ������ ������Ʈ
        reference.Child("GamePlayData").UpdateChildrenAsync(updateData).ContinueWith(task =>
        {
            if (task.IsFaulted) Debug.LogError($"Failed to retrieve data: {task.Exception}");
        });
    }

    public async void FirstSaveGameData()
    {
        await SetGameIndex();
        SaveGameData();
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

    private async Task SetUserData()
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
        if (diff.Hours > 0)  // 1�ð� �Ѱ� ���̳��ٸ�
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