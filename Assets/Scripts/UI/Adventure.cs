using UnityEngine;
using UnityEngine.SceneManagement;

public class Adventure : MonoBehaviour
{
    public void onClickNextScene()
    {
        SceneManager.LoadScene("Adventure");
    }
}
