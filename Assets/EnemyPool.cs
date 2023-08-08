using System.Collections.Generic;
using UnityEngine;

public class EnemyPool2 : MonoBehaviour
{
    // The prefabs of the enemies to pool
    public GameObject[] enemyPrefabs;
    public Transform respawnPoint;
    public float intervalRespawn = 5f; // Time interval between enemy respawns

    // The maximum number of enemies in the pool
    public int poolSize = 10;

    // The list to store the pooled enemies
    private List<GameObject> enemyPool;
    private float timeSinceLastRespawn;

    private void Start()
    {
        // Initialize the enemy pool
        enemyPool = new List<GameObject>();

        // Populate the pool with enemy instances
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = InstantiateEnemy();
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
    }

    private void Update()
    {
        // Check if it's time to respawn an enemy
        timeSinceLastRespawn += Time.deltaTime;
        if (timeSinceLastRespawn >= intervalRespawn)
        {
            timeSinceLastRespawn = 0f;
            RespawnRandomEnemy();
        }
    }

    // Method to retrieve an enemy from the pool
    public GameObject GetEnemyFromPool()
    {
        // Find an inactive enemy in the pool
        foreach (GameObject enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.SetActive(true);
                return enemy;
            }
        }

        // If all enemies in the pool are active, create a new enemy and add it to the pool
        GameObject newEnemy = InstantiateEnemy();
        newEnemy.SetActive(true);
        enemyPool.Add(newEnemy);

        return newEnemy;
    }

    // Method to return an enemy to the pool
    public void ReturnEnemyToPool(GameObject enemy)
    {
        enemy.SetActive(false);
    }

    // Helper method to instantiate a new enemy
    private GameObject InstantiateEnemy()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedPrefab = enemyPrefabs[randomIndex];
        GameObject newEnemy = Instantiate(selectedPrefab, transform.position, Quaternion.identity, transform);
        // Add any additional setup or configuration for the enemy here if needed
        return newEnemy;
    }

    // Method to respawn a random enemy from the pool at the respawn point
    private void RespawnRandomEnemy()
    {
        GameObject enemyToRespawn = GetEnemyFromPool();
        enemyToRespawn.transform.position = respawnPoint.position;
    }
}
