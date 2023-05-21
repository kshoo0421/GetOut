using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;
using Unity.VisualScripting;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseUser User;

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

    private static B_SceneChangeManager sceneChanger;
    private static DatabaseReference reference;

    //    void Start()
    void InitializeFM()
    {
        sceneChanger = B_SceneChangeManager.Instance;
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

    public void SignIn(string emailText, string passwordText)
    {
        IsSignInOnProgress = true;
        
        firebaseAuth.SignInWithEmailAndPasswordAsync(emailText, passwordText).ContinueWithOnMainThread(
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
                    Debug.Log(User.Email);
                    sceneChanger.ChangetoScene(1);
                }
            });
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