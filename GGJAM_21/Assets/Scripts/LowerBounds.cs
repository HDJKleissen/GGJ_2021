using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerBounds : MonoBehaviour
{
    public CompositeCollider2D levelCollider;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;
        if(levelCollider == null)
        {
            Debug.LogWarning("The lower bounds on this level does not have a preset reference to the level's CompositeCollider, set it in the inspector and save a frame!");
            levelCollider = FindObjectOfType<CompositeCollider2D>();
        }

        if (levelCollider != null)
        {
            BoxCollider2D bottomBounds = gameObject.AddComponent<BoxCollider2D>();
            BoxCollider2D leftBounds = gameObject.AddComponent<BoxCollider2D>();
            BoxCollider2D rightBounds = gameObject.AddComponent<BoxCollider2D>();
            BoxCollider2D topBounds = gameObject.AddComponent<BoxCollider2D>();

            bottomBounds.size = new Vector2(levelCollider.bounds.extents.x * 8, 1);
            bottomBounds.offset = new Vector3(levelCollider.bounds.center.x, levelCollider.bounds.min.y - 2.5f, 0);
            leftBounds.size = new Vector2(1, levelCollider.bounds.extents.y * 8);
            leftBounds.offset = new Vector3(levelCollider.bounds.min.x - 15, levelCollider.bounds.center.y, 0);
            rightBounds.size = new Vector2(1, levelCollider.bounds.extents.y * 8);
            rightBounds.offset = new Vector3(levelCollider.bounds.max.x + 15, levelCollider.bounds.center.y, 0);
            topBounds.size = new Vector2(levelCollider.bounds.extents.x * 8, 1);
            topBounds.offset = new Vector3(levelCollider.bounds.center.x, levelCollider.bounds.max.y + 15, 0);

            bottomBounds.isTrigger = true;
            leftBounds.isTrigger = true;
            rightBounds.isTrigger = true;
            topBounds.isTrigger = true;

            Camera.main.GetComponent<CameraController>().LowerLevelBoundsY = bottomBounds.bounds.max.y;
        }
        else
        {
            Debug.LogError("Level's CompositeCollider component not found, check the level to make sure it has a grid with compositecollider!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
