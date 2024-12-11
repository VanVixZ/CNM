using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetBuff : MonoBehaviour
{
    public float duration = 7f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (playerController != null)
            {
                FindObjectOfType<AudioManager>().PlaySound("PickUp");
                 // Kiểm tra khoảng cách giữa người chơi và buff Magnet
                float distance = Vector3.Distance(transform.position, other.transform.position);

                // Chỉ cho phép kích hoạt buff nếu player ở gần
                if (distance <= 5f) // Kiểm tra phạm vi xung quanh buff
                {
                    FindObjectOfType<AudioManager>().PlaySound("PickUp");
                    playerController.StartMagnet(duration);
                }
            }

            gameObject.SetActive(false);
        }
    }
    void Update()
    {
        transform.Rotate(0, 300 * Time.deltaTime, 0);
    }
}

