using UnityEngine;

public class CoinMovement : MonoBehaviour
{
    private float maxHeight;
    void Start()
    {
        maxHeight = this.transform.position.y + 3;
    }
    void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 8*Time.deltaTime);
        if(this.transform.position.y >= maxHeight){
            Destroy(this.gameObject);
        }
    }
}
