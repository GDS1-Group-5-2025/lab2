using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PowerUps : MonoBehaviour
{
    [SerializeField]
    private Vector2 initialVelocity;

    [SerializeField]
    private float reenableColliderAfter;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

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
        var player = other.collider.GetComponent<Mario>();
        if (player != null)
        {
            Destroy(gameObject);
        }
    }
}
