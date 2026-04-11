using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
    public BattleState state;

    public PlayerStats selectedPlayer;
    public EnemyStats selectedEnemy;
    public List<PlayerStats> allPlayers;
    public List<EnemyStats> allEnemies;

    [Header("Reward UI")]
    public GameObject rewardPanel;

    [Header("Run System UI")]
    public GameObject runConfirmPanel;

    [Header("Reward Amount")]
    public int rewardCoin = 10;
    public int rewardGem = 3;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        state = BattleState.PLAYERTURN;
        Debug.Log("เริ่มการต่อสู้! เทิร์นของคุณ");

        if (rewardPanel != null)
            rewardPanel.SetActive(false);

        if (runConfirmPanel != null)
            runConfirmPanel.SetActive(false);
    }

    // =============================
    // PLAYER ATTACK
    // =============================
    public void ExecuteAttack()
    {
        if (state != BattleState.PLAYERTURN) return;
        if (selectedPlayer == null || selectedEnemy == null) return;

        int randomDamage = Random.Range(15, 26);
        selectedEnemy.TakeDamage(randomDamage);
        selectedPlayer.AddMP(10);

        Debug.Log($"{selectedPlayer.catName} โจมตีศัตรู {selectedEnemy.catID} ดาเมจ {randomDamage}");
        StartCoroutine(CheckAfterDelay());
    }

    // =============================
    // ENEMY TURN
    // =============================
    IEnumerator EnemyTurnRoutine()
    {
        state = BattleState.ENEMYTURN;
        ClearSelection();

        EnemyStats attacker = allEnemies.Find(e => e.currentHP > 0 && e.isEnraged);

        if (attacker == null)
        {
            List<EnemyStats> aliveEnemies = allEnemies.FindAll(e => e.currentHP > 0);
            if (aliveEnemies.Count == 0) yield break;
            attacker = aliveEnemies[Random.Range(0, aliveEnemies.Count)];
        }

        Debug.Log($"ศัตรู {attacker.catID} กำลังโจมตี");
        yield return new WaitForSeconds(0.5f);

        List<PlayerStats> alivePlayers = allPlayers.FindAll(p => p.currentHP > 0);

        if (alivePlayers.Count > 0)
        {
            PlayerStats finalTarget = alivePlayers[Random.Range(0, alivePlayers.Count)];

            int baseDamage = Random.Range(10, 21);
            int finalDamage = Mathf.RoundToInt(baseDamage * attacker.damageMultiplier);

            Debug.Log($"ศัตรูโจมตี {finalTarget.catName} ดาเมจ {finalDamage}");

            finalTarget.TakeDamage(finalDamage);
            attacker.SetEnrage(false);
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(CheckAfterDelay());
    }

    // =============================
    // CHECK BATTLE (DELAY SAFE)
    // =============================
    IEnumerator CheckAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);

        if (!CheckBattleStatus())
        {
            if (state == BattleState.ENEMYTURN)
            {
                state = BattleState.PLAYERTURN;
                Debug.Log("กลับมาเป็นเทิร์นของผู้เล่น");
            }
            else
            {
                StartCoroutine(EnemyTurnRoutine());
            }
        }
    }

    // =============================
    // CHECK BATTLE STATUS
    // =============================
    bool CheckBattleStatus()
    {
        bool allEnemiesDead = allEnemies.TrueForAll(e => e.currentHP <= 0);
        bool allPlayersDead = allPlayers.TrueForAll(p => p.currentHP <= 0);

        // ===== WIN =====
        if (allEnemiesDead)
        {
            state = BattleState.WON;
            Debug.Log("<color=green>YOU WON</color>");

            GiveReward(10, 3);

            if (rewardPanel != null)
                rewardPanel.SetActive(true);

            return true;
        }

        // ===== LOSE =====
        if (allPlayersDead)
        {
            state = BattleState.LOST;
            Debug.Log("<color=red>YOU LOST</color>");
            return true;
        }

        return false;
    }

    // =============================
    // REWARD SYSTEM
    // =============================
    void GiveReward(int coin, int gem)
    {
        int currentCoin = PlayerPrefs.GetInt("COIN", 0);
        int currentGem = PlayerPrefs.GetInt("GEM", 0);

        currentCoin += coin;
        currentGem += gem;

        PlayerPrefs.SetInt("COIN", currentCoin);
        PlayerPrefs.SetInt("GEM", currentGem);
        PlayerPrefs.Save();

        Debug.Log($"Reward Added: +{coin} Coin, +{gem} Gem");
    }

    // =============================
    // PLAYER SELECTION
    // =============================
    void ClearSelection()
    {
        selectedEnemy = null;
        selectedPlayer = null;
    }

    public void SetSelectedPlayer(PlayerStats player)
    {
        if (state != BattleState.PLAYERTURN) return;
        selectedPlayer = player;
    }

    public void SetSelectedTarget(EnemyStats enemy)
    {
        if (state != BattleState.PLAYERTURN) return;
        selectedEnemy = enemy;
    }

    // =============================
    // MP SKILL
    // =============================
    public void ExecuteMPSkill()
    {
        if (state != BattleState.PLAYERTURN) return;
        if (selectedPlayer == null || selectedEnemy == null) return;

        if (selectedPlayer.currentMP >= 20)
        {
            int skillDamage = Random.Range(35, 51);
            selectedPlayer.UseMP(20);
            selectedEnemy.TakeDamage(skillDamage);

            StartCoroutine(CheckAfterDelay());
        }
    }

    // =============================
    // ULTIMATE
    // =============================
    public void ExecuteUltimate()
    {
        if (state != BattleState.PLAYERTURN || selectedPlayer == null) return;

        if (selectedPlayer.currentMP >= 50)
        {
            selectedPlayer.UseMP(50);

            if (selectedPlayer.catID == 3)
            {
                foreach (PlayerStats p in allPlayers)
                    if (p.currentHP > 0)
                        p.Heal(0.5f);
            }
            else if (selectedEnemy != null)
            {
                int ultDamage = Random.Range(70, 101);
                selectedEnemy.TakeDamage(ultDamage);
            }

            StartCoroutine(CheckAfterDelay());
        }
    }

    // =============================
    // RUN SYSTEM
    // =============================
    public void OpenRunConfirm()
    {
        if (runConfirmPanel != null)
            runConfirmPanel.SetActive(true);
    }

    public void ConfirmRun()
    {
        if (runConfirmPanel != null)
            runConfirmPanel.SetActive(false);

        Debug.Log("Run confirmed, returning to Main...");
        StartCoroutine(LoadSceneAsync("Main"));
    }

    public void CancelRun()
    {
        if (runConfirmPanel != null)
            runConfirmPanel.SetActive(false);
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone)
            yield return null;
    }

    // =============================
    // CALLED FROM ENEMY
    // =============================
    public void OnEnemyDied(EnemyStats deadEnemy)
    {
        Debug.Log($"Enemy {deadEnemy.catID} died, checking battle result...");
        StartCoroutine(CheckAfterDelay());
    }

    // =============================
    // BUTTON: NEXT LEVEL / BACK TO MAIN
    // =============================
    public void OnClickReturnToMain()
    {
        StartCoroutine(LoadSceneAsync("Main"));
    }
}