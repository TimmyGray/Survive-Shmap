using System.Collections.Generic;
using UnityEngine;

public class GameCotroller : MonoBehaviour
{
    public List<GameObject> enemies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update() { }

    private void SpawnEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            SpawnEnemy(enemy);
        }
    }

    private void SpawnEnemy(GameObject enemy)
    {
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
    }
}
