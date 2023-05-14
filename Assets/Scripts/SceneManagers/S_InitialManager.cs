using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class S_InitialManager : MonoBehaviour
{
    private B_SceneChangeManager sceneChanger = new B_SceneChangeManager();
    [SerializeField] private GameObject baseManagers;
    [SerializeField] private GameObject classManagers;

    [SerializeField] private TMP_InputField[] inputFields; // 모든 InputField를 배열로 가져옴
    private int currentInputFieldIndex = 0; // 현재 포커스를 가지고 있는 InputField 인덱스

    #region Start - Awake - Update
    private void Start()
    {
        inputFields[0].Select(); // 첫 번째 InputField에 포커스 설정
    }

    private void Awake()
    {
        DontDestroyOnLoad(baseManagers);
        DontDestroyOnLoad(classManagers);
    }

    private void Update()
    {
        // 현재 포커스를 가지고 있는 InputField
        TMP_InputField currentInputField = inputFields[currentInputFieldIndex];
        
        // Tab 키를 눌렀을 때 다음 InputField로 포커스 이동
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // 현재 InputField의 다음 순서 InputField로 포커스 이동
            currentInputFieldIndex = (currentInputFieldIndex + 1) % inputFields.Length;
            TMP_InputField nextInputField = inputFields[currentInputFieldIndex];

            // 다음 InputField에 포커스 설정
            nextInputField.Select();
            nextInputField.ActivateInputField();
        }
    }
    #endregion

    #region 씬 변경
    public void ChangeToScene1()
    {
        sceneChanger.ChangetoScene(1);
    }
    #endregion
}
