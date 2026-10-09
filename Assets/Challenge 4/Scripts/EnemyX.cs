using UnityEngine;

public class EnemyX : MonoBehaviour
{
    public float speed;

    private Rigidbody enemyRb;
    private GameObject playerGoal;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();

        // Fix the NullReferenceException: actually assign the Player Goal.
        playerGoal = GameObject.Find("Player Goal");

        // Read the current wave's enemy speed from SpawnManagerX.
        GameObject spawnManagerObject = GameObject.Find("Spawn Manager");
        if (spawnManagerObject != null)
        {
            SpawnManagerX spawnManager =
                spawnManagerObject.GetComponent<SpawnManagerX>();

            if (spawnManager != null)
            {
                speed = spawnManager.enemySpeed;
            }
        }
    }

    void FixedUpdate()
    {
        if (enemyRb == null || playerGoal == null)
        {
            return;
        }

        // Move toward the Player Goal.
        Vector3 lookDirection =
            (playerGoal.transform.position - transform.position).normalized;

        enemyRb.AddForce(
            lookDirection * speed * Time.fixedDeltaTime
        );
    }

    private void OnCollisionEnter(Collision other)
    {
        // If enemy collides with either goal, destroy it.
        if (other.gameObject.name == "Enemy Goal" ||
            other.gameObject.name == "Player Goal")
        {
            Destroy(gameObject);
        }
    }
}