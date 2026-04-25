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
    
    // จุดที่ 1: แก้ชื่อ Database ให้ตรงกับใน MongoDB Compass (testdb)
    const db = client.db("testdb"); 

    // จุดที่ 2: แปลงเป็น Number ให้ตรงกับ Type ใน Database (Int32)
    const weapon = await db.collection("weapons").findOne({
      item_id: Number(item_id),
    });

    if (!weapon) {
      // ส่งค่า item_id กลับไปดูด้วยว่าหาเลขอะไรอยู่ จะได้ Debug ง่ายขึ้นครับ
      return res.status(404).json({ 
        message: "Weapon not found", 
        searched_for: item_id 
      });
    }

    res.status(200).json(weapon);
  } catch (err) {
    console.error(err);
    res.status(500).json({ error: "DB error", details: err.message });
  }
}