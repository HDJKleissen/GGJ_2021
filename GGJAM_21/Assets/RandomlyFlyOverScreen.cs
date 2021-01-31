using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomlyFlyOverScreen : MonoBehaviour
{
    Rigidbody2D rigidBody2D;
    float rotationSpeed = 0;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        StartCoroutine(CoroutineHelper.DelaySeconds(() => StartNewRun(), Random.Range(8, 20)));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1), rotationSpeed * Time.deltaTime);
    }

    void StartNewRun()
    {
        transform.position = new Vector3(Random.Range(-40, 40), Random.Range(-20, 50), transform.position.z);
        rigidBody2D.velocity = new Vector2(-transform.position.x / 40 * 25, Random.Range(-15, 15));
        rotationSpeed = Random.Range(-Mathf.Abs(transform.position.x / 50 * 35), Mathf.Abs(transform.position.x / 500 * 35));
        StartCoroutine(CoroutineHelper.DelaySeconds(() => StartNewRun(), Random.Range(8, 20)));
    }
}
