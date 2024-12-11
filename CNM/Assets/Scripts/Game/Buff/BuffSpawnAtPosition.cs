using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpawnAtPosition : MonoBehaviour
{
    // Các vị trí cố định để spawn Buff
    public Transform[] spawnPositions;

    // Phương thức để trả về danh sách vị trí spawn
    public Transform[] GetSpawnPositions()
    {
        return spawnPositions;
    }
}
