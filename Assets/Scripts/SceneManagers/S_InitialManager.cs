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

    [SerializeField] private TMP_InputField[] inputFields; // ��� InputField�� �迭�� ������
    private int currentInputFieldIndex = 0; // ���� ��Ŀ���� ������ �ִ� InputField �ε���

    #region Start - Awake - Update
    private void Start()
    {
        inputFields[0].Select(); // ù ��° InputField�� ��Ŀ�� ����
    }

    private void Awake()
    {
        DontDestroyOnLoad(baseManagers);
        DontDestroyOnLoad(classManagers);
    }

    private void Update()
    {
        // ���� ��Ŀ���� ������ �ִ� InputField
        TMP_InputField currentInputField = inputFields[currentInputFieldIndex];
        
        // Tab Ű�� ������ �� ���� InputField�� ��Ŀ�� �̵�
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // ���� InputField�� ���� ���� InputField�� ��Ŀ�� �̵�
            currentInputFieldIndex = (currentInputFieldIndex + 1) % inputFields.Length;
            TMP_InputField nextInputField = inputFields[currentInputFieldIndex];

            // ���� InputField�� ��Ŀ�� ����
            nextInputField.Select();
            nextInputField.ActivateInputField();
        }
    }
    #endregion

    #region �� ����
    public void ChangeToScene1()
    {
        sceneChanger.ChangetoScene(1);
    }
    #endregion
}
