using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // 총알 이동 속도
    public float speed = 10f;

    // 적에게 주는 피해량
    public int damage = 10;

    void Update()
    {
        // 매 프레임 위쪽으로 이동
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // 화면 밖으로 나가면 삭제
        DestroyIfOutOfScreen();
    }

    private void DestroyIfOutOfScreen()
    {
        // WorldToViewportPoint = 월드 좌표를 뷰포트(0~1) 좌표로 변환
        // 0~1 범위를 벗어나면 화면 밖
        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);

        if (vp.x < 0 || vp.x > 1 || vp.y < 0 || vp.y > 1)
            Destroy(gameObject);
    }
}