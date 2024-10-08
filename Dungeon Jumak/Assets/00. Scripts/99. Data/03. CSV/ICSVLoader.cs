using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ICSVLoader
{
    /// <summary>
    /// CSV 파일에서 데이터를 로드합니다.
    /// </summary>
    /// <param name="_filePath">CSV 파일 경로</param>
    void LoadFromCSV(string _filePath);
    void LoadFromCSVToList(string _filePath);
}