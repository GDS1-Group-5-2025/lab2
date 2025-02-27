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
}
private void Start()
{
    _rigidbody= GetComponent<Rigidbody2D>(); 
    _collider = GetComponent<Collider2D>(); 
    _collider.enabled = false;
    _rigidbody.velocity initialVelocity; 
    StartCoroutine (routine: ReenableCollider());
}
private IEnumerator ReenableCollider()
{
    yield return new WaitForSeconds (_reenableColliderAfter);
    _collider.enabled = true;
}
private void OnCollisionEnter2D (Collision2D other)
{
    var player = other.collider.GetComponent<Player>();
    if (player != null)
    {
        Destroy (gameObject);
    }
}