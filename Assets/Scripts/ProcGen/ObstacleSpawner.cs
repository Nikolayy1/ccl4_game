using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] obstaclePrefabs;
    [SerializeField] float obstacleSpawnTime = 3f;
    [SerializeField] float minObstacleSpawnTime = 1.2f;
    [SerializeField] Transform obstacleParent;
    [SerializeField] float spawnAreaWidth = 4f;

    private int checkpointCounter = 0;

    void Start()
    {
        StartCoroutine(SpawnObstaclesRoutine());
    }


    public void DecreaseObstacleSpawnTime(float amount)
    {
        checkpointCounter++;

        if (checkpointCounter % 3 != 0) return; // Only reduce on every 3rd checkpoint

        obstacleSpawnTime -= amount;

        if (obstacleSpawnTime < minObstacleSpawnTime)
        {
            obstacleSpawnTime = minObstacleSpawnTime;
        }
    }

    IEnumerator SpawnObstaclesRoutine()
    {
        while (true)
        {
            GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnAreaWidth, spawnAreaWidth),
                transform.position.y,
                transform.position.z
);
            yield return new WaitForSeconds(obstacleSpawnTime);
            Instantiate(obstaclePrefab, spawnPosition, Random.rotation, obstacleParent);
        }
    }
}
