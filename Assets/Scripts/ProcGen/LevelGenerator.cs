using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.EditorTools;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CameraController cameraController; // Reference to the CameraController script
    [SerializeField] GameObject[] chunkPrefabs;
    [SerializeField] GameObject checkPointChunkPrefab;
    [SerializeField] Transform chunkParent; // Parent object for the chunks
    [SerializeField] ScoreManager scoreManager; // Reference to the ScoreManager script
    [SerializeField] GameManager gameManager;

    [Header("Level Settings")]
    [Tooltip("The number of chunks to spawn at the start of the game.")]
    [SerializeField] int startingChunksAmount = 12;
    [SerializeField] int checkPointChunkInterval = 8;
    [Tooltip("Do not change chunkLength unless you also change the chunk prefab's length.")]
    [SerializeField] float chunkLength = 10f;
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float minMoveSpeed = 2f;
    [SerializeField] float maxMoveSpeed = 20f;
    [SerializeField] float minGravityZ = -22f;
    [SerializeField] float maxGravityZ = -2f;

    List<GameObject> chunks = new List<GameObject>();
    int chunksSpawned = 0;
    private bool isPaused = false;


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
        float newMoveSpeed = moveSpeed + speedAmount;
        newMoveSpeed = Mathf.Clamp(newMoveSpeed, minMoveSpeed, maxMoveSpeed);

        if (newMoveSpeed != moveSpeed)
        {
            moveSpeed = newMoveSpeed; // Ensure the speed does not go below the minimum, coming to an halt

            float newGravityZ = Physics.gravity.z - speedAmount;
            newGravityZ = Mathf.Clamp(newGravityZ, minGravityZ, maxGravityZ);

            Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, newGravityZ);
            // Dynamically adjust global gravity on the Z-axis based on speed changes.
            // This keeps obstacles falling/moving consistently with the level’s forward speed.
            // Increasing moveSpeed will decrease gravity.z (pulling obstacles faster forward),
            // and decreasing moveSpeed will increase gravity.z (slowing them down).

            cameraController.ChangeCameraFOV(speedAmount); // Adjust camera FOV based on speed changes
            // This will zoom in or out based on the speedAmount, enhancing the gameplay experience.
        }

    }

    void SpawnStartingChunks()
    {
        for (int i = 0; i < startingChunksAmount; i++)
        {
            SpawnChunkSingular();
        }
    }

    private void SpawnChunkSingular() // the method that spawns a single chunk
    {
        float spawnPositionZ = CalculateSpawnPosZ();
        Vector3 chunkSpawnPosition = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);
        GameObject chunkToSpawn = ChooseChunkToSpawn();
        GameObject newChunkGO = Instantiate(chunkToSpawn, chunkSpawnPosition, Quaternion.identity, chunkParent);
        chunks.Add(newChunkGO); // expands the list one item at a time.
        Chunk newChunk = newChunkGO.GetComponent<Chunk>();
        newChunk.Init(this, scoreManager, gameManager, chunksSpawned);
        chunksSpawned++;
    }

    private GameObject ChooseChunkToSpawn()
    {
        GameObject chunkToSpawn;

        // checkppoint only spawns at intervals & only after the first 8 (inteval) chunks spawned, not right at the start
        if (chunksSpawned % checkPointChunkInterval == 0 && chunksSpawned != 0)
        {
            chunkToSpawn = checkPointChunkPrefab; // Spawn a checkpoint chunk at intervals
        }
        else
        {
            chunkToSpawn = chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
        }

        return chunkToSpawn;
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
        if (isPaused) return;

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

    public void PauseLevel(float duration)
    {
        StartCoroutine(PauseRoutine(duration));
    }

    private IEnumerator PauseRoutine(float duration)
    {
        isPaused = true;
        yield return new WaitForSeconds(duration);
        isPaused = false;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
}

