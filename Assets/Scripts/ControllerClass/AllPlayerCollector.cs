using UnityEngine;

using System.Data; // Add on
using Mono.Data.Sqlite; // Add on
using System;
using System.Collections.Generic; // Add on
public class AllPlayerCollector : MonoBehaviour
{
    public string DBFileName = "gamedata.db";
    public string DBFolder = "DB";
    void Start()
    {
        onLoadfromDatabase();
    }

 public List<Player> onLoadfromDatabase()
    {
        SQliteAdapter adapter = new SQliteAdapter(DBFileName,DBFolder);
        IDataReader reader = adapter.select("players","*");
        string username = "";
        int player_id = -1;

        List<Player> list_player = new List<Player>();

        while(reader.Read()){
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i) == "player_id")
                {
                    player_id = Convert.ToInt32(reader.GetValue(i));
                }

                if (reader.GetName(i) == "username")
                {
                    username = "" + reader.GetValue(i);
                }
            }
            Player temp_player = new Player(player_id, username);
            list_player.Add(temp_player);
            
        }
        Debug.Log(list_player[0].getUsername() + " : " + list_player[1].getUsername());
        adapter.disconnectDatabase();

        foreach (var item in list_player)
        {
            Debug.Log(item.getUsername());
        }
        return list_player;
    }

}

