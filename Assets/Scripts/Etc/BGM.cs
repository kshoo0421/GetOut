using UnityEngine;

public class BGM : MonoBehaviour
{
    private static BGM _instance;
    public static AudioSource bgm;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            bgm = GetComponent<AudioSource>();
            bgm.volume = 1.0f;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy( gameObject );
        }
    }
}