using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerBounds : MonoBehaviour
{
    public CompositeCollider2D levelCollider;
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D lowerBoundsCollider = GetComponent<BoxCollider2D>();
        if(levelCollider == null)
        {
            Debug.LogWarning("The lower bounds on this level does not have a preset reference to the level's CompositeCollider, set it in the inspector and save a frame!");
            levelCollider = FindObjectOfType<CompositeCollider2D>();
        }

        if (levelCollider != null)
        {
            lowerBoundsCollider.size = new Vector2(levelCollider.bounds.extents.x * 4, lowerBoundsCollider.size.y);
            transform.position = new Vector3(0, levelCollider.bounds.min.y - 2.5f, 0);
            Camera.main.GetComponent<CameraController>().LowerLevelBoundsY = lowerBoundsCollider.bounds.max.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
