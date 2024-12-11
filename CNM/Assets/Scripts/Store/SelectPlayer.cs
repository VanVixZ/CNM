using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public ShopElement[] characters;

    public int characterIndex;
    public GameObject[] shopCharacters;

    public GameObject magnetIcon;
    public GameObject x2Icon;
    public GameObject immortalIcon;

    private List<int> indexCharacters;

    void Start()
    {
        indexCharacters = new List<int>();

        // Tải dữ liệu isLocked cho từng nhân vật
        foreach (ShopElement c in characters)
        {
            if (c.price != 0)
                c.isLocked = PlayerPrefs.GetInt(c.name, 1) == 1 ? true : false;
        }

        // Lặp qua các nhân vật và lưu các chỉ số của nhân vật không bị khóa vào indexCharacters
        for (int i = 0; i < characters.Length; i++)
        {
            if (!characters[i].isLocked)
            {
                indexCharacters.Add(i); // Lưu chỉ số của nhân vật không bị khóa vào danh sách
                shopCharacters[i].SetActive(true); // Kích hoạt hiển thị nhân vật không bị khóa
            }
            else
            {
                shopCharacters[i].SetActive(false); // Tắt hiển thị nhân vật bị khóa
            }
        }

        // Lấy nhân vật đã được chọn từ PlayerPrefs
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);

        // Đảm bảo rằng nhân vật được chọn đã mở khóa (nằm trong indexCharacters)
        if (characterIndex >= indexCharacters.Count)
        {
            characterIndex = 0; // Nếu nhân vật được chọn không hợp lệ (bị khóa), chọn nhân vật đầu tiên không bị khóa
        }

        // Tắt hiển thị tất cả nhân vật trước
        foreach (GameObject ch in shopCharacters)
        {
            ch.SetActive(false);
        }

        // Kích hoạt nhân vật đã chọn trong danh sách các nhân vật không bị khóa
        if (indexCharacters.Count > 0)
        {
            shopCharacters[indexCharacters[characterIndex]].SetActive(true);
        }

 
        if (PlayerPrefs.GetInt("MagnetPurchased", 0) == 1)
        {
            magnetIcon.SetActive(true); 
        }

        if (PlayerPrefs.GetInt("X2Purchased", 0) == 1)
        {
            x2Icon.SetActive(true); 
        }

        if (PlayerPrefs.GetInt("ImmortalPurchased", 0) == 1)
        {
            immortalIcon.SetActive(true); 
        }
    }

    public void ChangeNextCharacter()
    {
        // Tắt hiển thị nhân vật hiện tại
        shopCharacters[indexCharacters[characterIndex]].SetActive(false);

        characterIndex++;
        if (characterIndex == indexCharacters.Count)
            characterIndex = 0; 

        shopCharacters[indexCharacters[characterIndex]].SetActive(true);

        // Lưu lại nhân vật đã chọn vào PlayerPrefs
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
    }

    public void ChangePreviousCharacter()
    {
        // Tắt hiển thị nhân vật hiện tại
        shopCharacters[indexCharacters[characterIndex]].SetActive(false);

        characterIndex--;
        if (characterIndex == -1)
            characterIndex = indexCharacters.Count - 1;

        shopCharacters[indexCharacters[characterIndex]].SetActive(true);

        // Lưu lại nhân vật đã chọn vào PlayerPrefs
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
    }
}
