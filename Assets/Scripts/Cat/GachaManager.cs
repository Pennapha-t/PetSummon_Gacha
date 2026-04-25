using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI; // อย่าลืมใส่ตัวนี้เพื่อคุม Text
using System.Collections;
using TMPro; // ถ้าใช้ TextMeshPro

public class GachaManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI diamondText; // ลากตัว Diamond ข้างบนมาใส่ตรงนี้
    public TextMeshProUGUI coinText;    // ลากตัว Coin ข้างบนมาใส่ตรงนี้

    private string baseUrl = "https://pet-summon-gacha.vercel.app/api";
    private int currentDiamonds;

    void Start()
    {
        // เมื่อเริ่มเกม ให้ไปดึงยอดเงินปัจจุบันจาก MongoDB มาโชว์ก่อน
        StartCoroutine(FetchCurrency());
    }

    // ฟังก์ชันดึงยอดเงิน
    IEnumerator FetchCurrency()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get($"{baseUrl}/currency"))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // สมมติว่า JSON ที่ได้คือ {"diamond": 600, "coin": 1200}
                var data = JsonUtility.FromJson<CurrencyData>(webRequest.downloadHandler.text);
                currentDiamonds = data.diamond;
                UpdateUI(data.coin, data.diamond);
            }
        }
    }

    // ฟังก์ชันเมื่อกดปุ่มสุ่ม (ปุ่มไดมอนด์ข้างล่าง)
    public void OnClickGachaButton(int cost)
    {
        if (currentDiamonds >= cost)
        {
            // 1. หักเงินในเครื่องก่อน (เพื่อให้ UI ลื่นไหล)
            currentDiamonds -= cost;
            UpdateUI(-1, currentDiamonds); // -1 คือไม่ต้องอัปเดต coin

            // 2. เรียก API สุ่มกาชา
            StartCoroutine(RollGacha());
            
            // 3. (Optional) ส่งข้อมูลไป Update ใน MongoDB ให้เงินลดจริง
            // StartCoroutine(UpdateDatabase(currentDiamonds)); 
        }
        else
        {
            Debug.Log("เงินไม่พอจ้า!");
        }
    }

    IEnumerator RollGacha()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get($"{baseUrl}/gacha"))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("ผลการสุ่ม: " + webRequest.downloadHandler.text);
                // นำข้อมูลแมวไปโชว์ในหน้าสุ่มต่อ...
            }
        }
    }

    void UpdateUI(int coin, int diamond)
    {
        if (diamond != -1) diamondText.text = diamond.ToString();
        if (coin != -1) coinText.text = coin.ToString();
    }
}

[System.Serializable]
public class CurrencyData {
    public int coin;
    public int diamond;
}