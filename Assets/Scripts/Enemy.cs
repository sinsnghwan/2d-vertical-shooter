using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 적 타입 열거형
    public enum EnemyType { A, B, C }
    public EnemyType enemyType;

    public float speed = 2f;
    public int health = 30;
    public Sprite[] sprites;

    // C타입 총알 프리팹
    public GameObject bulletPrefab;

    // C타입 총알 발사 위치
    public Transform firePoint;

    private SpriteRenderer sr;
    private Vector3 dir;
    private bool isMove = false;
    private float delta = 0;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isMove)
        {
            transform.Translate(dir * speed * Time.deltaTime, Space.World);

            // C타입만 1초마다 플레이어 향해 총 발사
            if (enemyType == EnemyType.C)
            {
                delta += Time.deltaTime;
                if (delta > 1f) { Fire(); delta = 0; }
            }
        }

        // 경계 밖으로 나가면 삭제
        if (AreaDrawer.Instance != null && AreaDrawer.Instance.IsOutOfBounds(transform.position))
            Destroy(gameObject);
    }

    // C타입: 플레이어를 향해 총알 2발 발사
    private void Fire()
    {
        var playerGo = GameObject.Find("Player");
        if (playerGo == null) return;

        // 플레이어를 향하는 방향벡터
        Vector3 dir = (playerGo.transform.position - transform.position).normalized;

        // 총알 1 (왼쪽 오프셋)
        GameObject b1 = Instantiate(bulletPrefab);
        b1.transform.position = firePoint.position + new Vector3(-0.36f, 0, 0);
        b1.GetComponent<EnemyBullet>().StartMove(dir);

        // 총알 2 (오른쪽 오프셋)
        GameObject b2 = Instantiate(bulletPrefab);
        b2.transform.position = firePoint.position + new Vector3(0.36f, 0, 0);
        b2.GetComponent<EnemyBullet>().StartMove(dir);
    }

    // 외부에서 호출: 이동 방향 설정 후 이동 시작
    public void StartMove(Vector3 dir)
    {
        this.dir = dir;
        DrawArrow.ForDebug2D(transform.position, dir, 10f, Color.red);
        isMove = true;
    }

    private void Hit(int damage)
    {
        health -= damage;
        sr.sprite = sprites[1];
        Invoke("ReturnDefaultSprite", 0.1f);

        if (health <= 0)
        {
            // 적 타입에 따라 점수 추가
            if (UIManager.Instance != null)
                UIManager.Instance.AddScoreByEnemyType(enemyType);

            Destroy(gameObject);
        }
    }

    private void ReturnDefaultSprite()
    {
        if (sr != null) sr.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Hit(other.GetComponent<PlayerBullet>().damage);
            Destroy(other.gameObject);
        }
    }
}