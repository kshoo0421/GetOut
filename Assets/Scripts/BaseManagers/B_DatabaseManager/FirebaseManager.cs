using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;

public class FirebaseManager : MonoBehaviour
{
    private static FirebaseUser? User;

    #region �̱��� ������
    private static FirebaseManager instance;
    private FirebaseManager() {}
    public static FirebaseManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<FirebaseManager>();
            InitializeFM();
        }
    }
    #endregion

    #region monobehavior
    [SerializeField] private static FirebaseApp firebaseApp;
    [SerializeField] private static FirebaseAuth firebaseAuth;

    [SerializeField] private static bool IsFirebaseReady { get; set; }
    [SerializeField] private static bool IsSignInOnProgress { get; set; }

    private static DatabaseReference reference;

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
                    reference = FirebaseDatabase.DefaultInstance.RootReference;
                }
            }
        );
    }
    #endregion

    #region ȸ������
    public bool checkEmailOverlap() // �̸��� �ߺ����� üũ
    {
        return true;
    }

    public bool checkPasswordOverlap()  // ��й�ȣ ���� �������� üũ
    {
        return true;
    }

    public void SignUp(string emailText, string passwordText)    // ȸ������
    {
        firebaseAuth.CreateUserWithEmailAndPasswordAsync(emailText, passwordText).ContinueWith(task =>
        {
            if(task.IsCanceled)
            {
                Debug.LogError("ȸ������ ���");
                return;
            }
            if(task.IsFaulted)
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

    #region �α���
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

    #region User ���� ��������
    public FirebaseUser GetCurUser()
    {
        if(User != FirebaseAuth.DefaultInstance.CurrentUser)
            User = FirebaseAuth.DefaultInstance.CurrentUser;
        return User;
    }
    #endregion

    #region �����ͺ��̽�
    public async void SaveData(ResultData resultData)
    {
        string json = JsonUtility.ToJson(resultData);
        await reference.Child("Test").SetValueAsync(json);
    }
    #endregion
}