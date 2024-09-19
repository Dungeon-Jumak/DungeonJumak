//Engine
using UnityEngine;

//Interface
using Interfaces;

//Ect
using Data.Character;

public class Monster : IDamageable, ITurnable, IMovable
{
    [SerializeField] private MonsterData_Base data;

    public MonsterData_Base MonsterData { set { data = value; } }
}
