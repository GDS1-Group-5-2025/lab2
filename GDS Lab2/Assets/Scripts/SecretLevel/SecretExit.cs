using UnityEngine;

public class SecretExit : MonoBehaviour
{
    [SerializeField] private bool entering, exiting;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject mainCam, secretCam, exitPipe;
    [SerializeField] private float yVal = 1;

    void Update()
    {
        if(entering){
            player.transform.position = new Vector2(player.transform.position.x + Time.deltaTime, player.transform.position.y);
            if(player.transform.position.x >= this.transform.position.x + 1){ 
                entering = false;
                exiting = true;
                player.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                player.transform.position = new Vector2(164.5f, 1);
                mainCam.GetComponent<Camera>().enabled = true;
                secretCam.GetComponent<Camera>().enabled = false;
                exitPipe.transform.GetChild(2).gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }
        if(exiting){ 
            yVal += Time.deltaTime;
            player.transform.position = new Vector2(exitPipe.transform.position.x, yVal);
            if(player.transform.position.y >= exitPipe.transform.position.y + 1){
                player.gameObject.GetComponent<MarioMovement>().enabled = true;
                exiting = false;
                player.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
        {
            if(
                collision.gameObject.CompareTag("Player") && 
                Input.GetKey(KeyCode.RightArrow) && 
                Mathf.Abs(collision.transform.position.y - this.transform.position.y) <= 0.75f
            ){
                player = collision.gameObject;
                player.gameObject.GetComponent<MarioMovement>().enabled = false;
                this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                exitPipe.gameObject.transform.GetChild(2).GetComponent<BoxCollider2D>().isTrigger = true;
                entering = true;
            }
        }
}
