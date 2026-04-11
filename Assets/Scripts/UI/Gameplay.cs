using UnityEngine;
using UnityEngine.SceneManagement;

public class Gameplay : MonoBehaviour
{
    public void onClickNextScene()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
