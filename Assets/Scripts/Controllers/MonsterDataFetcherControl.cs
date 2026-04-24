using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
public class MonsterDataFetcherControl : MonoBehaviour
{
    [Header("Monster IDs Settings")]

    public int[] targetMonsterIds = { 1, 3 };

    private readonly string apiUrl = "https://pet-summon-gacha.vercel.app/api/get_monsters";

    void Start(){
        StartCoroutine(PostGetMonsters(targetMonsterIds));
    }
    
    public IEnumerator PostGetMonsters(int[] ids){
        MonsterRequest requestData = new MonsterRequest(ids);
        string jsonBody = JsonUtility.ToJson(requestData);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler   = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError){
            Debug.LogError($"Error fetching monsters: {request.error}");
        } else {
            string jsonResponse = request.downloadHandler.text;
            MonsterData[] monsters = JSONHelper.FromJson<MonsterData>(jsonResponse);
            foreach (MonsterData m in monsters){
                Debug.Log($"[Monster] ID:{m.monster_id} | Name:{m.name} | HP:{m.hp} | Damage:{m.damage}");
                Debug.Log($"[Monster] ID:{m.monster_id} | Coin Drop: {m.coin_drops.min}-{m.coin_drops.max} (Rate: {m.coin_drops.drop_rate}%)");
                foreach (ItemDrop item in m.item_drops){
                    Debug.Log($"[Monster] ID:{m.monster_id} | Item Drop ID:{item.item_id} | Type:{item.type} | Rate:{item.drop_rate}%");
                }
            }
        }
        request.Dispose();
    }

}
