using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
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
            firebaseApp = FirebaseApp.Create();
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
                reference = firebaseDatabase.GetReference("https://getout-38c84-default-rtdb.firebaseio.com/");
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
    public async void SaveData(ResultData resultData)
    {
        string json = JsonUtility.ToJson(resultData);
        await reference.Child("WriteTest").SetRawJsonValueAsync(json);
    }
    #endregion
}