using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDownAndDie : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {  
     //   transform.rotation = Quaternion.Lerp(transform.rotation, Destination.rotation, Time.deltaTime * RotateSpeed);
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
