using System.Collections;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 move;
    public float forwardSpeed;
    public float maxSpeed;

    private int desiredLane = 1;//0:left, 1:middle, 2:right
    public float laneDistance = 2.5f;//The distance between tow lanes

    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public float gravity = -12f;
    public float jumpHeight = 2;
    private Vector3 velocity;

    public Animator animator;
    private bool isSliding = false;

    public float slideDuration = 1.5f;

    bool toggle = false;


    // //////////////////BUFF
    private float originalForwardSpeed;
    private Coroutine speedBoostCoroutine;
    

    private bool isInvincible = false; // Trạng thái vô hình
    private Coroutine invincibilityCoroutine;
    public GameObject explosionEffect; // Tham chiếu đến hiệu ứng phá hủy
    private AudioSource invincibilitySound;

    private bool isMagnetActive = false;
    private Coroutine magnetCoroutine;
    private AudioSource magnetSound;
    // ////////////////////
    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalForwardSpeed = forwardSpeed; // Lưu tốc độ gốc
        Time.timeScale = 1.2f;

        // Lấy AudioSource cho buff Invincible
        invincibilitySound = FindObjectOfType<AudioManager>().GetAudioSource("Invincible");
        if (invincibilitySound == null)
        {
            Debug.LogWarning("Invincible chưa được cấu hình trong AudioManager.");
        }
        


        // Lấy AudioSource cho buff Magnet
        magnetSound = FindObjectOfType<AudioManager>().GetAudioSource("Magnet");
        if (magnetSound == null)
        {
            Debug.LogWarning("MagnetSound chưa được cấu hình trong AudioManager.");
        }
    }

    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
            return;

        //Increase Speed
        if (toggle)
        {
            toggle = false;
            if (forwardSpeed < maxSpeed)
                forwardSpeed += 0.1f * Time.fixedDeltaTime;
        }
        else
        {
            toggle = true;
            if (Time.timeScale < 2f)
                Time.timeScale += 0.005f * Time.fixedDeltaTime;
        }
    }

    void Update()
    {
        if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
            return;

        animator.SetBool("isGameStarted", true);
        move.z = forwardSpeed;

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.17f, groundLayer);
        animator.SetBool("isGrounded", isGrounded);
        if (isGrounded && velocity.y < 0)
            velocity.y = -1f;

        if (isGrounded)
        {
            if (SwipeManager.swipeUp)
                Jump();

            if (SwipeManager.swipeDown && !isSliding)
                StartCoroutine(Slide());
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
            if (SwipeManager.swipeDown && !isSliding)
            {
                StartCoroutine(Slide());
                velocity.y = -10;
            }                

        }
        controller.Move(velocity * Time.deltaTime);

        //Gather the inputs on which lane we should be
        if (SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }
        if (SwipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }

        //Calculate where we should be in the future
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (desiredLane == 0)
            targetPosition += Vector3.left * laneDistance;
        else if (desiredLane == 2)
            targetPosition += Vector3.right * laneDistance;

        //transform.position = targetPosition;
        if (transform.position != targetPosition)
        {
            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = diff.normalized * 30 * Time.deltaTime;
            if (moveDir.sqrMagnitude < diff.magnitude)
                controller.Move(moveDir);
            else
                controller.Move(diff);
        }

        controller.Move(move * Time.deltaTime);
    }

    private void Jump()
    {   
        StopCoroutine(Slide());
        animator.SetBool("isSliding", false);
        animator.SetTrigger("jump");
        controller.center = Vector3.zero;
        controller.height = 2;
        isSliding = false;
   
        velocity.y = Mathf.Sqrt(jumpHeight * 2 * -gravity);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag == "Obstacle")
        {
            if (isInvincible)
            {
                // // // Nếu đang vô hình, xóa chướng ngại vật
                // Destroy(hit.gameObject);

                // // Hoặc chỉ ẩn đi
                
                FindObjectOfType<AudioManager>().PlaySound("Detroy");
                GameObject explosion = Instantiate(explosionEffect, hit.transform.position, Quaternion.identity);
                Destroy(explosion, 3f); // Xóa hiệu ứng sau 2 giây
                // Destroy(hit.gameObject); // Xóa chướng ngại vật
                hit.gameObject.SetActive(false);

                
                // Thêm hiệu ứng khi phá hủy chướng ngại vật (nếu có)
                GameObject effect = ObjectPool.instance.GetPooledObject();
                if (effect != null)
                {
                    effect.transform.position = hit.transform.position;
                    effect.SetActive(true);
                }
            }


            else
            {
                // Nếu không vô hình, trò chơi kết thúc
                PlayerManager.gameOver = true;
                if(isMagnetActive)
                {
                    magnetSound.Stop();
                    magnetSound.loop = false;
                }
                invincibilitySound.Stop();
                invincibilitySound.loop = false;
                FindObjectOfType<AudioManager>().PlaySound("GameOver");
            }
        }
        // Kiểm tra nếu buff Magnet đang hoạt động
        if (isMagnetActive)
        {
            // Tìm tất cả các viên gem trong game
            GameObject[] gems = GameObject.FindGameObjectsWithTag("Gem");
            
            // Lặp qua các viên gem để kiểm tra khoảng cách và hút chúng nếu gần
            foreach (GameObject gem in gems)
            {
                float distance = Vector3.Distance(transform.position, gem.transform.position); // Tính khoảng cách giữa player và gem

                if (distance <= 3f) // Chỉ hút gem trong phạm vi 10 đơn vị
                {
                    // Tăng tốc độ hút khi gem gần player
                    float speed = Mathf.Lerp(2f, 20f, 1 - (distance / 200f)); // Tăng tốc độ hút khi gem gần player
                    gem.transform.position = Vector3.MoveTowards(gem.transform.position, transform.position, speed * Time.deltaTime);
                }
            }
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        yield return new WaitForSeconds(0.25f/ Time.timeScale);
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;

        yield return new WaitForSeconds((slideDuration - 0.25f)/Time.timeScale);

        animator.SetBool("isSliding", false);

        controller.center = Vector3.zero;
        controller.height = 2;

        isSliding = false;
    }
    // Bắt đầu buff tốc độ
    public void StartSpeedBoost(float multiplier, float duration)
    {
        PlayerManager playerManager = FindObjectOfType<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.StartSpeed(duration);  // Gọi phương thức trong PlayerManager
        }
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine); // Dừng buff cũ nếu đang chạy
            forwardSpeed = originalForwardSpeed; // Đặt lại tốc độ gốc
        }
        speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine(multiplier, duration));
    }

    // Coroutine để xử lý buff tốc độ
    private IEnumerator SpeedBoostCoroutine(float multiplier, float duration)
    {
        forwardSpeed *= multiplier; // Tăng tốc độ
        yield return new WaitForSeconds(duration); // Đợi thời gian hiệu lực
        forwardSpeed = originalForwardSpeed; // Khôi phục tốc độ gốc
    }


    // Bắt đầu buff vo hinh
    public void StartInvincibility(float duration)
    {
        PlayerManager playerManager = FindObjectOfType<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.StartInvincibility(duration);  // Gọi phương thức trong PlayerManager
        }
        if (invincibilityCoroutine != null)
            StopCoroutine(invincibilityCoroutine);

        invincibilityCoroutine = StartCoroutine(InvincibilityCoroutine(duration));
    }

    public IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;
        // Thêm hiệu ứng, ví dụ: đổi màu nhân vật
        if (invincibilitySound != null)
        {
            invincibilitySound.loop = true; // Lặp âm thanh trong suốt thời gian buff
            invincibilitySound.Play();
        }

        yield return new WaitForSeconds(duration);

        // Dừng âm thanh
        if (invincibilitySound != null)
        {
            invincibilitySound.Stop();
            invincibilitySound.loop = false;
        }
        isInvincible = false;
        // Hết hiệu ứng, khôi phục trạng thái nhân vật
    }


    // Bắt đầu buff hut vat pham
    public void StartMagnet(float duration)
    {
        PlayerManager playerManager = FindObjectOfType<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.StartMagnet(duration);  // Gọi phương thức trong PlayerManager
        }
        if (magnetCoroutine != null)
            StopCoroutine(magnetCoroutine);

        magnetCoroutine = StartCoroutine(MagnetCoroutine(duration));
    }
    public IEnumerator MagnetCoroutine(float duration)
    {
        isMagnetActive = true;

        // Bắt đầu phát âm thanh
        if (magnetSound != null)
        {
            magnetSound.loop = true; // Lặp âm thanh trong suốt thời gian buff
            magnetSound.Play();
        }

        yield return new WaitForSeconds(duration);

        // Dừng âm thanh
        if (magnetSound != null)
        {
            magnetSound.Stop();
            magnetSound.loop = false;
        }

        isMagnetActive = false;
    }
}
