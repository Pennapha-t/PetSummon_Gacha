using UnityEngine;
using TMPro;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class NormalGachaController : MonoBehaviour
{
    [Header("DB Config")]
    public string DBFileName = "gamedata.db";
    public string DBFolder = "DB";

    [Header("Gacha Config")]
    public int playerId = 1;
    public int gachaId = 1;  

    [Header("UI")]
    public PlayerCurrencyUI currencyUI;

    string ConnectionString
    {
        get
        {
            string path = System.IO.Path.Combine(Application.dataPath, DBFolder, DBFileName);
            return "URI=file:" + path;
        }
    }

    public void OnClickGacha1000()
    {
        Debug.Log("Click Gold Gacha 1000");
        GoldGacha(1000, 1);
    }

    public void OnClickGacha10000()
    {
        Debug.Log("Click Gold Gacha 10000");
        GoldGacha(10000, 10);
    }

    public void OnClickDiamond100()
    {
        Debug.Log("Click Diamond Gacha 100");
        DiamondGacha(100, 1);
    }

    public void OnClickDiamond1000()
    {
        Debug.Log("Click Diamond Gacha 1000");
        DiamondGacha(1000, 10);
    }

    void GoldGacha(int cost, int pullCount)
    {
        using (var conn = new SqliteConnection(ConnectionString))
        {
            conn.Open();
            using (var tx = conn.BeginTransaction())
            {
                int gold = GetPlayerGold(conn);
                if (gold < cost)
                {
                    Debug.Log("Gold coin not enough!");
                    tx.Rollback();
                    return;
                }

                UpdatePlayerGold(conn, -cost);

                PerformGacha(conn, pullCount);

                tx.Commit();
            }
        }

        if (currencyUI != null)
            currencyUI.RefreshFromDB();
    }

    void DiamondGacha(int cost, int pullCount)
    {
        using (var conn = new SqliteConnection(ConnectionString))
        {
            conn.Open();
            using (var tx = conn.BeginTransaction())
            {
                int diamond = GetPlayerDiamond(conn);
                if (diamond < cost)
                {
                    Debug.Log("Diamond not enough!");
                    tx.Rollback();
                    return;
                }

                UpdatePlayerDiamond(conn, -cost);

                PerformGacha(conn, pullCount);

                tx.Commit();
            }
        }

        if (currencyUI != null)
            currencyUI.RefreshFromDB();
    }

    void PerformGacha(SqliteConnection conn, int pullCount)
    {
        for (int i = 0; i < pullCount; i++)
        {
            int itemId = GetRandomItemFromDropTable(conn);
            string itemName = GetItemName(conn, itemId);
            int petId = GetPetIdFromItem(conn, itemId);

            bool isDup = IsPetDuplicate(conn, petId);
            int statReward = isDup ? 5 : 0;

            if (!isDup)
                InsertPlayerPet(conn, petId);

            AddItemToInventory(conn, itemId);
            InsertGachaHistory(conn, itemId, isDup, statReward);

            Debug.Log($"สุ่มครั้งที่ {i + 1} ได้ {itemName} (pet_id={petId})");
        }
    }

    int GetPlayerGold(SqliteConnection conn)
    {
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT gold_coin FROM Player WHERE player_id=@p";
            cmd.Parameters.AddWithValue("@p", playerId);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }

    int GetPlayerDiamond(SqliteConnection conn)
    {
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT diamond FROM Player WHERE player_id=@p";
            cmd.Parameters.AddWithValue("@p", playerId);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }

    void UpdatePlayerGold(SqliteConnection conn, int delta)
    {
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "UPDATE Player SET gold_coin = gold_coin + @d WHERE player_id=@p";
            cmd.Parameters.AddWithValue("@d", delta);
            cmd.Parameters.AddWithValue("@p", playerId);
            cmd.ExecuteNonQuery();
        }
    }

    void UpdatePlayerDiamond(SqliteConnection conn, int delta)
    {
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "UPDATE Player SET diamond = diamond + @d WHERE player_id=@p";
            cmd.Parameters.AddWithValue("@d", delta);
            cmd.Parameters.AddWithValue("@p", playerId);
            cmd.ExecuteNonQuery();
        }
    }

    int GetRandomItemFromDropTable(SqliteConnection conn)
    {
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
                SELECT item_id, base_rate
                FROM Gacha_DropRate
                WHERE gacha_id = @gid";
            cmd.Parameters.AddWithValue("@gid", gachaId);

            using (var reader = cmd.ExecuteReader())
            {
                var ids = new System.Collections.Generic.List<int>();
                var rates = new System.Collections.Generic.List<float>();

                while (reader.Read())
                {
                    ids.Add(reader.GetInt32(0));
                    rates.Add(Convert.ToSingle(reader.GetValue(1)));
                }

                float total = 0;
                foreach (float r in rates) total += r;

                float rand = UnityEngine.Random.value * total;
                float sum = 0;
                for (int i = 0; i < ids.Count; i++)
                {
                    sum += rates[i];
                    if (rand <= sum)
                        return ids[i];
                }

                return ids[ids.Count - 1];
            }
        }
    }

    string GetItemName(SqliteConnection conn, int itemId)
    {
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT item_name FROM Item WHERE item_id=@id";
            cmd.Parameters.AddWithValue("@id", itemId);
            return cmd.ExecuteScalar().ToString();
        }
    }

    int GetPetIdFromItem(SqliteConnection conn, int itemId)
    {
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
                SELECT Pet.pet_id
                FROM Pet
                JOIN Item ON Pet.pet_name = Item.item_name
                WHERE Item.item_id = @id";
            cmd.Parameters.AddWithValue("@id", itemId);
            object r = cmd.ExecuteScalar();
            return r == null ? -1 : Convert.ToInt32(r);
        }
    }

    bool IsPetDuplicate(SqliteConnection conn, int petId)
    {
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT COUNT(*) FROM Player_Pet WHERE player_id=@p AND pet_id=@pet";
            cmd.Parameters.AddWithValue("@p", playerId);
            cmd.Parameters.AddWithValue("@pet", petId);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }
    }

    void InsertPlayerPet(SqliteConnection conn, int petId)
    {
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
                INSERT INTO Player_Pet (player_id, pet_id, level, exp, is_active)
                VALUES (@p, @pet, 1, 0, 0)";
            cmd.Parameters.AddWithValue("@p", playerId);
            cmd.Parameters.AddWithValue("@pet", petId);
            cmd.ExecuteNonQuery();
        }
    }

    void AddItemToInventory(SqliteConnection conn, int itemId)
    {
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
                UPDATE Inventory
                SET quantity = quantity + 1
                WHERE player_id=@p AND item_id=@item";
            cmd.Parameters.AddWithValue("@p", playerId);
            cmd.Parameters.AddWithValue("@item", itemId);
            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
            {
                cmd.CommandText = @"
                    INSERT INTO Inventory (player_id, item_id, quantity)
                    VALUES (@p, @item, 1)";
                cmd.ExecuteNonQuery();
            }
        }
    }

    void InsertGachaHistory(SqliteConnection conn, int itemId, bool isDup, int reward)
    {
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
                INSERT INTO Gacha_History
                (player_id, gacha_id, item_id, is_duplicate, stat_reward, pulled_at)
                VALUES
                (@p, @g, @item, @dup, @rew, datetime('now'))";
            cmd.Parameters.AddWithValue("@p", playerId);
            cmd.Parameters.AddWithValue("@g", gachaId);
            cmd.Parameters.AddWithValue("@item", itemId);
            cmd.Parameters.AddWithValue("@dup", isDup ? 1 : 0);
            cmd.Parameters.AddWithValue("@rew", reward);
            cmd.ExecuteNonQuery();
        }
    }
}
