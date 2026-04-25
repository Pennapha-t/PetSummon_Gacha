import { MongoClient } from "mongodb";
const uri = process.env.MONGODB_URI;
let clientPromise = new MongoClient(uri).connect();

export default async function handler(req, res) {
  try {
    const client = await clientPromise;
    const db = client.db("testdb");
    
    // ดึงข้อมูลแมวทั้งหมดมาแสดง
    const cats = await db.collection("characters").find({}).toArray();
    
    res.status(200).json(cats);
  } catch (e) {
    res.status(500).json({ error: "Database Error", details: e.message });
  }
}