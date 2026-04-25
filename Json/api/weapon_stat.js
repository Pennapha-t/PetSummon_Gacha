import { MongoClient } from "mongodb";

const uri = process.env.MONGODB_URI;

let client;
let clientPromise;

if (!global._mongoClientPromise) {
  client = new MongoClient(uri);
  global._mongoClientPromise = client.connect();
}

clientPromise = global._mongoClientPromise;

export default async function handler(req, res) {
  const { item_id } = req.query;

  // ตรวจสอบว่ามีการส่ง item_id มาหรือไม่
  if (!item_id) {
    return res.status(400).json({ message: "item_id is required" });
  }

  try {
    const client = await clientPromise;
    
    // 1. ตรวจสอบชื่อ Database (จากรูป Compass คือ testdb)
    const db = client.db("testdb"); 

    // 2. ใช้ $or ค้นหาทั้งแบบ Number และ String เพื่อป้องกันความผิดพลาดของ Type
    const weapon = await db.collection("weapons").findOne({
      $or: [
        { item_id: Number(item_id) }, // ค้นหาแบบตัวเลข (Int32 ตามรูป)
        { item_id: String(item_id) }  // เผื่อไว้กรณีเป็นข้อความ
      ]
    });

    if (!weapon) {
      // เพิ่มข้อมูล Debug เพื่อดูว่า API อัปเดตหรือยัง และกำลังหาด้วยค่าอะไร
      return res.status(404).json({ 
        message: "DEBUG: API Updated - Weapon not found", 
        searched_for: item_id,
        db_name: "testdb",
        collection: "weapons"
      });
    }

    // ถ้าเจอ ให้ส่งข้อมูลออกไป
    res.status(200).json(weapon);
  } catch (err) {
    console.error(err);
    res.status(500).json({ 
      error: "DB error", 
      details: err.message,
      hint: "เช็ค MONGODB_URI ใน Vercel Settings ว่าถูกต้องหรือไม่"
    });
  }
}