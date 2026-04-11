using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButton : MonoBehaviour
{
    public void onClickNextScene()
    {
        SceneManager.LoadScene("NormalGacha");
    }
}