using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;

public class StarMovement : MonoBehaviour
{
    [SerializeField] private bool moving = false;
    [SerializeField] private float origY;
    [SerializeField] private int moveDir;
    [SerializeField] private Rigidbody2D rb;
    void Start()
    {
        origY = this.transform.position.y;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if(!moving){
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + Time.deltaTime*2);
            if(this.transform.position.y >= origY + 1){
                this.transform.position = new Vector2(this.transform.position.x, origY + 1);
                this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                rb.gravityScale = 3;
                moving = true;
                rb.AddForce(new Vector2(400, 700));
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // if(collision.gameObject.CompareTag("wall")){
        //     moveDir *= -1;
        //     rb.AddForce(new Vector2(moveDir*400, 0));
        // }
        // else if(collision.gameObject.CompareTag("Floor")){
        //     rb.AddForce(new Vector2(0, 700));
        // }
        Vector2 pnt = collision.GetContact(0).point;
        if(this.transform.position.y > pnt.y){ rb.AddForce(new Vector2(0, 400)); }
        else if(this.transform.position.x < pnt.x){ rb.AddForce(new Vector2(-400, 0)); }
        else if(this.transform.position.x > pnt.x){ rb.AddForce(new Vector2(400, 0)); }
    }
}
