using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [SerializeField] private GameObject[] obstaclePrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnDistance = 50f;
    [SerializeField] private float spawnInterval = 2f;

    [Header("Lane Settings")]
    [SerializeField] private float laneDistance = 4f;

    private float timer;

    private void Update()
    {
        if (GameManager.Instance.IsGameOver())
            return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0f;
        }
    }

    private void SpawnObstacle()
    {
        int lane = Random.Range(0, 3);

        float xPosition = (lane - 1) * laneDistance;

        Vector3 spawnPosition = new Vector3(
            xPosition,
            0f,
            player.position.z + spawnDistance
        );

        int obstacleIndex =
            Random.Range(0, obstaclePrefabs.Length);

        Instantiate(
            obstaclePrefabs[obstacleIndex],
            spawnPosition,
            Quaternion.identity
        );
    }
}