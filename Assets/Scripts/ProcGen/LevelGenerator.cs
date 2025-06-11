using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject chunkPrefab;
    [SerializeField] int startingChunksAmount = 12;
    [SerializeField] Transform chunkParent; // Parent object for the chunks
    [SerializeField] float chunkLength = 10f;
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float minMoveSpeed = 2f;

    List<GameObject> chunks = new List<GameObject>();


    void Start()
    {
        SpawnStartingChunks();
    }

    void Update()
    {
        MoveChunks();
    }

    public void ChangeChunkMoveSpeed(float speedAmount)
    {
        moveSpeed += speedAmount;

        if (moveSpeed < minMoveSpeed)
        {
            moveSpeed = minMoveSpeed; // Ensure the speed does not go below the minimum, coming to an halt
        }
        Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, Physics.gravity.z - speedAmount);
        // Dynamically adjust global gravity on the Z-axis based on speed changes.
        // This keeps obstacles falling/moving consistently with the level’s forward speed.
        // Increasing moveSpeed will decrease gravity.z (pulling obstacles faster forward),
        // and decreasing moveSpeed will increase gravity.z (slowing them down).
    }

    void SpawnStartingChunks()
    {
        for (int i = 0; i < startingChunksAmount; i++)
        {
            SpawnChunkSingular();
        }
    }

    private void SpawnChunkSingular()
    {
        float spawnPositionZ = CalculateSpawnPosZ();

        Vector3 chunkSpawnPosition = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);
        GameObject newChunk = Instantiate(chunkPrefab, chunkSpawnPosition, Quaternion.identity, chunkParent);

        chunks.Add(newChunk); // expands the list one item at a time.
    }

    float CalculateSpawnPosZ()
    {
        float spawnPositionZ;

        if (chunks.Count == 0)
        {
            spawnPositionZ = transform.position.z; // First chunk at the initial position
        }
        else
        {
            spawnPositionZ = chunks[chunks.Count - 1].transform.position.z + chunkLength; // Position after the last chunk
        }

        return spawnPositionZ;
    }

    void MoveChunks()
    {
        for (int i = 0; i < chunks.Count; i++)
        {
            if (chunks[i] != null)
            {
                GameObject chunk = chunks[i];

                // Move the chunk towards the player
                chunk.transform.Translate(-transform.forward * (moveSpeed * Time.deltaTime));

                if (chunk.transform.position.z <= Camera.main.transform.position.z - chunkLength)
                {
                    chunks.Remove(chunk); // Remove the chunk from the list
                    Destroy(chunk); // Destroy the chunk if it has moved past the camera
                    SpawnChunkSingular(); // Spawn a new chunk to replace the one that was removed
                }
            }
        }
    }
}

