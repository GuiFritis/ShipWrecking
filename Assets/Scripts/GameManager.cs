using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Screens;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player player;
    [Header("Enemies")]
    public Vector2 enemySpawnPerimeter = Vector2.one;
    public List<EnemyBase> enemies = new List<EnemyBase>();
    public LayerMask overlapingEnemiesLayers;
    public int maxEnemiesSpawned = 6;
    public SOFloat timeToSpawnEnemy;
    [Space]
    public SOInt roundDuration;
    public SOFloat roundTime;
    public SOInt score;
    [Space]
    public InputActionReference menuKey;

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

        menuKey.action.Enable();
        menuKey.action.performed += ctx => CallMenu();

        score.Value = 0;
        roundTime.Value = 0f;
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
        StartGame();
    }

    private void StartGame()
    {
        player.ship.health.OnDeath += PlayerDied;
        StartCoroutine(EnemySpawning());
    }

    void Update()
    {
        CheckEndGame();
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
        CallEndGame();
    }

    #region END_GAME
    private void CheckEndGame()
    {
        roundTime.Value += Time.deltaTime;
        if(roundTime.Value >= roundDuration.Value)
        {
            CallEndGame();
        }
    }

    private void CallEndGame()
    {
        Save.SaveManager.Instance.SaveScore();
        menuKey.action.Disable();
        PauseGame();
        ScreenController.Instance.HideAllScreens();
        ScreenController.Instance.ShowScreen(GameplayScreenType.GAME_OVER);
    }
    #endregion

    #region ENEMY_SPAWNING
    IEnumerator EnemySpawning()
    {
        while(player.ship.health.GetCurHealth() > 0)
        {
            yield return new WaitForSeconds(timeToSpawnEnemy.Value);
            SpawnEnemy();
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
