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
                }
            }
        );
    }
    #endregion

    #region 회원가입
  
    public bool checkEmailOverlap() // 이메일 중복여부 체크
    {
        return true;
    }

    public bool checkPasswordOverlap()  // 비밀번호 조건 충족여부 체크
    {
        return true;
    }

    public void SignUp(string emailText, string passwordText)    // 회원가입
    {
        firebaseAuth.CreateUserWithEmailAndPasswordAsync(emailText, passwordText).ContinueWith(task =>
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
}