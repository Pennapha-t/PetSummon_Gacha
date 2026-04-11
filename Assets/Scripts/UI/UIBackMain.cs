using UnityEngine;
using UnityEngine.SceneManagement;

public class UIBackMain : MonoBehaviour
{
    public void onClickNextScene()
    {
        SceneManager.LoadScene("Main");
    }
}