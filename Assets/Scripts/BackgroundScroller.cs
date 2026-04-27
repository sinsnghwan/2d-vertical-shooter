using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    // 배경 그룹의 스크롤 속도
    public float scrollSpeed = 2f;

    // 배경 이미지 높이 (재사용 기준)
    private float _height;

    // 시작 위치 저장
    private Vector3 _startPos;

    void Start()
    {
        _startPos = transform.position;

        // 첫번째 자식의 SpriteRenderer로 높이 계산
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
            _height = sr.bounds.size.y;
    }

    void Update()
    {
        // 아래로 이동
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // 배경 높이 * 3 아래로 내려가면 시작 위치로 복귀
        if (transform.position.y <= _startPos.y - _height)
        {
            transform.position = _startPos;
        }
    }
}