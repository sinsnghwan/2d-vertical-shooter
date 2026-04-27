using UnityEngine;

public class AreaDrawer : MonoBehaviour
{
    // 싱글톤: 어디서든 AreaDrawer.Instance로 접근
    public static AreaDrawer Instance { get; private set; }

    // 화면 네 모서리 포인트
    public Transform topLeft;
    public Transform topRight;
    public Transform bottomLeft;
    public Transform bottomRight;

    void Awake()
    {
        Instance = this;
    }

    // position이 경계 밖인지 확인
    // true = 경계 밖 → 삭제 신호
    public bool IsOutOfBounds(Vector3 position)
    {
        if (topLeft == null || topRight == null || 
            bottomLeft == null || bottomRight == null)
            return false;

        float minX = Mathf.Min(topLeft.position.x, bottomLeft.position.x);
        float maxX = Mathf.Max(topRight.position.x, bottomRight.position.x);
        float minY = Mathf.Min(bottomLeft.position.y, bottomRight.position.y);
        float maxY = Mathf.Max(topLeft.position.y, topRight.position.y);

        return position.x < minX || position.x > maxX ||
               position.y < minY || position.y > maxY;
    }

    // Scene 뷰에서 초록 테두리로 경계 시각화
    void OnDrawGizmos()
    {
        if (topLeft == null || topRight == null || 
            bottomLeft == null || bottomRight == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(topLeft.position, topRight.position);
        Gizmos.DrawLine(topRight.position, bottomRight.position);
        Gizmos.DrawLine(bottomRight.position, bottomLeft.position);
        Gizmos.DrawLine(bottomLeft.position, topLeft.position);
    }
}