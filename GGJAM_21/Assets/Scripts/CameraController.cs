using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;

    public float CameraSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z), Time.deltaTime * CameraSpeed);
    }
}
