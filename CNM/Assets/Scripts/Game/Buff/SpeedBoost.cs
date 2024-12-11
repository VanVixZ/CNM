using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float speedMultiplier = 2f; // Tăng tốc độ gấp 2 lần
    public float duration = 5f;       // Thời gian hiệu lực của buff

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (playerController != null)
            {
                FindObjectOfType<AudioManager>().PlaySound("PickUp");
                playerController.StartSpeedBoost(speedMultiplier, duration);
            }

            // Hiệu ứng hình ảnh hoặc âm thanh (nếu có)
            GameObject effect = ObjectPool.instance.GetPooledObject();
            if (effect != null)
            {
                effect.transform.position = transform.position;
                effect.SetActive(true);
            }

            // Ẩn hoặc hủy vật phẩm sau khi chạm
            gameObject.SetActive(false);
        }
    }
    void Update()
    {
        transform.Rotate( 0, 300 * Time.deltaTime, 0);
    }
}
