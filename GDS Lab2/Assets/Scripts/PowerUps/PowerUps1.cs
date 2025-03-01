using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PowerUps1 : MonoBehaviour
{
    [SerializeField]
    Vector2 initialVelocity;

    [SerializeField]
    float reenableColliderAfter;

    Rigidbody2D _rigidbody;
    Collider2D _collider;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        _collider.enabled = false;
        _rigidbody.linearVelocity = initialVelocity;

        StartCoroutine(ReenableCollider());
    }

    IEnumerator ReenableCollider()
    {
        yield return new WaitForSeconds(reenableColliderAfter);
        _collider.enabled = true;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.collider.GetComponent<Player>();
        if (player != null)
        {
            Destroy(gameObject);
        }
    }
}
