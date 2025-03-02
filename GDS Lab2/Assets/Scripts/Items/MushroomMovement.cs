using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class MushroomMovement : MonoBehaviour
{
    [SerializeField] private bool moving = false;
    [SerializeField] private float origY;
    [SerializeField] private int moveDir;
    void Start()
    {
        origY = this.transform.position.y;
    }
    void Update()
    {
        if(!moving){
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + Time.deltaTime*2);
            if(this.transform.position.y >= origY + 1){
                this.transform.position = new Vector2(this.transform.position.x, origY + 1);
                this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 10;
                moving = true;
            }
        }
        else{
            this.transform.position = new Vector2(this.transform.position.x + (moveDir*3*Time.deltaTime), this.transform.position.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(moving){
            if(collision.gameObject.CompareTag("Wall")){
                moveDir *= -1;
            }
        }
    }
}
