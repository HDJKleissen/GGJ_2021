using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndRotateTowardsThenDie : MonoBehaviour
{
    // this sucks
    public Rigidbody2D player;
    public Transform Destination;
    public float MoveSpeed, RotateSpeed;

    public bool DestroyDestination = false;

    public Dictionary<string, string> stringParameters = new Dictionary<string, string>();
    public Dictionary<string, int> intParameters = new Dictionary<string, int>();

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, Destination.position) < 0.5f && Quaternion.Angle(transform.rotation, Destination.rotation) < 5)
        {
            Destroy(gameObject);
            if (DestroyDestination)
            {
                Destroy(Destination.gameObject);
            }
        }
        MoveSpeed = (player.velocity.magnitude + 5) * 1.5f;
        
        transform.position = Vector3.Lerp(transform.position, Destination.position, Time.deltaTime * MoveSpeed);
        
        transform.rotation = Quaternion.Lerp(transform.rotation, Destination.rotation, Time.deltaTime * RotateSpeed);
    }

    private void OnDestroy()
    {
        // Send notification that this object is about to be destroyed
        if (this.OnDestroyEvent != null) this.OnDestroyEvent(this);
    }

    /// <summary>
    /// This event is invoked when unity is calling OnDestroy
    /// </summary>
    public event OnDestroyDelegate OnDestroyEvent;

    public delegate void OnDestroyDelegate(MonoBehaviour instance);
}
