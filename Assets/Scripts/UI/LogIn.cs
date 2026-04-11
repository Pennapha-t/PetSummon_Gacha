using UnityEngine;
using UnityEngine.SceneManagement;

public class LogIn : MonoBehaviour
{
    public void onClickNextScene()
    {
        SceneManager.LoadScene("Main");
    }
}