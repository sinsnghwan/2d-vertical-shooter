# SpaceShooter

Unity 6로 만든 **2D 종스크롤 슈팅** 게임입니다.  
적을 격파하고 아이템을 모으며 파워를 올리고, 한정된 **붐(스크린 클리어)** 으로 위기를 넘기세요.

<p align="center">
  <strong>장르</strong> · Vertical Shooter &nbsp;·&nbsp;
  <strong>엔진</strong> · Unity 6000.4.1f1 &nbsp;·&nbsp;
  <strong>렌더</strong> · URP 17.4.0
</p>

---

## 목차

1. [특징](#특징)
2. [조작](#조작)
3. [게임 시스템](#게임-시스템)
4. [프로젝트 구조](#프로젝트-구조)
5. [스크립트 개요](#스크립트-개요)
6. [개발 환경](#개발-환경)
7. [클론 및 실행](#클론-및-실행)
8. [버전 관리 범위](#버전-관리-범위)
9. [크레딧](#크레딧)

---

## 특징

| 영역 | 설명 |
|------|------|
| **플레이** | WASD/방향키 이동, 마우스 좌클릭 홀드 연사 |
| **적** | A/B/C 타입 — C는 주기적으로 플레이어 방향으로 탄 발사 |
| **스폰** | 상단 랜덤 위치 또는 좌우 `EnemySpawner`에서 진입 |
| **아이템** | 적 처치 시 확률 드롭 — 코인 / 파워 / 붐 충전 |
| **붐** | 마우스 우클릭 — 화면 내 적·적탄 일괄 제거 (최대 3회 충전 UI) |
| **UI** | 라이프, 점수(천 단위 콤마), 게임 오버·재시도 |
| **배경** | `BackgroundScroller`로 세로 스크롤 연출 |

---

## 조작

| 입력 | 동작 |
|------|------|
| `WASD` / 방향키 | 플레이어 이동 |
| 마우스 **좌클릭** (홀드) | `fireRate` 간격 연사 |
| 마우스 **우클릭** | 붐 사용 (보유 시에만, `SkillBoom` 생성) |

---

## 게임 시스템

### 라이프 · 리스폰 · 게임 오버

- 적 또는 적 총알에 피격 시 `UIManager.HandlePlayerHit` 호출.
- 라이프가 남으면 일정 시간 후 시작 위치에 리스폰, 없으면 게임 오버 패널 표시 후 **Retry**로 씬 재로드.

### 파워(탄막)

| 단계 | 패턴 |
|:----:|------|
| 1 | 중앙 단발 |
| 2 | 좌·우 2발 |
| 3 | 중앙 강화탄 + 좌우 2발 |

- 파워 아이템 획득 시 점수 보너스와 함께 `power` 증가 (최대 3).

### 점수

- 적 타입별: A **100** · B **200** · C **300**
- 코인 아이템: **+1000**
- 파워·붐 아이템: 각 **+500** (코드 기준 `Player.cs` / `UIManager.cs`)

### 아이템 드롭 (`ItemManager`)

랜덤 `0~9`에 따른 대략적 비율:

| 구간 | 결과 |
|:----:|------|
| 0–2 | 없음 (~30%) |
| 3–5 | 코인 (~30%) |
| 6–7 | 파워 (~20%) |
| 8–9 | 붐 충전 (~20%) |

### 붐 (`SkillBoom`)

- `Enemy`, `EnemyBullet` 태그 오브젝트를 찾아 제거 후 일정 시간 뒤 자기 자신 파괴.

---

## 프로젝트 구조

```text
SpaceShooter/
├── Assets/
│   ├── Animations/          # 플레이어·아이템·스킬 붐 애니메이션
│   ├── Font/                # UI 폰트 (TMP SDF 등)
│   ├── Prefabs/
│   │   ├── Enemy/           # Enemy A/B/C, EnemyBullet A/C
│   │   ├── Item/            # Item_Coin, Item_Power, Item_Boom
│   │   └── SkillBoom.prefab
│   ├── Scenes/
│   │   └── GameScene.unity  # 메인 플레이 씬
│   ├── Scripts/             # 게임 로직 (C#)
│   ├── Settings/            # URP 등
│   └── …                    # 스프라이트·에셋팩 등
├── Packages/
│   └── manifest.json
├── ProjectSettings/
├── .gitignore
└── README.md
```

---

## 스크립트 개요

| 파일 | 역할 |
|------|------|
| `Player.cs` | 이동, 애니메이션 상태, 연사, 피격, 아이템 픽업, 붐 발동 |
| `Enemy.cs` | 타입별 이동·체력·C형 탄 발사, 경계 이탈 시 제거 |
| `EnemyBullet.cs` | 적 탄 이동 |
| `EnemySpawner.cs` | 사이드 진입용 시작/끝 지점·방향 |
| `GameManager.cs` | 주기적 적 생성 (상단 vs 스포너 랜덤) |
| `PlayerBullet.cs` | 플레이어 탄 이동·데미지 |
| `UIManager.cs` | 싱글톤 — 라이프, 점수, 붐 UI, 게임오버·리트라이 |
| `AreaDrawer.cs` | 싱글톤 — 플레이 영역 경계 |
| `Item.cs` | 아이템 하강 이동·경계 밖 제거 |
| `ItemManager.cs` | 싱글톤 — 적 사망 위치에 아이템 스폰 |
| `SkillBoom.cs` | 붐 발동 시 적·적탄 일괄 제거 |
| `BackgroundScroller.cs` | 배경 세로 스크롤·루프 |
| `DrawArrow.cs` | 에디터/디버그용 방향 화살표 |
| `Test.cs` | 스포너 방향 등 테스트용 |

---

## 개발 환경

- **Unity Editor:** `6000.4.1f1` (Unity 6)
- **렌더 파이프라인:** Universal RP `17.4.0`
- **입력:** Legacy `Input` API (`Input.GetAxisRaw`, `GetMouseButton` 등) — 프로젝트에 **Input System** 패키지는 포함되어 있으나 본 게임플레이 스크립트는 레거시 입력 사용.

### 주요 패키지 (manifest 기준)

- 2D Animation, Sprite, Tilemap, Aseprite Importer, PSD Importer  
- **Input System** `1.19.0`  
- **URP** `17.4.0`  
- TextMeshPro (UGUI 경로), Timeline, Visual Scripting 등

권장 IDE: **Rider**, Visual Studio, VS Code (Unity 연동 확장).

---

## 클론 및 실행

원격 저장소 이름은 GitHub에서 `2d-vertical-shooter`로 호스팅될 수 있습니다. 로컬 폴더명은 **SpaceShooter**입니다.

```bash
git clone https://github.com/sinsnghwan/2d-vertical-shooter.git
cd 2d-vertical-shooter   # 또는 클론 시 지정한 폴더명
```

1. **Unity Hub**에서 해당 폴더를 추가하고 **6000.4.1f1**으로 엽니다.  
2. `Assets/Scenes/GameScene.unity` 를 연 뒤 **Play** 로 실행합니다.  
3. 최초 열기 시 패키지 복원·셰이더 컴파일로 시간이 걸릴 수 있습니다.

---

## 버전 관리 범위

이 저장소는 일반적인 Unity **`.gitignore`** 를 따릅니다. 아래는 **커밋되지 않습니다** (용량·기생성 파일).

| 제외 | 이유 |
|------|------|
| `Library/` | 로컬 캐시·임포트 결과 |
| `Temp/`, `Logs/` | 빌드·에디터 임시 |
| `obj/`, `UserSettings/` | 사용자/빌드 산출물 |
| `*.csproj`, `*.sln` | IDE/Unity가 재생성 |

팀원은 동일 Unity 버전으로 열면 `Library`가 자동으로 다시 생성됩니다.

---

## 크레딧

- **SpaceShooter** — 학습·포트폴리오용 2D 슈팅 프로젝트  
- 스프라이트·에셋: `Assets` 내 **Vertical 2D Shooting BE4** 등 번들 에셋 사용  
- UI: **TextMesh Pro**, 커스텀 폰트 에셋

---

<p align="center">
  <sub>README는 저장소 초기화 커밋 기준 프로젝트 상태를 반영합니다.</sub>
</p>
