using UnityEngine;

public class Test : MonoBehaviour
{
    public EnemySpawner[] spawners;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < spawners.Length; i++)
            {
                var spawner = spawners[i];
                // startPoint에서 GetDir() 방향으로 화살표 표시
                DrawArrow.ForDebug2D(spawner.startPoint.position, spawner.GetDir(), 10f);
            }
        }
    }
}