using UnityEngine;

public class fireballs : MonoBehaviour
{
    [SerializeField] private MarioState mS;
    [SerializeField] private GameObject fireball;
    private int dir = 1;
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow)){ dir = -1; }
        if(Input.GetKey(KeyCode.RightArrow)){ dir = 1; }
        if(mS.currentState == MarioStateEnum.Fire && Input.GetKeyDown(KeyCode.Return)){
            GameObject fB = Instantiate(fireball, transform.position, Quaternion.identity);
            fB.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir*400, 0));
        }
    }
}
