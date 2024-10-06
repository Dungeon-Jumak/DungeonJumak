using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Scriptable/testMonster", order = int.MaxValue)]
public class TestMonster : ScriptableObject
{
    public string MonsterName;
    public int MonsterID;
    public float MonsterHealth;
}
