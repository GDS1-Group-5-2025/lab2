using UnityEngine;

public class Sunflower : MonoBehaviour
{
    [SerializeField] private bool moving = false;
    [SerializeField] private float origY;
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
                moving = true;
            }
        }
    }
}
