using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // 싱글톤: 어디서든 ItemManager.Instance로 접근
    public static ItemManager Instance;

    // 아이템 프리팹 배열 (Coin, Boom, Power)
    public GameObject[] items;

    void Awake()
    {
        Instance = this;
    }

    // 적이 죽은 위치에 랜덤 아이템 생성
    public void CreateItem(Vector3 pos)
    {
        // 0~9 랜덤 (0~2: None 30%, 3~5: Coin 30%, 6~7: Power 20%, 8~9: Boom 20%)
        int rand = Random.Range(0, 10);

        GameObject prefab = null;

        if (rand >= 3 && rand <= 5)
            prefab = items[0]; // Coin
        else if (rand >= 6 && rand <= 7)
            prefab = items[1]; // Power
        else if (rand >= 8 && rand <= 9)
            prefab = items[2]; // Boom
        else
            return; // None = 아이템 안 나옴

        GameObject go = Instantiate(prefab, pos, Quaternion.identity);
        Item item = go.GetComponent<Item>();
        if (item != null)
            item.BeginMove();
    }
}