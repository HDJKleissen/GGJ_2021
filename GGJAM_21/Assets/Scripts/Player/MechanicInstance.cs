using UnityEngine;

public class MechanicInstance : MonoBehaviour
{
    public string mechanicName;

    public GameObject GetChildByName(string name)
    {
        foreach(Transform childTransform in GetComponentsInChildren<Transform>())
        {
            if (childTransform.gameObject.name == name)
            {
                return childTransform.gameObject;
            }
        }

        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
