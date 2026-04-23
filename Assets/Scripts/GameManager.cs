using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 적 프리팹 배열
    public GameObject[] enemies;

    // 위에서 아래로 내려오는 생성 위치들
    public Transform[] spawnPoints;

    // 사이드에서 오는 EnemySpawner들
    public EnemySpawner[] spawners;

    private float delta = 0;
    private int span = 0;

    void Update()
    {
        delta += Time.deltaTime;

        if (delta > span)
        {
            CreateEnemy();
            delta = 0;
            span = Random.Range(1, 4);
        }
    }

    private void CreateEnemy()
    {
        // 랜덤 프리팹 선택
        GameObject prefab = enemies[Random.Range(0, enemies.Length)];
        GameObject enemyGo = Instantiate(prefab);
        var enemy = enemyGo.GetComponent<Enemy>();

        // 0 = 위에서 아래로 / 1 = 사이드에서
        int dice = Random.Range(0, 2);

        if (dice == 0)
        {
            // 위에서 아래로
            Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            enemyGo.transform.position = sp.position;
            enemy.StartMove(Vector2.down);
        }
        else
        {
            // 사이드에서
            EnemySpawner spawner = spawners[Random.Range(0, spawners.Length)];
            enemyGo.transform.position = spawner.startPoint.position;
            enemy.StartMove(spawner.GetDir());
        }
    }
}