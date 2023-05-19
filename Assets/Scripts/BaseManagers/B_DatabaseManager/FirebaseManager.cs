using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseUser User;

    #region 싱글톤 생성용
    private static FirebaseManager instance;
    private FirebaseManager() {}
    public static FirebaseManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FirebaseManager();
            }
            return instance;
        }
    }
    #endregion

    #region monobehavior
    [SerializeField] private static FirebaseApp firebaseApp;
    [SerializeField] private static FirebaseAuth firebaseAuth;

    [SerializeField] private bool IsFirebaseReady { get; set; }
    [SerializeField] private bool IsSignInOnProgress { get; set; }

    private B_SceneChangeManager sceneChanger;

    void Start()
    {
        sceneChanger = new B_SceneChangeManager();
        signInButton.interactable = false;

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
                }
                signInButton.interactable = IsFirebaseReady;
            }
        );
    }
    #endregion

    #region 회원가입
    [SerializeField] private TMP_InputField emailSignUpField;
    [SerializeField] private TMP_InputField passwordSignUpField;

    public bool checkEmailOverlap() // 이메일 중복여부 체크
    {
        return true;
    }

    public bool checkPasswordOverlap()  // 비밀번호 조건 충족여부 체크
    {
        return true;
    }

    public void SignUp()    // 회원가입
    {
        firebaseAuth.CreateUserWithEmailAndPasswordAsync(emailSignUpField.text, passwordSignUpField.text).ContinueWith(task =>
        {
            if(task.IsCanceled)
            {
                Debug.LogError("회원가입 취소");
                return;
            }
            if(task.IsFaulted)
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

    #region 로그인
    [SerializeField] private TMP_InputField emailLogInField;
    [SerializeField] private TMP_InputField passwordLogInField;
    [SerializeField] private Button signInButton;

    public void SignIn()
    {
        if(!IsFirebaseReady || IsSignInOnProgress || User != null)
        {
            return;
        }

        IsSignInOnProgress = true;
        signInButton.interactable = false;

        firebaseAuth.SignInWithEmailAndPasswordAsync(emailLogInField.text, passwordLogInField.text).ContinueWithOnMainThread(
            (task) =>
            {
                Debug.Log(message: $"Sign in status : {task.Status}");

                IsSignInOnProgress = false;
                signInButton.interactable = true;

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
}