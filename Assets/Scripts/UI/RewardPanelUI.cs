using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardPanelUI : MonoBehaviour
{
    [Header("Reward")]
    public int goldReward = 10;
    public int gemReward  = 3;

    [Header("Scene")]
    public string nextLevelScene = "Level2";
    public string adventureScene = "Main";

    public void OnClickNextLevel()
    {
        GiveReward();
        Debug.Log("ไปด่านถัดไป: " + nextLevelScene);
        SceneManager.LoadScene(nextLevelScene);
    }

    public void OnClickEnd()
    {
        GiveReward();
        Debug.Log("กลับไป Adventure: " + adventureScene);
        SceneManager.LoadScene(adventureScene);
    }

    void GiveReward()
    {
        Debug.Log("=================================");
        Debug.Log("ผู้เล่นได้รับรางวัล!");
        Debug.Log("Gold: " + goldReward);
        Debug.Log("Gems: " + gemReward);
        Debug.Log("=================================");
    }
}