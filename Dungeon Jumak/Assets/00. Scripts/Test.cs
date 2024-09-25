using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        DataManager<PlayerData>.Instance.Data.CurPlayerLV++;
        DataManager<PlayerData>.Instance.Data.CurPlayerLV++;
        DataManager<PlayerData>.Instance.Data.CurPlayerLV++;

        Debug.Log(DataManager<PlayerData>.Instance.Data.CurPlayerLV);
    }
}
