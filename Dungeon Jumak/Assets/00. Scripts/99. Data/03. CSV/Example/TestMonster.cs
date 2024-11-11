using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class MonsterData
{
    public string MonsterName;
    public int MonsterID;
    public float MonsterHealth;
}

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Scriptable/CSV/testMonster", order = int.MaxValue)]
public class TestMonster : ScriptableObject
{
    public MonsterData data;
}
