using UnityEngine;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    public void onClickNextScene()
    {
        SceneManager.LoadScene("Shop");
    }
}
