//Engine
using UnityEngine;

//Interface
using Interfaces;

//Ect
using Data.Monster;

public class Monster : IDamageable, ITurnable, IMovable
{
    [SerializeField] private MonsterData_Base m_monsterData;

    public MonsterData_Base MonsterData { set { MonsterData = value; } }
}
