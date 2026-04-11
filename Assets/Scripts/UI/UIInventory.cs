using UnityEngine;
using UnityEngine.SceneManagement;

public class UIInventory : MonoBehaviour
{
    public void onClickNextScene()
    {
        SceneManager.LoadScene("Inventory");
    }
}