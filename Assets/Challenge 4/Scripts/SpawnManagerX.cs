using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;

    private float spawnRangeX = 10f;
    private float spawnZMin = 15f;
    private float spawnZMax = 25f;

    public int enemyCount;
    public int waveCount = 1;

    public GameObject player;

    [Header("Enemy Difficulty")]
    public float enemySpeed;
    public float enemySpeedIncrease = 10f;

    private float startingEnemySpeed;

    void Start()
    {
        // Preserve the starter prefab's original speed as wave 1 speed.
        EnemyX enemyScript = enemyPrefab != null
            ? enemyPrefab.GetComponent<EnemyX>()
            : null;

        startingEnemySpeed = enemyScript != null ? enemyScript.speed : 1f;
        enemySpeed = startingEnemySpeed;
    }

    void Update()
    {
        // Count ENEMIES, not powerups.
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // Start the next wave only after every enemy is gone.
        if (enemyCount == 0)
        {
            SpawnEnemyWave(waveCount);
        }
    }

    // Generate random spawn position for powerups and enemy balls.
    Vector3 GenerateSpawnPosition()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float zPos = Random.Range(spawnZMin, spawnZMax);
        return new Vector3(xPos, 0f, zPos);
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        Vector3 powerupSpawnOffset = new Vector3(0f, 0f, -15f);

        // Wave 1 uses the starter enemy speed; each later wave is faster.
        enemySpeed = startingEnemySpeed + ((waveCount - 1) * enemySpeedIncrease);

        // If no powerups remain, spawn one at the player's end.
        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0)
        {
            Instantiate(
                powerupPrefab,
                GenerateSpawnPosition() + powerupSpawnOffset,
                powerupPrefab.transform.rotation
            );
        }

        // Wave 1 = 1 enemy, wave 2 = 2 enemies, wave 3 = 3 enemies, etc.
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(
                enemyPrefab,
                GenerateSpawnPosition(),
                enemyPrefab.transform.rotation
            );
        }

        waveCount++;
        ResetPlayerPosition();
    }

    // Move player back to position in front of own goal.
    void ResetPlayerPosition()
    {
        if (player == null)
        {
            return;
        }

        player.transform.position = new Vector3(0f, 1f, -7f);

        Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }
    }
}