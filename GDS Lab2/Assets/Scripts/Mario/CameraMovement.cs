using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private Transform _camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Camera.main != null)
            _camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_camera)
            return;

        if (transform.position.x > _camera.position.x)
        {
            _camera.position = new Vector3(transform.position.x, _camera.position.y, _camera.position.z);
        }
    }
}
