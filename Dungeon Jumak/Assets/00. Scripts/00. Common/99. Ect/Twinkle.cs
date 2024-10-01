// System
using System.Collections;

// Engine
using UnityEngine;

// Ect
using TMPro;

/// <summary>
/// UI의 Text를 한 글자씩 차례대로 출력해줄 수 있도록 하는 스크립트입니다.
/// 한 글자씩 출력하고자 하는 텍스트에 해당 스크립트를 컴포넌트로 추가하면 됩니다.
/// </summary>
public class Twinkle : MonoBehaviour
{
    [SerializeField] private float delay = 0.125f;

    private TMP_Text targetText;
    private string text;

    private void Start()
    {
        targetText = GetComponent<TMP_Text>();
        text = targetText.text.ToString();

        StartCoroutine(RepeatedTextPrint(delay));
    }

    IEnumerator RepeatedTextPrint(float d)
    {
        while (true) 
        {
            targetText.text = ""; 
            yield return StartCoroutine(TextPrint(d));
        }
    }

    IEnumerator TextPrint(float d)
    {
        int count = 0;

        // 한 글자씩 출력
        while (count < text.Length)
        {
            targetText.text += text[count].ToString();
            count++;

            yield return new WaitForSeconds(d);
        }

        // 출력이 끝나면 잠시 멈춤
        yield return new WaitForSeconds(0.5f); // 1초 대기 (원하는 대기 시간 설정 가능)
    }
}
