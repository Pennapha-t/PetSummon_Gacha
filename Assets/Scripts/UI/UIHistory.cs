using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHistory : MonoBehaviour
{
    public void onClickNextScene()
    {
        SceneManager.LoadScene("History");
    }
}
