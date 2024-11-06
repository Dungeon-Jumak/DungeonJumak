// Engine
using UnityEngine;
using Data.Character;

public class MO_DamageHandler : MonoBehaviour
{
    private float currentHp;
    private MonsterDataSO data;

    public void Initialize(MonsterDataSO data)
    {
        this.data = data;
        // currentHp = data.maxHp;
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} is dead.");
        gameObject.SetActive(false);
    }
}
