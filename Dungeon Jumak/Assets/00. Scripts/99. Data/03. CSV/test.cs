using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Scriptable/test", order = int.MaxValue)]
public class test : ScriptableObject
{
    public string characterName;
    public int characterID;
    public float characterHealth;
}
