using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float timeSinceCloud = 0f;
    public float secondsBetweenClouds = 2f;
    private const int maxClouds = 20;

    public GameObject cloudPrefab;
    public float cloudWidth = 1.2f;
    public float cloudHeight = 2f;

    void Update()
    {
        timeSinceCloud += Time.deltaTime;
        int numObjects = GameObject.FindGameObjectsWithTag("Cloud").Length;

        if (timeSinceCloud >= secondsBetweenClouds || numObjects < 4)
        {
            if (numObjects < maxClouds)
            {
                GameObject newCloudObj = Instantiate(cloudPrefab);
                int tries = 0;

                while (IsCollidingWithCloud(newCloudObj) && tries < 30000)
                {
                    newCloudObj.transform.position = new Vector3(
                        Random.Range(-cloudWidth, cloudWidth),
                        Random.Range(0, cloudHeight),
                        0f
                    );
                    //Debug.Log(newCloudObj.transform.position);
                    tries++;
                }
            }

            timeSinceCloud = 0f;
        }
    }

    private bool IsCollidingWithCloud(GameObject obj)
    {
        Collider2D[] colliders = obj.GetComponents<Collider2D>();

        foreach (Collider2D collider in colliders)
        {
            Collider2D[] colliders2 = Physics2D.OverlapAreaAll(
                new Vector2(collider.bounds.min.x, collider.bounds.min.y),
                new Vector2(collider.bounds.max.x, collider.bounds.max.y)
            );

            if (colliders2.Length > 0)
            {
                return true;
            }
        }

        return false;
    }

}
