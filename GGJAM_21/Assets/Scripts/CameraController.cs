using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject actualTarget;
    public GameObject target;
    Player playerScript;
    public float LowerLevelBoundsY = 0;

    Camera attachedCamera;

    public float CameraSpeed;
    // Start is called before the first frame update
    void Start()
    {
        attachedCamera = GetComponent<Camera>();
        playerScript = target.GetComponent<Player>();
        actualTarget = GameObject.FindGameObjectWithTag("goal");
        if(actualTarget != null)
        {
            Vector3 acTarPos = actualTarget.transform.position;
            transform.position = new Vector3(acTarPos.x, acTarPos.y, transform.position.z);
            StartCoroutine(CoroutineHelper.DelaySeconds(() =>
            {
                actualTarget = target;
                playerScript.CanControl = true;
            }, 2f));
            StartCoroutine(CoroutineHelper.DelaySeconds(() =>
            {
                playerScript.playerRigidBody.bodyType = RigidbodyType2D.Dynamic;
            }, 2.5f));
        }
        else
        {
            actualTarget = target;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float camHeight = (attachedCamera.transform.position - attachedCamera.ViewportToWorldPoint(new Vector3(0.5f, 1, attachedCamera.transform.position.z))).y;
        float lowestCamY = Mathf.Max(
            LowerLevelBoundsY + camHeight * 1.2f,
            actualTarget.transform.position.y);

        float extraY = 0;

        if(playerScript != null)
        {
            extraY = playerScript.IsCrouching ? -camHeight / 2 : 0;
        }

        transform.position = Vector3.Lerp(
            transform.position, 
            new Vector3(actualTarget.transform.position.x, lowestCamY + extraY, transform.position.z), 
            Time.deltaTime * CameraSpeed * (lowestCamY < LowerLevelBoundsY + camHeight * 1.25f? 1.2f: 1));
    }
}
