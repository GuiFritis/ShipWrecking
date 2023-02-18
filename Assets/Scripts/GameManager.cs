using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Screens;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player player;
    public Vector2 enemySpawnPerimeter = Vector2.one;
    [Header("Enemies")]
    public List<EnemyBase> enemies = new List<EnemyBase>();
    public LayerMask overlapingEnemiesLayers;
    public int maxEnemiesSpawned = 6;
    public SOFloat timeToSpawnEnemy;
    public SOInt roundDuration;

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

    private void CallMenu()
    {
        if(Time.timeScale == 0)
        {
            UnpauseGame();
        }
        else
        {
            //pause
            PauseGame();
            ScreenController.Instance.ShowScreen(GameplayScreenType.MENU, true);
        }
    }

    private void PlayerDied(HealthBase hp)
    {

    }

    #region END_GAME
    private IEnumerator CountRound()
    {
        float roundTimer = 0f;
        while(roundTimer < roundDuration.Value)
        {
            roundTimer ++;
            yield return new WaitForSeconds(1f);
        }
    }
    #endregion

    #region ENEMY_SPAWNING
    IEnumerator EnemySpawning()
    {
        while(player.ship.health.GetCurHealth() > 0)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeToSpawnEnemy.Value);
        }
    }

    private void SpawnEnemy()
    {
        if(_enemiesSpawned >= maxEnemiesSpawned) return;

        _spawnPosition = GetRandomPointInPerimeter();

        while(Physics2D.OverlapPoint(_spawnPosition, overlapingEnemiesLayers) != null)
        {
            _spawnPosition = GetRandomPointInPerimeter();
        }

        _enemy = Instantiate(enemies[Random.Range(0, enemies.Count)], _spawnPosition, Quaternion.identity);
        _enemiesSpawned++;

        _enemy.player = player;
        _enemy.health.OnDeath += hp => _enemiesSpawned--;
    }

    private Vector2 GetRandomPointInPerimeter()
    {
        Vector2 point = (Vector2)player.transform.position;
        if(Random.Range(0, 2) == 0)
        {
            point.x += enemySpawnPerimeter.x/2 * (Random.Range(0, 2)==0?1:-1);
            point.y += Random.Range(0, enemySpawnPerimeter.y/2) * (Random.Range(0, 2)==0?1:-1);
        }
        else
        {
            point.x += Random.Range(0, enemySpawnPerimeter.x/2) * (Random.Range(0, 2)==0?1:-1);
            point.y += enemySpawnPerimeter.y/2 * (Random.Range(0, 2)==0?1:-1);
        }
        return point;
    }
    #endregion   

    #region PAUSE
    public void PauseGame()
    {
        Time.timeScale = 0;
        player.Pause();
    }

    public void UnpauseGame()
    {
        //unpause
        Time.timeScale = 1;
        player.Unpause();
        ScreenController.Instance.ShowScreen(GameplayScreenType.MENU, false);
    }
    #endregion

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(player.transform.position, enemySpawnPerimeter);
    }
}
