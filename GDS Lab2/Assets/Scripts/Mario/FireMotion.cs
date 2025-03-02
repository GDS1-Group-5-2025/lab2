using UnityEngine;

public class FireMotion : MonoBehaviour
{
    private float timer = 0;
    void Update()
    {
        if(timer > 0.1f && this.gameObject.GetComponent<BoxCollider2D>().isTrigger){ this.gameObject.GetComponent<BoxCollider2D>().isTrigger = false; }
        timer += Time.deltaTime;
        if(timer >= 20){ Destroy(this.gameObject); }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Floor")){
            this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 250));
        }
        else{ Destroy(this.gameObject); }
    }
}
