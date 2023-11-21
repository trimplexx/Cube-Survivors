using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            collision.rigidbody.velocity = Vector3.zero; // Zatrzymaj wszelkie ruchy
            collision.rigidbody.angularVelocity = Vector3.zero; // Zatrzymaj wszelkie obroty
        }
        Destroy(gameObject);
    }
}