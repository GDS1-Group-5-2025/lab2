using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private float collY;
    [SerializeField] private bool flagTriggered;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject flagSheet;
    [SerializeField] private int stage;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            flagTriggered = true;
            collY = collision.transform.position.y;
            Destroy(collision.gameObject.GetComponent<MarioMovement>());
            stage = 0;
        }
    }

    void Update()
    {
        if(flagTriggered){
            if(stage == 0){
                //Set Sprite to climb
                player.transform.position = new Vector2(this.transform.position.x - 0.4f, collY);
                flagSheet.transform.position = new Vector2(this.transform.position.x - 0.75f, collY);
            }
        }
    }
}
