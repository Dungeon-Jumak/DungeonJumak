// Engine
using UnityEngine;

/// <summary>
/// ���� ��ü�� �����ϴ� �̱��� ���� �Ŵ��� Ŭ�����Դϴ�.
/// �ٸ� �Ŵ������� �̱���ó�� ����ϱ� ���� �ҷ����� �뵵�θ� �ִ��� Ȱ���ϰ� 
/// �߰����� ����� �ִ��� �ٸ� ��ũ��Ʈ�� Ȱ���ϵ��� �սô�.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager s_Instance;
    public static GameManager Instance
    {
        get
        {
            return s_Instance ?? null;
        }
    }

    #endregion

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
