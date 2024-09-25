//Engine
using UnityEngine;

//Interface
using Interfaces;

//Ect
using Data.Character;

public class Monster : MonoBehaviour, IDamageable, ITurnable, IMovable
{
    [SerializeField] private MonsterData_Base data;
}
