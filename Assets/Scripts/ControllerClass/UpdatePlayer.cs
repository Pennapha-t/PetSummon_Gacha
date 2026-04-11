using System.Data;
using UnityEngine;

public class UpdatePlayer : MonoBehaviour
{
    public string DBFileName = "gamedata.db";
    public string DBFolder = "DB";
    void Start()
    {
        testUpdate();
    }

    // Update is called once per frame
    void testUpdate()
    {

        Debug.Log("UpdatePlayer | testUpdate");

        SQliteAdapter adapter = new SQliteAdapter(DBFileName, DBFolder);
        adapter.update1Col("players", "gold_coin", "" + 1200, "username", "'TiaoKai'");

        adapter.update1Col("players", "gold_coin", "" + 1000, "username", "'Maru'");
    }
}
