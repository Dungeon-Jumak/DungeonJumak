using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntrancePoint : MonoBehaviour
{
    [SerializeField] private EntranceController m_controller;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Customer")) return;

        var customer = collision.GetComponent<Customer>();

        m_controller.EntranceToJumak(customer);
    }
}
