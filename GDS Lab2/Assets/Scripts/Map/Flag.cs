using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private float checkVal;
    [SerializeField] private bool flagTriggered;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject flagSheet;
    [SerializeField] private int stage;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            flagTriggered = true;
            checkVal = collision.transform.position.y;
            Destroy(collision.gameObject.GetComponent<MarioMovement>());
            stage = 0;
        }
    }

    void Update()
    {
        if(flagTriggered){
            if(stage == 0){
                //Set Sprite to climb
                player.transform.position = new Vector2(this.transform.position.x - 0.4f, checkVal);
                flagSheet.transform.position = new Vector2(this.transform.position.x - 0.75f, checkVal);
                stage = 1;
            }
            else if(stage == 1){
                checkVal -= 7*Time.deltaTime;
                if(checkVal <= 2){
                    checkVal = 2;
                    stage = 3;
                }
                player.transform.position = new Vector2(this.transform.position.x - 0.4f, checkVal);
                flagSheet.transform.position = new Vector2(this.transform.position.x - 0.75f, checkVal);
                if(stage == 3){ 
                    player.transform.position = new Vector2(this.transform.position.x + 0.4f, 2);
                    player.transform.localScale = new Vector2(-1, 1);
                    player.gameObject.GetComponent<Rigidbody2D>().gravityScale = 10;
                }
            }
            else if(stage == 3){
                if(player.transform.position.y <= 1.1f){
                    checkVal = 200;
                    player.transform.position = new Vector2(200, 1);
                    stage = 4;
                    player.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                    player.transform.localScale = new Vector2(1, 1);
                }
            }
            else if(stage == 4){
                checkVal += 3*Time.deltaTime;
                player.transform.position = new Vector2(checkVal, 1);
                if(player.transform.position.x >= 205){
                    stage = 5;
                }
            }
            else if(stage == 5){
                Destroy(player);
                stage = 6;
            }

        }
    }
}
