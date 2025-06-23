using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Chunk : MonoBehaviour
{
    [SerializeField] GameObject fencePrefab;
    [SerializeField] GameObject potionPrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject bearTrapPrefab;
    [SerializeField] GameObject robberPrefab;
    [SerializeField] GameObject scavengerPrefab;



    [SerializeField] float potionSpawnChance = 0.3f;       // 30% chance to spawn a potion
    [SerializeField] float coinSpawnChance = 0.5f;         // 50% chance to spawn coins
    [SerializeField] float coinSeperationLength = 2f;      // Z spacing between coins
    [SerializeField] int robberSpawnThreshold = 250;
    [SerializeField] int skipSpawnUntilChunk = 1;

    [SerializeField] float[] lanes = { -2.5f, 0f, 2.5f };   // X positions for the 3 lanes, now -3,0,3

    LevelGenerator levelGenerator;
    ScoreManager scoreManager;
    GameManager gameManager;

    // Each index (0 = left, 1 = center, 2 = right) maps to an X position in `lanes`
    List<int> availableLanes = new List<int> { 0, 1, 2 };

    static int lastRobberScoreThreshold = 0;
    private int chunkIndex = 0;



    void Start()
    {
        if (chunkIndex < skipSpawnUntilChunk) return; // Skip spawning if this is one of the first two chunks

        SpawnFences();   // May spawn 0–2 fences randomly on available lanes
        SpawnPotion();   // 30% chance to spawn a potion on a remaining free lane
        SpawnCoins();    // 50% chance to spawn 1–5 coins on a remaining lane, spaced in Z
        SpawnBearTrap(); // 15 % chance to spawn bear trap on a free lane
    }

    public void Init(LevelGenerator levelGenerator, ScoreManager scoreManager, GameManager gameManager, int chunkIndex)
    {
        this.levelGenerator = levelGenerator;
        this.scoreManager = scoreManager;
        this.gameManager = gameManager;
        TrySpawnRobber();
        TrySpawnScavenger();
        this.chunkIndex = chunkIndex;
    }

    void SpawnFences()
    {
        List<int> freshLanes = new List<int>(availableLanes); // Local copy for safe iteration
        int fencesToSpawn = Random.Range(0, lanes.Length);    // 0–2 fences

        for (int i = 0; i < fencesToSpawn; i++)
        {
            if (freshLanes.Count <= 0) break;

            int randomLaneIndex = Random.Range(0, freshLanes.Count);
            int selectedLane = freshLanes[randomLaneIndex];
            freshLanes.RemoveAt(randomLaneIndex);

            Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
            Instantiate(fencePrefab, spawnPosition, Quaternion.identity, this.transform);

            availableLanes.Remove(selectedLane); // Make sure no other pickup uses this lane
        }
    }

    void SpawnPotion()
    {
        // Only spawn if we pass the random check and a lane is still available
        if (Random.value > potionSpawnChance || availableLanes.Count <= 0) return;

        int selectedLane = SelectLane();
        Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
        Potion newPotion = Instantiate(potionPrefab, spawnPosition, Quaternion.identity, this.transform).GetComponent<Potion>();
        newPotion.Init(levelGenerator);
    }

    void SpawnCoins()
    {
        // 50% chance to spawn coins, and only if we have a lane left
        if (Random.value > coinSpawnChance || availableLanes.Count <= 0) return;

        int selectedLane = SelectLane();
        int maxCoinsToSpawn = 6;
        int coinsToSpawn = Random.Range(1, maxCoinsToSpawn); // Spawn 1 to 5 coins

        float topZ = transform.position.z + (coinSeperationLength * 2f); // Starting point at top of chunk

        for (int i = 0; i < coinsToSpawn; i++)
        {
            float spawnZ = topZ - (i * coinSeperationLength); // Step downward along Z
            Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y + 1f, spawnZ);
            Coin newCoin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity, this.transform).GetComponent<Coin>();
            newCoin.Init(scoreManager, gameManager);  // Initialize the coin with the score manager
        }
    }

    void SpawnBearTrap()
    {
        if (Random.value > 0.15f || availableLanes.Count <= 0) return; // 15% spawn chance

        int selectedLane = SelectLane();
        Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
        Instantiate(bearTrapPrefab, spawnPosition, Quaternion.identity, this.transform);
    }

    void SpawnRobber()
    {
        if (availableLanes.Count == 0) return;

        int selectedLane = SelectLane();
        Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
        Instantiate(robberPrefab, spawnPosition, Quaternion.identity, this.transform);
    }

    void TrySpawnRobber()
    {
        if (scoreManager != null &&
            scoreManager.GetScore() >= robberSpawnThreshold * (lastRobberScoreThreshold + 1) &&
            availableLanes.Count > 0)
        {
            Debug.Log(">> Robber spawn condition met.");
            SpawnRobber();
            lastRobberScoreThreshold++;
        }
    }

    void TrySpawnScavenger()
    {
        if (PotionTracker.Instance != null &&
            PotionTracker.Instance.ShouldSpawnScavenger() &&
            availableLanes.Count > 0)
        {
            int lane = SelectLane();
            Vector3 pos = new Vector3(lanes[lane], transform.position.y + 3f, transform.position.z);
            Instantiate(scavengerPrefab, pos, Quaternion.identity);

            PotionTracker.Instance.SetScavengerActive(true);
            PotionTracker.Instance.ResetPotionCounter();

            Debug.Log(">> Scavenger spawned due to potion milestone.");
        }
    }




    // Removes a lane from the list so it's not reused by other pickups
    int SelectLane()
    {
        int randomIndex = Random.Range(0, availableLanes.Count);
        int selected = availableLanes[randomIndex];
        availableLanes.RemoveAt(randomIndex);
        return selected;
    }
}
