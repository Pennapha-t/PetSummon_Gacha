using UnityEngine;

public class BattleMenu : MonoBehaviour {
    public void OnAtkClick() {
        Debug.Log("กดปุ่มโจมตี!");
        // สั่งให้ตัวละครที่เลือกไว้ทำการโจมตี
    }

    public void OnRunClick() {
        Debug.Log("พยายามหนี...");
        // ใส่คำสั่งเปลี่ยน Scene กลับหน้าหลัก
    }
}