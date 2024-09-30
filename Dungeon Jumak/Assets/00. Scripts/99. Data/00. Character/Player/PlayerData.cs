// System
using System;

/// <summary>
/// 플레이어의 데이터를 저장하는 클래스입니다.
/// </summary>
[Serializable]
public class PlayerData
{
    // 닉네임
    private string name = "";

    // 레벨
    private int curPlayerLV = 1;
    private int maxPlayerLV;

    // 코인
    private int curCoin = 0;
    private int maxCoin = 999999;

    // 경험치 (XP)
    private float curXP = 0;
    private float maxXP = 5;

    // 획득한 재료 목록
    private int[] ingredient = new int[5] { 10, 10, 10, 10, 10 }; //0: 돼지고기, 1: 부추, 2: 콩나물, 3: 오징어, 4: 소고기

    // 획득한 장비 목록

    // 획득한 스킬? 목록 등 .. 계속 추가하면 됨

    #region Property - Get / Set

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int CurPlayerLV
    {
        get { return curPlayerLV; }
        set { curPlayerLV = value; }
    }

    public int MaxPlayerLV
    {
        get { return maxPlayerLV; }
        set { maxPlayerLV = value; }
    }

    public int CurCoin
    {
        get { return curCoin; }
        set { curCoin = value; }
    }

    public int MaxCoin
    {
        get { return maxCoin; }
        set { maxCoin = value; }
    }

    public float CurXP
    {
        get { return curXP; }
        set { curXP = value; }
    }

    public float MaxXP
    {
        get { return maxXP; }
        set { maxXP = value; }
    }

    public int[] Ingredient
    {
        get { return ingredient; }
        set { ingredient = value; }
    }

    #endregion
}
