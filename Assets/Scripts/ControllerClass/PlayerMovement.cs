using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movSpeed = 5f;
    float speedX, speedY;
    Rigidbody2D rb;
    PlayerMana manaSystem;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        manaSystem = GetComponent<PlayerMana>(); // เชื่อมกับระบบมานา
    }

    void Update()
    {
        // รับค่า Input ใน Update (รวดเร็วและแม่นยำต่อการกด)
        speedX = Input.GetAxisRaw("Horizontal");
        speedY = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate() 
    {
        float currentSpeed = movSpeed;

        // ถ้ามานาหมด ให้ความเร็วลดลงครึ่งหนึ่ง
        if (manaSystem != null && manaSystem.currentMana <= 0) {
            currentSpeed = movSpeed * 0.5f; 
    }

    rb.linearVelocity = new Vector2(speedX * currentSpeed, speedY * currentSpeed);
}
}