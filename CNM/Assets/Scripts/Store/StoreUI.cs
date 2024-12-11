using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public Text gemText;
    public Button buttonBack;
    public Button buttonItems;
    public Button buttonCharacters;

    public GameObject itemList;       // Danh sách Items
    public GameObject characterList; // Danh sách Characters

    public Button magnet;
    public Button x2;
    public Button immortal;
    public Button player00;
    public Button player01;
    public Button player02;

    public Sprite disabledSprite;

    public ShopElement[] characters;
    public int characterIndex;//0:Wheel, 1:Amy, 2:Michelle ...
    void Start()
    {
        // Hiển thị số lượng gems
        gemText.text = "Gem: " + PlayerPrefs.GetInt("TotalGems", 0);

        // Hiển thị mặc định danh sách Items và ẩn danh sách Characters
        itemList.SetActive(true);
        characterList.SetActive(false);

        // Kiểm tra xem vật phẩm đã mua và hiển thị chúng
        if (PlayerPrefs.GetInt("MagnetPurchased", 0) == 1 || PlayerPrefs.GetInt("X2Purchased", 0) == 1 || PlayerPrefs.GetInt("ImmortalPurchased", 0) == 1)
        {
            // Khóa các item khác
            magnet.interactable = false;
            x2.interactable = false;
            immortal.interactable = false;
            SpriteState spriteState = new SpriteState
            {
                disabledSprite = disabledSprite
            };
            magnet.image.sprite = disabledSprite;
            x2.image.sprite = disabledSprite;
            immortal.image.sprite = disabledSprite;
        }
        // Kiểm tra nhân vật đã mua
        ShopElement c = characters[1];
        ShopElement c2 = characters[2];

        //PlayerPrefs.SetInt(c.name, 1);
        //c.isLocked = true;
        //PlayerPrefs.SetInt(c2.name, 1);
        //c2.isLocked = true;


        if (PlayerPrefs.GetInt(c.name) == 0)
        {
            player01.interactable = false;
            SpriteState spriteState = new SpriteState
            {
                disabledSprite = disabledSprite
            };
            player01.image.sprite = disabledSprite;
        }
        if (PlayerPrefs.GetInt(c2.name) == 0)
        {
            player02.interactable = false;
            SpriteState spriteState = new SpriteState
            {
                disabledSprite = disabledSprite
            };
            player02.image.sprite = disabledSprite;
        }    

    }

    public void ButtonBack()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ShowItems()
    {
        // Hiển thị danh sách Items và ẩn danh sách Characters
        itemList.SetActive(true);
        characterList.SetActive(false);
    }

    public void ShowCharacters()
    {
        // Hiển thị danh sách Characters và ẩn danh sách Items
        itemList.SetActive(false);
        characterList.SetActive(true);
    }
    public void BuyMagnet()
    {
        int totalGems = PlayerPrefs.GetInt("TotalGems", 0);
        int magnetPrice = 100;

        if (totalGems >= magnetPrice)
        {
            totalGems -= magnetPrice;
            PlayerPrefs.SetInt("TotalGems", totalGems);

            // Khóa các item khác
            magnet.interactable = false;
            x2.interactable = false;
            immortal.interactable = false;

            SpriteState spriteState = new SpriteState
            {
                disabledSprite = disabledSprite
            };

            magnet.image.sprite = disabledSprite;
            x2.image.sprite = disabledSprite;
            immortal.image.sprite = disabledSprite;

            PlayerPrefs.SetInt("MagnetPurchased", 1);

            gemText.text = "Gem: " + PlayerPrefs.GetInt("TotalGems", 0);
        }
        else
        {
            Debug.Log("Không đủ Gems để mua Magnet!");
        }
    }

    public void BuyX2()
    {
        // Lấy số gems hiện tại
        int totalGems = PlayerPrefs.GetInt("TotalGems", 0);

        int x2Price = 100;

        // Kiểm tra xem người chơi có đủ gems để mua không
        if (totalGems >= x2Price)
        {
            // Trừ đi số gems khi mua
            totalGems -= x2Price;
            PlayerPrefs.SetInt("TotalGems", totalGems); // Lưu lại số gems mới

            // Khóa các item khác
            magnet.interactable = false;
            x2.interactable = false;
            immortal.interactable = false;
            SpriteState spriteState = new SpriteState
            {
                disabledSprite = disabledSprite
            };
            magnet.image.sprite = disabledSprite;
            x2.image.sprite = disabledSprite;
            immortal.image.sprite = disabledSprite;

            // Lưu trạng thái vật phẩm đã mua
            PlayerPrefs.SetInt("X2Purchased", 1); // Đánh dấu đã mua

            // Hiển thị số lượng gems
            gemText.text = "Gem: " + PlayerPrefs.GetInt("TotalGems", 0);
        }
        else
        {
            Debug.Log("Không đủ Gems để mua !");
        }
    }
    public void BuyImmortal()
    {
        // Lấy số gems hiện tại
        int totalGems = PlayerPrefs.GetInt("TotalGems", 0);

        int immortalPrice = 100;

        // Kiểm tra xem người chơi có đủ gems để mua không
        if (totalGems >= immortalPrice)
        {
            // Trừ đi số gems khi mua
            totalGems -= immortalPrice;
            PlayerPrefs.SetInt("TotalGems", totalGems); // Lưu lại số gems mới

            // Khóa các item khác
            magnet.interactable = false;
            x2.interactable = false;
            immortal.interactable = false;
            SpriteState spriteState = new SpriteState
            {
                disabledSprite = disabledSprite
            };
            magnet.image.sprite = disabledSprite;
            x2.image.sprite = disabledSprite;
            immortal.image.sprite = disabledSprite;

            // Lưu trạng thái vật phẩm đã mua
            PlayerPrefs.SetInt("ImmortalPurchased", 1); // Đánh dấu đã mua

            // Hiển thị số lượng gems
            gemText.text = "Gem: " + PlayerPrefs.GetInt("TotalGems", 0);
        }
        else
        {
            Debug.Log("Không đủ Gems để mua !");
        }
    }
    public void UnlockPlayer01()
    {
        int characterIndex = 1;
        ShopElement c = characters[characterIndex];
        if (PlayerPrefs.GetInt("TotalGems", 0) < c.price)
            return;

        int newGems = PlayerPrefs.GetInt("TotalGems", 0) - characters[characterIndex].price;
        PlayerPrefs.SetInt("TotalGems", newGems);

        c.isLocked = false;
        PlayerPrefs.SetInt(c.name, 0);
        // Khóa nhân vật
        player01.interactable = false;
        SpriteState spriteState = new SpriteState
        {
            disabledSprite = disabledSprite
        };
        player01.image.sprite = disabledSprite;
        gemText.text = "Gem: " + PlayerPrefs.GetInt("TotalGems", 0);
    }
    public void UnlockPlayer02()
    {
        int characterIndex = 2;
        ShopElement c = characters[characterIndex];
        if (PlayerPrefs.GetInt("TotalGems", 0) < c.price)
            return;

        int newGems = PlayerPrefs.GetInt("TotalGems", 0) - characters[characterIndex].price;
        PlayerPrefs.SetInt("TotalGems", newGems);

        c.isLocked = false;
        PlayerPrefs.SetInt(c.name, 0);
        // Khóa nhân vật
        player02.interactable = false;
        SpriteState spriteState = new SpriteState
        {
            disabledSprite = disabledSprite
        };
        player02.image.sprite = disabledSprite;
        gemText.text = "Gem: " + PlayerPrefs.GetInt("TotalGems", 0);
    }
}
