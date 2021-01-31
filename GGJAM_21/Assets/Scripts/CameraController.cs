using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public float LowerLevelBoundsY = 0;

    Camera attachedCamera;

    public float CameraSpeed;
    // Start is called before the first frame update
    void Start()
    {
        attachedCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float camHeight = (attachedCamera.transform.position - attachedCamera.ViewportToWorldPoint(new Vector3(0.5f, 1, attachedCamera.transform.position.z))).y;
        float lowestCamY = Mathf.Max(
            LowerLevelBoundsY + camHeight * 1.2f,
            target.transform.position.y);

        transform.position = Vector3.Lerp(
            transform.position, 
            new Vector3(target.transform.position.x, lowestCamY, transform.position.z), 
            Time.deltaTime * CameraSpeed * (lowestCamY < LowerLevelBoundsY + camHeight * 1.25f? 1.2f: 1));
    }
}
