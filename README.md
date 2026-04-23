# 🚀 2D Vertical Space Shooter

Unity로 제작한 2D 종스크롤 슈팅 게임입니다.  
플레이어 전투기를 조종해 다양한 유형의 적을 격파하고 최고 점수를 노리세요.

---

## 📋 목차

- [프로젝트 개요](#-프로젝트-개요)
- [개발 환경](#-개발-환경)
- [게임플레이](#-게임플레이)
- [프로젝트 구조](#-프로젝트-구조)
- [스크립트 설명](#-스크립트-설명)
- [적 타입 & 점수 시스템](#-적-타입--점수-시스템)
- [플레이어 파워 시스템](#-플레이어-파워-시스템)
- [UI 시스템](#-ui-시스템)
- [설치 및 실행](#-설치-및-실행)

---

## 🎮 프로젝트 개요

| 항목 | 내용 |
|------|------|
| 장르 | 2D 종스크롤 슈팅 (Vertical Shooter) |
| 엔진 | Unity 6000.4.1f1 (Unity 6) |
| 렌더 파이프라인 | Universal Render Pipeline (URP) 17.4.0 |
| 개발 언어 | C# |
| 플랫폼 | PC (Android 빌드 설정 포함) |

---

## 🛠 개발 환경

### Unity 버전
- **Unity 6000.4.1f1** (Unity 6)

### 주요 패키지
| 패키지 | 버전 | 용도 |
|--------|------|------|
| Universal Render Pipeline | 17.4.0 | 렌더링 |
| 2D Animation | 14.0.3 | 2D 애니메이션 |
| 2D Sprite | 1.0.0 | 스프라이트 |
| 2D Tilemap | 1.0.0 | 타일맵 |
| Input System | 1.19.0 | 입력 처리 |
| TextMesh Pro | (내장) | UI 텍스트 |
| 2D Aseprite | 4.0.1 | 픽셀아트 애니메이션 임포트 |
| 2D PSD Importer | 13.0.2 | PSD 파일 임포트 |

### 권장 IDE
- JetBrains Rider
- Visual Studio
- Visual Studio Code

---

## 🕹 게임플레이

### 조작 방법

| 입력 | 동작 |
|------|------|
| `WASD` / 방향키 | 플레이어 이동 |
| `마우스 좌클릭 (홀드)` | 총알 연속 발사 |

### 게임 규칙
- 적 또는 적 총알에 맞으면 **목숨 1개 감소**
- 목숨 3개에서 시작하며, 모두 잃으면 **GAME OVER**
- 피격 후 **2초 뒤 초기 위치에 리스폰**
- 적을 격파할수록 점수 획득 (천 단위 콤마 표시)
- GAME OVER 시 **Retry 버튼**으로 재시작 가능

---

## 📁 프로젝트 구조

```
SpaceShooter/
├── Assets/
│   ├── Animations/              # 플레이어 애니메이션
│   │   ├── Idle.anim            # 기본 상태 애니메이션
│   │   ├── Left.anim            # 왼쪽 이동 애니메이션
│   │   ├── Right.anim           # 오른쪽 이동 애니메이션
│   │   └── Player.controller   # 애니메이터 컨트롤러
│   ├── Font/                    # 커스텀 폰트 (determination)
│   ├── Prefabs/                 # 프리팹
│   │   ├── Enemy A.prefab       # 적 타입 A
│   │   ├── Enemy B.prefab       # 적 타입 B
│   │   ├── Enemy C.prefab       # 적 타입 C (총알 발사)
│   │   ├── EnemyBullet_A.prefab # 적 총알 (A타입용)
│   │   ├── EnemyBullet_C.prefab # 적 총알 (C타입용)
│   │   ├── PlayerBullet.prefab  # 플레이어 기본 총알
│   │   └── PlayerBullet_1.prefab# 플레이어 강화 총알 (power 3)
│   ├── Scenes/
│   │   └── GameScene.unity      # 메인 게임 씬
│   ├── Scripts/                 # C# 스크립트
│   │   ├── Player.cs            # 플레이어 이동 & 발사 & 피격
│   │   ├── Enemy.cs             # 적 이동 & 공격 & 피격
│   │   ├── EnemyBullet.cs       # 적 총알 이동
│   │   ├── EnemySpawner.cs      # 사이드 적 생성기
│   │   ├── GameManager.cs       # 게임 흐름 관리 (적 랜덤 생성)
│   │   ├── UIManager.cs         # UI (목숨, 점수, 게임오버)
│   │   ├── AreaDrawer.cs        # 게임 경계 관리
│   │   ├── PlayerBullet.cs      # 플레이어 총알 이동 & 데미지
│   │   ├── DrawArrow.cs         # 디버그용 화살표 그리기
│   │   └── Test.cs              # 스포너 방향 디버그 테스트
│   ├── Settings/                # URP 렌더링 설정
│   ├── TextMesh Pro/            # TMP 리소스
│   └── Vertical 2D Shooting BE4/# 에셋팩 스프라이트 리소스
├── Packages/
│   └── manifest.json            # 패키지 의존성 목록
├── ProjectSettings/             # Unity 프로젝트 설정
└── .gitignore                   # Unity 전용 gitignore
```

---

## 📝 스크립트 설명

### `Player.cs` — 플레이어 컨트롤러

플레이어의 **이동**, **발사**, **피격 처리**를 담당합니다.

```
주요 변수:
- speed (5f)          : 이동 속도
- power (1~3)         : 발사 패턴 단계
- fireRate (0.1f)     : 발사 간격 (초)
- sideOffset (0.25f)  : 좌우 총알 간격
- respawnDelay (2f)   : 피격 후 리스폰 대기 시간
```

**동작 흐름:**
1. `WASD` / 방향키로 이동, 화면 경계 클램프 처리
2. 이동 방향에 따라 Idle / Left / Right 애니메이션 전환
3. 마우스 좌클릭 홀드 시 `fireRate` 간격으로 `Fire()` 호출
4. 적 또는 적 총알과 충돌 시 `UIManager.HandlePlayerHit()` 호출

---

### `Enemy.cs` — 적 컨트롤러

3가지 적 타입(A, B, C)의 **이동**, **피격**, **반격**을 담당합니다.

```
타입별 동작:
- A타입: 단순 직선 이동
- B타입: 단순 직선 이동 (체력·속도 다름)
- C타입: 직선 이동 + 1초마다 플레이어 향해 총알 2발 발사
```

**Hit 처리:**
- 플레이어 총알에 맞으면 `damage` 만큼 체력 감소
- 피격 시 0.1초간 피격 스프라이트로 전환 후 원래 스프라이트 복귀
- 체력이 0 이하가 되면 점수 추가 후 `Destroy()`

---

### `EnemyBullet.cs` — 적 총알

`StartMove(dir)` 호출로 방향을 받아 해당 방향으로 일정 속도로 이동합니다.

```
speed (1f) : 이동 속도
```

---

### `PlayerBullet.cs` — 플레이어 총알

매 프레임 위쪽(`Vector3.up`)으로 이동하며, 화면 밖으로 나가면 자동 삭제됩니다.

```
speed  (10f) : 이동 속도
damage (10)  : 적에게 주는 데미지
```

---

### `GameManager.cs` — 게임 매니저

**1~3초 랜덤 간격**으로 적을 생성합니다.

```
적 생성 방식 (50% 확률로 선택):
- 위에서 아래 : spawnPoints 중 랜덤 위치에서 아래로 이동
- 사이드에서  : EnemySpawner 중 랜덤 스포너의 방향으로 이동
```

---

### `EnemySpawner.cs` — 사이드 스포너

화면 **좌/우측**에 배치되어 적이 비스듬히 진입하는 경로를 정의합니다.

```
startPoint : 적 생성 위치 Transform
endPoint   : 이동 목표 위치 Transform
normalized : 방향 벡터 정규화 여부
GetDir()   : endPoint - startPoint 방향 벡터 반환
```

---

### `UIManager.cs` — UI 매니저 (싱글톤)

**목숨 표시**, **점수 표시**, **게임오버 패널**, **리스폰** 처리를 담당합니다.

```
images[]      : 목숨 아이콘 Image 배열 (Alpha로 표시/숨김)
gameOverPanel : GAME OVER 패널 GameObject
retryButton   : Retry 버튼
scoreText     : TextMeshProUGUI 점수 텍스트
```

| `UIManager.Instance` 메서드 | 설명 |
|-----------------------------|------|
| `HandlePlayerHit()` | 목숨 감소 → 게임오버 or 리스폰 처리 |
| `DecreaseLife()` | 목숨 아이콘 Alpha를 0으로 변경, 마지막 목숨이면 게임오버 |
| `AddScoreByEnemyType()` | 적 타입별 점수 추가 후 텍스트 갱신 |
| `OnRetryButtonClick()` | 현재 씬 재로드 |

---

### `AreaDrawer.cs` — 경계 관리자 (싱글톤)

화면 네 모서리 Transform으로 게임 영역 경계를 정의합니다.

- `IsOutOfBounds(position)`: 해당 좌표가 경계 밖이면 `true` 반환
- Scene 뷰에서 **초록색 Gizmos 테두리**로 경계를 시각적으로 표시

---

### `DrawArrow.cs` — 디버그 화살표

`DrawArrow.ForDebug2D(pos, dir, length, color)` 로 Scene 뷰에 방향 화살표를 그립니다.  
개발 단계에서 스포너 방향 디버깅에 사용됩니다.

---

## ⚔️ 적 타입 & 점수 시스템

| 적 타입 | 이동 패턴 | 공격 | 획득 점수 |
|---------|-----------|------|-----------|
| **A** | 직선 이동 | 없음 | 100점 |
| **B** | 직선 이동 | 없음 | 200점 |
| **C** | 직선 이동 | 1초마다 플레이어 방향으로 총알 2발 | 300점 |

> 점수는 천 단위 콤마로 표시됩니다. (예: 1,500)

---

## 🔫 플레이어 파워 시스템

| 파워 단계 | 발사 패턴 | 총알 수 |
|-----------|-----------|---------|
| **Power 1** | 중앙 단발 | 1발 |
| **Power 2** | 좌우 2발 | 2발 |
| **Power 3** | 중앙 강화탄 + 좌우 2발 | 3발 |

- **Power 1, 2**: `PlayerBullet` (기본 총알) 사용
- **Power 3**: `PlayerBullet_1` (강화 총알, 중앙) + `PlayerBullet` (좌우) 사용

---

## 🎨 애니메이션

플레이어는 이동 방향에 따라 3가지 상태를 전환합니다.

| 상태 | 조건 | 애니메이션 |
|------|------|-----------|
| `Idle (0)` | 좌우 입력 없음 | 정면 |
| `Left (1)` | 왼쪽 이동 중 | 왼쪽 기울기 |
| `Right (2)` | 오른쪽 이동 중 | 오른쪽 기울기 |

Animator Parameter: `State` (int)

---

## 💻 설치 및 실행

### 요구 사항
- Unity **6000.4.1f1** 이상

### 실행 방법

1. 저장소 클론
   ```bash
   git clone https://github.com/sinsnghwan/2d-vertical-shooter.git
   ```

2. Unity Hub에서 프로젝트 열기
   - Unity Hub → `Add` → 클론된 폴더 선택

3. `Assets/Scenes/GameScene.unity` 씬 열기

4. Play 버튼 (`▶`) 클릭하여 실행

> 처음 열 때 패키지 임포트 및 셰이더 컴파일로 시간이 걸릴 수 있습니다.

---

## 📌 참고 에셋

- **Vertical 2D Shooting BE4** — 스프라이트 에셋팩 (배경, 적, 총알, 폭발 이펙트 등)
- **TextMesh Pro** — UI 텍스트 렌더링
- **determination 폰트** — 커스텀 게임 UI 폰트
