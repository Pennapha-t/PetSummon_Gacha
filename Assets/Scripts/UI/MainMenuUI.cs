using UnityEngine;
using TMPro;   // สำคัญมาก

public class MainMenuUI : MonoBehaviour
{
    public TMP_Text coinText;
    public TMP_Text gemText;

    void Start()
    {
        int coin = PlayerPrefs.GetInt("COIN", 0);
        int gem = PlayerPrefs.GetInt("GEM", 0);

        coinText.text = coin.ToString();
        gemText.text = gem.ToString();
    }
}