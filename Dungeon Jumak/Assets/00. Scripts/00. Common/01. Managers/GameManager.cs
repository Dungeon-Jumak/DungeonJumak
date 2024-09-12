// Engine
using UnityEngine;

/// <summary>
/// 게임 전체를 관리하는 싱글톤 게임 매니저 클래스입니다.
/// 다른 매니저들을 싱글톤처럼 사용하기 위해 불러오는 용도로만 최대한 활용하고 
/// 추가적인 기능은 최대한 다른 스크립트를 활용하도록 합시다.
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
