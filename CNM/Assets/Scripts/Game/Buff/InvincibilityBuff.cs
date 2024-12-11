using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityBuff : MonoBehaviour
{
    public float duration = 5f; // Thời gian hiệu lực

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (playerController != null)
            {
                FindObjectOfType<AudioManager>().PlaySound("PickUp");
                playerController.StartInvincibility(duration);
                
            }

            // Hiệu ứng hình ảnh/âm thanh (nếu có)
            GameObject effect = ObjectPool.instance.GetPooledObject();
            if (effect != null)     
            {
                effect.transform.position = transform.position  ;
                effect.SetActive(true);
            }

            // Ẩn hoặc hủy vật phẩm buff
            gameObject.SetActive(false);
        }
    }
    void Update()
    {
        transform.Rotate(0, 300 * Time.deltaTime, 0);
    }
}


