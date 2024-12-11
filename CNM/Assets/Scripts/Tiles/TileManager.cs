using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private List<GameObject> activeTiles;
    public GameObject[] tilePrefabs; // Các prefab tile
    public float tileLength = 30;
    public int numberOfTiles = 3;
    public int totalNumOfTiles = 8;

    public float zSpawn = 0;

    private Transform playerTransform;
    private int previousIndex;

    public GameObject[] buffPrefabs; // Các prefab buff
    public float spawnChance = 0.2f; // Xác suất spawn buff

    void Start()
    {
        activeTiles = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Spawn các tile ban đầu
        for (int i = 0; i < numberOfTiles; i++)
        {
            if (i == 0)
                SpawnTile();
            else
                SpawnTile(Random.Range(0, totalNumOfTiles));
        }
    }

    void Update()
    {
        // Kiểm tra khoảng cách player với tile cuối, tạo tile mới nếu cần
        if (playerTransform.position.z - 30 >= zSpawn - (numberOfTiles * tileLength))
        {
            int index = Random.Range(0, totalNumOfTiles);
            while (index == previousIndex)
                index = Random.Range(0, totalNumOfTiles);

            DeleteTile();
            SpawnTile(index);
        }
    }

    public void SpawnTile(int index = 0)
    {
        // Spawn một tile mới
        GameObject tile = tilePrefabs[index];
        if (tile.activeInHierarchy)
            tile = tilePrefabs[index + 8];

        if (tile.activeInHierarchy)
            tile = tilePrefabs[index + 16];

        tile.transform.position = Vector3.forward * zSpawn;
        tile.transform.rotation = Quaternion.identity;
        tile.SetActive(true);

        // Kích hoạt lại các obstacle trong tile
        foreach (Transform child in tile.transform)
        {
            if (child.CompareTag("Obstacle"))
            {
                child.gameObject.SetActive(true); // Đảm bảo các obstacle được kích hoạt lại
            }
           
        }

        
        // Tự động spawn buff trong tile
        SpawnBuffsInTile(tile);

        activeTiles.Add(tile);
        zSpawn += tileLength;
        previousIndex = index;
    }

    private void DeleteTile()
    {
        // Xóa tile đầu tiên trong danh sách
        activeTiles[0].SetActive(false);
        activeTiles.RemoveAt(0);
        PlayerManager.score += 3;
    }

    private void SpawnBuffsInTile(GameObject tile)
    {
        // Lấy danh sách các vị trí spawn từ BuffSpawnAtPosition
        BuffSpawnAtPosition buffSpawnManager = tile.GetComponent<BuffSpawnAtPosition>();
        if (buffSpawnManager != null)
        {
            Transform[] spawnPositions = buffSpawnManager.GetSpawnPositions();

            // Duyệt qua các vị trí spawn và spawn buff theo xác suất
            foreach (Transform spawnPosition in spawnPositions)
            {
                if (Random.value <= spawnChance) // Kiểm tra xác suất spawn buff
                {
                    SpawnBuffAtPosition(spawnPosition.position);
                }
            }
        }
    }

    private void SpawnBuffAtPosition(Vector3 position)
    {
        // Spawn buff tại vị trí cụ thể
        if (buffPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, buffPrefabs.Length); // Chọn ngẫu nhiên một loại buff
            Instantiate(buffPrefabs[randomIndex], position, Quaternion.Euler(0, 180, 0));
        }
        else
        {
            Debug.LogError("Không có buff Prefab nào được gán!");
        }
    }
}
