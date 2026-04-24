using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
public class WeaponDataFetcherControl : MonoBehaviour{
    [Header("Weapon Settings")]
    public int itemId = 0;
    private readonly string baseUrl = "https://pet-summon-gacha.vercel.app/api/weapon_stat";
    void Start(){
        StartCoroutine(GetWeaponData(itemId));
    }
    public IEnumerator GetWeaponData(int itemId)
{
        string url     = $"{baseUrl}?item_id={itemId}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError){
            Debug.LogError($"Error fetching data: {request.error}");
        } else {
            string jsonResponse = request.downloadHandler.text;
            WeaponData weaponData = JsonUtility.FromJson<WeaponData>(jsonResponse);
            Debug.Log($"{itemId} : Name: {weaponData.name} | Damage: {weaponData.damage} | Sharpness: {weaponData.sharpness} ");
        }
        request.Dispose();
    }
}
