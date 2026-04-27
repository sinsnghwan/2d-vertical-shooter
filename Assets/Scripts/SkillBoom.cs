using UnityEngine;

public class SkillBoom : MonoBehaviour
{
    // 폭탄 지속 시간
    public float duration = 2f;

    private bool _isUsed = false; // 적 제거가 이미 실행됐는지 체크

    void Start()
    {
        // 폭탄 시작하자마자 모든 적과 적 총알 제거
        if (!_isUsed)
        {
            _isUsed = true;
            RemoveAllEnemies();
        }

        // duration초 후에 자기 자신 삭제
        Destroy(gameObject, duration);
    }

    private void RemoveAllEnemies()
    {
        // Tag가 Enemy인 모든 오브젝트 찾아서 삭제
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
            Destroy(enemy);

        // Tag가 EnemyBullet인 모든 오브젝트 찾아서 삭제
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (GameObject bullet in bullets)
            Destroy(bullet);
    }
}