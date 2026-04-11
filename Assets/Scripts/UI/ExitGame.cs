using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void Exit()
    {
        Debug.Log("Game is closing...");
        Application.Quit();
    }
}