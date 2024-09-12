//Engine
using UnityEngine;

//Interface
using Interfaces;

//Ect
using Data.Character;

public class Monster : IDamageable, ITurnable, IMovable
{
    [SerializeField] private MonsterData_Base m_monsterData;

    public MonsterData_Base MonsterData { set { MonsterData = value; } }
}
