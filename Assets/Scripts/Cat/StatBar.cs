using UnityEngine;
using UnityEngine.UI; // ต้องมีตัวนี้เพื่อเรียกใช้ Slider

public class StatBar : MonoBehaviour
{
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>(); // ดึง Component Slider มาเก็บไว้
    }

    // ฟังก์ชันสำหรับตั้งค่า Max เริ่มต้น
    public void SetMaxStat(int value)
    {
        slider.maxValue = value;
        slider.value = value;
    }

    // ฟังก์ชันสำหรับปรับค่าปัจจุบัน (เอาไว้เรียกใช้ตอนโดนโจมตี หรือใช้สกิล)
    public void SetCurrentStat(int value)
    {
        slider.value = value;
    }
}