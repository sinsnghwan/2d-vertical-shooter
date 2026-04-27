using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;

    // power 1 = 단발 / power 2 = 좌우 2발 / power 3 = 중앙 강화탄 + 좌우 2발
    public int power = 1;

    // 기본 총알 프리팹
    public GameObject sideBulletPrefab;

    // 중앙 강화 총알 프리팹 (power 3 전용)
    public GameObject centerBulletPrefab;

    // 총알 생성 위치
    public Transform firePoint;

    // 발사 간격 (초)
    public float fireRate = 0.1f;

    // 좌우 총알 간격
    public float sideOffset = 0.25f;

    // 피격 후 리스폰까지 대기 시간
    [SerializeField] private float respawnDelay = 2f;

    private float _fireTimer;
    private Vector2 _spriteExtents;
    private Animator _animator;
    private Vector3 _startPosition;
    private bool _isHitProcessing;

    private const int StateIdle  = 0;
    private const int StateLeft  = 1;
    private const int StateRight = 2;
    // 폭탄 프리팹
    public GameObject skillBoomPrefab;

    void Start()
    {
        _startPosition = transform.position;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            _spriteExtents = sr.bounds.extents;

        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();

        // 마우스 누르고 있는 동안 fireRate 간격으로 발사
        if (Input.GetMouseButton(0))
        {
            _fireTimer += Time.deltaTime;
            if (_fireTimer >= fireRate)
            {
                Fire();
                _fireTimer = 0f;
            }
        }
        // 우클릭 시 폭탄 사용
        if (Input.GetMouseButtonDown(1))
        {
            if (UIManager.Instance != null && UIManager.Instance.UseBoom())
            {
                // SkillBoom 프리팹 생성
                Instantiate(skillBoomPrefab, Vector3.zero, Quaternion.identity);
            }
        }
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, v, 0).normalized;
        transform.Translate(dir * speed * Time.deltaTime);

        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, min.x + _spriteExtents.x, max.x - _spriteExtents.x);
        pos.y = Mathf.Clamp(pos.y, min.y + _spriteExtents.y, max.y - _spriteExtents.y);
        transform.position = pos;

        if (_animator != null)
        {
            if (h < 0)      _animator.SetInteger("State", StateLeft);
            else if (h > 0) _animator.SetInteger("State", StateRight);
            else            _animator.SetInteger("State", StateIdle);
        }
    }

    private void Fire()
    {
        switch (power)
        {
            case 1:
                // 중앙 단발
                SpawnBullet(sideBulletPrefab, Vector3.zero);
                break;

            case 2:
                // 좌우 2발
                SpawnBullet(sideBulletPrefab, Vector3.left  * 0.1f);
                SpawnBullet(sideBulletPrefab, Vector3.right * 0.1f);
                break;

            case 3:
                // 중앙 강화탄 + 좌우 2발
                SpawnBullet(centerBulletPrefab, Vector3.zero);
                SpawnBullet(sideBulletPrefab,   Vector3.left  * sideOffset);
                SpawnBullet(sideBulletPrefab,   Vector3.right * sideOffset);
                break;
        }
    }

    private void SpawnBullet(GameObject prefab, Vector3 offset)
    {
        // firePoint 기준으로 offset 위치에 총알 생성
        Instantiate(prefab, firePoint.position + offset, firePoint.rotation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyBullet"))
        {
            if (_isHitProcessing) return;
            _isHitProcessing = true;

            if (UIManager.Instance != null)
                UIManager.Instance.HandlePlayerHit(gameObject, _startPosition, respawnDelay);
        }
        // 아이템과 충돌
        if (other.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();
            if (item == null) return;

            switch (item.itemType)
            {
                case Item.ItemType.Coin:
                    // 코인 = 점수 +1000
                    if (UIManager.Instance != null)
                        UIManager.Instance.AddScore(1000);
                    break;

                case Item.ItemType.Power:
                    // 파워업 = 점수 +500, power 증가 (MAX 3)
                    if (UIManager.Instance != null)
                        UIManager.Instance.AddScore(500);
                    if (power < 3) power++;
                    break;

                case Item.ItemType.Boom:
                    // 붐 = 점수 +500, 붐 카운트 증가
                    if (UIManager.Instance != null)
                    {
                        UIManager.Instance.AddScore(500);
                        UIManager.Instance.AddBoom();
                    }
                    break;
            }

            item.StopMove();
            Destroy(other.gameObject);
        }
    }

    private void OnEnable()
    {
        _isHitProcessing = false;
    }
}