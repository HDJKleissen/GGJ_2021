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
            StartCoroutine(CoroutineHelper.Chain(CoroutineHelper.DelaySeconds(() =>
            {
                actualTarget = target;
            }, 2f),
                CoroutineHelper.WaitUntil(() => actualTarget.transform.position.x - transform.position.x < 0.5f || actualTarget.transform.position.y - transform.position.y < 0.5f),
                CoroutineHelper.Do(() =>
            {
                playerScript.playerRigidBody.bodyType = RigidbodyType2D.Dynamic;
                playerScript.CanControl = true;
            })));
        }
        else
        {
            actualTarget = target;
        }
    }
    private void Update()
    {
        Debug.Log("----------------");
        Debug.Log(Vector2.Distance(actualTarget.transform.position, transform.position));
        Debug.Log(Vector2.Distance(new Vector2(actualTarget.transform.position.x, actualTarget.transform.position.y), new Vector2(transform.position.x, transform.position.y)));
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
