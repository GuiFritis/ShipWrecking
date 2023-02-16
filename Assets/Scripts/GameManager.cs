using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player player;
    public Vector2 enemySpawnPerimeter = Vector2.one;
    [Header("Enemies")]
    public List<EnemyBase> enemies = new List<EnemyBase>();
    public LayerMask overlapingEnemiesLayers;
    public int maxEnemiesSpawned = 6;
    public float timeToSpawnEnemy = 2f;

    private Vector2 _spawnPosition = Vector2.one;
    private EnemyBase _enemy;
    private int _enemiesSpawned = 0;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void OnValidate()
    {
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        catch{}
    }

    void Start()
    {
        player.ship.health.OnDeath += PlayerDied;
        StartCoroutine(EnemySpawning());
    }

    IEnumerator EnemySpawning()
    {
        while(player.ship.health.GetCurHealth() > 0)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeToSpawnEnemy);
        }
    }

    private void SpawnEnemy()
    {
        if(_enemiesSpawned >= maxEnemiesSpawned) return;

        _spawnPosition = (Vector2)player.transform.position;
        if(Random.Range(0, 2) == 0)
        {
            _spawnPosition.x += enemySpawnPerimeter.x/2 * (Random.Range(0, 2)==0?1:-1);
            _spawnPosition.y += Random.Range(0, enemySpawnPerimeter.y/2) * (Random.Range(0, 2)==0?1:-1);
        }
        else
        {
            _spawnPosition.x += Random.Range(0, enemySpawnPerimeter.x/2) * (Random.Range(0, 2)==0?1:-1);
            _spawnPosition.y += enemySpawnPerimeter.y/2 * (Random.Range(0, 2)==0?1:-1);
        }
        if(Physics2D.OverlapPoint(_spawnPosition, overlapingEnemiesLayers) == null)
        {
            _enemy = Instantiate(enemies[Random.Range(0, enemies.Count-1)], _spawnPosition, Quaternion.identity);
            _enemiesSpawned++;

            _enemy.player = player;
            _enemy.health.OnDeath += hp => _enemiesSpawned--;
        }
    }

    private void PlayerDied(HealthBase hp)
    {

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(player.transform.position, enemySpawnPerimeter);
    }
}
