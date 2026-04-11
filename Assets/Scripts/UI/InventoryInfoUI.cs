using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;

public class InventoryInfoUI : MonoBehaviour
{
    [Header("DB Config")]
    public string DBFileName = "gamedata.db";
    public string DBFolder   = "DB";
    public int playerId      = 1;  

    [Header("UI")]
    public TextMeshProUGUI infoText; 

    string ConnectionString
    {
        get
        {
            string path = Path.Combine(Application.dataPath, DBFolder, DBFileName);
            return "URI=file:" + path;
        }
    }

    void Start()
    {
        UpdateInfoFromDB();
    }

    public void UpdateInfoFromDB()
    {
        if (infoText == null)
        {
            Debug.LogError("infoText is not assigned!");
            return;
        }

        using (var conn = new SqliteConnection(ConnectionString))
        {
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT p.pet_name
                    FROM Gacha_History gh
                    JOIN Item i ON gh.item_id = i.item_id
                    JOIN Pet  p ON p.pet_name = i.item_name
                    WHERE gh.player_id = @pid
                    ORDER BY gh.history_id DESC
                    LIMIT 1;
                ";

                cmd.Parameters.AddWithValue("@pid", playerId);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    string petName = result.ToString();
                    infoText.text = $"สัตว์ที่ได้รับล่าสุด: {petName}";
                }
                else
                {
                    infoText.text = "ยังไม่เคยได้รับสัตว์จากกาชา";
                }
            }
        }
    }
}