using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // 적이 왼쪽에서 오는지 오른쪽에서 오는지 구분
    public enum Direction { LEFT, RIGHT }

    // 적 생성 위치 (부모 자식으로 설정)
    public Transform startPoint;

    // 이동 방향의 목표 위치
    public Transform endPoint;

    // 좌/우 구분
    public Direction direction;

    // true면 단위벡터, false면 원래 벡터
    public bool normalized;

    // 이동 방향 벡터 반환
    public Vector3 GetDir()
    {
        if (normalized)
            // normalized = 방향만 남기고 길이를 1로 통일
            return (endPoint.position - startPoint.position).normalized;
        else
            return endPoint.position - startPoint.position;
    }
}