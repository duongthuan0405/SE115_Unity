using UnityEngine;

public class WallsGenerator : MonoBehaviour
{
    [SerializeField] GameObject ground;     
    [SerializeField] GameObject wallPrefab;
    [SerializeField] float wallHeight = 2f;

    void Start()
    {
        GenerateWalls();
    }

    void GenerateWalls()
    {
        if (ground == null || wallPrefab == null)
        {
            Debug.LogError("ground == null || wallPrefab == null");
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

        Renderer wallRenderer = wallPrefab.GetComponent<Renderer>();

        if (wallRenderer == null)
        {
            Debug.LogError("Wall prefab phải có Renderer!");
            return;
        }

        float wallLength = wallRenderer.bounds.size.x; 
        float wallThickness = wallRenderer.bounds.size.z;

        float halfX = groundSize.x / 2f;
        float halfZ = groundSize.z / 2f;

        GenerateWallLine(new Vector3(groundCenter.x - halfX, groundCenter.y, groundCenter.z - halfZ), Vector3.right, groundSize.x, wallLength); // cạnh dưới
        GenerateWallLine(new Vector3(groundCenter.x - halfX, groundCenter.y, groundCenter.z + halfZ), Vector3.right, groundSize.x, wallLength); // cạnh trên
        GenerateWallLine(new Vector3(groundCenter.x - halfX, groundCenter.y, groundCenter.z - halfZ), Vector3.forward, groundSize.z, wallLength, true); // cạnh trái
        GenerateWallLine(new Vector3(groundCenter.x + halfX, groundCenter.y, groundCenter.z - halfZ), Vector3.forward, groundSize.z, wallLength, true); // cạnh phải
    }

    void GenerateWallLine(Vector3 startPos, Vector3 dir, float totalLength, float pieceLength, bool rotate90 = false)
    {
        int count = Mathf.CeilToInt(totalLength / pieceLength);

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = startPos + dir * (i * pieceLength);
            Quaternion rot = rotate90 ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;
            GameObject wall = Instantiate(wallPrefab, pos, rot);

            wall.transform.position += dir * (pieceLength / 2f);
            wall.transform.parent = this.transform;
        }
    }
}
