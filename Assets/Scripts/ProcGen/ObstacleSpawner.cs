using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] obstaclePrefabs;
    [SerializeField] float obstacleSpawnInterval = 1f;
    [SerializeField] Transform obstacleParent;
    [SerializeField] float spawnAreaWidth = 4f;
    void Start()
    {
        StartCoroutine(SpawnObstaclesRoutine());
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
            yield return new WaitForSeconds(obstacleSpawnInterval);
            Instantiate(obstaclePrefab, spawnPosition, Random.rotation, obstacleParent);
        }
    }
}
