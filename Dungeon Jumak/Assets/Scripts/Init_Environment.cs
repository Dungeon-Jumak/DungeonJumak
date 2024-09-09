//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class Init_Environment : MonoBehaviour
{
    [Header("�ֹ� ȯ�� RectTransform")]
    [SerializeField] private RectTransform g_kitchenEnv;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //Debug
            Debug.Log("���ε�..");
            Init();
        }

    }

    private void Init()
    {
        g_kitchenEnv.localPosition = new Vector3(0 - Screen.width, g_kitchenEnv.localPosition.y, g_kitchenEnv.localPosition.z);
    }
}
