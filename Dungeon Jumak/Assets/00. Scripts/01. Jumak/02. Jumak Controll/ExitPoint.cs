using UnityEngine;

public class ExitPoint : MonoBehaviour
{
    [SerializeField] private EntranceController m_controller;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Customer_Exit")) return;

        var customer = collision.GetComponent<Customer>();

        m_controller.JumakToVillage(customer);
    }
}
