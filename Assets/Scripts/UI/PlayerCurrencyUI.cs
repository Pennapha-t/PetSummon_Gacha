using UnityEngine;
using TMPro;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class PlayerCurrencyUI : MonoBehaviour
{
    public static PlayerCurrencyUI Instance;

    [Header("DB Config")]
    public string DBFileName = "gamedata.db";
    public string DBFolder   = "DB";
    public int playerId      = 1;  

    [Header("UI References")]
    public TextMeshProUGUI goldText;  
    public TextMeshProUGUI diamondText;

    string ConnectionString
    {
        get
        {
            string path = System.IO.Path.Combine(Application.dataPath, DBFolder, DBFileName);
            return "URI=file:" + path;
        }
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        RefreshFromDB();
    }

    // =========================
    // READ
    // =========================
    public void RefreshFromDB()
    {
        using (var conn = new SqliteConnection(ConnectionString))
        {
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT gold_coin, diamond FROM Player WHERE player_id=@pid";
                cmd.Parameters.AddWithValue("@pid", playerId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int gold    = Convert.ToInt32(reader.GetValue(0));
                        int diamond = Convert.ToInt32(reader.GetValue(1));

                        if (goldText    != null) goldText.text    = gold.ToString();
                        if (diamondText != null) diamondText.text = diamond.ToString();
                    }
                }
            }
        }
    }

    // =========================
    // ADD GOLD
    // =========================
    public void AddGold(int amount)
    {
        using (var conn = new SqliteConnection(ConnectionString))
        {
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    UPDATE Player
                    SET gold_coin = gold_coin + @amount
                    WHERE player_id = @pid
                ";

                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@pid", playerId);

                cmd.ExecuteNonQuery();
            }
        }

        RefreshFromDB();
    }

    // =========================
    // ADD DIAMOND
    // =========================
    public void AddDiamond(int amount)
    {
        using (var conn = new SqliteConnection(ConnectionString))
        {
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    UPDATE Player
                    SET diamond = diamond + @amount
                    WHERE player_id = @pid
                ";

                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@pid", playerId);

                cmd.ExecuteNonQuery();
            }
        }

        RefreshFromDB();
    }
}