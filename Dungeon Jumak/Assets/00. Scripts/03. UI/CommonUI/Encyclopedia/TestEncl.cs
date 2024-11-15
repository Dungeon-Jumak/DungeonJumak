using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEncl : MonoBehaviour
{
    private DataManager<EncyclopediaData> g_EData;

    private void Awake()
    {
        g_EData = DataManager<EncyclopediaData>.Instance;
    }
    public void OnClickBtn()
    {
        g_EData.Data.Monsters["0"] = true;
    }
    public void OnClickBtn1()
    {
        g_EData.Data.Customers["11"] = true;
    }
}
