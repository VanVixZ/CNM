using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;

    public static bool isGameStarted;
    public GameObject startingText;
    public GameObject newRecordPanel;

    public static int score;
    public Text scoreText;
    public TextMeshProUGUI gemsText;
    public TextMeshProUGUI newRecordText;

    public TextMeshProUGUI buffTimerTextInvincibility; // Biến để hiển thị thời gian của buff vô hình
    public TextMeshProUGUI buffTimerTextMagnet; // Biến để hiển thị thời gian của buff vô hình
    
    private float speedEndTime;
    private bool isSpeed;
    public Image myImageSpeed;


    private float invincibilityEndTime;
    private bool isInvincible;
    public Image myImageInvincible;

    private float magnetEndTime;
    private bool isMagnet;
    public Image myImageMagnet;

    public static bool isGamePaused;
    public GameObject[] characterPrefabs;

    public PlayerController playerController;


    private void Awake()
    {
        int index = PlayerPrefs.GetInt("SelectedCharacter");
        GameObject go = Instantiate(characterPrefabs[index], transform.position, Quaternion.identity);
        playerController = go.GetComponent<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("PlayerController is missing on the instantiated character!");
        }
    }

    void Start()
    {
        score = 0;
        Time.timeScale = 1;
        gameOver = isGameStarted = isGamePaused= false;
        myImageInvincible.gameObject.SetActive(false);
        myImageMagnet.gameObject.SetActive(false);
        myImageSpeed.gameObject.SetActive(false);

        //Buff begin start
        BuffBeginStar();

        PlayerPrefs.SetInt("MagnetPurchased", 0);
        PlayerPrefs.SetInt("X2Purchased", 0);
        PlayerPrefs.SetInt("ImmortalPurchased", 0);

        // adManager.RequestBanner();
        // adManager.RequestInterstitial();
        // adManager.RequestRewardBasedVideo();
    }

    public void BuffBeginStar()
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController is null. Buffs cannot be applied!");
            return;
        }

        if (PlayerPrefs.GetInt("MagnetPurchased", 0) == 1)
        {
            playerController.StartMagnet(7f);
        }

        if (PlayerPrefs.GetInt("X2Purchased", 0) == 1)
        {
            playerController.StartSpeedBoost(2f, 5f);
        }

        if (PlayerPrefs.GetInt("ImmortalPurchased", 0) == 1)
        {
            playerController.StartInvincibility(5f);
        }
    }

    void Update()
    {
        //Update UI
        gemsText.text = PlayerPrefs.GetInt("TotalGems", 0).ToString();
        scoreText.text = score.ToString();

        //Game Over
        if (gameOver)
        {
            Time.timeScale = 0;
            if (score > PlayerPrefs.GetInt("HighScore", 0))
            {
                newRecordPanel.SetActive(true);
                newRecordText.text = "New \nRecord\n" + score;
                PlayerPrefs.SetInt("HighScore", score);
            }
            myImageInvincible.gameObject.SetActive(false);
            myImageMagnet.gameObject.SetActive(false);
            myImageSpeed.gameObject.SetActive(false);
            gameOverPanel.SetActive(true);
            Destroy(gameObject);
        }

        //Start Game
        if (SwipeManager.tap  && !isGameStarted)
        {
            isGameStarted = true;
            Destroy(startingText);
        }

        if (isInvincible)
        {
            float remainingTime = invincibilityEndTime - Time.time;
            // buffTimerTextInvincibility.text = $"Invincibility: {remainingTime:F1}s";  // Cập nhật thời gian lên UI
            myImageInvincible.gameObject.SetActive(true);
            if (remainingTime <= 0)
            {
                myImageInvincible.gameObject.SetActive(false);
                isInvincible = false;
                // buffTimerTextInvincibility.text = "";  // Xóa thời gian khi buff hết
            }
        }

        if (isMagnet)
        {
            float remainingTime = magnetEndTime - Time.time;
            // buffTimerTextMagnet.text = $"Magnet: {remainingTime:F1}s";  // Hiển thị thời gian còn lại
            myImageMagnet.gameObject.SetActive(true); 
            if (remainingTime <= 0)
            {
                myImageMagnet.gameObject.SetActive(false);
                isMagnet = false;
                // buffTimerTextMagnet.text = "";  // Xóa thông tin buff khi hết thời gian
            }
        }
        if (isSpeed)
        {
            float remainingTime = speedEndTime - Time.time;
            // buffTimerTextMagnet.text = $"Magnet: {remainingTime:F1}s";  // Hiển thị thời gian còn lại
            myImageSpeed.gameObject.SetActive(true); 
            if (remainingTime <= 0)
            {
                myImageSpeed.gameObject.SetActive(false);
                isSpeed = false;
                // buffTimerTextMagnet.text = "";  // Xóa thông tin buff khi hết thời gian
            }
        }
    }
    // Phương thức bắt đầu buff vô hình
    public void StartInvincibility(float duration)
    {
        isInvincible = true;
        invincibilityEndTime = Time.time + duration; 
    }

    // Phương thức bắt đầu buff hut
    public void StartMagnet(float duration)
    {
        isMagnet = true;
        magnetEndTime = Time.time + duration;  
    }
    // Phương thức bắt đầu buff hut
    public void StartSpeed(float duration)
    {
        isSpeed = true;
        speedEndTime = Time.time + duration;  
    }
}
