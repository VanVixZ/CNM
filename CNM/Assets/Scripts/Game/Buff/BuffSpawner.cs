using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    public GameObject[] buffPrefabs; // Các prefab của buff (x2, bất tử, hút vật phẩm)
    public float spawnChance = 0.5f; // Xác suất spawn (50%)
    private float[] lanes = { -2.5f, 0f, 2.5f }; // Các vị trí đường nhỏ (trục x)
    private float minZ = -15f; // Giới hạn z min
    private float maxZ = 15f;  // Giới hạn z max

    void Start()
    {
        TrySpawnBuff();
    }

    public void TrySpawnBuff()
    {
        if (Random.value > spawnChance) return; // Không spawn nếu không đạt xác suất

        // Chọn ngẫu nhiên một loại buff
        int randomBuffIndex = Random.Range(0, buffPrefabs.Length);
        GameObject buff = Instantiate(buffPrefabs[randomBuffIndex]);

        // Chọn ngẫu nhiên vị trí hợp lệ
        float randomLane = lanes[Random.Range(0, lanes.Length)];
        float randomZ = Random.Range(minZ, maxZ);
        Vector3 spawnPosition = transform.position + new Vector3(randomLane, 0.5f, randomZ);

        Debug.Log("Spawning buff at position: " + spawnPosition); // Debug log

        // Đặt buff vào vị trí đã chọn
        buff.transform.position = spawnPosition;
        buff.transform.SetParent(transform); // Gắn buff vào tile
    }
}
