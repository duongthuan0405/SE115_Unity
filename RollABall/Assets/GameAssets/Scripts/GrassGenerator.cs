using System;
using UnityEngine;

public class GrassGenerator : MonoBehaviour
{
    [SerializeField] GameObject ground;
    [SerializeField] int grassCount = 500;
    [SerializeField] GameObject grassPrefab;
    void Start()
    {
        if (ground == null)
        {
            Debug.LogError("ground == null");
            return;
        }

        Renderer groundRenderer = ground.GetComponent<Renderer>();
        if (groundRenderer == null)
        {
            Debug.LogError("groundRenderer == null");
            return;
        }

        Bounds groundBounds = groundRenderer.bounds;
        Vector3 groundCenter = groundBounds.center;
        Vector3 groundSize = groundBounds.size;

        float halfX = groundSize.x / 2f;
        float halfZ = groundSize.z / 2f;

        GenerateRandomGrass(groundCenter.x - halfX, groundCenter.x + halfX, groundCenter.z - halfZ, groundCenter.z + halfZ);
    }

    private void GenerateRandomGrass(float xStart, float xEnd, float zStart, float zEnd)
    {
        for (int i = 1; i <= grassCount; i++)
        {
            float randomX = UnityEngine.Random.Range(xStart, xEnd);
            float randomZ = UnityEngine.Random.Range(zStart, zEnd);

            float y = ground.transform.position.y;

            Vector3 spawnPos = new Vector3(randomX, y, randomZ);

            GameObject grass = Instantiate(grassPrefab, spawnPos, Quaternion.identity);

            grass.transform.SetParent(this.transform); ;
        }
    }

    void Update()
    {
        
    }
}
