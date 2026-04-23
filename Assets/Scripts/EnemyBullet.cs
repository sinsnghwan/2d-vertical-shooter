using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // 이동 방향
    private Vector3 dir;

    // 이동 속도
    public float speed = 1f;

    void Update()
    {
        // 설정된 방향으로 이동
        transform.Translate(dir * speed * Time.deltaTime);
    }

    // 외부에서 호출 : 이동 방향 설정
    public void StartMove(Vector3 dir)
    {
        this.dir = dir;
    }
}