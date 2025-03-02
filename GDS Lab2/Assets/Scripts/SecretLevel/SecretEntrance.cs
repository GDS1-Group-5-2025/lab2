using UnityEngine;

public class SecretEntrance : MonoBehaviour
{
    [SerializeField] private bool entering;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject mainCam, secretCam;
    void Update()
    {
        if(entering){
            player.transform.position = new Vector2(this.transform.position.x, player.transform.position.y - Time.deltaTime);
            if(player.transform.position.y <= this.transform.position.y - 1){ 
                entering = false;
                player.transform.position = new Vector2(151, -4);
                player.gameObject.GetComponent<CompositeCollider2D>().isTrigger = false;
                mainCam.GetComponent<Camera>().enabled = false;
                secretCam.GetComponent<Camera>().enabled = true;
                player.gameObject.GetComponent<MarioMovement>().enabled = true;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(
            collision.gameObject.CompareTag("Player") && 
            Input.GetKey(KeyCode.DownArrow) && 
            collision.transform.position.y > this.transform.position.y + 0.5f && 
            Mathf.Abs(collision.transform.position.x - this.transform.position.x) <= 0.75f
        ){
            collision.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 1);
            player = collision.gameObject;
            player.gameObject.GetComponent<CompositeCollider2D>().isTrigger = true;
            entering = true;
            player.gameObject.GetComponent<MarioMovement>().enabled = false;
        }
    }
}
